using LMS.Domain.Entities;
using LMS.Domain.Interfaces.Repositories;
using LMS.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Persistence.Repositories;

public class LessonRepository : BaseRepository<Lesson>, ILessonRepository
{
    public LessonRepository(LmsDbContext context) : base(context) { }

    public async Task<IEnumerable<Lesson>> GetByModuleIdAsync(Guid moduleId)
        => await _dbSet
            .Where(x => x.ModuleId == moduleId)
            .OrderBy(x => x.Order)
            .ToListAsync();

    public async Task<bool> ExistsByOrderAsync(Guid moduleId, int order, Guid? excludeId = null)
        => await _dbSet.AnyAsync(x =>
            x.ModuleId == moduleId &&
            x.Order == order &&
            (excludeId == null || x.Id != excludeId));
}
