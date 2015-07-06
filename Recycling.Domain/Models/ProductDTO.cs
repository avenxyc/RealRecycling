using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Recycling.Domain.Models
{
    public class ProductDTO
    {
        public Product product { get; set; }
        public IEnumerable<Constituent> constituents { get; set; }
        public Region region { get; set; }
    }
}