using LMS.Domain.Entities;
using LMS.Domain.Interfaces.Repositories;
using LMS.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Persistence.Repositories;

public class EnrollmentRepository : BaseRepository<Enrollment>, IEnrollmentRepository
{
    public EnrollmentRepository(LmsDbContext context) : base(context) { }

    public async Task<bool> ExistsAsync(Guid userId, Guid courseId)
        => await _dbSet.AnyAsync(x => x.UserId == userId && x.CourseId == courseId);

    public async Task<Enrollment?> GetWithProgressAsync(Guid enrollmentId)
        => await _dbSet
            .Include(x => x.Progresses)
            .FirstOrDefaultAsync(x => x.Id == enrollmentId);
}
