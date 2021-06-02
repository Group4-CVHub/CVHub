using CVHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CVHub.Configurations
{
    public class TemplateConfiguration : IEntityTypeConfiguration<Template>
    {
        public void Configure(EntityTypeBuilder<Template> modelBuilder)
        {
            modelBuilder.HasKey(t => t.TemplateId);
            modelBuilder.HasMany(t => t.Cvs)
                        .WithOne(c => c.Template)
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}