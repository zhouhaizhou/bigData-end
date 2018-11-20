using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfSmcGridService.Model
{
    public class RelateResultsGroup
    {
        public List<Articles> articleLists { get; set; }
        public List<Books> bookLists { get; set; }
        public List<Copyrights> copyrightLists { get; set; }
    }
}