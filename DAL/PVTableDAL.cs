using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class PVTableDAL
    {
        private static IDbConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);
        public int AddPV(PVTable pVTable)
        {

            string insertSql = "INSERT INTO [dbo].[PVTable] ([URL] ,[OpenId] ,[ActName],[VisitTime]) VALUES (@Url ,@OpenId ,@ActName,GETDATE())";
            var result = conn.Execute(insertSql, pVTable);
            return result;
        }

    }
}
