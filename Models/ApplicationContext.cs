using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CConstsProject.Models
{
    public class ApplicationContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Family> Families { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Outgo> Outgos { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Task> Tasks { get; set; }
        DbSet<TaskManager> TaskManagers { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            :base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(u => u.Outgoes).WithOne(o => o.User);
            modelBuilder.Entity<User>().HasMany(u => u.Incomes).WithOne(i => i.User);
            modelBuilder.Entity<User>().HasOne(u => u.Family).WithMany(f => f.Users);
            modelBuilder.Entity<Item>().HasMany(i => i.Outgos).WithOne(o => o.Item);
            
        }
    }
}
