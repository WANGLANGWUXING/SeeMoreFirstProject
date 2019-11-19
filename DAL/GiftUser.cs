using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public  class GiftUser
    {
        public int GiftUserId { get; set; }
        public int GiftId { get; set; }
        public string OpenId { get; set; }
        public string GiftShowId { get; set; }
        public DateTime GetTime { get; set; }

    }
}
