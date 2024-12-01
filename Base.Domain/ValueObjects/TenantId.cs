namespace Base.Domain.ValueObjects
{
    public record TenantId
    {
        public Guid Value { get; }
        
        public TenantId(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentException("Tenant ID cannot be empty", nameof(value));
                
            Value = value;
        }

        public static TenantId New() => new(Guid.NewGuid());
        
        public static implicit operator Guid(TenantId tenantId) => tenantId.Value;
        public static explicit operator TenantId(Guid value) => new(value);

        public override string ToString() => Value.ToString();
    }
}
