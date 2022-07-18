using System;
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
using Newtonsoft.Json;
using System.Net.Http;
using System.IO;

namespace Backend.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class IusersController : ControllerBase
    {
        private readonly Services.IUsersService _usersService;
        private readonly Services.AppSettings _appSettings;

        public IusersController(Services.IUsersService usersService, IOptions<Services.AppSettings> options)
        {
            _usersService = usersService;
            _appSettings = options.Value;
        }

        [HttpPost]
        [Route("AddImage")]
        public ActionResult<String> AddImage([FromForm] IFormCollection idobj)
        {
            try
            {
                if (idobj.Files.Count == 0)
                {
                    return BadRequest();
                }
                var f = idobj.Files[0];
                var imgName = f.FileName;
                List<byte> imgBytes = new List<byte>();
                
                using (Stream stream = f.OpenReadStream())
                {
                    imgBytes.Add((byte)stream.ReadByte());    
                }

                var imgPath = _usersService.AddImage(imgBytes, imgName);
                if(imgPath != null)
                {
                    return Ok(JsonConvert.SerializeObject(imgPath));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        // GET: api/Iusers/5
        [HttpGet("{id}")]
        public ActionResult<Models.SystemModels.IUser> GetIuser([FromRoute] int id)
        {
            var user = _usersService.GetUser(id);
            if(user == null)
            {
                return NotFound();
            }
            else
            {
                return user;
            }
        }

        // PUT: api/Iusers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public ActionResult<Models.SystemModels.IUser> PutIuser([FromBody] object iuser)
        {
            try
            {
                var ubody = JsonConvert.DeserializeObject<Models.SystemModels.IUser>(iuser.ToString());
                var updated = _usersService.UpdateUser(ubody);
                if(updated != null)
                {
                    return Ok(updated);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // POST: api/Iusers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        public ActionResult<Models.SystemModels.IUser> PostIuser([FromBody] object iuser)
        {
            var u = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.SystemModels.IUser>(iuser.ToString());
            if (u == null)
            {
                return BadRequest();
            }
            var registered = _usersService.RegisterUser(u);
            if(registered != null)
            {
                return Ok(registered);
            }
            else
            {
                return BadRequest();
            }
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

            var logged = _usersService.LoginUser(new Services.UserCredentialsLogin()
            {
                Email = u.Email,
                Password = u.Password
            },
            _appSettings.JWT_Secret);

            if(logged == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(logged);
            }
        }
    }
}
