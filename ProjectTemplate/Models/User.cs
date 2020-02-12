using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectTemplate.Models
{
    public class User
    {
        public int UserID { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
        public string Alias { set; get; }
        public string Email { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string JobTitle { set; get; }
        public string HireDate { set; get; }
        public bool Anonymous { set; get; }
        public bool Success { set; get; }

        public string ErrorMessage { set; get; }

    }
}