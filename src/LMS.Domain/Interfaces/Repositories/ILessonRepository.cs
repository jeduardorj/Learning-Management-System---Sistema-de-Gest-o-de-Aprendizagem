using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces.Repositories;

public interface ILessonRepository : IBaseRepository<Lesson>
{
    Task<IEnumerable<Lesson>> GetByModuleIdAsync(Guid moduleId);
    Task<bool> ExistsByOrderAsync(Guid moduleId, int order, Guid? excludeId = null);
}
