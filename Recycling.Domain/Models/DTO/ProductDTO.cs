using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Recycling.Domain.Models
{
    public class ProductDTO
    {
        public Product product { get; set; }
        public List<Constituent> constituents { get; set; }
        public List<ProductHasConstituent> pHasCs { get; set; }
        public Region region { get; set; }
        public List<LocatedIn> recyclabilities { get; set; }
    }
}