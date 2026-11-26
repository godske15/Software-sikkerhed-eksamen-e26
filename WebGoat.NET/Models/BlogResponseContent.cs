using System;
using System.Linq;

namespace WebGoatCore.Models
{
    // Step 3: Domæneprimitiven repræsenterer en gyldig svartekst.
    public sealed class BlogResponseContent
    {
        public const int MaxLength = 2000;

        public string Value { get; }

        public BlogResponseContent(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Respons kan ikke være tomt.", nameof(value));

            if (value.Length > MaxLength)
                throw new ArgumentException($"Responslængden kan ikke overstige {MaxLength} karakterer", nameof(value));

            if (!value.Any(char.IsLetter))
                throw new ArgumentException(
                    "Respons skal indeholde mindst ét bogstav.",
                    nameof(value));

            Value = value;
        }
    }
}
