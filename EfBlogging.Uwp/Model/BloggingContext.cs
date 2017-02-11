using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfBlogging.Wpf.Model;
using Microsoft.EntityFrameworkCore;

namespace EfBlogging.Uwp.Model
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 6d2da4e8-a126-4b00-ae05-dfefadaddf2d
            // 6d2da4e8-a126-4b00-ae05-dfefadaddf2d_1.0.0.0_x86__brppa21vfw7f4
            optionsBuilder.UseSqlite("Filename=Blogging.db");
        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Post>()
        //        .HasOne(p => p.Blog)
        //        .WithMany(b => b.Posts);
        //}

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Post>().HasKey(x => new { x.BlogId, x.PostId });
        //}
    }
}
