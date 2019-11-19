using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectDAL
{
    public class ProOwners
    {
        public int Id { get; set; }
        public string OwnerName { get; set; }
        public int OrderNum { get; set; }
        public System.DateTime AddTime { get; set; }
    }
}