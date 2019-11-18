using System.Collections.Generic;
using NewsBlog.Models;
using NewsBlog.Repositories;

namespace NewsBlog.Services
{
    public class BlogService
    {
        private readonly UnitOfWork _unitOfWork;

        public BlogService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public void SaveArticle(BlogItem blog)
        {
            _unitOfWork.BlogItems.SaveArticle(blog);
        }
        public IEnumerable<BlogItem> GetNews()
        {
            return _unitOfWork.BlogItems.GetAll();
        }

        public BlogItem Article(int id)
        {
            return _unitOfWork.BlogItems.Get(id);
        }

        public void AddItem(BlogItem item)
        {
            _unitOfWork.BlogItems.Create(item);
            _unitOfWork.Save();
        }

        public void DeleteArticle(int id)
        {
            _unitOfWork.BlogItems.Delete(id);
            _unitOfWork.Save();
        }

        public void UpdateArticle(BlogItem item)
        {
            _unitOfWork.BlogItems.Update(item);
            _unitOfWork.Save();
        }


    }
}