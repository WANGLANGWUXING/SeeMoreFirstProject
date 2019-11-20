using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiKeDAL
{
   public  class KCommentDAL
    {
       //private static IDbConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["WeiKeConnectionString"]);


        public int AddComment(KComment comment)
        {

            string insertSql = "INSERT INTO [dbo].[KComment] ([KId] ,[UId],[UserName] ,[Comment] ,[CreateTime]) VALUES (@KId ,@UId,@UserName ,@Comment ,GETDATE())";
            return DapperHelper<KComment>.Execute(insertSql, comment);
        }

        public List<KComment> SelKCommentByKTableId(string KTableId)
        {
            string selectSql = "SELECT * FROM [dbo].[KComment] where KId=@KTableId";
            return DapperHelper<KComment>.Query(selectSql, new { KTableId });
        }

    }
}
