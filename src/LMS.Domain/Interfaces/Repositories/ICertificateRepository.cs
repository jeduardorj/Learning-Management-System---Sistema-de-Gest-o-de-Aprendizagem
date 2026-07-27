using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces.Repositories;

public interface ICertificateRepository : IBaseRepository<Certificate>
{
    Task<Certificate?> GetByCodeAsync(string code);
    Task<Certificate?> GetByUserAndCourseAsync(Guid userId, Guid courseId);
    Task<IEnumerable<Certificate>> GetByUserIdAsync(Guid userId);
}
