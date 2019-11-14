using System.Collections.Generic;
using NewsBlog.Models;
using NewsBlog.Repositories;

namespace NewsBlog.Services
{
    public class BlogService
    {
        UnitOfWork _unitOfWork;

        public BlogService()
        {
            _unitOfWork = new UnitOfWork();
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


    }
}