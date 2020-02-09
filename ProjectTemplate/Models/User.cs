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
    }
}