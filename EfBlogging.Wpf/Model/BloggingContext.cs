using System.Data.Entity;

namespace EfBlogging.Wpf.Model
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }    
    }
}
