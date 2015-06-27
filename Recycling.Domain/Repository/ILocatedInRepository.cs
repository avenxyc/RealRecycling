using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Recycling.Domain.Models;
using Recycling.Domain.Repository;

namespace Recycling.Domain.Repository
{
    public interface ILocatedInRepository : IRepository<LocatedIn>
    {
    }
}

