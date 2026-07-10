using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces.Repositories;

public interface IEnrollmentRepository : IBaseRepository<Enrollment>
{
    Task<bool> ExistsAsync(Guid userId, Guid courseId);
    Task<Enrollment?> GetWithProgressAsync(Guid enrollmentId);
}
