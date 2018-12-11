using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfSmcGridService.Model
{
    public class Comment
    {
        public int id { get; set; }
        public string time { get; set; }
        public string content { get; set; }
        public string sender { get; set; }
        public string account { get; set; }
        public string receiver { get; set; }
        public bool show = false;
        public List<Comment> answer { get; set; }
    }
}