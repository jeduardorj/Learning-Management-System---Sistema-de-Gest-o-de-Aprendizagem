using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces.Repositories;

public interface IModuleRepository : IBaseRepository<Module>
{
    Task<IEnumerable<Module>> GetByCourseIdAsync(Guid courseId);
    Task<Module?> GetByIdWithLessonsAsync(Guid moduleId);
    Task<bool> ExistsByOrderAsync(Guid courseId, int order, Guid? excludeId = null);
}
