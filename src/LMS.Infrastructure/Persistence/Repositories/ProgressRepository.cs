using LMS.Domain.Entities;
using LMS.Domain.Interfaces.Repositories;
using LMS.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Persistence.Repositories;

public class ProgressRepository : BaseRepository<Progress>, IProgressRepository
{
    public ProgressRepository(LmsDbContext context) : base(context) { }

    public async Task<Progress?> GetByEnrollmentAndLessonAsync(Guid enrollmentId, Guid lessonId)
        => await _dbSet
            .FirstOrDefaultAsync(x => x.EnrollmentId == enrollmentId && x.LessonId == lessonId);

    public async Task<IEnumerable<Progress>> GetByEnrollmentIdAsync(Guid enrollmentId)
        => await _dbSet
            .Include(x => x.Lesson)
            .Where(x => x.EnrollmentId == enrollmentId)
            .OrderBy(x => x.Lesson.Order)
            .ToListAsync();

    public async Task<int> CountCompletedAsync(Guid enrollmentId)
        => await _dbSet
            .CountAsync(x => x.EnrollmentId == enrollmentId && x.Completed);

    public async Task<int> CountTotalLessonsInCourseAsync(Guid courseId)
        => await _context.Set<Lesson>()
            .CountAsync(x => x.Module.CourseId == courseId);
}
