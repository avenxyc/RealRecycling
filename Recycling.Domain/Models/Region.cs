using Iesi.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Recycling.Domain.Models
{
    public class Region : DbEntity
    {
        public virtual string RegionName { get; set; }

        public virtual ISet<LocatedIn> LocatedIns { get; set; }
    }
}