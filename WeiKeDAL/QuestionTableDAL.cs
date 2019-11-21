using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiKeDAL
{
    public class QuestionTableDAL
    {

        public List<QuestionTable> GetQuestionTables(string KId)
        {
            var selSql = "select * from QuestionTable where KId=@KId";
            return DapperHelper<QuestionTable>.Query(selSql, new { KId });
        }
    }
}
