using FluentAssertions;
using LMS.Application.DTOs.Auth;
using LMS.Application.Interfaces;
using LMS.Application.Services;
using LMS.Domain.Entities;
using LMS.Domain.Enums;
using LMS.Domain.Interfaces.Repositories;
using Moq;

namespace LMS.UnitTests.Application.Services;

public class AuthServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IJwtTokenService> _mockJwtTokenService;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockJwtTokenService = new Mock<IJwtTokenService>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();

        _mockUnitOfWork.Setup(x => x.Users).Returns(_mockUserRepository.Object);
        _mockUnitOfWork.Setup(x => x.CommitAsync()).ReturnsAsync(1);

        _mockJwtTokenService.Setup(x => x.GenerateAccessToken(It.IsAny<User>()))
            .Returns("access-token");
        _mockJwtTokenService.Setup(x => x.GenerateRefreshToken())
            .Returns("refresh-token");
        _mockJwtTokenService.Setup(x => x.GetRefreshTokenExpiration())
            .Returns(DateTime.UtcNow.AddDays(7));

        _authService = new AuthService(
            _mockUnitOfWork.Object,
            _mockJwtTokenService.Object,
            _mockPasswordHasher.Object);
    }

    [Fact]
    public async Task RegisterAsync_ShouldCreateUser_WhenEmailNotExists()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            Name = "João Silva",
            Email = "joao@teste.com",
            Password = "Teste@123",
            ConfirmPassword = "Teste@123"
        };

        _mockUserRepository
            .Setup(x => x.ExistsByEmailAsync(request.Email))
            .ReturnsAsync(false);

        _mockPasswordHasher
            .Setup(x => x.Hash(request.Password))
            .Returns("hashed-password");

        _mockUserRepository
            .Setup(x => x.AddAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _authService.RegisterAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().Be("access-token");
        result.RefreshToken.Should().Be("refresh-token");
        result.Email.Should().Be(request.Email);
        result.Role.Should().Be(UserRole.Student.ToString());

        _mockUserRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrowException_WhenEmailAlreadyExists()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            Name = "João",
            Email = "joao@teste.com",
            Password = "Teste@123",
            ConfirmPassword = "Teste@123"
        };

        _mockUserRepository
            .Setup(x => x.ExistsByEmailAsync(request.Email))
            .ReturnsAsync(true);

        // Act
        var act = async () => await _authService.RegisterAsync(request);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*E-mail já cadastrado*");

        _mockUserRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var request = new LoginRequestDto
        {
            Email = "joao@teste.com",
            Password = "Teste@123"
        };

        var user = new User("João", request.Email, "hashed-password", UserRole.Student);

        _mockUserRepository
            .Setup(x => x.GetByEmailAsync(request.Email))
            .ReturnsAsync(user);

        _mockPasswordHasher
            .Setup(x => x.Verify(request.Password, user.PasswordHash))
            .Returns(true);

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().Be("access-token");
        result.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowUnauthorized_WhenPasswordIsWrong()
    {
        // Arrange
        var request = new LoginRequestDto
        {
            Email = "joao@teste.com",
            Password = "SenhaErrada@123"
        };

        var user = new User("João", request.Email, "hashed-password", UserRole.Student);

        _mockUserRepository
            .Setup(x => x.GetByEmailAsync(request.Email))
            .ReturnsAsync(user);

        _mockPasswordHasher
            .Setup(x => x.Verify(request.Password, user.PasswordHash))
            .Returns(false);

        // Act
        var act = async () => await _authService.LoginAsync(request);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("*E-mail ou senha inválidos*");
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowUnauthorized_WhenUserNotFound()
    {
        // Arrange
        var request = new LoginRequestDto
        {
            Email = "naoexiste@teste.com",
            Password = "Teste@123"
        };

        _mockUserRepository
            .Setup(x => x.GetByEmailAsync(request.Email))
            .ReturnsAsync((User?)null);

        // Act
        var act = async () => await _authService.LoginAsync(request);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("*E-mail ou senha inválidos*");
    }
}
