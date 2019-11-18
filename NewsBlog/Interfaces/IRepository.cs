using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsBlog.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        T Get(int id);
        void Create(T item);
        void Update(T id);
        void Delete(T id);
    }
}
