using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectTemplate.Models
{
    public class UserPost
    {
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public int PostId { set; get; }
        public string Post { set; get; }
        public string PostTime { set; get; }
        public int PointValue { set; get; }
        public string PostTopic { set; get; }
        public string Anonymous { set; get; }
        public int? ParentId {set; get;}
        public int UserPointTotal { set; get; }
        public bool Success { get; set; }
        public bool IsCEO { get; set; }
    }
}