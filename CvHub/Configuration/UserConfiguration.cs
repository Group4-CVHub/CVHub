﻿using CVHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CVHub.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> modelBuilder)
        {
            modelBuilder.HasKey(u => u.UserId);
            modelBuilder.Property(u => u.UserId)
                        .ValueGeneratedOnAdd();
            modelBuilder.HasIndex(u => u.Email)
                        .IsUnique();      
            modelBuilder.HasMany(u => u.Cvs)
                        .WithOne(c => c.User);
        }
    }
}
