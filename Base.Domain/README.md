# Base.Domain Project

## Overview
Base.Domain is a foundational library that provides core domain-driven design (DDD) building blocks for enterprise applications. It implements common patterns and practices for building robust, maintainable, and scalable domain models.

## Features

### 1. Entity Base Classes
- **BaseEntity**: Abstract base class with soft delete capabilities
- **AuditableEntity**: Extends BaseEntity with creation and modification tracking
- **TenantAwareEntity**: Supports multi-tenancy with proper tenant isolation

### 2. Domain Events
- Built-in domain event handling
- Standard events for common operations:
  - `EntityDeletedEvent`
  - `TenantAssignedEvent`
- Extensible event system for custom domain events

### 3. Value Objects
- **UserId**: Strongly-typed user identifier
- **TenantId**: GUID-based tenant identifier with validation
- Immutable by design
- Include proper validation and comparison logic

### 4. Common Interfaces
- **ISoftDelete**: Soft deletion capability
- **ICreatedAuditableEntity**: Creation audit information
- **IModifiedAuditableEntity**: Modification audit information
- **ITrackable**: Version and concurrency tracking
- **IHasStatus**: Generic status management
- **ITenantEntity**: Multi-tenant support

### 5. Services
- **IDateTimeProvider**: Abstract time-dependent operations

## Technical Details

### Entity Framework Core Integration
```csharp
public class YourDbContext : DbContext
{
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Handle soft delete
        foreach (var entry in ChangeTracker.Entries<ISoftDelete>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.IsDeleted = false;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.MarkAsDeleted("system");
                    break;
            }
        }

        // Handle audit fields
        foreach (var entry in ChangeTracker.Entries<ICreatedAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.MarkAsCreated("current_user");
            }
        }

        foreach (var entry in ChangeTracker.Entries<IModifiedAuditableEntity>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.MarkAsUpdated("current_user");
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
```

### Multi-Tenant Query Filtering
```csharp
public class YourDbContext : DbContext
{
    private readonly ITenantProvider _tenantProvider;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply global query filter for multi-tenancy
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ITenantEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, nameof(ITenantEntity.TenantId));
                var tenantId = Expression.Constant(_tenantProvider.CurrentTenantId);
                var filter = Expression.Lambda(Expression.Equal(property, tenantId), parameter);
                
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
            }
        }
    }
}
```

## Advanced Examples

### 1. Complex Domain Entity
```csharp
public class Order : TenantAwareEntity, IHasStatus<OrderStatus>, ITrackable
{
    private readonly List<OrderItem> _items = new();
    
    public OrderStatus Status { get; private set; }
    public int Version { get; private set; }
    public string ConcurrencyStamp { get; private set; } = Guid.NewGuid().ToString();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public Money TotalAmount { get; private set; }

    public Order(TenantId tenantId) : base(tenantId)
    {
        Status = OrderStatus.Draft;
    }

    public void AddItem(Product product, int quantity)
    {
        if (Status != OrderStatus.Draft)
            throw new InvalidOperationException("Can only add items to draft orders");

        var item = new OrderItem(product, quantity);
        _items.Add(item);
        RecalculateTotal();
        IncrementVersion();
    }

    public void UpdateStatus(OrderStatus newStatus, string updatedBy)
    {
        ValidateStatusTransition(newStatus);
        Status = newStatus;
        MarkAsUpdated(updatedBy);
        IncrementVersion();
        AddDomainEvent(new OrderStatusChangedEvent(Id, Status));
    }

    private void ValidateStatusTransition(OrderStatus newStatus)
    {
        // Implement status transition rules
        if (Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Cannot change status of cancelled order");
        
        if (Status == OrderStatus.Delivered && newStatus != OrderStatus.Returned)
            throw new InvalidOperationException("Delivered order can only be returned");
    }

    public void IncrementVersion()
    {
        Version++;
        ConcurrencyStamp = Guid.NewGuid().ToString();
    }

    private void RecalculateTotal()
    {
        TotalAmount = new Money(_items.Sum(i => i.Price.Amount));
    }
}
```

### 2. Custom Domain Event Handler
```csharp
public class OrderStatusChangedEventHandler : IEventHandler<OrderStatusChangedEvent>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<OrderStatusChangedEventHandler> _logger;

    public OrderStatusChangedEventHandler(
        IEmailService emailService,
        ILogger<OrderStatusChangedEventHandler> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Handle(OrderStatusChangedEvent @event)
    {
        try
        {
            await _emailService.SendOrderStatusUpdateEmail(@event.OrderId, @event.NewStatus);
            _logger.LogInformation("Order {OrderId} status updated to {Status}", 
                @event.OrderId, @event.NewStatus);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process order status change for {OrderId}", 
                @event.OrderId);
            throw;
        }
    }
}
```

### 3. Value Object Implementation
```csharp
public record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency = "USD")
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));
            
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be empty", nameof(currency));

        Amount = amount;
        Currency = currency;
    }

    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot add money with different currencies");

        return new Money(left.Amount + right.Amount, left.Currency);
    }

    public static Money Zero(string currency = "USD") => new(0, currency);
}
```

## Troubleshooting Guide

### Common Issues and Solutions

1. **Entity Framework Tracking Issues**
   
   Problem: Changes to entities are not being tracked correctly.
   
   Solution:
   ```csharp
   // Ensure properties have private setters
   public string Name { get; private set; }
   
   // Use entity methods instead of direct property modification
   entity.UpdateName("new name"); // Instead of entity.Name = "new name"
   ```

2. **Multi-Tenant Data Leaks**
   
   Problem: Data from different tenants is visible across boundaries.
   
   Solution:
   ```csharp
   // Always include tenant filter in queries
   public async Task<T> GetById<T>(int id) where T : ITenantEntity
   {
       var entity = await _dbContext.Set<T>()
           .FirstOrDefaultAsync(e => e.Id == id && e.TenantId == _currentTenant.Id);
           
       if (entity == null)
           throw new EntityNotFoundException(typeof(T), id);
           
       return entity;
   }
   ```

3. **Concurrency Conflicts**
   
   Problem: Multiple users updating the same entity simultaneously.
   
   Solution:
   ```csharp
   // Use optimistic concurrency control
   modelBuilder.Entity<Order>()
       .Property(e => e.ConcurrencyStamp)
       .IsConcurrencyToken();
   
   try
   {
       await _dbContext.SaveChangesAsync();
   }
   catch (DbUpdateConcurrencyException ex)
   {
       // Handle concurrency conflict
       var entry = ex.Entries.Single();
       var currentValues = entry.CurrentValues;
       var databaseValues = await entry.GetDatabaseValuesAsync();
       
       // Implement conflict resolution strategy
   }
   ```

4. **Domain Event Handling**
   
   Problem: Domain events not being processed.
   
   Solution:
   ```csharp
   public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
   {
       // Collect domain events before saving
       var domainEvents = ChangeTracker.Entries<BaseEntity>()
           .Select(e => e.Entity)
           .Where(e => e.DomainEvents.Any())
           .SelectMany(e => e.DomainEvents)
           .ToList();
           
       var result = await base.SaveChangesAsync(cancellationToken);
       
       // Process domain events after successful save
       foreach (var domainEvent in domainEvents)
       {
           await _domainEventDispatcher.Dispatch(domainEvent);
       }
       
       return result;
   }
   ```

### Performance Optimization

1. **Efficient Querying**
```csharp
// Use specific queries instead of loading entire entities
public async Task<OrderSummaryDto> GetOrderSummary(int orderId)
{
    return await _dbContext.Orders
        .Where(o => o.Id == orderId)
        .Select(o => new OrderSummaryDto
        {
            Id = o.Id,
            Status = o.Status,
            TotalAmount = o.TotalAmount
        })
        .FirstOrDefaultAsync();
}
```

2. **Batch Operations**
```csharp
public async Task UpdateStatusForBatch(IEnumerable<int> orderIds, OrderStatus newStatus)
{
    await _dbContext.Orders
        .Where(o => orderIds.Contains(o.Id))
        .ExecuteUpdateAsync(s => s
            .SetProperty(o => o.Status, newStatus)
            .SetProperty(o => o.UpdatedDate, DateTime.UtcNow));
}
```

### Debugging Tips

1. Enable detailed logging:
```csharp
services.AddDbContext<YourDbContext>(options =>
    options.UseSqlServer(connectionString)
           .EnableSensitiveDataLogging()
           .EnableDetailedErrors()
           .LogTo(Console.WriteLine));
```

2. Track domain events:
```csharp
public class DebuggableDomainEventDispatcher : IDomainEventDispatcher
{
    private readonly ILogger<DebuggableDomainEventDispatcher> _logger;
    
    public async Task Dispatch(IDomainEvent @event)
    {
        _logger.LogInformation("Dispatching domain event: {@Event}", @event);
        // Dispatch logic
    }
}
```

## Usage Examples

### Creating a Tenant-Aware Entity
```csharp
public class CustomerEntity : TenantAwareEntity
{
    public string Name { get; private set; }
    
    public CustomerEntity(TenantId tenantId, string name) : base(tenantId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
            
        Name = name;
    }
    
    protected CustomerEntity() { } // For ORM
}
```

### Using Status Management
```csharp
public class OrderEntity : AuditableEntity, IHasStatus<OrderStatus>
{
    public OrderStatus Status { get; private set; }
    
    public void UpdateStatus(OrderStatus newStatus, string updatedBy)
    {
        Status = newStatus;
        MarkAsUpdated(updatedBy);
    }
}
```

### Implementing Version Tracking
```csharp
public class ProductEntity : AuditableEntity, ITrackable
{
    public int Version { get; private set; }
    public string ConcurrencyStamp { get; private set; } = Guid.NewGuid().ToString();
    
    public void IncrementVersion()
    {
        Version++;
        ConcurrencyStamp = Guid.NewGuid().ToString();
    }
}
```

## Best Practices

### 1. Entity Design
- Use private setters for properties
- Validate inputs in constructors and methods
- Raise domain events for significant state changes
- Include protected constructors for ORM support

### 2. Multi-tenancy
- Always assign entities to a tenant
- Prevent cross-tenant data access
- Use strongly-typed TenantId
- Validate tenant assignments

### 3. Audit Trail
- Track creation and modification details
- Include user information for all changes
- Maintain deletion history
- Use UTC timestamps

### 4. Domain Events
- Raise events for significant state changes
- Keep events immutable
- Include relevant context in events
- Clear events after processing

## Project Structure
```
Base.Domain/
├── CommonInterfaces/
│   ├── IIsActive.cs
│   ├── ISoftDelete.cs
│   ├── ITrackable.cs
│   ├── IHasStatus.cs
│   └── ITenantEntity.cs
├── CommonModels/
│   ├── BaseEntity.cs
│   ├── AuditableEntity.cs
│   └── TenantAwareEntity.cs
├── DomainEvents/
│   ├── IDomainEvent.cs
│   ├── EntityDeletedEvent.cs
│   └── TenantAssignedEvent.cs
├── ValueObjects/
│   ├── UserId.cs
│   └── TenantId.cs
└── Services/
    └── IDateTimeProvider.cs
```

## Dependencies
- .NET 6.0 or later
- No external dependencies required

## Contributing
- Follow existing patterns and naming conventions
- Maintain encapsulation and immutability
- Add appropriate XML documentation
- Include unit tests for new features

## License
This project is licensed under the MIT License - see the LICENSE file for details.
