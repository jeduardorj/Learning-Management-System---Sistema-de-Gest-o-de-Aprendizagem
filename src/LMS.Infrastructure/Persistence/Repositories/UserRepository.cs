using LMS.Domain.Entities;
using LMS.Domain.Interfaces.Repositories;
using LMS.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Persistence.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(LmsDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email)
        => await _dbSet.FirstOrDefaultAsync(x => x.Email == email);

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        => await _dbSet.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

    public async Task<bool> ExistsByEmailAsync(string email)
        => await _dbSet.AnyAsync(x => x.Email == email);
}
