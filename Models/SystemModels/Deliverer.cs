using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.SystemModels
{
    public enum ApprovalStatus
    {
        APPROVED = 0,
        CANCELED = 1,
        ON_HOLD = 2
    }

    public class Deliverer : IUser
    {
        public List<Purchase> Purchases { get; set; }
        public Purchase CurrentDelivery { get; set; }

        public ApprovalStatus Status { get; set; }

        public Deliverer(Deliverer rhs) : base(rhs)
        {
            Purchases = rhs.Purchases;
            CurrentDelivery = rhs.CurrentDelivery;
            Status = rhs.Status;
        }

        public Deliverer(
            long id, string username, string email, string password,
            string firstName, string lastName, DateTime dateOfBirth,
            string address, string picturePath, ApprovalStatus status,
            List<Purchase> myDeliveries = null,
            Purchase currentDelivery = null
            ) :
            base(
                id, username, email, password, firstName,
                lastName, dateOfBirth, address, picturePath, UserType.DELIVERER)
        {
            Status = status;

            if(myDeliveries == null)
            {
                myDeliveries = new List<Purchase>();
            }

            Purchases = myDeliveries;
            CurrentDelivery = currentDelivery;
        }
    }
}
