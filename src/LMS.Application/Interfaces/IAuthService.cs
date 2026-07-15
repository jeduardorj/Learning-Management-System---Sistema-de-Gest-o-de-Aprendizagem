using LMS.Application.DTOs.Auth;

namespace LMS.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);
    Task RevokeTokenAsync(string refreshToken);
}
