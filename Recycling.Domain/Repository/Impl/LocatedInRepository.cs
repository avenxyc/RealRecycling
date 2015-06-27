using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Recycling.Domain.Models;
using NHibernate;
using NHibernate.Linq;

namespace Recycling.Domain.Repository.Impl
{
    public class LocatedInRepository : Repository<LocatedIn>, ILocatedInRepository
    {
        ISession _session;
        public LocatedInRepository(ISession session)
            : base(session)
        {
            _session = session;
        }


        
    }
}