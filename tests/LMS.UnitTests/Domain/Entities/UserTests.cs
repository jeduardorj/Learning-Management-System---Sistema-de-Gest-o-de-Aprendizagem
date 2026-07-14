using FluentAssertions;
using LMS.Domain.Entities;
using LMS.Domain.Enums;

namespace LMS.UnitTests.Domain.Entities;

public class UserTests
{
    [Fact]
    public void Constructor_ShouldSetProvidedValues()
    {
        var user = new User("Alice", "alice@example.com", "hash", UserRole.Admin);

        user.Name.Should().Be("Alice");
        user.Email.Should().Be("alice@example.com");
        user.PasswordHash.Should().Be("hash");
        user.Role.Should().Be(UserRole.Admin);
    }

    [Fact]
    public void Constructor_ShouldNotSetRefreshTokenData()
    {
        var user = new User("Alice", "alice@example.com", "hash", UserRole.Student);

        user.RefreshToken.Should().BeNull();
        user.RefreshTokenExpiresAt.Should().BeNull();
    }

    [Fact]
    public void Constructor_ShouldInitializeEmptyCollections()
    {
        var user = new User("Alice", "alice@example.com", "hash", UserRole.Student);

        user.Enrollments.Should().BeEmpty();
        user.Certificates.Should().BeEmpty();
    }

    [Fact]
    public void UpdateRefreshToken_ShouldSetTokenExpiryAndMarkUpdated()
    {
        var user = new User("Alice", "alice@example.com", "hash", UserRole.Student);
        var expiresAt = DateTime.UtcNow.AddDays(7);

        user.UpdateRefreshToken("refresh-token", expiresAt);

        user.RefreshToken.Should().Be("refresh-token");
        user.RefreshTokenExpiresAt.Should().Be(expiresAt);
        user.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void RevokeRefreshToken_ShouldClearTokenAndMarkUpdated()
    {
        var user = new User("Alice", "alice@example.com", "hash", UserRole.Student);
        user.UpdateRefreshToken("refresh-token", DateTime.UtcNow.AddDays(7));

        user.RevokeRefreshToken();

        user.RefreshToken.Should().BeNull();
        user.RefreshTokenExpiresAt.Should().BeNull();
        user.UpdatedAt.Should().NotBeNull();
    }
}
