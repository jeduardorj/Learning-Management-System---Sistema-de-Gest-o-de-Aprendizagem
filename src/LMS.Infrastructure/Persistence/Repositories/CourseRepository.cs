using LMS.Domain.Entities;
using LMS.Domain.Interfaces.Repositories;
using LMS.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Persistence.Repositories;

public class CourseRepository : BaseRepository<Course>, ICourseRepository
{
    public CourseRepository(LmsDbContext context) : base(context) { }

    public async Task<bool> ExistsByTitleAsync(string title)
        => await _dbSet.AnyAsync(x => x.Title == title);
}
