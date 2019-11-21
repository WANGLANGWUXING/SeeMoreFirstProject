using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiKeDAL
{
    public class QuestionTableDAL
    {

        public List<QuestionTableView> GetQuestionTables(int KId,int UId)
        {
            var selSql = "select a.Id QId,Question,b.Id AId,AnswerDesc,AnswerTime from QuestionTable a left join AnswerStatusTable b  on UId =@UId and a.Id = b.QId where a.KId =@KId";
            return DapperHelper<QuestionTableView>.Query(selSql, new { KId, UId });
        }
    }
}
