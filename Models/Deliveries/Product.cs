using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Deliveries
{
    // Inherit if specific products are needed
    public class Product
    {
        public Product(string name, float pricePerOne, uint quantity)
        {
            Name = name;
            PricePerOne = pricePerOne;
            Quantity = quantity;
        }

        public Product(Product rhs)
        {
            Name = rhs.Name;
            PricePerOne = rhs.PricePerOne;
            Quantity = rhs.Quantity;
        }

        public String Name { get; set; }
        public float PricePerOne { get; set; }
        public uint Quantity { get; set; }

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(Name) && !String.IsNullOrWhiteSpace(Name) 
                && Quantity != 0 && PricePerOne != 0;
        }
    }
}
