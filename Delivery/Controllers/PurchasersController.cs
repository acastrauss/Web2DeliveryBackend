using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataLayer.DBModels;

namespace Delivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasersController : ControllerBase
    {
        private readonly DeliveryDBContext _context;

        public PurchasersController(DeliveryDBContext context)
        {
            _context = context;
        }

        // GET: api/Purchasers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Purchaser>>> GetPurchasers()
        {
            return await _context.Purchasers.ToListAsync();
        }

        // GET: api/Purchasers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Purchaser>> GetPurchaser(int id)
        {
            var purchaser = await _context.Purchasers.FindAsync(id);

            if (purchaser == null)
            {
                return NotFound();
            }

            return purchaser;
        }

        // PUT: api/Purchasers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchaser(int id, Purchaser purchaser)
        {
            if (id != purchaser.UserId)
            {
                return BadRequest();
            }

            _context.Entry(purchaser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaserExists(id))
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

        // POST: api/Purchasers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Purchaser>> PostPurchaser(Purchaser purchaser)
        {
            _context.Purchasers.Add(purchaser);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PurchaserExists(purchaser.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPurchaser", new { id = purchaser.UserId }, purchaser);
        }

        // DELETE: api/Purchasers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchaser(int id)
        {
            var purchaser = await _context.Purchasers.FindAsync(id);
            if (purchaser == null)
            {
                return NotFound();
            }

            _context.Purchasers.Remove(purchaser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PurchaserExists(int id)
        {
            return _context.Purchasers.Any(e => e.UserId == id);
        }
    }
}
