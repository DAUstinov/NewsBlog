using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using NewsBlog.Interfaces;
using NewsBlog.Models;

namespace NewsBlog.Repositories
{
    public class Repository<T> : IRepository<T> where T:class
    {
        private readonly BlogContext _dbContext;
        private readonly DbSet<T> _set;

        public Repository(BlogContext dbContext)
        {
            _dbContext = dbContext;
        
            _set = _dbContext.Set<T>();
        }
        
        public Task<List<T>> GetAll()
        {
            return GetQuery().ToListAsync();
        }
        
        public T Get(int id)
        {
            return _set.Find(id);
        }
        
        public void Create(T entity)
        {
            _set.Add(entity);
        }
        
        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
        
        public void Delete(T entity)
        {
            _set.Remove(entity);
        }
        
        
        protected virtual IQueryable<T> GetQuery()
        {
            return _set;
        }

    //        private readonly BlogContext _db;
    //
    //        public BlogRepository(BlogContext context)
    //        {
    //            this._db = context;
    //        }
    //        
    //        public IEnumerable<BlogItem> GetAll()
    //        {
    //            return _db.BlogItems;
    //        }
    //
    //        public BlogItem Get(int id)
    //        {
    //            return _db.BlogItems.Find(id);
    //        }
    //
    //        public void Create(BlogItem blogItem)
    //        {
    //            _db.BlogItems.Add(blogItem);
    //        }
    //
    //        public void Update(BlogItem blogItem)
    //        {
    //            _db.Entry(blogItem).State = EntityState.Modified;
    //        }
    //
    //        public void Delete(int id)
    //        {
    //            BlogItem blogItem = _db.BlogItems.Find(id);
    //            if (blogItem != null) 
    //                _db.BlogItems.Remove(blogItem);
    //            _db.SaveChanges();
    //        }
    //    }
    //
    //    public class TagRepository : IRepository<Tag>
    //    {
    //        private readonly BlogContext _db;
    //
    //        public TagRepository(BlogContext blog)
    //        {
    //            this._db = blog;
    //        }
    //
    //        public IEnumerable<Tag> GetAll()
    //        {
    //            return _db.Tags;
    //        }
    //        
    //        public Tag Get(int id)
    //        {
    //            return _db.Tags.Find(id);
    //        }
    //
    //        public void Create(Tag tag)
    //        {
    //            _db.Tags.Add(tag);
    //        }
    //
    //        public void Update(Tag tag)
    //        {
    //            _db.Entry(tag).State = EntityState.Modified;
    //        }
    //
    //        public void Delete(int id)
    //        {
    //            Tag tag = _db.Tags.Find(id);
    //            if (tag != null)
    //                _db.Tags.Remove(tag);
    //            _db.SaveChanges();
    //        }
    //
    }
}