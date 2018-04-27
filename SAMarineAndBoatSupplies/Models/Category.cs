using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SAMarineAndBoatSupplies.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        [MinLength(1)]
        public string Name { get; set; }
    }
}
