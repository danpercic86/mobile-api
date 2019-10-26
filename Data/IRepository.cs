using System.Linq;

namespace itec_mobile_api_final.Data
{
    public interface IRepository<T>
    {    
        T Get<TKey>(TKey id);
        IQueryable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(int id);
    }
}