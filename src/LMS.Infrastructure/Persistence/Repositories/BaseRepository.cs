using LMS.Domain.Entities;
using LMS.Domain.Interfaces.Repositories;
using LMS.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Persistence.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly LmsDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(LmsDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id)
        => await _dbSet.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<T>> GetAllAsync()
        => await _dbSet.ToListAsync();

    public async Task AddAsync(T entity)
        => await _dbSet.AddAsync(entity);

    public void Update(T entity)
        => _dbSet.Update(entity);

    public void Delete(T entity)
        => entity.MarkAsDeleted();
}
