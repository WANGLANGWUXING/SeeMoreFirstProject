using System;

namespace WeiKeDAL
{
    public class QuestionTable
    {
        public int Id { get; set; }
        public int KId { get; set; }
        public string Question { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class QuestionTableView
    {
        public int QId { get; set; }
        public string Question { get; set; }
        public int AId { get; set; }
        public string AnswerDesc { get; set; }
        public DateTime AnswerTime { get; set; }
    }
}
