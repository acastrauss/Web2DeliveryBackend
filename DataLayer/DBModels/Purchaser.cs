using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DataLayer.DBModels
{
    public partial class Purchaser
    {
        public Purchaser()
        {
            Purchase = new HashSet<Purchase>();
        }

        public int UserId { get; set; }

        public virtual Iuser User { get; set; }
        public virtual ICollection<Purchase> Purchase { get; set; }
    }
}
