using LMS.Domain.Entities;
using LMS.Domain.Interfaces.Repositories;
using LMS.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Persistence.Repositories;

public class CertificateRepository : BaseRepository<Certificate>, ICertificateRepository
{
    public CertificateRepository(LmsDbContext context) : base(context) { }

    public async Task<Certificate?> GetByCodeAsync(string code)
        => await _dbSet
            .Include(x => x.User)
            .Include(x => x.Course)
            .FirstOrDefaultAsync(x => x.Code == code);

    public async Task<Certificate?> GetByUserAndCourseAsync(Guid userId, Guid courseId)
        => await _dbSet
            .FirstOrDefaultAsync(x => x.UserId == userId && x.CourseId == courseId);

    public async Task<IEnumerable<Certificate>> GetByUserIdAsync(Guid userId)
        => await _dbSet
            .Include(x => x.Course)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.IssuedAt)
            .ToListAsync();
}
