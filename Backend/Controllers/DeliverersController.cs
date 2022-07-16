using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataLayer.DBModels;
using Newtonsoft.Json;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Clients;
using Microsoft.Extensions.Options;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliverersController : ControllerBase
    {
        private readonly DeliveryDBContext _context;

        private Models.IDBModels.ICRUD _DBCrud = new DataLayer.MSSQLDB.CRUD.MSSQLDelivererCRUD();
        private readonly Models.IDBModels.IConversion _DBConvert; /*= new DataLayer.MSSQLDB.Conversion.MSSQLConversion();*/
        private readonly AppSettings _appSettings;

        public DeliverersController(DeliveryDBContext context, IOptions<AppSettings> appSet, Models.IDBModels.IConversion _convert)
        {
            _context = context;
            _appSettings = appSet.Value;
            _DBConvert = _convert;
        }

        // GET: api/Deliverers
        [HttpGet]
        public ActionResult<IEnumerable<Models.SystemModels.Deliverer>> GetDeliverers()
        {
            var delivsDb = _DBCrud.ReadAll();
            List<Models.SystemModels.Deliverer> deliverers = new List<Models.SystemModels.Deliverer>();
            delivsDb.ToList().ForEach(d =>
            {
                deliverers.Add(_DBConvert.ConvertDelivererSystem(d));
            });

            return deliverers;
        }


        //// GET: api/Deliverers/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Deliverer>> GetDeliverer(int id)
        //{
        //    var deliverer = await _context.Deliverers.FindAsync(id);

        //    if (deliverer == null)
        //    {
        //        return NotFound();
        //    }

        //    return deliverer;
        //}

        // GET: api/Deliverers/5
        [HttpGet("{id}")]
        //[Route("GetDelivererApproved")]
        public ActionResult<bool> GetDeliverer(int id)
        {
            var delivDb = _DBCrud.ReadById(id) as DataLayer.DBModels.Deliverer;
            if(delivDb == null)
            {
                return NotFound();
            }

            return delivDb.ApprovalStatus == (int)Models.SystemModels.ApprovalStatus.APPROVED; ;
        }

        class StatusChangeData
        {
            public int id { get; set; }
            public int status { get; set; }
            public int adminId { get; set; }
        }

        [HttpPost]
        [Route("ChangeStatus")]
        public ActionResult<bool> ChangeStatus([FromBody]object statusChange)
        {
            var scd = JsonConvert.DeserializeObject<StatusChangeData>(statusChange.ToString());
            MailMessage message = new MailMessage();

            // Set subject of the message, body and sender information
            message.Subject = "Approval";
            message.Body = scd.status == 0 ? "Your profile is approved" : "Your profile is blocked";
            message.From = new MailAddress(_appSettings.Email, _appSettings.EmailSenderName, false);

            // Add To recipients and CC recipients
            message.To.Add(new MailAddress(_appSettings.Email, _appSettings.EmailSenderName, false));

            // Save message in EML, EMLX, MSG and MHTML formats
            message.Save("EmailMessage.eml", SaveOptions.DefaultEml);
            message.Save("EmailMessage.emlx", SaveOptions.CreateSaveOptions(MailMessageSaveType.EmlxFormat));
            message.Save("EmailMessage.msg", SaveOptions.DefaultMsgUnicode);
            message.Save("EmailMessage.mhtml", SaveOptions.DefaultMhtml);
            SmtpClient client = new SmtpClient();

            // Specify your mailing Host, Username, Password, Port # and Security option
            client.Host = _appSettings.EmailHost;
            client.Username = _appSettings.Email;
            client.Password = _appSettings.EmailPassword;
            client.Port = 587;
            client.SecurityOptions = SecurityOptions.SSLExplicit;
            try
            {
                // Send this email
                client.Send(message);
            }
            catch (Exception ex)
            {
            }

            return ((DataLayer.MSSQLDB.CRUD.MSSQLDelivererCRUD)_DBCrud).ChangeStatus(
                scd.id, (Models.SystemModels.ApprovalStatus)scd.status, scd.adminId);
        }


        // PUT: api/Deliverers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeliverer(int id, Deliverer deliverer)
        {
            if (id != deliverer.UserId)
            {
                return BadRequest();
            }

            _context.Entry(deliverer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DelivererExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Deliverers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Deliverer>> PostDeliverer(Deliverer deliverer)
        {
            _context.Deliverers.Add(deliverer);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DelivererExists(deliverer.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDeliverer", new { id = deliverer.UserId }, deliverer);
        }

        // DELETE: api/Deliverers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeliverer(int id)
        {
            var deliverer = await _context.Deliverers.FindAsync(id);
            if (deliverer == null)
            {
                return NotFound();
            }

            _context.Deliverers.Remove(deliverer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DelivererExists(int id)
        {
            return _context.Deliverers.Any(e => e.UserId == id);
        }
    }
}
