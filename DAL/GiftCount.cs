using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class GiftCount
    {
        public int GiftCountId { get; set; }
        public int GiftId { get; set; }
        public int Count { get; set; }
        public int Remainder { get; set; }
        public double Money { get; set; }
        public int MinMoney { get; set; }
        public int MaxMoney { get; set; }
    }
}
