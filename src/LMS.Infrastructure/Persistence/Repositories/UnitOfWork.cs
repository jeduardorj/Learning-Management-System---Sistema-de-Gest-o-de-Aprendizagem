using LMS.Domain.Interfaces.Repositories;
using LMS.Infrastructure.Persistence.Context;

namespace LMS.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly LmsDbContext _context;

    public IUserRepository Users { get; }
    public ICourseRepository Courses { get; }
    public IEnrollmentRepository Enrollments { get; }

    public UnitOfWork(LmsDbContext context)
    {
        _context = context;
        Users = new UserRepository(context);
        Courses = new CourseRepository(context);
        Enrollments = new EnrollmentRepository(context);
    }

    public async Task<int> CommitAsync()
        => await _context.SaveChangesAsync();

    public void Dispose()
        => _context.Dispose();
}
