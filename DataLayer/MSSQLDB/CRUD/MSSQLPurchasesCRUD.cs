using Microsoft.EntityFrameworkCore;
using Models.IDBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.MSSQLDB.CRUD
{
    public class MSSQLPurchasesCRUD : Models.IDBModels.ICRUD
    {
        public IDBModel Create(IDBModel model)
        {
            throw new NotImplementedException();
        }

        public IDBModel DeleteModel(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<IDBModel> ReadAll()
        {
            List<IDBModel> purchs = new List<IDBModel>();

            using (DBModels.DeliveryDBContext _context = new DBModels.DeliveryDBContext())
            {
                _context.Purchases.Include(x => x.ConsistOfs).ThenInclude(x => x.Product).ToList().ForEach(p => purchs.Add(p));
            }

            return purchs;
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
