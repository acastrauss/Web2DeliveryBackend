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
        private readonly Services.IDelivererService _delivererService;
        private readonly Services.AppSettings _appSettings;

        public DeliverersController(Services.IDelivererService delivererService, IOptions<Services.AppSettings> options)
        {
            _delivererService = delivererService;
            _appSettings = options.Value;
        }

        // GET: api/Deliverers
        [HttpGet]
        public ActionResult<IEnumerable<Models.SystemModels.Deliverer>> GetDeliverers()
        {
            return Ok(_delivererService.GetAll());
        }


        // GET: api/Deliverers/5
        [HttpGet("{id}")]
        //[Route("GetDelivererApproved")]
        public ActionResult<bool> GetDeliverer(int id)
        {
            return _delivererService.GetApproved(id);
        }

        [HttpPost]
        [Route("ChangeStatus")]
        public ActionResult<bool> ChangeStatus([FromBody]object statusChange)
        {
            var scd = JsonConvert.DeserializeObject<Services.StatusChangeData>(statusChange.ToString());
            return _delivererService.ChangeStatus(scd, new Services.EmailRequiredInfo()
            {
                EmailLogin = _appSettings.Email,
                Host = _appSettings.EmailHost,
                PasswordLogin = _appSettings.EmailPassword,
                ReceiverEmail = _appSettings.Email,
                ReceiverName = _appSettings.EmailSenderName,
                SenderEmail = _appSettings.Email,
                SenderName = _appSettings.EmailSenderName
            });
        }
    }
}
