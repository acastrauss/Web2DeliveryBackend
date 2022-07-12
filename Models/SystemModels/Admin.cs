using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.SystemModels
{
    public class Admin : IUser
    {
        public ICollection<Deliverer> Deliverers;
        public ICollection<Product> Products;

        public Admin(Admin rhs) : base(rhs)
        {
            UType = UserType.ADMIN;
        }

        public Admin(
            long id, string username, string email, string password,
            string firstName, string lastName, DateTime dateOfBirth,
            string address, string picturePath,
            ICollection<Deliverer> deliverers = null,
            ICollection<Product> products = null
            )
            : base(
                  id, username, email, password,
                  firstName, lastName, dateOfBirth,
                  address, picturePath, UserType.ADMIN)
        {
            if(deliverers == null)
            {
                deliverers = new List<Deliverer>();
            }
            Deliverers = deliverers;

            if(products == null)
            {
                products = new List<Product>();
            }
            Products = products;
        }
    }
}
