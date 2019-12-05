using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class CollectLike
    {
        public int Id { get; set; }
        public string UserShareId { get; set; }
        public string HelpOpenId { get; set; }
        public string Url { get; set; }
        public string ActivityName { get; set; }
        public System.DateTime AddTime { get; set; }
    }
}
