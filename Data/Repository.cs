using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using itec_mobile_api_final.Entities;
using Microsoft.EntityFrameworkCore;

namespace itec_mobile_api_final.Data
{
    public class Repository<T> : IRepository<T> where T: Entity
    {
        protected readonly DbContext Context;
        protected DbSet<T> DbSet;
        private IQueryable<T> _queryable;
        public IQueryable<T> Queryable => _queryable;

        public Repository(DbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
            _queryable = DbSet;
        }
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
            DbSet.Add(entity);
            Context.SaveChanges();
        }

        public void Update(T entity)
        {
            Save();
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
            Context.SaveChanges();
        }

        public void Delete(int id)
        {
            this.Delete(DbSet.Find(id));
        }
        public void Save()
        {
            Context.SaveChanges();
        }
    }
}