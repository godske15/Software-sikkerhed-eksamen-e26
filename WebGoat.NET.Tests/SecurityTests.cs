using WebGoatCore.Models;
using Xunit;

namespace WebGoat.NET.Tests;

public sealed class SecurityTests
{
    [Theory]
    [InlineData("<script>alert('xss')</script>")]
    [InlineData("<img src=x onerror=alert(1)>")]
    [InlineData("<svg/onload=alert(1)>")]
    public void BlogContent_RejectsHtmlMarkup(string content)
    {
        // Arrange
        var maliciousContent = content;

        // Act
        var exception = Record.Exception(() => BlogEntry.BlogContent.Create(maliciousContent));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    [Theory]
    [InlineData("<script>alert('xss')</script>")]
    [InlineData("<img src=x onerror=alert(1)>")]
    [InlineData("<svg/onload=alert(1)>")]
    public void BlogTitle_RejectsHtmlMarkup(string title)
    {
        // Arrange
        var maliciousTitle = title;

        // Act
        var exception = Record.Exception(() => BlogEntry.BlogTitle.Create(maliciousTitle));

        // Assert
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void BlogContent_AcceptsPlainText()
    {
        // Arrange
        const string plainText = "This is a safe comment.";

        // Act
        var content = BlogEntry.BlogContent.Create(plainText);

        // Assert
        Assert.Equal("This is a safe comment.", content.Value);
    }

    [Fact]
    public void BlogTitle_AcceptsPlainText()
    {
        // Arrange
        const string plainText = "A safe title";

        // Act
        var title = BlogEntry.BlogTitle.Create(plainText);

        // Assert
        Assert.Equal("A safe title", title.Value);
    }
}
