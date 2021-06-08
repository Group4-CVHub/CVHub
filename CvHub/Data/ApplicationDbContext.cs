using CVHub.Configuration;
using CVHub.Configurations;
using CVHub.Models;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<Education> Educations { get; set; }
        public DbSet<Work> WorkPlaces { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new CvConfiguration());

            modelBuilder
                .ApplyConfiguration(new TemplateConfiguration());

            modelBuilder
                .ApplyConfiguration(new UserConfiguration());

            modelBuilder
                .ApplyConfiguration(new EducationConfiguration());

            modelBuilder
                .ApplyConfiguration(new WorkConfiguration());
        }
    }
}