using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FristProject.Models
{
    public class WeixinSign
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ticket { get; set; }
        public DateTime AddTime { get; set; }
    }
}