using FluentAssertions;
using LMS.Domain.Entities;
using LMS.Domain.Enums;

namespace LMS.UnitTests.Domain.Entities;

public class UserTests
{
    [Fact]
    public void Constructor_ShouldCreateUser_WithCorrectValues()
    {
        // Arrange & Act
        var user = new User("João Silva", "joao@teste.com", "hash123", UserRole.Student);

        // Assert
        user.Name.Should().Be("João Silva");
        user.Email.Should().Be("joao@teste.com");
        user.PasswordHash.Should().Be("hash123");
        user.Role.Should().Be(UserRole.Student);
        user.RefreshToken.Should().BeNull();
        user.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void UpdateRefreshToken_ShouldSetToken_AndExpiration()
    {
        // Arrange
        var user = new User("João", "joao@teste.com", "hash", UserRole.Student);
        var token = "refresh-token-value";
        var expiration = DateTime.UtcNow.AddDays(7);

        // Act
        user.UpdateRefreshToken(token, expiration);

        // Assert
        user.RefreshToken.Should().Be(token);
        user.RefreshTokenExpiresAt.Should().BeCloseTo(expiration, TimeSpan.FromSeconds(1));
        user.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void RevokeRefreshToken_ShouldClearToken_AndExpiration()
    {
        // Arrange
        var user = new User("João", "joao@teste.com", "hash", UserRole.Student);
        user.UpdateRefreshToken("token", DateTime.UtcNow.AddDays(7));

        // Act
        user.RevokeRefreshToken();

        // Assert
        user.RefreshToken.Should().BeNull();
        user.RefreshTokenExpiresAt.Should().BeNull();
        user.UpdatedAt.Should().NotBeNull();
    }
}
