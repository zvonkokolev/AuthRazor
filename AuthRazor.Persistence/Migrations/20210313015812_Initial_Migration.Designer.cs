﻿// <auto-generated />
using AuthRazor.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AuthRazor.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210313015812_Initial_Migration")]
    partial class Initial_Migration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AuthRazor.Core.AuthUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserRole")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AuthUsers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "admin@htl.at",
                            Password = "78yFrayVwYcgAO4k1oGLqioKZDMhSToo2YvfG4MybGg=fc2dcbe11501936f9cb9ba75aad63ac1",
                            UserRole = "Administrator"
                        },
                        new
                        {
                            Id = 2,
                            Email = "user@htl.at",
                            Password = "ycNT4ybVAbOGBOVVTS+yrEQp/rfyFHE/Vh5vqb+wgg8=87548afff9f75927b93d4f4c48d9b38c",
                            UserRole = "User"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}