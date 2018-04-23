using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAMarineAndBoatSupplies.Models.AdminViewModel
{
    public class OrderViewModel
    {
        public List<OrderDetail> OrderDetails { get; set; }
        public List<Product> Products { get; set; }
        public Order Order { get; set; }
    }
}
