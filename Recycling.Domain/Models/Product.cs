using Iesi.Collections.Generic;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Recycling.Domain.Models
{
    public class Product : DbEntity
    {
        public Product()
        {
            ProductHasConstituents = new HashedSet<ProductHasConstituent> { };
        }

        [Required]
        [RegularExpression("^8[0-9]{11}([0-9]{2})?$", ErrorMessage=("Your UPC is not valid, please enter a valid UPC"))]
        [StringLength(13, ErrorMessage = "The UPC code should be 13 digits.")]
        public virtual string UPC { get; set; }

        [Required]
        [DisplayName("Product Name")]
        [StringLength(30, ErrorMessage = "The Product Name is limited under 30 charaters.")]
        public virtual string ProductName { get; set; }

        [Required]
        [DisplayName("Company Name")]
        [StringLength(30, ErrorMessage = "The Company Name is limited under 30 charaters.")]
        public virtual string CompanyName { get; set; }

        [DisplayName("Parent Company")]
        [StringLength(30, ErrorMessage = "The Parent Company Name is limited under 30 charaters.")]
        public virtual string ParentCompany { get; set; }

        [Required]
        [DisplayName("Weight(g)/Volumn(L)")]
        public virtual double Weight { get; set; }

        [Required]
        [DisplayName("Total Weight")]
        public virtual double TotalWeight { get; set; }

        [DisplayName("Number of Constituents")]
        public virtual int NumberOfConstituent { get; set; }

        [DisplayName("Last Updated")]
        public virtual DateTime LastUpdated { get; set; }

        public virtual string ImagePath { get; set; }

        public virtual string Author { get; set; }

        public virtual ItemCategory Category { get; set; }

        public virtual System.Collections.Generic.IEnumerable<ProductHasConstituent> ProductHasConstituents { get; set; }
    }
}

