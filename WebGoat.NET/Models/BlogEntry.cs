using System;
using System.Collections.Generic;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
namespace WebGoatCore.Models
{
    public class BlogEntry
    {
        /// <summary>Validated title for a blog entry.</summary>
        public readonly record struct BlogTitle
        {
            public const int MaxLength = 500;
            public string Value { get; }

            private BlogTitle(string value) => Value = value;

            public static BlogTitle Create(string value)
            {
                Validate(value, nameof(value), MaxLength, "title");
                return new BlogTitle(value.Trim());
            }

            public override string ToString() => Value;
        }

        /// <summary>Validated plain-text content for a blog entry or response.</summary>
        public readonly record struct BlogContent
        {
            public const int MaxLength = 500;
            public string Value { get; }

            private BlogContent(string value) => Value = value;

            public static BlogContent Create(string value)
            {
                Validate(value, nameof(value), MaxLength, "content");
                return new BlogContent(value);
            }

            public override string ToString() => Value;
        }

        private static void Validate(string value, string parameterName, int maxLength, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"Blog {fieldName} must not be empty.", parameterName);

            if (value.Length > maxLength)
                throw new ArgumentException($"Blog {fieldName} must be at most {maxLength} characters.", parameterName);

            if (value.Contains('<') || value.Contains('>'))
                throw new ArgumentException($"Blog {fieldName} must contain plain text only.", parameterName);
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PostedDate { get; set; }
        public string Contents { get; set; }
        public string Author { get; set; }

        public virtual IList<BlogResponse> Responses { get; set; }
    }
}
