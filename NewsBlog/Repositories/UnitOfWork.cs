using System;
using NewsBlog.Models;

namespace NewsBlog.Repositories
{
        public class UnitOfWork
        {

            private BlogContext _db = new BlogContext();
            private BlogRepository _blogRepository;
            private TagRepository _tagRepository;

            public TagRepository Tags
            {
                get
                {
                    if (_tagRepository ==null)
                        _tagRepository = new TagRepository(_db);
                    return _tagRepository;
                }
            }

            public BlogRepository BlogItems
            {
                get
                {
                    if (_blogRepository == null)
                        _blogRepository = new BlogRepository(_db);
                    return _blogRepository;
                }
            }

            public void Save()
            {
                _db.SaveChanges();
            }

            private bool _disposed = false;

            public virtual void Dispose(bool disposing)
            {
                if (!this._disposed)
                {
                    if (disposing)
                    {
                        _db.Dispose();
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