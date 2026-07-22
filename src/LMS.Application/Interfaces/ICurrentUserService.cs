namespace LMS.Application.Interfaces;

public interface ICurrentUserService
{
    Guid UserId { get; }
    string Role { get; }
    bool IsAdmin { get; }
}
