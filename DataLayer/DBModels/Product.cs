using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.DBModels
{
    public partial class Product : Models.IDBModels.IDBModel
    {
        public Product()
        {
            ConsistOfs = new HashSet<ConsistOf>();
        }

        public int Id { get; set; }
        public int AddedBy { get; set; }
        public float Price { get; set; }
        public string Name { get; set; }
        public string Ingredients { get; set; }

        public virtual Admin AddedByNavigation { get; set; }
        public virtual ICollection<ConsistOf> ConsistOfs { get; set; }
    }
}
