using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.SystemModels
{
    public enum PurhaseStatus
    {
        ORDERED = 0,
        ACCEPTED = 1,
        DELIVERED = 2,
        CANCELED = 3
    }

    // Inherit, if needed, for specific deliveries
    public class Purchase
    {
        public Purchase(
            int id,
            List<Product> deliveryItems, float totalPrice,
            string comment, string address, PurhaseStatus status = PurhaseStatus.ORDERED)
        {
            PurchaseItems = deliveryItems;
            TotalPrice = totalPrice;
            Comment = comment;
            Address = address;
            Id = id;
            Status = status;
        }

        public Purchase(Purchase rhs)
        {
            PurchaseItems = new List<Product>();
            rhs.PurchaseItems.ForEach(di => PurchaseItems.Add(di));

            TotalPrice = CalculateTotalPrice();
            Comment = rhs.Comment;
            Address = rhs.Address;
            Id = rhs.Id;
            Status = rhs.Status;
        }

        public Purchase() { }
        public List<Product> PurchaseItems { get; set; }

        // maybe add to config
        public float DeliveryPrice { get; } = 250.0f;
        public double TotalPrice { get; set; }
        public String Comment { get; set; }
        public String Address { get; set; }
        public int Id { get; set; }
        public PurhaseStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeliveredAt { get; set; }

        public int? DeliveredBy { get; set; }
        public int? OrderedBy { get; set; }
        public float CalculateTotalPrice()
        {
            float totalPrice = 0;
            PurchaseItems.ForEach(di => totalPrice += di.Price);
            totalPrice += DeliveryPrice;
            return totalPrice;
        }

        public bool IsValid()
        {
            return
                PurchaseItems.Count != 0 &&
                TotalPrice != 0 &&
                !String.IsNullOrEmpty(Address) && !String.IsNullOrWhiteSpace(Address);
        }
    }
}
