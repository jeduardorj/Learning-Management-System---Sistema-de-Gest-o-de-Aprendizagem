using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Persistence.Configurations;

public class ModuleConfiguration : IEntityTypeConfiguration<Module>
{
    public void Configure(EntityTypeBuilder<Module> builder)
    {
        builder.ToTable("Modules");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Order).IsRequired();

        builder.HasIndex(x => new { x.CourseId, x.Order }).HasDatabaseName("IX_Modules_CourseId_Order");

        builder.HasMany(x => x.Lessons).WithOne(x => x.Module)
            .HasForeignKey(x => x.ModuleId).OnDelete(DeleteBehavior.Cascade);
    }
}
