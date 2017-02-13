using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using EfBlogging.Uwp.Model;

namespace EfBlogging.Uwp.Migrations
{
    [DbContext(typeof(BloggingContext))]
    [Migration("20170213075658_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("EfBlogging.Wpf.Model.Blog", b =>
                {
                    b.Property<int>("BlogId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("BlogId");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("EfBlogging.Wpf.Model.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BlogId");

                    b.Property<string>("Content");

                    b.Property<string>("Title");

                    b.HasKey("PostId");

                    b.HasIndex("BlogId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("EfBlogging.Wpf.Model.Post", b =>
                {
                    b.HasOne("EfBlogging.Wpf.Model.Blog", "Blog")
                        .WithMany("Posts")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
