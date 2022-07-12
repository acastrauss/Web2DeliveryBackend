using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataLayer.DBModels;

namespace Backend.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class IusersController : ControllerBase
    {
        private readonly object _context;

        private readonly Models.IDBModels.ICRUD _DBCrud = new DataLayer.MSSQLDB.CRUD.MSSQLUsersCRUD();
        private readonly Models.IDBModels.IConversion _DBConvert = new DataLayer.MSSQLDB.Conversion.MSSQLConversion();

        public IusersController()
        {
            _context = null;
        }

        // GET: api/Iusers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Iuser>>> GetIusers()
        {
            //return await _context.Iusers.ToListAsync();
            return NotFound();
        }

        // GET: api/Iusers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Iuser>> GetIuser(int id)
        {
            //var iuser = await _context.Iusers.FindAsync(id);

            //if (iuser == null)
            //{
            //    return NotFound();
            //}

            //return iuser;
            return NotFound();

        }

        // PUT: api/Iusers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIuser(int id, Iuser iuser)
        {
            //if (id != iuser.Id)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(iuser).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!IuserExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return NoContent();
        }

        // POST: api/Iusers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        public ActionResult<Iuser> PostIuser([FromBody] object iuser)
        {
            var u = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.SystemModels.IUser>(iuser.ToString());

            if(u == null)
            {
                return BadRequest();
            }

            try
            {
                var userDb = _DBConvert.ConvertIUserDB(u);
                _DBCrud.Create(userDb);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        [Route("LoginUser")]
        public ActionResult<Models.SystemModels.IUser> LoginUser([FromBody] object iuser)
        {
            var u = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.SystemModels.IUser>(iuser.ToString());

            if (u == null)
            {
                return BadRequest();
            }

            try
            {
                var userDb = _DBConvert.ConvertIUserDB(u);
                var loggedUser = ((DataLayer.MSSQLDB.CRUD.MSSQLUsersCRUD)_DBCrud).ExistsByEmailPassword(new DataLayer.MSSQLDB.CRUD.UserCredentialsCheck()
                {
                    Email = u.Email,
                    Passowrd = u.Password
                });

                if(loggedUser == null)
                {
                    return BadRequest();
                }
                else
                {
                    Models.SystemModels.IUser user = _DBConvert.ConvertIUserSystem(loggedUser);
                    return user;
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        // DELETE: api/Iusers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIuser(int id)
        {
            //var iuser = await _context.Iusers.FindAsync(id);
            //if (iuser == null)
            //{
            //    return NotFound();
            //}

            //_context.Iusers.Remove(iuser);
            //await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IuserExists(int id)
        {
            //return _context.Iusers.Any(e => e.Id == id);
            return false;
        }
    }
}
