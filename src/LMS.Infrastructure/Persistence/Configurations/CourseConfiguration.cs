using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Persistence.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Courses");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(2000);

        builder.HasIndex(x => x.Title).HasDatabaseName("IX_Courses_Title");
        builder.HasIndex(x => x.IsActive).HasDatabaseName("IX_Courses_IsActive");

        builder.HasMany(x => x.Modules).WithOne(x => x.Course)
            .HasForeignKey(x => x.CourseId).OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Enrollments).WithOne(x => x.Course)
            .HasForeignKey(x => x.CourseId).OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Certificates).WithOne(x => x.Course)
            .HasForeignKey(x => x.CourseId).OnDelete(DeleteBehavior.Restrict);
    }
}
