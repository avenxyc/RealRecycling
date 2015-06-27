using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Recycling.Domain.Models
{
    public class ProductHasConstituent : DbEntity
    {
        public virtual string UPC { get; set; }

        public virtual string ConstituentName { get; set; }
        public virtual double PartWeight { get; set; }


        public virtual Product Product { get; set; }
        public virtual Constituent Constituent { get; set; }
    }
}