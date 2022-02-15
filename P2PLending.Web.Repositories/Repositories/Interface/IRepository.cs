using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P2PLending.Web.Repositories.Repositories.Interface
{
    public interface IRepository<T> where T: class
    {
        IQueryable<T> Queryable();
        IQueryable<T> AsNoTracking();
        T Get(int id);
        T Add(T entity);
        T Update(T entity);
        IList<T> AddRange(IList<T> entities);
        IList<T> UpdateRange(IList<T> entities);
        bool Delete(T entity);
        bool DeleteRange(IList<T> entities);
        bool DeleteWhere(Func<T, bool> predicate);
    }
}
