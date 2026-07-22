namespace LMS.Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    ICourseRepository Courses { get; }
    IEnrollmentRepository Enrollments { get; }
    IModuleRepository Modules { get; }
    ILessonRepository Lessons { get; }
    IProgressRepository Progresses { get; }
    Task<int> CommitAsync();
}
