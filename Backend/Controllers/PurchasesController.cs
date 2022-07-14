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
