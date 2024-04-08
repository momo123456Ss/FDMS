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
    [Migration("20240406045357_edit-flight-document-0604-1.2")]
    partial class editflightdocument060412
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

            modelBuilder.Entity("FDMS.Entity.AccountSession", b =>
                {
                    b.Property<int>("AccountSessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountSessionId"), 1L, 1);

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AccountEmail")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("ExpirationTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("IssuedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("NotBefore")
                        .HasColumnType("datetime2");

                    b.HasKey("AccountSessionId");

                    b.HasIndex("AccountEmail");

                    b.ToTable("AccountSession");
                });

            modelBuilder.Entity("FDMS.Entity.DocumentType", b =>
                {
                    b.Property<int>("DocumentTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DocumentTypeId"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Creator")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("TotalGroups")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("DocumentTypeId");

                    b.HasIndex("Creator");

                    b.ToTable("DocumentType");
                });

            modelBuilder.Entity("FDMS.Entity.DocumentType_Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("DocumentTypeId")
                        .HasColumnType("int");

                    b.Property<int>("GroupPermissionId")
                        .HasColumnType("int");

                    b.Property<bool>("NoPermission")
                        .HasColumnType("bit");

                    b.Property<bool>("ReadAndModify")
                        .HasColumnType("bit");

                    b.Property<bool>("ReadOnly")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("DocumentTypeId");

                    b.HasIndex("GroupPermissionId");

                    b.ToTable("DocumentType_Permission");
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

            modelBuilder.Entity("FDMS.Entity.Flight", b =>
                {
                    b.Property<int>("FlightId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FlightId"), 1L, 1);

                    b.Property<string>("AccountConfirm")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("ArrivalDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DepartureDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FlightCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool?>("IsConfirm")
                        .HasColumnType("bit");

                    b.Property<string>("PointOfLoading")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PointOfUnLoading")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Route")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Signature")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("TotalDocuments")
                        .HasColumnType("int");

                    b.HasKey("FlightId");

                    b.HasIndex("AccountConfirm");

                    b.ToTable("Flight");
                });

            modelBuilder.Entity("FDMS.Entity.Flight_Account", b =>
                {
                    b.Property<int>("Flight_AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Flight_AccountId"), 1L, 1);

                    b.Property<string>("AccountEmail")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("FlightId")
                        .HasColumnType("int");

                    b.HasKey("Flight_AccountId");

                    b.HasIndex("AccountEmail");

                    b.HasIndex("FlightId");

                    b.ToTable("Flight_Account");
                });

            modelBuilder.Entity("FDMS.Entity.FlightDocument", b =>
                {
                    b.Property<int>("FlightDocumentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FlightDocumentId"), 1L, 1);

                    b.Property<string>("Creator")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("DocumentTypeId")
                        .HasColumnType("int");

                    b.Property<string>("FileName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FileSize")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("FileType")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("FileUrl")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FileViewUrl")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("FlightDocumentIdFK")
                        .HasColumnType("int");

                    b.Property<int>("FlightId")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.Property<int>("VersionPatch")
                        .HasColumnType("int");

                    b.HasKey("FlightDocumentId");

                    b.HasIndex("Creator");

                    b.HasIndex("DocumentTypeId");

                    b.HasIndex("FlightDocumentIdFK");

                    b.HasIndex("FlightId");

                    b.ToTable("FlightDocument");
                });

            modelBuilder.Entity("FDMS.Entity.FlightDocument_GroupPermission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("FlightDocumentId")
                        .HasColumnType("int");

                    b.Property<int>("GroupPermissionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FlightDocumentId");

                    b.HasIndex("GroupPermissionId");

                    b.ToTable("FlightDocument_GroupPermissions");
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

            modelBuilder.Entity("FDMS.Entity.SystemNofication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("SystemNofication");
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

            modelBuilder.Entity("FDMS.Entity.AccountSession", b =>
                {
                    b.HasOne("FDMS.Entity.Account", "AccountNavigation")
                        .WithMany("AccountSessions")
                        .HasForeignKey("AccountEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountNavigation");
                });

            modelBuilder.Entity("FDMS.Entity.DocumentType", b =>
                {
                    b.HasOne("FDMS.Entity.Account", "AccountNavigation")
                        .WithMany("DocumentTypes")
                        .HasForeignKey("Creator")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountNavigation");
                });

            modelBuilder.Entity("FDMS.Entity.DocumentType_Permission", b =>
                {
                    b.HasOne("FDMS.Entity.DocumentType", "DocumentTypeNavigation")
                        .WithMany("DocumentType_Permissions")
                        .HasForeignKey("DocumentTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FDMS.Entity.GroupPermission", "GroupPermissionNavigation")
                        .WithMany("DocumentType_Permissions")
                        .HasForeignKey("GroupPermissionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("DocumentTypeNavigation");

                    b.Navigation("GroupPermissionNavigation");
                });

            modelBuilder.Entity("FDMS.Entity.Flight", b =>
                {
                    b.HasOne("FDMS.Entity.Account", "AccountNavigation")
                        .WithMany()
                        .HasForeignKey("AccountConfirm");

                    b.Navigation("AccountNavigation");
                });

            modelBuilder.Entity("FDMS.Entity.Flight_Account", b =>
                {
                    b.HasOne("FDMS.Entity.Account", "AccountNavigation")
                        .WithMany("Flight_Accounts")
                        .HasForeignKey("AccountEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FDMS.Entity.Flight", "FlightNavigation")
                        .WithMany("Flight_Accounts")
                        .HasForeignKey("FlightId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountNavigation");

                    b.Navigation("FlightNavigation");
                });

            modelBuilder.Entity("FDMS.Entity.FlightDocument", b =>
                {
                    b.HasOne("FDMS.Entity.Account", "AccountNavigation")
                        .WithMany("FlightDocuments")
                        .HasForeignKey("Creator")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FDMS.Entity.DocumentType", "DocumentTypeNavigation")
                        .WithMany("FlightDocuments")
                        .HasForeignKey("DocumentTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FDMS.Entity.FlightDocument", "FlightDocumentNavigation")
                        .WithMany("FlightDocuments")
                        .HasForeignKey("FlightDocumentIdFK")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("FDMS.Entity.Flight", "FlightNavigation")
                        .WithMany("FlightDocuments")
                        .HasForeignKey("FlightId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountNavigation");

                    b.Navigation("DocumentTypeNavigation");

                    b.Navigation("FlightDocumentNavigation");

                    b.Navigation("FlightNavigation");
                });

            modelBuilder.Entity("FDMS.Entity.FlightDocument_GroupPermission", b =>
                {
                    b.HasOne("FDMS.Entity.FlightDocument", "FlightDocumentNavigation")
                        .WithMany("FlightDocument_GroupPermissions")
                        .HasForeignKey("FlightDocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FDMS.Entity.GroupPermission", "GroupPermissionNavigation")
                        .WithMany("FlightDocument_GroupPermissions")
                        .HasForeignKey("GroupPermissionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("FlightDocumentNavigation");

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
                    b.Navigation("AccountSessions");

                    b.Navigation("Account_GroupPermissions");

                    b.Navigation("DocumentTypes");

                    b.Navigation("FlightDocuments");

                    b.Navigation("Flight_Accounts");
                });

            modelBuilder.Entity("FDMS.Entity.DocumentType", b =>
                {
                    b.Navigation("DocumentType_Permissions");

                    b.Navigation("FlightDocuments");
                });

            modelBuilder.Entity("FDMS.Entity.Flight", b =>
                {
                    b.Navigation("FlightDocuments");

                    b.Navigation("Flight_Accounts");
                });

            modelBuilder.Entity("FDMS.Entity.FlightDocument", b =>
                {
                    b.Navigation("FlightDocument_GroupPermissions");

                    b.Navigation("FlightDocuments");
                });

            modelBuilder.Entity("FDMS.Entity.GroupPermission", b =>
                {
                    b.Navigation("Account_GroupPermissions");

                    b.Navigation("DocumentType_Permissions");

                    b.Navigation("FlightDocument_GroupPermissions");
                });

            modelBuilder.Entity("FDMS.Entity.Role", b =>
                {
                    b.Navigation("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
