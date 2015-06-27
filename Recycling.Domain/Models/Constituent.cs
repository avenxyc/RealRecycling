using Iesi.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Recycling.Domain.Models
{
    public class Constituent : DbEntity
    {
        public Constituent()
        {
            LocatedIns = new HashedSet<LocatedIn> { };
            ProductHasConstituents = new HashedSet<ProductHasConstituent> { };
        }
        public virtual string ConstituentName { get; set; }
        public virtual string Type { get; set; }

        public virtual ISet<LocatedIn> LocatedIns { get; set; }
        public virtual ISet<ProductHasConstituent> ProductHasConstituents { get; set; }
    }
}