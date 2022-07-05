using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.IDBModels
{
    public interface ICRUD
    {
        int Create(IDBModel model);
        IDBModel ReadById(int id);
        ICollection<IDBModel> ReadAll();
        int UpdateModel(IDBModel model);
        int DeleteModel(int id);
    }
}
