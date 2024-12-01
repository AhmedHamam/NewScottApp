namespace Base.Domain.ValueObjects
{
    /// <summary>
    /// Represents a strongly-typed tenant identifier
    /// </summary>
    public record TenantId
    {
        /// <summary>
        /// Gets the underlying GUID value of the tenant identifier
        /// </summary>
        public Guid Value { get; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TenantId"/> class
        /// </summary>
        /// <param name="value">The GUID value for the tenant identifier</param>
        /// <exception cref="ArgumentException">Thrown when value is empty</exception>
        public TenantId(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentException("Tenant ID cannot be empty", nameof(value));
                
            Value = value;
        }

        /// <summary>
        /// Creates a new tenant identifier with a random GUID
        /// </summary>
        /// <returns>A new tenant identifier</returns>
        public static TenantId New() => new(Guid.NewGuid());
        
        /// <summary>
        /// Implicitly converts a TenantId to its underlying GUID value
        /// </summary>
        public static implicit operator Guid(TenantId tenantId) => tenantId.Value;

        /// <summary>
        /// Explicitly converts a GUID to a TenantId
        /// </summary>
        public static explicit operator TenantId(Guid value) => new(value);

        /// <summary>
        /// Returns a string representation of the tenant identifier
        /// </summary>
        public override string ToString() => Value.ToString();
    }
}
