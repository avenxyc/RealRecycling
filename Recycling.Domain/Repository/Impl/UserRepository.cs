using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Recycling.Domain.Models;
using NHibernate;
using NHibernate.Linq;

namespace Recycling.Domain.Repository.Impl
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        ISession _session;
        public UserRepository(ISession session)
            : base(session)
        {
            _session = session;
        }

        public override IQueryable<User> FindByUsername(string text)
        {
            return _session.Query<User>().Where(t => t.Username == text);
        }

        
    }
}