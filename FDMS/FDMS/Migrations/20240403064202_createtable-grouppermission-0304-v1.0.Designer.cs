﻿// <auto-generated />
using System;
using FDMS.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FDMS.Migrations
{
    [DbContext(typeof(FDMSContext))]
    [Migration("20240403064202_createtable-grouppermission-0304-v1.0")]
    partial class createtablegrouppermission0304v10
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.28")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("FDMS.Entity.Account", b =>
                {
                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("IsActived")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Email");

                    b.HasIndex("RoleId");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("FDMS.Entity.Account_GroupPermission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AccountEmail")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("GroupPermissionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountEmail");

                    b.HasIndex("GroupPermissionId");

                    b.ToTable("Account_GroupPermission");
                });

            modelBuilder.Entity("FDMS.Entity.FDHistory", b =>
                {
                    b.Property<int>("FDHistoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FDHistoryId"), 1L, 1);

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.HasKey("FDHistoryId");

                    b.ToTable("FDHistory");
                });

            modelBuilder.Entity("FDMS.Entity.General", b =>
                {
                    b.Property<string>("GeneralId")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<bool>("CAPTCHA")
                        .HasColumnType("bit");

                    b.Property<string>("Logo")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Theme")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("GeneralId");

                    b.ToTable("General");
                });

            modelBuilder.Entity("FDMS.Entity.GroupPermission", b =>
                {
                    b.Property<int>("GroupPermissionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GroupPermissionId"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Creator")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("TotalMembers")
                        .HasColumnType("int");

                    b.HasKey("GroupPermissionId");

                    b.HasIndex("Creator");

                    b.ToTable("GroupPermission");
                });

            modelBuilder.Entity("FDMS.Entity.Role", b =>
                {
                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("RoleId");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("FDMS.Entity.Account", b =>
                {
                    b.HasOne("FDMS.Entity.Role", "RoleNavigation")
                        .WithMany("Accounts")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RoleNavigation");
                });

            modelBuilder.Entity("FDMS.Entity.Account_GroupPermission", b =>
                {
                    b.HasOne("FDMS.Entity.Account", "AccountNavigation")
                        .WithMany("Account_GroupPermissions")
                        .HasForeignKey("AccountEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FDMS.Entity.GroupPermission", "GroupPermissionNavigation")
                        .WithMany("Account_GroupPermissions")
                        .HasForeignKey("GroupPermissionId")
                        .IsRequired();

                    b.Navigation("AccountNavigation");

                    b.Navigation("GroupPermissionNavigation");
                });

            modelBuilder.Entity("FDMS.Entity.GroupPermission", b =>
                {
                    b.HasOne("FDMS.Entity.Account", "AccountNavigation")
                        .WithMany()
                        .HasForeignKey("Creator")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountNavigation");
                });

            modelBuilder.Entity("FDMS.Entity.Account", b =>
                {
                    b.Navigation("Account_GroupPermissions");
                });

            modelBuilder.Entity("FDMS.Entity.GroupPermission", b =>
                {
                    b.Navigation("Account_GroupPermissions");
                });

            modelBuilder.Entity("FDMS.Entity.Role", b =>
                {
                    b.Navigation("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
