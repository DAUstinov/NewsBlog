using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using NewsBlog.Interfaces;
using NewsBlog.Models;

namespace NewsBlog.Repositories
{
    public class Repository<T> : IRepository<T> where T:class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _set;
        public Repository(DbContext dbContext)
        {
            this._dbContext = dbContext;
        
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

    }
}