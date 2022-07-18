using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.IDBModels
{
    public enum CRUDServiceType
    {
        User = 0,
        Admin = 1,
        Deliverer = 2,
        Purchaser = 3,
        Product = 4,
        Purchase = 5
    }

    public interface ICRUD
    {

        IDBModel Create(IDBModel model);
        IDBModel ReadById(int id);
        ICollection<IDBModel> ReadAll();
        IDBModel UpdateModel(IDBModel model);
        IDBModel DeleteModel(int id);
    }

    public delegate ICRUD CRUDServiceResolver(CRUDServiceType serviceType);
}
