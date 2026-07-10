using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Persistence.Configurations;

public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
{
    public void Configure(EntityTypeBuilder<Certificate> builder)
    {
        builder.ToTable("Certificates");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code).IsRequired().HasMaxLength(100);

        builder.HasIndex(x => new { x.UserId, x.CourseId })
            .IsUnique().HasDatabaseName("IX_Certificates_UserId_CourseId");

        builder.HasIndex(x => x.Code)
            .IsUnique().HasDatabaseName("IX_Certificates_Code");
    }
}
