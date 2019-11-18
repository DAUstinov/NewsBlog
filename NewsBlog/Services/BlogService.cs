using System.Collections.Generic;
using System.Threading.Tasks;
using NewsBlog.Models;
using NewsBlog.Repositories;

namespace NewsBlog.Services
{
    public class BlogService
    {
        private readonly UnitOfWork _unitOfWork;

        public BlogService(BlogContext blogContext)
        {
            this._unitOfWork = new UnitOfWork(blogContext);
        }

        public Task<List<BlogItem>> GetNews()
        {
            return _unitOfWork.GetRepository<BlogItem>().GetAll();
        }

        public BlogItem GetArticle(int id)
        {
            return _unitOfWork.GetRepository<BlogItem>().Get(id);
        }

        public void AddTags(Tag tags)
        {
            _unitOfWork.GetRepository<Tag>().Create(tags);
            _unitOfWork.SaveChanges();
        }

        public void AddItem(BlogItem item)
        {
            _unitOfWork.GetRepository<BlogItem>().Create(item);
            _unitOfWork.SaveChanges();
        }

        public void DeleteArticle(int id)
        {
            _unitOfWork.GetRepository<BlogItem>().Delete(GetArticle(id));
            _unitOfWork.SaveChanges();
        }

        public void UpdateArticle(BlogItem item)
        {
            _unitOfWork.GetRepository<BlogItem>().Update(item);
            _unitOfWork.SaveChanges();
        }

    }
}