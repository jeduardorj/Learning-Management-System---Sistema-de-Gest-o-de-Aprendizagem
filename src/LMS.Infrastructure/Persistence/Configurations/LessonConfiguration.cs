using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Persistence.Configurations;

public class LessonConfiguration : BaseEntityConfiguration<Lesson>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Lesson> builder)
    {
        builder.ToTable("Lessons");

        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Content).IsRequired().HasMaxLength(5000);
        builder.Property(x => x.Order).IsRequired();

        builder.HasIndex(x => new { x.ModuleId, x.Order }).HasDatabaseName("IX_Lessons_ModuleId_Order");
    }
}
