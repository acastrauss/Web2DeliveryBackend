using Models.IDBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.MSSQLDB.CRUD
{
    public class MSSQLProductCRUD : Models.IDBModels.ICRUD
    {
        public IDBModel Create(IDBModel model)
        {
            var product = model as DBModels.Product;

            if (product == null)
            {
                throw new MSSQLModelException();
            }

            using (DBModels.DeliveryDBContext _context = new DBModels.DeliveryDBContext())
            {
                _context.Products.Add(product);
                _context.SaveChanges();
            }

            return product;
        }

        public IDBModel DeleteModel(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<IDBModel> ReadAll()
        {
            var prods = new List<IDBModel>();

            using (var _context = new DBModels.DeliveryDBContext())
            {
                _context.Products.ToList().ForEach(p => prods.Add(p));
            }

            return prods;
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
