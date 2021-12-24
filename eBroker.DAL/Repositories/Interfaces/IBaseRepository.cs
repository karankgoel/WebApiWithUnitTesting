using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace eBroker.DAL.Repositories
{
    public interface IBaseRepository<T> where T: class
    {
        T GetById(int id);
        void Insert(T entity);
        void Update(T entityToUpdate);
        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, string includeProperties = "");
    }
}
