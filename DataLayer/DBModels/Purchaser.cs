using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.DBModels
{
    public partial class Purchaser : Models.IDBModels.IDBModel
    {
        public Purchaser()
        {
            Purchases = new HashSet<Purchase>();
        }

        public int UserId { get; set; }

        public virtual Iuser User { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
