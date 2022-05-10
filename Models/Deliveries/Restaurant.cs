using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Deliveries
{
    public class Restaurant
    {
        public List<Product> Products { get; set; }

        public Restaurant(List<Product> products)
        {
            Products = products;
        }

        public Restaurant() 
        {
            Products = new List<Product>();
        }

        public Restaurant(Restaurant rhs)
        {
            Products = rhs.Products;
        }
    }
}
