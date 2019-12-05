using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class ShareActivityUser
    {
        public int Id { get; set; }
        public string UserShareId { get; set; }
        public string OpenId { get; set; }
        public string UserImg { get; set; }
        public string NickName { get; set; }
        public string ActivityName { get; set; }
        public System.DateTime AddTime { get; set; }
    }
}
