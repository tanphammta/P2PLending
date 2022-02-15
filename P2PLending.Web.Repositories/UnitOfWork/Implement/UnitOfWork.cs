using P2PLending.Web.DAL.DataContext;
using P2PLending.Web.Repositories.UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Repositories.UnitOfWork.Implement
{
    public class UnitOfWork: IUnitOfWork
    {
        public P2PLendingDbContext Context { get; }

        public UnitOfWork(P2PLendingDbContext context)
        {
            Context = context;
        }
        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public void BeginTransaction()
        {
            if(Context.Database.CurrentTransaction != null)
            {
                return;
            }
            Context.Database.BeginTransaction();
        }

        public void Commit()
        {
            if (Context.Database.CurrentTransaction == null)
            {
                return;
            }
            Context.Database.CommitTransaction();
        }

        public void Rollback()
        {
            if (Context.Database.CurrentTransaction == null)
            {
                return;
            }
            Context.Database.RollbackTransaction();
        }
    }
}
