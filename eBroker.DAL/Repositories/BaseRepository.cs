using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace eBroker.DAL.Repositories
{
    public class BaseRepository<T> : IDisposable, IBaseRepository<T> where T : class
    {
        private readonly EBrokerContext _dbContext;
        private DbSet<T> _dbSet;

        public BaseRepository(EBrokerContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public virtual T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Insert(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Update(T entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query.ToList();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
