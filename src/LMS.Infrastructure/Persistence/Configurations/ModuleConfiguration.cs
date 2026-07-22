using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Persistence.Configurations;

public class ModuleConfiguration : BaseEntityConfiguration<Module>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Module> builder)
    {
        builder.ToTable("Modules");

        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Order).IsRequired();

        builder.HasIndex(x => new { x.CourseId, x.Order }).HasDatabaseName("IX_Modules_CourseId_Order");

        builder.HasMany(x => x.Lessons).WithOne(x => x.Module)
            .HasForeignKey(x => x.ModuleId).OnDelete(DeleteBehavior.Cascade);
    }
}
