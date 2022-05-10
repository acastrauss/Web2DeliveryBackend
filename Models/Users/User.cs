using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public enum UserType
    {
        ADMIN = 0,
        DELIVERER = 1,
        CONSUMER = 2
    }

    public static class InvalidDate
    {
        public static DateTime InvalidBirthDate = DateTime.Now.AddDays(200);
    }


    public abstract class User
    {
        public String Username { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String Address { get; set; }
        public String PicturePath { get; set; }

        public UserType Type { get; set; }

        public User(
            string username, string email, string password,
            string firstName, string lastName, DateTime dateOfBirth,
            string address, string picturePath, UserType userType)
        {
            Username = username;
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Address = address;
            PicturePath = picturePath;
            Type = userType;
        }

        public User(User rhs)
        {
            Username = rhs.Username;
            Email = rhs.Email;
            Password = rhs.Password;
            FirstName = rhs.FirstName;
            LastName = rhs.LastName;
            DateOfBirth = rhs.DateOfBirth;
            Address = rhs.Address;
            PicturePath = rhs.PicturePath;
            Type = rhs.Type;
        }

        public bool IsValid()
        {
            return
                !String.IsNullOrEmpty(Username) && !String.IsNullOrWhiteSpace(Username) &&
                !String.IsNullOrEmpty(Email) && !String.IsNullOrWhiteSpace(Email) &&
                !String.IsNullOrEmpty(Password) && !String.IsNullOrWhiteSpace(Password) &&
                !String.IsNullOrEmpty(FirstName) && !String.IsNullOrWhiteSpace(FirstName) &&
                !String.IsNullOrEmpty(LastName) && !String.IsNullOrWhiteSpace(LastName) &&
                !DateOfBirth.Equals(InvalidDate.InvalidBirthDate) &&
                !String.IsNullOrEmpty(Address) && !String.IsNullOrWhiteSpace(Address) &&
                !String.IsNullOrEmpty(PicturePath) && !String.IsNullOrWhiteSpace(PicturePath);
        }
    }
}
