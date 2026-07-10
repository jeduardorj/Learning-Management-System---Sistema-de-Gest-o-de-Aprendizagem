using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Persistence.Configurations;

public class ProgressConfiguration : IEntityTypeConfiguration<Progress>
{
    public void Configure(EntityTypeBuilder<Progress> builder)
    {
        builder.ToTable("Progresses");
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.EnrollmentId, x.LessonId })
            .IsUnique().HasDatabaseName("IX_Progresses_EnrollmentId_LessonId");

        builder.HasOne(x => x.Lesson).WithMany(x => x.Progresses)
            .HasForeignKey(x => x.LessonId).OnDelete(DeleteBehavior.Restrict);
    }
}
