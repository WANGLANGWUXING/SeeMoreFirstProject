using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiKeDAL
{
    public class SelKTable
    {
        public int RowIndex { get; set; }
        public int Id { get; set; }
        public int UId { get; set; }
        public int KId { get; set; }
        public string KName { get; set; }
        public string KType { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
