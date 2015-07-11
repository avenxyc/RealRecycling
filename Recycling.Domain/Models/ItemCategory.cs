using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Recycling.Domain.Models
{
    public class ItemCategory : DbEntity
    {
        [DisplayName("Constituent Name")]
        public virtual string CategoryName { get; set; }
    }
}