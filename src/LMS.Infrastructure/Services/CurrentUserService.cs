using System.Security.Claims;
using LMS.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LMS.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Guid.TryParse(value, out var id)
                ? id
                : throw new UnauthorizedAccessException("Usuário não autenticado.");
        }
    }

    public string Role
        => _httpContextAccessor.HttpContext?.User
            .FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

    public bool IsAdmin => Role == "Admin";
}
