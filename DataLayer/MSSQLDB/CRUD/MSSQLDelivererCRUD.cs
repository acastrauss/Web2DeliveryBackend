using Microsoft.EntityFrameworkCore;
using Models.IDBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.MSSQLDB.CRUD
{
    public class MSSQLDelivererCRUD : Models.IDBModels.ICRUD
    {
        public IDBModel Create(IDBModel model)
        {
            var userDb = model as DBModels.Deliverer;

            if (userDb == null)
            {
                throw new MSSQLModelException();
            }

            using (DBModels.DeliveryDBContext _context = new DBModels.DeliveryDBContext())
            {
                _context.Deliverers.Add(userDb);
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
            List<IDBModel> delivs = new List<IDBModel>();

            using (DBModels.DeliveryDBContext _context = new DBModels.DeliveryDBContext())
            {
                _context.Deliverers.Include(x => x.Purchases).Include(x => x.User).ToList().ForEach(d => delivs.Add(d));
            }

            return delivs;
        }

        public IDBModel ReadById(int id)
        {
            DataLayer.DBModels.Deliverer delivs = null;

            using (DBModels.DeliveryDBContext _context = new DBModels.DeliveryDBContext())
            {
                delivs = _context.Deliverers.Include(x => x.Purchases).Include(x => x.User).ToList().Where(x => x.UserId == id).FirstOrDefault();
            }

            return delivs;
        }

        public IDBModel UpdateModel(IDBModel model)
        {
            throw new NotImplementedException();
        }

        public bool ChangeStatus(int id, Models.SystemModels.ApprovalStatus status, int adminId)
        {
            bool retVal = true;

            using (DBModels.DeliveryDBContext _context = new DBModels.DeliveryDBContext())
            {
                var delivQ = _context.Deliverers.Where(x => x.UserId == id);

                if (delivQ.Count() > 0)
                {
                    var d = delivQ.First();
                    d.ApprovalStatus = (int)status;
                    d.ApprovedFrom = adminId;
                    _context.Entry(d).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                else
                {
                    retVal = false;
                }
            }

            return retVal;
        }
    }
}
