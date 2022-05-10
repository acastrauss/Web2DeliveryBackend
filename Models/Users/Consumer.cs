using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Users
{
    public class Consumer : User
    {
        public Deliveries.Delivery CurrentDelivery { get; set; }
        public List<Deliveries.Delivery> PreviousDeliveries { get; set; }

        public Consumer(Consumer rhs) : base(rhs)
        {
            Type = UserType.CONSUMER;
            PreviousDeliveries = rhs.PreviousDeliveries;
            CurrentDelivery = rhs.CurrentDelivery;
        }

        public Consumer(
            string username, string email, string password,
            string firstName, string lastName, DateTime dateOfBirth,
            string address, string picturePath,
            List<Deliveries.Delivery> previousDeliveries = null,
            Deliveries.Delivery currentDelivery = null):
            base(username, email, password, firstName,
                lastName, dateOfBirth, address, picturePath, UserType.CONSUMER)
        {
            if(previousDeliveries == null)
            {
                previousDeliveries = new List<Deliveries.Delivery>();
            }

            PreviousDeliveries = previousDeliveries;

            CurrentDelivery = null;
        }
    }
}
