using System;
using System.Text.RegularExpressions;

namespace WebGoatCore.Models;

// Domæneprimitiv for indholdet af et svar på et blogopslag.
// Invariant: et BlogResponseContents-objekt kan kun eksistere med gyldigt indhold.
public class BlogResponseContents
{
    public const int MaxLength = 500;

    // Tilladelsesliste (allowlist) over tegn et svar må indeholde:
    //   \p{L}  bogstaver i alle sprog (inkl. æøå)
    //   \p{N}  tal
    //   \s     mellemrum og linjeskift
    //   .,!?'"():;-  almindelig tegnsætning
    // Alt andet (fx < og >, som bruges i HTML/XSS) afvises.
    // Timeout beskytter mod ReDoS (regex der bruger urimeligt lang tid).
    private static readonly Regex AllowedCharacters = new Regex(
        @"^[\p{L}\p{N}\s.,!?'""():;\-]+$",
        RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(100));

    // readonly: feltet kan kun sættes i konstruktøren, så objektet er immutable
    private readonly string contents;

    // Konstruktøren håndhæver invarianten før feltet sættes
    public BlogResponseContents(string contents)
    {
        IsContentsValid(contents);
        this.contents = contents;
    }

    // Returnerer den validerede værdi
    public string GetValue()
    {
        return contents;
    }

    // Kaster ArgumentException hvis indholdet bryder invarianten.
    // Rækkefølgen er vigtig: null skal tjekkes først, ellers crasher .Length.
    private static void IsContentsValid(string contents)
    {
        if (string.IsNullOrWhiteSpace(contents))
        {
            throw new ArgumentException("The contents of the blog response must not be empty.");
        }

        if (contents.Length > MaxLength)
        {
            throw new ArgumentException($"The contents of the blog response must be at most {MaxLength} characters.");
        }

        bool hasOnlyAllowedCharacters;
        try
        {
            hasOnlyAllowedCharacters = AllowedCharacters.IsMatch(contents);
        }
        catch (RegexMatchTimeoutException)
        {
            hasOnlyAllowedCharacters = false;
        }

        if (!hasOnlyAllowedCharacters)
        {
            throw new ArgumentException("The contents of the blog response contains illegal characters.");
        }
    }
}
