using Models.SystemModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DelivererService : IDelivererService
    {
        private Models.IDBModels.ICRUD _DBCrud;/*= new DataLayer.MSSQLDB.CRUD.MSSQLDelivererCRUD();*/
        private readonly Models.IDBModels.IConversion _DBConvert; /*= new DataLayer.MSSQLDB.Conversion.MSSQLConversion();*/
        private readonly IEmailService _emailService;

        public DelivererService(Models.IDBModels.IConversion conversion, IEmailService emailService, Models.IDBModels.CRUDServiceResolver crudResolver)
        {
            _DBConvert = conversion;
            _emailService = emailService;
            _DBCrud = crudResolver(Models.IDBModels.CRUDServiceType.Deliverer);
        }

        public bool ChangeStatus(StatusChangeData scd, Services.EmailRequiredInfo eri)
        {
            String text = scd.status == 0 ? "Your profile is approved" : "Your profile is blocked";
            _emailService.SendMessage(eri, text);
            return ((DataLayer.MSSQLDB.CRUD.MSSQLDelivererCRUD)_DBCrud).ChangeStatus(
                scd.id, (Models.SystemModels.ApprovalStatus)scd.status, scd.adminId);
        }

        public IEnumerable<Deliverer> GetAll()
        {
            var delivsDb = _DBCrud.ReadAll();
            List<Models.SystemModels.Deliverer> deliverers = new List<Models.SystemModels.Deliverer>();
            delivsDb.ToList().ForEach(d =>
            {
                deliverers.Add(_DBConvert.ConvertDelivererSystem(d));
            });
            return deliverers;
        }

        public bool GetApproved(int id)
        {
            var delivDb = _DBCrud.ReadById(id) as DataLayer.DBModels.Deliverer;
            if (delivDb == null)
            {
                return false;
            }
            else
            {
                return delivDb.ApprovalStatus == (int)Models.SystemModels.ApprovalStatus.APPROVED;
            }
        }
    }
}
