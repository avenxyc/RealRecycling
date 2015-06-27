using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Recycling.Domain.Models
{
    public class ProductView
    {
        [Key]
        [Required]
        [RegularExpression("^\\d{10,13}$", ErrorMessage = "UPC code must be a number between 10 and 13 digits.")]
        public string UPC { get; set; }

        [Required]
        [DisplayName("Product Name")]
        [StringLength(40)]
        public string ProductName { get; set; }

        [Required]
        [DisplayName("Company Name")]
        [StringLength(40)]
        public string CompanyName { get; set; }

        [DisplayName("Parent Company")]
        [StringLength(40)]
        public string ParentCompany { get; set; }

        [Required]
        public double Weight { get; set; }

        [Required]
        [DisplayName("Total Weight")]
        public double TotalWeight { get; set; }

        [Required]
        [DisplayName("Number of Constituents")]
        public uint NumberOfConstituent { get; set; }

        [Required]
        public string Category { get; set; }

        public string ImageURL { get; set; }


        [Required]
        [DisplayName("Part Weight")]
        public double PartWeight { get; set; }

        [Required]
        [DisplayName("Constituent Name")]
        public string ConstituentName { get; set; }

        [Required]
        public string Recyclability { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Region { get; set; }

    }
}