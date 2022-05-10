using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Deliveries
{
    public enum DeliveryStatus
    {
        ORDERED = 0,
        ACCEPTED = 1,
        DELIVERED = 2,
        CANCELED = 3
    }

    // Inherit, if needed, for specific deliveries
    public class Delivery
    {
        public Delivery(
            List<Product> deliveryItems, float totalPrice,
            string comment, string address, DeliveryStatus status = DeliveryStatus.ORDERED)
        {
            DeliveryItems = deliveryItems;
            TotalPrice = totalPrice;
            Comment = comment;
            Address = address;
            Id = Guid.NewGuid();
            Status = status;
        }

        public Delivery(Delivery rhs)
        {
            DeliveryItems = new List<Product>();
            rhs.DeliveryItems.ForEach(di => DeliveryItems.Add(di));

            TotalPrice = CalculateTotalPrice();
            Comment = rhs.Comment;
            Address = rhs.Address;
            Id = rhs.Id;
            Status = rhs.Status;
        }

        public List<Product> DeliveryItems { get; set; }

        // maybe add to config
        public float DeliveryPrice { get; } = 250.0f;
        public float TotalPrice { get; private set; }
        public String Comment { get; set; }
        public String Address { get; set; }
        public Guid Id { get; set; }
        public DeliveryStatus Status { get; set; }

        public float CalculateTotalPrice()
        {
            float totalPrice = 0;
            DeliveryItems.ForEach(di => totalPrice += di.PricePerOne * di.Quantity);
            totalPrice += DeliveryPrice;
            return totalPrice;
        }

        public bool IsValid()
        {
            return
                DeliveryItems.Count != 0 &&
                TotalPrice != 0 &&
                !String.IsNullOrEmpty(Address) && !String.IsNullOrWhiteSpace(Address) &&
                Id != Guid.Empty;
        }
    }
}
