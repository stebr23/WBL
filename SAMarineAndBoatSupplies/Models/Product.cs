using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SAMarineAndBoatSupplies.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "Please enter the product name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a description of the product")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter the price")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        
        [Display(Name = "Image Name")]
        public string ImagePath { get; set; }

        public int CategoryId { get; set; }
    }
}
