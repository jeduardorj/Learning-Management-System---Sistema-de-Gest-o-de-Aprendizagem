using LMS.Application.DTOs.Auth;
using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Domain.Enums;
using LMS.Domain.Interfaces.Repositories;

namespace LMS.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        var emailExists = await _unitOfWork.Users.ExistsByEmailAsync(request.Email);
        if (emailExists)
            throw new InvalidOperationException("E-mail já cadastrado.");

        var passwordHash = _passwordHasher.Hash(request.Password);

        var user = new User(
            name: request.Name,
            email: request.Email,
            passwordHash: passwordHash,
            role: UserRole.Student
        );

        await _unitOfWork.Users.AddAsync(user);

        var accessToken = _jwtTokenService.GenerateAccessToken(user);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();
        var refreshExpiration = _jwtTokenService.GetRefreshTokenExpiration();

        user.UpdateRefreshToken(refreshToken, refreshExpiration);

        await _unitOfWork.CommitAsync();

        return BuildResponse(user, accessToken, refreshToken);
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);

        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("E-mail ou senha inválidos.");

        var accessToken = _jwtTokenService.GenerateAccessToken(user);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();
        var refreshExpiration = _jwtTokenService.GetRefreshTokenExpiration();

        user.UpdateRefreshToken(refreshToken, refreshExpiration);
        await _unitOfWork.CommitAsync();

        return BuildResponse(user, accessToken, refreshToken);
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        var user = await _unitOfWork.Users.GetByRefreshTokenAsync(request.RefreshToken);

        if (user is null || user.RefreshTokenExpiresAt < DateTime.UtcNow)
            throw new UnauthorizedAccessException("Refresh token inválido ou expirado.");

        var accessToken = _jwtTokenService.GenerateAccessToken(user);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();
        var refreshExpiration = _jwtTokenService.GetRefreshTokenExpiration();

        user.UpdateRefreshToken(refreshToken, refreshExpiration);
        await _unitOfWork.CommitAsync();

        return BuildResponse(user, accessToken, refreshToken);
    }

    public async Task RevokeTokenAsync(string refreshToken)
    {
        var user = await _unitOfWork.Users.GetByRefreshTokenAsync(refreshToken);
        if (user is null) return;

        user.RevokeRefreshToken();
        await _unitOfWork.CommitAsync();
    }

    private static LoginResponseDto BuildResponse(User user, string accessToken, string refreshToken)
        => new()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            UserName = user.Name,
            Email = user.Email,
            Role = user.Role.ToString()
        };
}
