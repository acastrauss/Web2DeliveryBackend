using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.DBModels
{
    public partial class Iuser : Models.IDBModels.IDBModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public int? UserType { get; set; }
        public string PicturePath { get; set; }

        public virtual Admin Admin { get; set; }
        public virtual Deliverer Deliverer { get; set; }
        public virtual Purchaser Purchaser { get; set; }
    }
}
