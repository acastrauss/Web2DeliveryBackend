﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataLayer.DBModels;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace Backend.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class IusersController : ControllerBase
    {
        private readonly object _context;

        private Models.IDBModels.ICRUD _DBCrud = new DataLayer.MSSQLDB.CRUD.MSSQLUsersCRUD();
        private readonly Models.IDBModels.IConversion _DBConvert = new DataLayer.MSSQLDB.Conversion.MSSQLConversion();
        private readonly AppSettings _appSettings;

        public IusersController(DataLayer.DBModels.DeliveryDBContext con, IOptions<AppSettings> appSet)
        {
            _context = null;
            _appSettings = appSet.Value;
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
        public ActionResult<Models.SystemModels.IUser> GetIuser([FromRoute] int id)
        {
            var userdb = _DBCrud.ReadById(id);
            if(userdb != null)
            {
                return _DBConvert.ConvertIUserSystem(userdb);
            }
            else
            {
                return NotFound();
            }
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

                var iuserSys = _DBConvert.ConvertIUserSystem(userDb);

                Models.IDBModels.IDBModel specifUser = null;

                switch (u.UType)
                {
                    case Models.SystemModels.UserType.ADMIN:
                        _DBCrud = new DataLayer.MSSQLDB.CRUD.MSSQLAdminCRUD();
                        var admin = new Models.SystemModels.Admin(
                                iuserSys.Id, iuserSys.Username, iuserSys.Email, iuserSys.Password, iuserSys.FirstName, iuserSys.LastName,
                                iuserSys.DateOfBirth, iuserSys.Address, iuserSys.PicturePath
                            );
                        specifUser = _DBConvert.ConvertAdminDB(admin);
                        break;
                    case Models.SystemModels.UserType.DELIVERER:
                        _DBCrud = new DataLayer.MSSQLDB.CRUD.MSSQLDelivererCRUD();
                        var deliv = new Models.SystemModels.Deliverer(
                                iuserSys.Id, iuserSys.Username, iuserSys.Email, iuserSys.Password, iuserSys.FirstName, iuserSys.LastName,
                                iuserSys.DateOfBirth, iuserSys.Address, iuserSys.PicturePath, Models.SystemModels.ApprovalStatus.ON_HOLD
                            );
                        specifUser = _DBConvert.ConvertDelivereDB(deliv);
                        break;
                    case Models.SystemModels.UserType.PURCHASER:
                        _DBCrud = new DataLayer.MSSQLDB.CRUD.MSSQLPurchaserCRUD();
                        var purch = new Models.SystemModels.Purchaser(
                                iuserSys.Id, iuserSys.Username, iuserSys.Email, iuserSys.Password, iuserSys.FirstName, iuserSys.LastName,
                                iuserSys.DateOfBirth, iuserSys.Address, iuserSys.PicturePath
                            );
                        specifUser = _DBConvert.ConvertPurchaserDB(purch);
                        break;
                }

                _DBCrud.Create(specifUser);
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
                    var role = String.Empty;
                    switch (user.UType)
                    {
                        case Models.SystemModels.UserType.ADMIN:
                            role = "admin";
                            break;
                        case Models.SystemModels.UserType.DELIVERER:
                            role = "deliverer";
                            break;
                        case Models.SystemModels.UserType.PURCHASER:
                            role = "purchaser";
                            break;
                    }

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                       {
                        new Claim(ClaimTypes.Role, role)
                       }),
                        Expires = DateTime.UtcNow.AddDays(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature),
                        Issuer = "https://localhost:5001",
                        Audience = "https://localhost:44339/api/"
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                    var token = tokenHandler.WriteToken(securityToken);
                    user.Token = token;

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
