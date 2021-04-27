using CVHub.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CVHub.Configurations
{
    public class TemplateConfiguration : IEntityTypeConfiguration<Template>
    {
        public void Configure(EntityTypeBuilder<Template> modelBuilder)
        {
            modelBuilder.HasKey(t => t.TemplateId);
            modelBuilder.Property(t => t.TemplateId)
                        .ValueGeneratedOnAdd();
            modelBuilder.HasMany(t => t.Cvs)
                        .WithOne(c => c.Template);
        }
    }
}