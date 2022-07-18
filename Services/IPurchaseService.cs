using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PurchaseWrapper
    {
        public Models.SystemModels.Purchase purchase { get; set; } = new Models.SystemModels.Purchase();
        public int? deliveredBy { get; set; }
        public int deliveredTo { get; set; }
    }

    public class PurchaseAcceptData
    {
        public int purchaseId { get; set; }
        public int delivererId { get; set; }
    }

    public class FinishPurchaseData
    {
        public int purchaseId { get; set; }
    }

    public interface IPurchaseService
    {
        IEnumerable<Models.SystemModels.Purchase> GetAll();
        Models.SystemModels.Purchase CreatePurchase(PurchaseWrapper p);
        Models.SystemModels.Purchase AcceptPurchase(PurchaseAcceptData pad);
        Models.SystemModels.Purchase FinishPurchase(FinishPurchaseData fpd);
    }
}
