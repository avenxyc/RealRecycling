using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;

namespace Recycling.Domain.Services
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
    }

    public interface ISessionProvider
    {
        ISession Session { get; }
    }

    public interface IUnitOfWorkFactory
    {
        IUnitOfWork UnitOfWork { get; }
    }
}