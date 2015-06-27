using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Recycling.Domain.Models
{
    public class ItemCategory : DbEntity
    {
        public virtual string CategoryName { get; set; }
    }
}