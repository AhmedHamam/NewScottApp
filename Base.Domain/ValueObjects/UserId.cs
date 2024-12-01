namespace Base.Domain.ValueObjects
{
    /// <summary>
    /// Represents a strongly-typed user identifier
    /// </summary>
    public record UserId
    {
        /// <summary>
        /// Gets the underlying string value of the user identifier
        /// </summary>
        public string Value { get; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UserId"/> class
        /// </summary>
        /// <param name="value">The string value for the user identifier</param>
        /// <exception cref="ArgumentException">Thrown when value is null or empty</exception>
        public UserId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("User ID cannot be empty", nameof(value));
                
            Value = value;
        }

        /// <summary>
        /// Implicitly converts a UserId to its underlying string value
        /// </summary>
        public static implicit operator string(UserId userId) => userId.Value;

        /// <summary>
        /// Explicitly converts a string to a UserId
        /// </summary>
        public static explicit operator UserId(string value) => new(value);
    }
}
