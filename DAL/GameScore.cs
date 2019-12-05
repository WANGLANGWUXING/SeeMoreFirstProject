using System;

namespace DAL
{
    public class GameScore
    {
        public int Id { get; set; }
        public string OpenId { get; set; }
        public int Score { get; set; }
        public string ActivityName { get; set; }
        public DateTime AddTime { get; set; }
    }
}
