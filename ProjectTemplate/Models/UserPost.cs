using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectTemplate.Models
{
    public class UserPost
    {
        public string FristName { set; get; }
        public string LasName { set; get; }
        public int PostId { set; get; }
        public string Post { set; get; }
        public string PostTime { set; get; }
        public int PointValue { set; get; }
        public bool Success { get; set; }
    }
}