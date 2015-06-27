using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;

namespace Recycling.Domain.Services.Impl
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISessionProvider _session;
        private ITransaction _transaction;

        public UnitOfWork(ISessionProvider session)
        {
            _session = session;
            _transaction = _session.Session.BeginTransaction();
        }

        public void Dispose()
        {
            if (_transaction != null)
                if (_transaction.IsActive)
                    _transaction.Rollback();
        }

        public void SaveChanges()
        {
            if (_transaction == null)
                throw new InvalidOperationException("UnitOfWork has already been saved.");
            if (_transaction.IsActive)
            {
                try
                {
                    _transaction.Commit();
                    _transaction.Dispose();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            _transaction = null;
        }
    }
}