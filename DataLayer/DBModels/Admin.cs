using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.DBModels
{
    public partial class Admin : Models.IDBModels.IDBModel
    {
        public Admin()
        {
            Deliverers = new HashSet<Deliverer>();
            Products = new HashSet<Product>();
        }

        public int UserId { get; set; }

        public virtual Iuser User { get; set; }
        public virtual ICollection<Deliverer> Deliverers { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
