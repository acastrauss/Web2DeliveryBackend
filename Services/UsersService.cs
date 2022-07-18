using Microsoft.IdentityModel.Tokens;
using Models.SystemModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;

namespace Services
{
    public class UsersService : IUsersService
    {
        private Models.IDBModels.ICRUD _DBCrud = new DataLayer.MSSQLDB.CRUD.MSSQLUsersCRUD();
        private readonly Models.IDBModels.IConversion _DBConvert; /*= new DataLayer.MSSQLDB.Conversion.MSSQLConversion();*/
        
        public UsersService(Models.IDBModels.IConversion conversion)
        {
            _DBConvert = conversion;
        }

        public string AddImage(List<byte> image, String imgName)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "imgs", imgName);

                using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    image.ForEach(b => fileStream.WriteByte(b));
                }
                return path;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IUser GetUser(int id)
        {
            var userdb = _DBCrud.ReadById(id);
            if (userdb != null)
            {
                return _DBConvert.ConvertIUserSystem(userdb);
            }
            else
            {
                return null;
            }
        }


        public IUser LoginUser(UserCredentialsLogin userCredentials, string jwtSecret)
        {
            try
            {
                var loggedUser = ((DataLayer.MSSQLDB.CRUD.MSSQLUsersCRUD)_DBCrud).ExistsByEmailPassword(new DataLayer.MSSQLDB.CRUD.UserCredentialsCheck()
                {
                    Email = userCredentials.Email,
                    Passowrd = userCredentials.Password
                });

                if (loggedUser == null) return null;
                
                Models.SystemModels.IUser user = _DBConvert.ConvertIUserSystem(loggedUser);
                var role = user.UType.ToString().ToLower();

                user.Token = TokenCreator.CreateToken(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Role, role),
                        new Claim(ClaimTypes.Sid, user.Id.ToString()),
                        new Claim(ClaimTypes.StreetAddress, user.Address)
                    },
                    DateTime.UtcNow.AddDays(1),
                    jwtSecret
                );

                return user;
            }
            catch (Exception e)
            {
                return null;
            }
        
        }

        public IUser RegisterUser(IUser userModels)
        {
            var u = userModels;

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
                return _DBConvert.ConvertIUserSystem(specifUser);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public IUser UpdateUser(IUser userModels)
        {
            Models.SystemModels.IUser ubody = userModels;
            // if password didnt change
            bool passowrdChanged = !ubody.Password.Equals(String.Empty);
            bool pictureChanged = !ubody.PicturePath.Equals(String.Empty);

            var currentUdb = _DBCrud.ReadById((int)ubody.Id) as DataLayer.DBModels.Iuser;
            if (currentUdb == null) return null;

            if (!passowrdChanged)
            {
                ubody.Password = currentUdb.Password;
            }
            if (!pictureChanged)
            {
                ubody.PicturePath = currentUdb.PicturePath;
            }

            ubody.UType = (Models.SystemModels.UserType)currentUdb.UserType;
            ubody.DateOfBirth = currentUdb.DateOfBirth;

            var userDb = _DBConvert.ConvertIUserDB(ubody);
            var newUserDb = _DBCrud.UpdateModel(userDb);

            if (newUserDb == null) return null;
            else return _DBConvert.ConvertIUserSystem(newUserDb);
        }
    }
}
