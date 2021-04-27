using CVHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                        .HasForeignKey(c => c.TemplateId);
            modelBuilder.HasOne(c => c.User)
                        .WithMany(u => u.Cvs)
                        .HasForeignKey(c => c.UserId);
        }
    }
}
