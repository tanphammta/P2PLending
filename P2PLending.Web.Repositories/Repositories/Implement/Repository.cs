using Microsoft.EntityFrameworkCore;
using P2PLending.Web.DAL.DataContext;
using P2PLending.Web.Repositories.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P2PLending.Web.Repositories.Repositories.Implement
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public P2PLendingDbContext _context;
        private DbSet<T> _dbSet;
        protected DbSet<T> DbSet
        {
            get
            {
                if (_dbSet == null)
                {
                    _dbSet = _context.Set<T>();
                }

                return _dbSet;
            }
        }
        public Repository(P2PLendingDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public T Add(T entity)
        {
            _dbSet.Add(entity);
            return entity;
        }

        public bool Delete(T entity)
        {
            _dbSet.Remove(entity); 
            return true;
        }

        public T Get(int id)
        {
            return _dbSet.Find(id);
        }

        public IQueryable<T> Queryable() => _dbSet.AsQueryable();

        public IQueryable<T> AsNoTracking() => _dbSet.AsNoTracking();

        public T Update(T entity)
        {
            _dbSet.Update(entity);
            return entity;
        }

        public IList<T> AddRange(IList<T> entities)
        {
            _dbSet.AddRange(entities);
            return entities;
        }

        public IList<T> UpdateRange(IList<T> entities)
        {
            _dbSet.UpdateRange(entities);
            return entities;
        }

        public bool DeleteRange(IList<T> entities)
        {
            _dbSet.RemoveRange(entities);
            return true;
        }

        public bool DeleteWhere(Func<T, bool> predicate)
        {
            var datas = _dbSet.Where(predicate);
            _dbSet.RemoveRange(datas);
            return true;
        }
    }
}
