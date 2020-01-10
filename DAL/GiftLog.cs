using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class GiftLog
    {
        public int Id { get; set; }
        public string OpenId { get; set; }
        public string NickName { get; set; }
        public string ActivityName { get; set; }
        public int GiftId { get; set; }
        public string GiftName { get; set; }
        public string GiftDesc { get; set; }
        public DateTime AddTime { get; set; }
        public string Name { get; set; }
        public string Telphone { get; set; }
        public string GiftCustomNum { get; set; }
        public string Unit { get; set; }
    }
}
