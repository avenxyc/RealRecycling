using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Recycling.Domain.Models
{
    public class ProductImage
    {
        [Key]
        [ForeignKey("Product")]
        public string UPC { get; set; }

        public string Description { get; set; }

        public System.DateTime UploadDate { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        public virtual Product Product { get; set; }
    }
}