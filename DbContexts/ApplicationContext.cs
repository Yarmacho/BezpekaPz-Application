using Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Application.DbContexts
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<File> Files { get; set; }

        public ApplicationContext([NotNull] DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(builder => 
            {
                builder.HasKey("Login");
                builder.HasMany(u => u.Files).WithOne(f => f.User);
            });

            modelBuilder.Entity<File>().Property(f => f.Id).UseIdentityColumn();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .LogTo(m => Debug.WriteLine(m));
        }
    }
}
