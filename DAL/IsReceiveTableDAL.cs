using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public class IsReceiveTableDAL
    {
        private static IDbConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);
        public int AddIsReceiveTable(string openId)
        {

            string insertSql = "INSERT INTO [dbo].[IsReceiveTable] ([OpenId] ,[IsReceive] ,[CreateTime]) VALUES (@OpenId ,1 ,GETDATE())";
            var result = conn.Execute(insertSql, new { OpenId = openId });
            return result;
        }


        public IsReceiveTable SelIsReceiveTable(string openId)
        {
            string selSql = "select * from IsReceiveTable where OpenId=@OpenId";

            IsReceiveTable isReceiveTable = conn.Query<IsReceiveTable>(selSql, new { OpenId = openId }).FirstOrDefault();
            return isReceiveTable;
        }
    }
}
