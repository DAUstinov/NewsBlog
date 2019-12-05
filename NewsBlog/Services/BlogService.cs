using System.Data.Entity;
using NewsBlog.Models;
using NewsBlog.Repositories;

namespace NewsBlog.Services
{
    public class BlogService
    {
        private readonly UnitOfWork _unitOfWork;

        public BlogService(DbContext dbContext)
        {
            this._unitOfWork = new UnitOfWork(dbContext);
        }

        public BlogItem GetArticle(int id)
        {
            return _unitOfWork.GetRepository<BlogItem>().Get(id);
        }

        public Tag GetTag(int id)
        {
            return _unitOfWork.GetRepository<Tag>().Get(id);
        }
        public Category GetCategory(int id)
        {
            return _unitOfWork.GetRepository<Category>().Get(id);
        }

        public void AddCategory(Category category)
        {
            _unitOfWork.GetRepository<Category>().Create(category);
            _unitOfWork.SaveChanges();
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

        public void DeleteTag(int id)
        {
            _unitOfWork.GetRepository<Tag>().Delete(GetTag(id));
            _unitOfWork.SaveChanges();
        }
        public void DeleteArticle(int id)
        {
            _unitOfWork.GetRepository<BlogItem>().Delete(GetArticle(id));
            _unitOfWork.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            _unitOfWork.GetRepository<Category>().Update(category);
            _unitOfWork.SaveChanges();
        }
        
    }
}