
using CvHub.Configuration;
using CvHub.Models;
using CVHub.Configurations;
using CVHub.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVHub.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Cv> Cvs { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new CvConfiguration());

            modelBuilder
                .ApplyConfiguration(new TemplateConfiguration());

            modelBuilder
                .ApplyConfiguration(new UserConfiguration());
        }
    }
}