using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System;
using System.Collections;

namespace Recycling.Domain.Repository
{
    public interface IRepository<TEntity>
             where TEntity : class
    {
        TEntity GetById(int id);
        void SaveOrUpdate(TEntity entity);
        void Save(TEntity entity);
        void Delete(TEntity entity);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> FindByUsername(string text);
        IQueryable<TEntity> Query { get; }
    }
}