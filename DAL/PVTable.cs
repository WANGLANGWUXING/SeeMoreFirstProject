using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class PVTable
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string OpenId { get; set; }
        public System.DateTime VisitTime { get; set; }
    }
}
