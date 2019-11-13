
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
}