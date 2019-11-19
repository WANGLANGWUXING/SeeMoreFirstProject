using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class Gift
    {
        public int GiftId { get; set; }
        public string GiftName { get; set; }
        public string GiftRealName { get; set; }
        public string GiftPY { get; set; }
        public string GiftDesc { get; set; }
        public string GiftUrl { get; set; }      
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Unit { get; set; }
        public int Probability { get; set; }
        public string ActivityName { get; set; }
    }
}
