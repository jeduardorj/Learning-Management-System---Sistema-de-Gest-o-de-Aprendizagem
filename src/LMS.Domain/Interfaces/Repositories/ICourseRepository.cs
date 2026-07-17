using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces.Repositories;

public interface ICourseRepository : IBaseRepository<Course>
{
    Task<bool> ExistsByTitleAsync(string title);
    IQueryable<Course> GetQueryable();
}
