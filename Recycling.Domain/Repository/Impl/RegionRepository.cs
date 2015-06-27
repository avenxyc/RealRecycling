using NHibernate;
using Recycling.Domain.Models;
using Recycling.Domain.Repository;
using Recycling.Domain.Repository.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Recycling.Domain.Repository.Impl
{
    public class RegionRepository : Repository<Region>, IRegionRepository
    {
        ISession _session;
        public RegionRepository(ISession session)
            : base(session)
        {
            _session = session;
        }
    }
}