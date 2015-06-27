using NHibernate;
using NHibernate.Linq;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections;

namespace Recycling.Domain.Repository.Impl
{

    public class Repository<TEntity>
        : IRepository<TEntity>
        where TEntity : class
    {
        readonly ISession _session;

        public Repository(ISession session)
        {
            _session = session;
        }

        protected ISession Session { get { return _session; } }

        public TEntity GetById(int id)
        {
            return _session.Get<TEntity>(id);
        }

        public void SaveOrUpdate(TEntity entity)
        {
            _session.SaveOrUpdate(entity);
            _session.Flush();
        }

        public void Save(TEntity entity)
        {
            _session.Save(entity);
            _session.Flush();

        }

        public void Delete(TEntity entity)
        {
            _session.Delete(entity);
            _session.Flush();
        }


        public IQueryable<TEntity> GetAll()
        {
            return _session.Query<TEntity>();
        }
        public virtual IQueryable<TEntity> FindByUsername(string text)
        {
            return null;
        }

        public IQueryable<TEntity> Query { get { return _session.Query<TEntity>(); } }


    }
}
