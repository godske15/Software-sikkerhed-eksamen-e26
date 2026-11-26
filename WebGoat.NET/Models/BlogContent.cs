using System;

namespace WebGoatCore.Models
{
    /// <summary>Validated plain-text content shared by blog entries and responses.</summary>
    public readonly record struct BlogContent
    {
        public const int MaxLength = 500;
        public string Value { get; }

        private BlogContent(string value) => Value = value;

        public static BlogContent Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Blog content must not be empty.", nameof(value));
            if (value.Length > MaxLength)
                throw new ArgumentException($"Blog content must be at most {MaxLength} characters.", nameof(value));
            if (value.Contains('<') || value.Contains('>'))
                throw new ArgumentException("Blog content must contain plain text only.", nameof(value));
            return new BlogContent(value);
        }

        public override string ToString() => Value;
    }
}
