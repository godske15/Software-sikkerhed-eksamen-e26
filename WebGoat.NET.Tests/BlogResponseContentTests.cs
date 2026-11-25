using WebGoatCore.Models;

namespace WebGoat.NET.Tests;

// Tests for domæneprimitiven BlogResponseContents.
// Reglerne (invarianten) som testene beskriver:
//   - Indholdet må ikke være null, tomt eller kun bestå af whitespace
//   - Indholdet må højst være 500 tegn (500 er tilladt, 501 er ikke)
//   - Indholdet må indeholde bogstaver (inkl. æøå), tal, mellemrum,
//     linjeskift og almindelig tegnsætning
//   - Indholdet må ikke indeholde HTML/script-tegn som < og >
public class BlogResponseContentTests
{
    // ---------- Positive tests: gyldigt input må IKKE kaste en exception ----------

    [Theory]
    [InlineData("Tak for et godt indlæg!")]
    [InlineData("Hej med dig, Søren. Hvad synes du om det?")]
    [InlineData("Jeg har 2 spørgsmål - kan du hjælpe?")]
    [InlineData("Første linje\nAnden linje")]
    [InlineData("a")]
    public void BlogResponseContents_ValidText_DoesNotThrowException(string input)
    {
        // Arrange: input kommer fra [InlineData] ovenfor

        // Act
        var exception = Record.Exception(() => new BlogResponseContents(input));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void BlogResponseContents_ExactlyMaxLength_DoesNotThrowException()
    {
        // Arrange: grænseværdi - præcis 500 tegn skal være tilladt
        string input = new string('a', 500);

        // Act
        var exception = Record.Exception(() => new BlogResponseContents(input));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void BlogResponseContents_ValidText_GetValueReturnsSameText()
    {
        // Arrange
        string input = "Tak for et godt indlæg!";

        // Act
        var contents = new BlogResponseContents(input);

        // Assert: primitiven må ikke ændre på en gyldig værdi
        Assert.Equal(input, contents.GetValue());
    }

    // ---------- Negative tests: ugyldigt input SKAL kaste ArgumentException ----------

    [Fact]
    public void BlogResponseContents_Null_ThrowsArgumentException()
    {
        // Arrange: null! fortæller compileren "jeg sender null med vilje".
        // Det er vigtigt at teste, fordi en angriber kan sende en request uden feltet.
        string input = null!;

        // Act + Assert
        Assert.Throws<ArgumentException>(() => new BlogResponseContents(input));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\n\n")]
    public void BlogResponseContents_EmptyOrWhitespace_ThrowsArgumentException(string input)
    {
        // Arrange: input kommer fra [InlineData] ovenfor

        // Act + Assert
        Assert.Throws<ArgumentException>(() => new BlogResponseContents(input));
    }

    [Fact]
    public void BlogResponseContents_OneOverMaxLength_ThrowsArgumentException()
    {
        // Arrange: grænseværdi - 501 tegn er én for meget
        string input = new string('a', 501);

        // Act + Assert
        Assert.Throws<ArgumentException>(() => new BlogResponseContents(input));
    }

    [Theory]
    [InlineData("<script>alert('xss')</script>")]
    [InlineData("<img src=x onerror=alert(1)>")]
    [InlineData("<svg/onload=alert(1)>")]
    [InlineData("Pænt svar <b>med fed</b>")]
    public void BlogResponseContents_HtmlOrScript_ThrowsArgumentException(string input)
    {
        // Arrange: typiske XSS-payloads. Svaret vises på blog-siden,
        // så HTML i indholdet ville kunne køre i andre brugeres browser.

        // Act + Assert
        Assert.Throws<ArgumentException>(() => new BlogResponseContents(input));
    }
}
