using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Recycling.Domain.Models
{
    public class LocatedInDTO
    {
        public Constituent Constituent { get; set; }
        public List<LocatedIn> LocatedIns { get; set; }
        
    }
}