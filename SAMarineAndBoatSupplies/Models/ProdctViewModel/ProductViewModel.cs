﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAMarineAndBoatSupplies.Models.ProdctViewModel
{
    public class ProductViewModel
    {
        public List<Product> Products { get; set; }
        public Product Product { get; set; }
        public string Title { get; set; }
        public List<Category> Categories { get; set; }
    }
}

