using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiKeDAL
{
    public class WKLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string LogName { get; set; }
        public string LogDesc { get; set; }
        public System.DateTime AddTime { get; set; }
    }
}
