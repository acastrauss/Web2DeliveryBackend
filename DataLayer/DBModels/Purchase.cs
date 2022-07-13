using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.DBModels
{
    public partial class Purchase : Models.IDBModels.IDBModel
    {
        public Purchase()
        {
            ConsistOfs = new HashSet<ConsistOf>();
        }

        public int Id { get; set; }
        public string DeliverToAddress { get; set; }
        public string Comment { get; set; }
        public double TotalPrice { get; set; }
        public int? DeliveredBy { get; set; }
        public int? DeliveredTo { get; set; }
        public int? Status { get; set; }

        public virtual Deliverer DeliveredByNavigation { get; set; }
        public virtual Purchaser DeliveredToNavigation { get; set; }
        public virtual ICollection<ConsistOf> ConsistOfs { get; set; }
    }
}
