﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NameBandit.Data;

namespace NameBandit.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("NameBandit.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("NameCategories");
                });

            modelBuilder.Entity("NameBandit.Models.Name", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Active")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<bool>("Female")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Male")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("NameComboId")
                        .HasColumnType("int");

                    b.Property<bool>("Prefix")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Suffix")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Text")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Vibration")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("NameComboId");

                    b.ToTable("Names");
                });

            modelBuilder.Entity("NameBandit.Models.NameCombo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("NameCombinations");
                });

            modelBuilder.Entity("NameBandit.Models.SyncLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Log")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("NameSyncLogs");
                });

            modelBuilder.Entity("NameBandit.Models.Name", b =>
                {
                    b.HasOne("NameBandit.Models.Category", null)
                        .WithMany("Names")
                        .HasForeignKey("CategoryId");

                    b.HasOne("NameBandit.Models.NameCombo", "NameCombo")
                        .WithMany("Names")
                        .HasForeignKey("NameComboId");
                });

            modelBuilder.Entity("NameBandit.Models.NameCombo", b =>
                {
                    b.HasOne("NameBandit.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");
                });
#pragma warning restore 612, 618
        }
    }
}
