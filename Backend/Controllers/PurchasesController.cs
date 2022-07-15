using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataLayer.DBModels;
using Newtonsoft.Json;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly DeliveryDBContext _context;

        private Models.IDBModels.ICRUD _DBCrud = new DataLayer.MSSQLDB.CRUD.MSSQLPurchasesCRUD();
        private readonly Models.IDBModels.IConversion _DBConvert = new DataLayer.MSSQLDB.Conversion.MSSQLConversion();

        public PurchasesController(DeliveryDBContext context)
        {
            _context = context;
        }

        // GET: api/Purchases
        [HttpGet]
        public ActionResult<IEnumerable<Models.SystemModels.Purchase>> GetPurchases()
        {
            var purchsDb = _DBCrud.ReadAll();
            List<Models.SystemModels.Purchase> purchases = new List<Models.SystemModels.Purchase>();
            purchsDb.ToList().ForEach(d =>
            {
                purchases.Add(_DBConvert.ConvertPurchaseSystem(d));
            });

            return purchases;
        }

        // GET: api/Purchases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Purchase>> GetPurchase(int id)
        {
            var purchase = await _context.Purchases.FindAsync(id);

            if (purchase == null)
            {
                return NotFound();
            }

            return purchase;
        }

        // PUT: api/Purchases/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchase(int id, Purchase purchase)
        {
            if (id != purchase.Id)
            {
                return BadRequest();
            }

            _context.Entry(purchase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseExists(id))
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

        class PurchaseWrapper
        {
            public Models.SystemModels.Purchase purchase { get; set; } = new Models.SystemModels.Purchase();
            public int? deliveredBy { get; set; }
            public int deliveredTo { get; set; }
        }

        // POST: api/Purchases
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Models.SystemModels.Purchase> PostPurchase([FromBody] object purchase)
        {
            var p = JsonConvert.DeserializeObject<PurchaseWrapper>(purchase.ToString());
            var pdb = _DBConvert.ConvertPurchaseDB(p.purchase);
            ((DataLayer.DBModels.Purchase)pdb).DeliveredTo = p.deliveredTo;
            ((DataLayer.DBModels.Purchase)pdb).DeliveredBy = p.deliveredBy;
            var purch = _DBConvert.ConvertPurchaseSystem(_DBCrud.Create(pdb));

            return purch;
        }

        class PurchaseAcceptData
        {
            public int purchaseId { get; set; }
            public int delivererId { get; set; }
        }

        [HttpPost]
        [Route("AcceptPurchase")]

        public ActionResult<Models.SystemModels.Purchase> AcceptPurchase([FromBody] object purchase)
        {
            var pad = JsonConvert.DeserializeObject<PurchaseAcceptData>(purchase.ToString());
            var currPurch = _DBCrud.ReadById(pad.purchaseId) as DataLayer.DBModels.Purchase;
            if(currPurch == null)
            {
                return NotFound();
            }

            var rand = new Random();

            currPurch.Status = (int)Models.SystemModels.PurhaseStatus.ACCEPTED;
            currPurch.DeliveredAt = DateTime.Now.AddSeconds(rand.Next() % 50 + 10);
            currPurch.DeliveredBy = pad.delivererId;
            return _DBConvert.ConvertPurchaseSystem(_DBCrud.UpdateModel(currPurch));
        }

        class FinishPurchaseData
        {
            public int purchaseId { get; set; }
        }

        [HttpPost]
        [Route("FinishPurchase")]
        public ActionResult<Models.SystemModels.Purchase> FinishPurchase([FromBody] object purchase)
        {
            var pad = JsonConvert.DeserializeObject<FinishPurchaseData>(purchase.ToString());
            var currPurch = _DBCrud.ReadById(pad.purchaseId) as DataLayer.DBModels.Purchase;
            if (currPurch == null)
            {
                return NotFound();
            }

            var rand = new Random();

            currPurch.Status = (int)Models.SystemModels.PurhaseStatus.DELIVERED;
            return _DBConvert.ConvertPurchaseSystem(_DBCrud.UpdateModel(currPurch));
        }

        // DELETE: api/Purchases/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchase(int id)
        {
            var purchase = await _context.Purchases.FindAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }

            _context.Purchases.Remove(purchase);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PurchaseExists(int id)
        {
            return _context.Purchases.Any(e => e.Id == id);
        }
    }
}
