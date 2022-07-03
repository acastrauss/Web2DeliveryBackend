using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DataLayer.DBModels
{
    public partial class Purchase
    {
        public Purchase()
        {
            ConsistOf = new HashSet<ConsistOf>();
        }

        public int Id { get; set; }
        public string DeliverTo { get; set; }
        public string Comment { get; set; }
        public int TotalPrice { get; set; }
        public int? DeliveredBy { get; set; }
        public int? DeliveredTo { get; set; }
        public int? Status { get; set; }

        public virtual Deliverer DeliveredByNavigation { get; set; }
        public virtual Purchaser DeliveredToNavigation { get; set; }
        public virtual ICollection<ConsistOf> ConsistOf { get; set; }
    }
}
