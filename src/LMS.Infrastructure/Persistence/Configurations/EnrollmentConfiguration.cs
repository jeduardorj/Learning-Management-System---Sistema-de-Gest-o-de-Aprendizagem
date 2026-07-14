using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Persistence.Configurations;

public class EnrollmentConfiguration : BaseEntityConfiguration<Enrollment>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Enrollment> builder)
    {
        builder.ToTable("Enrollments");

        builder.HasIndex(x => new { x.UserId, x.CourseId })
            .IsUnique().HasDatabaseName("IX_Enrollments_UserId_CourseId");

        builder.HasMany(x => x.Progresses).WithOne(x => x.Enrollment)
            .HasForeignKey(x => x.EnrollmentId).OnDelete(DeleteBehavior.Cascade);
    }
}
