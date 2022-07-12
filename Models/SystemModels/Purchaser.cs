using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.SystemModels
{
    public class Purchaser : IUser
    {
        public Purchase CurrentDelivery { get; set; }
        public List<Purchase> PreviousPurchases { get; set; }

        public Purchaser(Purchaser rhs) : base(rhs)
        {
            UType = UserType.PURCHASER;
            PreviousPurchases = rhs.PreviousPurchases;
            CurrentDelivery = rhs.CurrentDelivery;
        }

        public Purchaser(
            long id, string username, string email, string password,
            string firstName, string lastName, DateTime dateOfBirth,
            string address, string picturePath,
            List<Purchase> previousDeliveries = null,
            Purchase currentDelivery = null):
            base(id, username, email, password, firstName,
                lastName, dateOfBirth, address, picturePath, UserType.PURCHASER)
        {
            if(previousDeliveries == null)
            {
                previousDeliveries = new List<Purchase>();
            }

            PreviousPurchases = previousDeliveries;

            CurrentDelivery = null;
        }
    }
}
