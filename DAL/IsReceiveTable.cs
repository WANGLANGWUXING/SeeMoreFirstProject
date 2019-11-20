using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class IsReceiveTable
    {
        public int Id { get; set; }
        public string OpenId { get; set; }
        public int IsReceive { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
