using Models.IDBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.MSSQLDB.CRUD
{
    public class MSSQLModelException : Exception
    {
        public override string Message => "Wrong model for MSSQL CRUD operation.";
    }

    public class UserCredentialsCheck
    {
        public String Email { get; set; }
        public String Passowrd { get; set; }
    }

    public class MSSQLUsersCRUD : Models.IDBModels.ICRUD
    {
        public IDBModel ExistsByEmailPassword(UserCredentialsCheck userCredentials)
        {
            DBModels.Iuser dbModel = null;

            using (DBModels.DeliveryDBContext _context = new DBModels.DeliveryDBContext())
            {
                var queryRes = _context.Iusers.Where(x => x.Email.Equals(userCredentials.Email) && x.Password.Equals(userCredentials.Passowrd));
                if(queryRes.Count() > 0)
                {
                    dbModel = queryRes.First();
                }
            }

            return dbModel;
        }

        public IDBModel Create(IDBModel model)
        {
            var userDb = model as DBModels.Iuser;

            if(userDb == null)
            {
                throw new MSSQLModelException();
            }

            using (DBModels.DeliveryDBContext _context = new DBModels.DeliveryDBContext())
            {
                _context.Iusers.Add(userDb);
                _context.SaveChanges();
            }

            return userDb;
        }

        public IDBModel DeleteModel(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<IDBModel> ReadAll()
        {
            throw new NotImplementedException();
        }

        public IDBModel ReadById(int id)
        {
            throw new NotImplementedException();
        }

        public IDBModel UpdateModel(IDBModel model)
        {
            throw new NotImplementedException();
        }
    }
}
