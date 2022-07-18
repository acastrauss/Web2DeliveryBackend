using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserCredentialsLogin
    {
        public String Email { get; set; }
        public String Password { get; set; }
    }

    public interface IUsersService
    {

        Models.SystemModels.IUser LoginUser(UserCredentialsLogin userCredentials, string jwtSecret);
        Models.SystemModels.IUser RegisterUser(Models.SystemModels.IUser userModels);
        Models.SystemModels.IUser UpdateUser(Models.SystemModels.IUser userModels);
        Models.SystemModels.IUser GetUser(int id);
        String AddImage(List<byte> image, String imgName);
    }
}
