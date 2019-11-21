using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiKeDAL
{
    public class AnswerStatusDAL
    {
        public int AddAnswer(int UId,int QId,string answer)
        {
            string insertSql = "INSERT INTO [dbo].[AnswerStatusTable] ([UId] ,[QId] ,[AnswerDesc] ,[AnswerTime] )  VALUES(@UId, @QId, @answer, GETDATE())";
            return DapperHelper<KComment>.Execute(insertSql, new { UId,QId,answer});
        }
    }
}
