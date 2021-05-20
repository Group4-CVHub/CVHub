using CVHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CVHub.Configuration
{
    public class WorkConfiguration : IEntityTypeConfiguration<Work>
    {
        public void Configure(EntityTypeBuilder<Work> modelBuilder)
        {
            modelBuilder.HasKey(w => w.WorkId);
            modelBuilder.Property(w => w.WorkId)
                        .ValueGeneratedOnAdd();
            modelBuilder.HasOne(w => w.Cv)
                        .WithMany(w => w.WorkPlaces)
                        .HasForeignKey(w => w.CvId)
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
