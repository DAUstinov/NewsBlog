using System.Data.Entity;

namespace NewsBlog.Models
{
    public class BlogContext : DbContext
    {
        public BlogContext() : base("IdentityDb") { }
        public DbSet<BlogItem> BlogItems { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}