using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.IDBModels
{
    public interface IConversion
    {
        #region System_Conversion
        SystemModels.Admin ConvertAdminSystem(IDBModel model);
        SystemModels.Deliverer ConvertDelivererSystem(IDBModel model);
        SystemModels.Product ConvertProductSystem(IDBModel model);
        SystemModels.Purchase ConvertPurchaseSystem(IDBModel model);
        SystemModels.Purchaser ConvertPurchaserSystem(IDBModel model);
        SystemModels.IUser ConvertIUserSystem(IDBModel model);
        #endregion

        #region DB_Conversion
        IDBModel ConvertAdminDB(SystemModels.Admin model);
        IDBModel ConvertDelivereDB(SystemModels.Deliverer model);
        IDBModel ConvertProductDB(SystemModels.Product model);
        IDBModel ConvertPurchaseDB(SystemModels.Purchase model);
        IDBModel ConvertPurchaserDB(SystemModels.Purchaser model);
        IDBModel ConvertIUserDB(SystemModels.IUser model);
        #endregion

    }
}
