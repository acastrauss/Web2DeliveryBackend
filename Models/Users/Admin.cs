﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Users
{
    public class Admin : User
    {
        public Admin(Admin rhs) : base(rhs)
        {
            Type = UserType.ADMIN;
        }

        public Admin(
            string username, string email, string password,
            string firstName, string lastName, DateTime dateOfBirth,
            string address, string picturePath)
            : base(
                  username, email, password,
                  firstName, lastName, dateOfBirth,
                  address, picturePath, UserType.ADMIN)
        {}
    }
}
