using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DataLayer.DBModels
{
    public partial class Deliverer
    {
        public Deliverer()
        {
            Purchase = new HashSet<Purchase>();
        }

        public int UserId { get; set; }
        public int? ApprovalStatus { get; set; }
        public int? ApprovedFrom { get; set; }

        public virtual Admin ApprovedFromNavigation { get; set; }
        public virtual Iuser User { get; set; }
        public virtual ICollection<Purchase> Purchase { get; set; }
    }
}
