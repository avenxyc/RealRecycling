using Iesi.Collections.Generic;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Recycling.Domain.Models
{
    public class Region : DbEntity
    {
        [DisplayName("Region Name")]
        public virtual string RegionName { get; set; }

        public virtual System.Collections.Generic.IEnumerable<LocatedIn> LocatedIns { get; set; }
    }
}