using LMS.Domain.Entities;

namespace LMS.Application.Interfaces;

public interface IJwtTokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    DateTime GetRefreshTokenExpiration();
}
