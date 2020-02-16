using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectTemplate.Models
{
    public class Question
    {
        public int QuestionId { set; get; }
        public string QuestionText { set; get; }
        public List<QuestionResponse> Responses { set; get; }
    }
}