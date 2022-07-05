using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.SystemModels
{
    // Inherit if specific products are needed
    public class Product
    {
        public Product(string name, float price, string ingredients)
        {
            Name = name;
            Price = price;
            Ingredients = ingredients;
        }

        public Product(Product rhs)
        {
            Name = rhs.Name;
            Price = rhs.Price;
            Ingredients = rhs.Ingredients;
        }

        public int Id { get; set; }
        public String Name { get; set; }
        public float Price { get; set; }
        public string Ingredients { get; set; }

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(Name) && !String.IsNullOrWhiteSpace(Name) 
                && Price != 0 && !String.IsNullOrEmpty(Ingredients);
        }
    }
}
