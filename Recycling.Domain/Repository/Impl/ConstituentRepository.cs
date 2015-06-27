using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Recycling.Domain.Models;
using NHibernate;
using NHibernate.Linq;

namespace Recycling.Domain.Repository.Impl
{
    public class ConstituentRepository : Repository<Constituent>, IConstituentRepository
    {
        ISession _session;
        public ConstituentRepository(ISession session)
            : base(session)
        {
            _session = session;
        }


        
    }
}