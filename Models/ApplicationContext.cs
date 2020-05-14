using CCostsProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FileInfo = CCostsProject.Models.FileInfo;

namespace CConstsProject.Models
{
    public class ApplicationContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Family> Families { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Plan> Goals { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<FileInfo> FileInfos { get; set; }
        public DbSet<Invite> Invites { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            :base(options)
        {
            Database.EnsureCreated();
        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<Outgo>().HasOne(o => o.User).WithMany(u => u.Outgoes).HasForeignKey(o=>o.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Income>().HasOne(i => i.User).WithMany(u => u.Incomes).HasForeignKey(o=>o.UserId).OnDelete(DeleteBehavior.Cascade);*/
            modelBuilder.Entity<User>().HasOne(u => u.Family).WithMany(f => f.Users).HasForeignKey(u=>u.FamilyId).OnDelete(DeleteBehavior.SetNull);
            
            /*
            modelBuilder.Entity<Outgo>().HasOne(o => o.Item).WithMany(i => i.Outgos).HasForeignKey(o=>o.ItemId).OnDelete(DeleteBehavior.Cascade);
            */
            //modelBuilder.Entity<Currency>().HasMany(c => c.Users).WithOne(u => u.Currency).HasForeignKey(u => u.CurrencyId);
            //modelBuilder.Entity<Currency>().HasMany(c => c.Incomes).WithOne(i => i.Currency).HasForeignKey(i => i.CurrencyId);
            //modelBuilder.Entity<Currency>().HasMany(c => c.Outgoes).WithOne(o => o.Currency).HasForeignKey(o => o.CurrencyId);
        }
    }
}
