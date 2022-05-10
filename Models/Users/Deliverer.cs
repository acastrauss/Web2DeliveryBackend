using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Users
{
    public class Deliverer : User
    {
        public List<Deliveries.Delivery> NewDeliveries { get; set; }
        public List<Deliveries.Delivery> MyDeliveries { get; set; }
        public Deliveries.Delivery CurrentDelivery { get; set; }

        public bool Verified { get; set; }

        public Deliverer(Deliverer rhs) : base(rhs)
        {
            NewDeliveries = rhs.NewDeliveries;
            MyDeliveries = rhs.MyDeliveries;
            CurrentDelivery = rhs.CurrentDelivery;
            Verified = rhs.Verified;
        }

        public Deliverer(
            string username, string email, string password,
            string firstName, string lastName, DateTime dateOfBirth,
            string address, string picturePath, bool verified = false,
            List<Deliveries.Delivery> myDeliveries = null,
            List<Deliveries.Delivery> newDeliveries = null,
            Deliveries.Delivery currentDelivery = null
            ) :
            base(
                username, email, password, firstName,
                lastName, dateOfBirth, address, picturePath, UserType.DELIVERER)
        {
            Verified = verified;

            if(myDeliveries == null)
            {
                myDeliveries = new List<Deliveries.Delivery>();
            }

            if(newDeliveries == null)
            {
                newDeliveries = new List<Deliveries.Delivery>();
            }

            NewDeliveries = newDeliveries;
            MyDeliveries = myDeliveries;
            CurrentDelivery = currentDelivery;
        }
    }
}
