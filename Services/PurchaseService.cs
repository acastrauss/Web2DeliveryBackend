using Models.SystemModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PurchaseService : IPurchaseService
    {
        private Models.IDBModels.ICRUD _DBCrud = new DataLayer.MSSQLDB.CRUD.MSSQLPurchasesCRUD();
        private readonly Models.IDBModels.IConversion _DBConvert;/*= new DataLayer.MSSQLDB.Conversion.MSSQLConversion();*/
    
        public PurchaseService(Models.IDBModels.IConversion _convert)
        {
            _DBConvert = _convert;
        }

        public Purchase AcceptPurchase(PurchaseAcceptData pad)
        {
            var currPurch = _DBCrud.ReadById(pad.purchaseId) as DataLayer.DBModels.Purchase;
            if (currPurch == null)
            {
                return null;
            }

            var rand = new Random();

            currPurch.Status = (int)Models.SystemModels.PurhaseStatus.ACCEPTED;
            currPurch.DeliveredAt = DateTime.Now.AddSeconds(rand.Next() % 50 + 10);
            currPurch.DeliveredBy = pad.delivererId;
            return _DBConvert.ConvertPurchaseSystem(_DBCrud.UpdateModel(currPurch));
        }

        public Purchase CreatePurchase(PurchaseWrapper p)
        {
            var pdb = _DBConvert.ConvertPurchaseDB(p.purchase);
            ((DataLayer.DBModels.Purchase)pdb).DeliveredTo = p.deliveredTo;
            ((DataLayer.DBModels.Purchase)pdb).DeliveredBy = p.deliveredBy;
            var purch = _DBConvert.ConvertPurchaseSystem(_DBCrud.Create(pdb));

            return purch;
        }

        public Purchase FinishPurchase(FinishPurchaseData fpd)
        {
            var currPurch = _DBCrud.ReadById(fpd.purchaseId) as DataLayer.DBModels.Purchase;
            if (currPurch == null)
            {
                return null;
            }

            currPurch.Status = (int)Models.SystemModels.PurhaseStatus.DELIVERED;
            return _DBConvert.ConvertPurchaseSystem(_DBCrud.UpdateModel(currPurch));
        }

        public IEnumerable<Purchase> GetAll()
        {
            var purchsDb = _DBCrud.ReadAll();
            List<Models.SystemModels.Purchase> purchases = new List<Models.SystemModels.Purchase>();
            purchsDb.ToList().ForEach(d =>
            {
                purchases.Add(_DBConvert.ConvertPurchaseSystem(d));
            });

            return purchases;
        }
    }
}
