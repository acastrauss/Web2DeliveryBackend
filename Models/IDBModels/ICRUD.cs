using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.IDBModels
{
    public interface ICRUD
    {


        IDBModel Create(IDBModel model);
        IDBModel ReadById(int id);
        ICollection<IDBModel> ReadAll();
        IDBModel UpdateModel(IDBModel model);
        IDBModel DeleteModel(int id);
    }
}
