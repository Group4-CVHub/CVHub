using CVHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CVHub.Configurations
{
    public class CvConfiguration : IEntityTypeConfiguration<Cv>
    {
        public void Configure(EntityTypeBuilder<Cv> modelBuilder)
        {
            modelBuilder.HasKey(c => c.CvId);
            modelBuilder.Property(c => c.CvId)
                        .ValueGeneratedOnAdd();
            modelBuilder.HasOne(c => c.Template)
                        .WithMany(t => t.Cvs)
                        .HasForeignKey(c => c.TemplateId)
                        .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.HasOne(c => c.User)
                        .WithMany(u => u.Cvs)
                        .HasForeignKey(c => c.UserId)
                        .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.HasMany(c => c.Educations)
                        .WithOne(c => c.Cv)
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
