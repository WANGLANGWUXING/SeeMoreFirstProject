using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiKeDAL
{
    public class AnswerStatusTable
    {
        public int Id { get; set; }
        public int UId { get; set; }
        public int QId { get; set; }
        public string AnswerDesc { get; set; }
        public int Score { get; set; }
        public System.DateTime AnswerTime { get; set; }
        public System.DateTime MarkingTime { get; set; }
    }
}
