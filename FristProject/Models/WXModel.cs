using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FristProject.Models
{
    public class WXModel
    {
        public string Openid { get; set; }
        public string Nickname { get; set; }
        public int Sex { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Headimgurl { get; set; }
    }
}