using Models.IDBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.MSSQLDB.CRUD
{
    public class MSSQLAdminCRUD : Models.IDBModels.ICRUD
    {
        public IDBModel Create(IDBModel model)
        {
            var userDb = model as DBModels.Admin;

            if (userDb == null)
            {
                throw new MSSQLModelException();
            }

            using (DBModels.DeliveryDBContext _context = new DBModels.DeliveryDBContext())
            {
                _context.Admins.Add(userDb);
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
