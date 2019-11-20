using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiKeDAL
{
    public class QuestionTable
    {
        public int Id { get; set; }
        public int KId { get; set; }
        public string Question { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
