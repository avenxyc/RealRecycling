using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Recycling.Domain.Models;
using NHibernate;
using NHibernate.Linq;

namespace Recycling.Domain.Repository.Impl
{
    public class CatetoryRepository : Repository<ItemCategory>, ICatetoryRepository
    {
        ISession _session;
        public CatetoryRepository(ISession session)
            : base(session)
        {
            _session = session;
        }


        
    }
}