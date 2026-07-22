using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces.Repositories;

public interface IProgressRepository : IBaseRepository<Progress>
{
    Task<Progress?> GetByEnrollmentAndLessonAsync(Guid enrollmentId, Guid lessonId);
    Task<IEnumerable<Progress>> GetByEnrollmentIdAsync(Guid enrollmentId);
    Task<int> CountCompletedAsync(Guid enrollmentId);
    Task<int> CountTotalLessonsInCourseAsync(Guid courseId);
}
