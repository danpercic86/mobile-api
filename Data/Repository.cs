using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using itec_mobile_api_final.Entities;
using Microsoft.EntityFrameworkCore;

namespace itec_mobile_api_final.Data
{
    public class Repository<T> : IRepository<T> where T : Entity
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

        public async Task<T> GetAsync<TKey>(TKey id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<IQueryable<T>> GetAllAsync()
        {
            return await new Task<IQueryable<T>>(() => DbSet);
        }

        public async Task AddAsync(T entity)
        {
            DbSet.Add(entity);
            await Save();
        }

        public async Task UpdateAsync(T entity)
        {
            DbSet.Update(entity);
            await Save();
        }

        public async Task DeleteAsync(T entity)
        {
            DbSet.Remove(entity);
            await Save();
        }

        public async Task DeleteAsync(int id)
        {
            await this.DeleteAsync(await DbSet.FindAsync(id));
        }

        public async Task Save()
        {
            await Context.SaveChangesAsync();
        }
    }
}