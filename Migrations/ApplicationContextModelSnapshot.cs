﻿// <auto-generated />
using System;
using CConstsProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CConstsProject.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CConstsProject.Models.Family", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdditionalInformation");

                    b.HasKey("Id");

                    b.ToTable("Families");
                });

            modelBuilder.Entity("CConstsProject.Models.Income", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<int?>("UserId");

                    b.Property<string>("WorkType");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Incomes");
                });

            modelBuilder.Entity("CConstsProject.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("AvarageCost");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("CConstsProject.Models.Outgo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<int?>("ItemId");

                    b.Property<double>("Money");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("UserId");

                    b.ToTable("Outgos");
                });

            modelBuilder.Entity("CConstsProject.Models.Task", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("False_rule");

                    b.Property<int?>("TaskManagerId");

                    b.Property<string>("True_rule");

                    b.HasKey("Id");

                    b.HasIndex("TaskManagerId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("CConstsProject.Models.TaskManager", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DueDate");

                    b.Property<string>("Name");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.ToTable("TaskManagers");
                });

            modelBuilder.Entity("CConstsProject.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("CashSum");

                    b.Property<int?>("FamilyId");

                    b.Property<string>("FullName");

                    b.Property<string>("Password");

                    b.Property<string>("Position");

                    b.Property<string>("UserName");

                    b.Property<string>("WelcomeString");

                    b.HasKey("Id");

                    b.HasIndex("FamilyId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CConstsProject.Models.Income", b =>
                {
                    b.HasOne("CConstsProject.Models.User", "User")
                        .WithMany("Incomes")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("CConstsProject.Models.Outgo", b =>
                {
                    b.HasOne("CConstsProject.Models.Item", "Item")
                        .WithMany("Outgos")
                        .HasForeignKey("ItemId");

                    b.HasOne("CConstsProject.Models.User", "User")
                        .WithMany("Outgoes")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("CConstsProject.Models.Task", b =>
                {
                    b.HasOne("CConstsProject.Models.TaskManager", "TaskManager")
                        .WithMany("Tasks")
                        .HasForeignKey("TaskManagerId");
                });

            modelBuilder.Entity("CConstsProject.Models.User", b =>
                {
                    b.HasOne("CConstsProject.Models.Family", "Family")
                        .WithMany("Users")
                        .HasForeignKey("FamilyId");
                });
#pragma warning restore 612, 618
        }
    }
}
