using System.Linq;
using itec_mobile_api_final.Entities;
using Microsoft.EntityFrameworkCore;

namespace itec_mobile_api_final.Data
{
    public class Repository<T> :IRepository<T> where T: Entity
    {
        protected readonly DbContext Context;
        protected DbSet<T> DbSet;

//        public Repository(<T> context)
//        {
//            Context = context;
//            DbSet = Context.Set<T>();
//        }

        public T Get<TKey>(TKey id)
        {
            return DbSet.Find(id);
        }

        public IQueryable<T> GetAll()
        {
            return DbSet;
        }

        public void Add(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            Save();
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public void Delete(int id)
        {
            DbSet.Remove(DbSet.Find(id));
        }
        private void Save()
        {
            Context.SaveChanges();
        }
    }
}