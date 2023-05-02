﻿using System;
using System.ComponentModel.DataAnnotations;

namespace LoanAPI.Data
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public Double? Salary { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool Isblocked { get; set; } // TODO: set default value to false
    }
    public class LoginUser
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }

    public static class Role
    {
        public const string Admin = "Admin";
        public const string User  = "User";
    }
}
