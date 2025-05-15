using Microsoft.EntityFrameworkCore;
using DotNetApiTemplate.Data.Entities;

namespace DotNetApiTemplate.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // 這裡添加您的實體DbSet
        // 例如: public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 這裡可以添加實體的配置
            // 例如: modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        }
    }
} 