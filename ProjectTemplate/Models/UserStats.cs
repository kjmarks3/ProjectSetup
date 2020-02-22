using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectTemplate.Models
{
    public class UserStats
    {
        public int UserId { set; get; }
        public int PostTotal { set; get; }
        public int PointTotal { set; get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Rank { get; set; }
    }
}