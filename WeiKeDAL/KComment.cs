using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiKeDAL
{
    public class KComment
    {
        public int Id { get; set; }
        public int KId { get; set; }
        public int UId { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
        public string CreateTime { get; set; }
    }
}
