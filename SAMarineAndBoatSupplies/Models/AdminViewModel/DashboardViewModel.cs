using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAMarineAndBoatSupplies.Models.AdminViewModel
{
    public class DashboardViewModel
    {
        public List<Order> Orders { get; set; }
        public Order Order { get; set; }
        public List<Category> Categories { get; set; }
        public Category Category { get; set; }
    }
}
