using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.DBModels
{
    public partial class Deliverer : Models.IDBModels.IDBModel
    {
        public Deliverer()
        {
            Purchases = new HashSet<Purchase>();
        }

        public int UserId { get; set; }
        public int? ApprovalStatus { get; set; }
        public int? ApprovedFrom { get; set; }

        public virtual Admin ApprovedFromNavigation { get; set; }
        public virtual Iuser User { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
