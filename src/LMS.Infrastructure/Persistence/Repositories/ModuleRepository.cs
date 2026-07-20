using LMS.Domain.Entities;
using LMS.Domain.Interfaces.Repositories;
using LMS.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Persistence.Repositories;

public class ModuleRepository : BaseRepository<Module>, IModuleRepository
{
    public ModuleRepository(LmsDbContext context) : base(context) { }

    public async Task<IEnumerable<Module>> GetByCourseIdAsync(Guid courseId)
        => await _dbSet
            .Where(x => x.CourseId == courseId)
            .OrderBy(x => x.Order)
            .ToListAsync();

    public async Task<Module?> GetByIdWithLessonsAsync(Guid moduleId)
        => await _dbSet
            .Include(x => x.Lessons.OrderBy(l => l.Order))
            .FirstOrDefaultAsync(x => x.Id == moduleId);

    public async Task<bool> ExistsByOrderAsync(Guid courseId, int order, Guid? excludeId = null)
        => await _dbSet.AnyAsync(x =>
            x.CourseId == courseId &&
            x.Order == order &&
            (excludeId == null || x.Id != excludeId));
}
