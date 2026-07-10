using LMS.Domain.Entities;
using LMS.Domain.Enums;
using LMS.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Persistence.Seeds;

public static class AdminUserSeed
{
    public static async Task SeedAsync(LmsDbContext context)
    {
        if (await context.Users.IgnoreQueryFilters().AnyAsync(x => x.Role == UserRole.Admin))
            return;

        var admin = new User(
            name: "Administrador",
            email: "admin@lms.com",
            passwordHash: "seed-hash-substituir-na-sprint-2",
            role: UserRole.Admin
        );

        await context.Users.AddAsync(admin);
        await context.SaveChangesAsync();
    }
}
