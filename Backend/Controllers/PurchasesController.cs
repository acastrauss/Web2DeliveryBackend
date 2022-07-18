using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataLayer.DBModels;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly Services.IPurchaseService _purchaseService;
        public PurchasesController(Services.IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        // GET: api/Purchases
        [HttpGet]
        public ActionResult<IEnumerable<Models.SystemModels.Purchase>> GetPurchases()
        {
            return Ok(_purchaseService.GetAll());
        }


        // POST: api/Purchases
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Models.SystemModels.Purchase> PostPurchase([FromBody] object purchase)
        {
            var p = JsonConvert.DeserializeObject<Services.PurchaseWrapper>(purchase.ToString());
            return _purchaseService.CreatePurchase(p);
        }

        [HttpPost]
        [Route("AcceptPurchase")]

        public ActionResult<Models.SystemModels.Purchase> AcceptPurchase([FromBody] object purchase)
        {
            var pad = JsonConvert.DeserializeObject<Services.PurchaseAcceptData>(purchase.ToString());
            var purch = _purchaseService.AcceptPurchase(pad);
            if (purch == null)
            {
                return NotFound();
            }
            else
            {
                return purch;
            }
        }

        [HttpPost]
        [Route("FinishPurchase")]
        public ActionResult<Models.SystemModels.Purchase> FinishPurchase([FromBody] object purchase)
        {
            var fpd = JsonConvert.DeserializeObject<Services.FinishPurchaseData>(purchase.ToString());
            var purch = _purchaseService.FinishPurchase(fpd);
            if(purch == null)
            {
                return NotFound();
            }
            else
            {
                return purch;
            }
        }

    }
}
