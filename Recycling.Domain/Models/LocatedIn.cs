using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Recycling.Domain.Models
{
    public class LocatedIn : DbEntity
    {
        public virtual string ConstituentName { get; set; }
        public virtual string RegionName { get; set; }
        public virtual string Classification { get; set; }
        public virtual string Recyclability { get; set; }

        public virtual Constituent Constituent { get; set; }
        public virtual Region Region { get; set; }
    }
}