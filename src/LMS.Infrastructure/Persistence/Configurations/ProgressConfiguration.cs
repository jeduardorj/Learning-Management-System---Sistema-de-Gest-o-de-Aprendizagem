using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Persistence.Configurations;

public class ProgressConfiguration : BaseEntityConfiguration<Progress>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Progress> builder)
    {
        builder.ToTable("Progresses");

        builder.HasIndex(x => new { x.EnrollmentId, x.LessonId })
            .IsUnique().HasDatabaseName("IX_Progresses_EnrollmentId_LessonId");

        builder.HasOne(x => x.Lesson).WithMany(x => x.Progresses)
            .HasForeignKey(x => x.LessonId).OnDelete(DeleteBehavior.Restrict);
    }
}
