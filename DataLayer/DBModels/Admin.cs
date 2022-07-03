using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DataLayer.DBModels
{
    public partial class Admin
    {
        public Admin()
        {
            Deliverer = new HashSet<Deliverer>();
        }

        public int UserId { get; set; }

        public virtual Iuser User { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<Deliverer> Deliverer { get; set; }
    }
}
