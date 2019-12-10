using System;

namespace DAL
{
    public class GameScore
    {
        public int Id { get; set; }
        public string OpenId { get; set; }
        public string WeiXinImg { get; set; }
        public int Score { get; set; }
        public string ActivityName { get; set; }
        public System.DateTime AddTime { get; set; }
    }
}
