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
            modelBuilder.Entity<Outgo>().HasOne(o => o.User).WithMany(u => u.Outgoes).HasForeignKey(o=>o.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Income>().HasOne(i => i.User).WithMany(u => u.Incomes).HasForeignKey(o=>o.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().HasOne(u => u.Family).WithMany(f => f.Users).HasForeignKey(u=>u.FamilyId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Outgo>().HasOne(o => o.Item).WithMany(i => i.Outgos).HasForeignKey(o=>o.ItemId).OnDelete(DeleteBehavior.Cascade);
            
        }
    }
}
