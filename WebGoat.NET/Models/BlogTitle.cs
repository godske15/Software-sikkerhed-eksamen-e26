using System;

namespace WebGoatCore.Models
{
    /// <summary>Validated title for a blog entry.</summary>
    public readonly record struct BlogTitle
    {
        public const int MaxLength = 500;
        public string Value { get; }

        private BlogTitle(string value) => Value = value;

        public static BlogTitle Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Blog title must not be empty.", nameof(value));
            if (value.Length > MaxLength)
                throw new ArgumentException($"Blog title must be at most {MaxLength} characters.", nameof(value));
            if (value.Contains('<') || value.Contains('>'))
                throw new ArgumentException("Blog title must contain plain text only.", nameof(value));

            return new BlogTitle(value.Trim());
        }

        public override string ToString() => Value;
    }
}
