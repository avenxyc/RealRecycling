using Iesi.Collections.Generic;
using System;
using System.ComponentModel;
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
        [DisplayName("Constituent Name")]
        public virtual string ConstituentName { get; set; }
        public virtual string Type { get; set; }

        public virtual System.Collections.Generic.IEnumerable<LocatedIn> LocatedIns { get; set; }
        public virtual System.Collections.Generic.IEnumerable<ProductHasConstituent> ProductHasConstituents { get; set; }
    }
}