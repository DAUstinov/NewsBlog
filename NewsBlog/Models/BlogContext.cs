using System.Data.Entity;

namespace NewsBlog.Models
{
    public class BlogContext : DbContext
    {
        public BlogContext() : base("IdentityDb") { }
        public DbSet<BlogItem> BlogItems { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}