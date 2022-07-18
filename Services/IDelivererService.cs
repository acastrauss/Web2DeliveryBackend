using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class StatusChangeData
    {
        public int id { get; set; }
        public int status { get; set; }
        public int adminId { get; set; }
    }

    public interface IDelivererService
    {
        bool ChangeStatus(StatusChangeData scd, Services.EmailRequiredInfo eri);
        bool GetApproved(int id);
        IEnumerable<Models.SystemModels.Deliverer> GetAll();
    }
}
