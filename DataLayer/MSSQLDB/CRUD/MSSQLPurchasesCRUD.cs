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
            var purchs = model as DBModels.Purchase;

            if (purchs == null)
            {
                throw new MSSQLModelException();
            }

            using (DBModels.DeliveryDBContext _context = new DBModels.DeliveryDBContext())
            {
                _context.Purchases.Add(purchs);
                _context.SaveChanges();
            }

            return purchs;
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
            DataLayer.DBModels.Purchase purchase = null;

            using (DBModels.DeliveryDBContext _context = new DBModels.DeliveryDBContext())
            {
                purchase =_context.Purchases.Include(x => x.ConsistOfs).ThenInclude(x => x.Product).Where(x => x.Id == id).FirstOrDefault();
            }

            return purchase;
        }

        public IDBModel UpdateModel(IDBModel model)
        {
            var purch = model as DBModels.Purchase;

            if (purch == null)
            {
                throw new MSSQLModelException();
            }

            using (DBModels.DeliveryDBContext _context = new DBModels.DeliveryDBContext())
            {
                if (_context.Purchases.Where(x => x.Id == purch.Id).Count() > 0)
                {
                    _context.Entry(purch).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                }
                else
                {
                    purch = null;
                }
            }

            return purch;
        }
    }
}
