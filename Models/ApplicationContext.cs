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
            modelBuilder.Entity<User>().HasOne(u => u.Family).WithMany(f => f.Users).HasForeignKey(u=>u.FamilyId).OnDelete(DeleteBehavior.SetNull);
           // modelBuilder.Entity<Transaction>().Property(x => x.Money).HasColumnType("decimal(8,2");
            //modelBuilder.Query<Item>().Property(x => x.AmountOfMoney).HasColumnType("decimal(8,2");
            // modelBuilder.Query<User>().Property(x => x.Money).HasColumnType("decimal(8,2)");
            base.OnModelCreating(modelBuilder);
        }
    }
}
