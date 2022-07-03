using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DataLayer.DBModels
{
    public partial class Product
    {
        public Product()
        {
            ConsistOf = new HashSet<ConsistOf>();
        }

        public int Id { get; set; }
        public int AddedBy { get; set; }

        public virtual Admin IdNavigation { get; set; }
        public virtual ICollection<ConsistOf> ConsistOf { get; set; }
    }
}
