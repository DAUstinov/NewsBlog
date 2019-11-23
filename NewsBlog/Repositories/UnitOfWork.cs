using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using NewsBlog.Interfaces;
using NewsBlog.Models;


namespace NewsBlog.Repositories
{
        public class UnitOfWork : IUnitOfWork
        {
            private readonly DbContext _dbContext;

                private readonly IDictionary<Type, object> _repositories;
                private readonly IDictionary<Type, Type> _customRepositoryTypes;


                public UnitOfWork(DbContext dbContext)
                {
                    _dbContext = dbContext;

                    _repositories = new Dictionary<Type, object>();
                    _customRepositoryTypes = new Dictionary<Type, Type>();
                }


                public IRepository<T> GetRepository<T>()
                    where T : class
                {
                    if (!_repositories.TryGetValue(typeof(T), out var repository))
                    {
                        repository = _customRepositoryTypes.TryGetValue(typeof(T), out var repositoryType)
                            ? Activator.CreateInstance(repositoryType, _dbContext)
                            : new Repository<T>(_dbContext);

                        _repositories.Add(typeof(T), repository);
                    }

                    return (IRepository<T>)repository;
                }

                public void SaveChanges()
                { 
                    _dbContext.SaveChangesAsync();
                }
        

        //private BlogContext _db = new BlogContext();
        //    private BlogRepository _blogRepository;
        //    private TagRepository _tagRepository;
        //
        //    public TagRepository Tags
        //    {
        //        get
        //        {
        //            if (_tagRepository ==null)
        //                _tagRepository = new TagRepository(_db);
        //            return _tagRepository;
        //        }
        //    }
        //
        //    public BlogRepository BlogItems
        //    {
        //        get
        //        {
        //            if (_blogRepository == null)
        //                _blogRepository = new BlogRepository(_db);
        //            return _blogRepository;
        //        }
        //    }
        //
        //    public void Save()
        //    {
        //        _db.SaveChanges();
        //    }

            private bool _disposed = false;

            public virtual void Dispose(bool disposing)
            {
                if (!this._disposed)
                {
                    if (disposing)
                    {
                        _dbContext.Dispose();
                    }
                    this._disposed = true;
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

        }

}