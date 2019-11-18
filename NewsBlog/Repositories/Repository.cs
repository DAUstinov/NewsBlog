
using System.Collections.Generic;
using System.Data.Entity;
using NewsBlog.Interfaces;
using NewsBlog.Models;

namespace NewsBlog.Repositories
{
    public class BlogRepository : IRepository<BlogItem>
    {
        private readonly BlogContext _db;

        public BlogRepository(BlogContext context)
        {
            this._db = context;
        }

        public void SaveArticle(BlogItem blog)
        {
            if (blog.NewsId == 0)
                _db.BlogItems.Add(blog);
            else
            {
                BlogItem dbEntry = _db.BlogItems.Find(blog.NewsId);
                if (dbEntry!=null)
                {
                    dbEntry.Name = blog.Name;
                    dbEntry.Category = blog.Category;
                    dbEntry.Description = blog.Description;
                    dbEntry.ShortDescription = blog.ShortDescription;
                    dbEntry.Image = blog.Image;
                }
            }

            _db.SaveChanges();

        }

        public IEnumerable<BlogItem> GetAll()
        {
            return _db.BlogItems;
        }

        public BlogItem Get(int id)
        {
            return _db.BlogItems.Find(id);
        }

        public void Create(BlogItem blogItem)
        {
            _db.BlogItems.Add(blogItem);
        }

        public void Update(BlogItem blogItem)
        {
            _db.Entry(blogItem).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            BlogItem blogItem = _db.BlogItems.Find(id);
            if (blogItem != null) 
                _db.BlogItems.Remove(blogItem);
            _db.SaveChanges();
        }
    }

    public class TagRepository : IRepository<Tag>
    {
        private readonly BlogContext _db;

        public TagRepository(BlogContext blog)
        {
            this._db = blog;
        }

        public IEnumerable<Tag> GetAll()
        {
            return _db.Tags;
        }
        
        public Tag Get(int id)
        {
            return _db.Tags.Find(id);
        }

        public void Create(Tag tag)
        {
            _db.Tags.Add(tag);
        }

        public void Update(Tag tag)
        {
            _db.Entry(tag).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Tag tag = _db.Tags.Find(id);
            if (tag != null)
                _db.Tags.Remove(tag);
            _db.SaveChanges();
        }

    }
}