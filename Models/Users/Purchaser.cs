using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Users
{
    public class Purchaser : IUser
    {
        public Deliveries.Purchase CurrentDelivery { get; set; }
        public List<Deliveries.Purchase> PreviousDeliveries { get; set; }

        public Purchaser(Purchaser rhs) : base(rhs)
        {
            Type = UserType.CONSUMER;
            PreviousDeliveries = rhs.PreviousDeliveries;
            CurrentDelivery = rhs.CurrentDelivery;
        }

        public Purchaser(
            long id, string username, string email, string password,
            string firstName, string lastName, DateTime dateOfBirth,
            string address, string picturePath,
            List<Deliveries.Purchase> previousDeliveries = null,
            Deliveries.Purchase currentDelivery = null):
            base(id, username, email, password, firstName,
                lastName, dateOfBirth, address, picturePath, UserType.CONSUMER)
        {
            if(previousDeliveries == null)
            {
                previousDeliveries = new List<Deliveries.Purchase>();
            }

            PreviousDeliveries = previousDeliveries;

            CurrentDelivery = null;
        }
    }
}
