using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class IsShareTable
    {
        public int Id { get; set; }
        public string OpenId { get; set; }
        public int IsShare { get; set; }
        public string ActivityName { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
