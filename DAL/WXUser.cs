using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class WXUser
    {
        public int UserId { get; set; }
        public string OpenId { get; set; }
        public string Nickname { get; set; }
        public string Phone { get; set; }

    }
}
