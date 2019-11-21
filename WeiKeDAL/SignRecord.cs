using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiKeDAL
{
    public class SignRecord
    {
        public int RowIndex { get; set; }
        public int Id { get; set; }
        public int SId { get; set; }
        public string SignDate { get; set; }
        public System.DateTime SignTime { get; set; }
    }
}
