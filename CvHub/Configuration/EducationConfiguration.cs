using CVHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CVHub.Configuration
{
    public class EducationConfiguration : IEntityTypeConfiguration<Education>
    {
        public void Configure(EntityTypeBuilder<Education> modelBuilder)
        {
            modelBuilder.HasKey(e => e.EducationId);
            modelBuilder.Property(e => e.EducationId)
                        .ValueGeneratedOnAdd();
            modelBuilder.HasOne(e => e.Cv)
                        .WithMany(e => e.Educations)
                        .HasForeignKey(e => e.CvId)
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
