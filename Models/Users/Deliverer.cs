using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Users
{
    public class Deliverer : IUser
    {
        public List<Deliveries.Purchase> NewDeliveries { get; set; }
        public List<Deliveries.Purchase> MyDeliveries { get; set; }
        public Deliveries.Purchase CurrentDelivery { get; set; }

        public bool Verified { get; set; }

        public Deliverer(Deliverer rhs) : base(rhs)
        {
            NewDeliveries = rhs.NewDeliveries;
            MyDeliveries = rhs.MyDeliveries;
            CurrentDelivery = rhs.CurrentDelivery;
            Verified = rhs.Verified;
        }

        public Deliverer(
            long id, string username, string email, string password,
            string firstName, string lastName, DateTime dateOfBirth,
            string address, string picturePath, bool verified = false,
            List<Deliveries.Purchase> myDeliveries = null,
            List<Deliveries.Purchase> newDeliveries = null,
            Deliveries.Purchase currentDelivery = null
            ) :
            base(
                id, username, email, password, firstName,
                lastName, dateOfBirth, address, picturePath, UserType.DELIVERER)
        {
            Verified = verified;

            if(myDeliveries == null)
            {
                myDeliveries = new List<Deliveries.Purchase>();
            }

            if(newDeliveries == null)
            {
                newDeliveries = new List<Deliveries.Purchase>();
            }

            NewDeliveries = newDeliveries;
            MyDeliveries = myDeliveries;
            CurrentDelivery = currentDelivery;
        }
    }
}
