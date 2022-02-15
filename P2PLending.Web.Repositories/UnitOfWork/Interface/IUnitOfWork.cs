using P2PLending.Web.DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Repositories.UnitOfWork.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        P2PLendingDbContext Context { get; }

        void SaveChanges();
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
