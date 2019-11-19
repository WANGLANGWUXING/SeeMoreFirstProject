using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{

    public  class JYCPriceTimeDAL
    {

        private static IDbConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);

        //添加
        public int AddJYCPriceTime(string openId)
        {
            string insertSql = "INSERT INTO [dbo].[JYCPriceTime] ([OpenId] ,[Time] ,[CreateTime]) VALUES (@OpenId,1,GETDATE())";
            var result = conn.Execute(insertSql, new { OpenId=openId });
            return result;
        }


        //查询数量
        public int SelJYCPriceTime(string openId)
        {
            string selectSql = "select count(*) count  from JYCPriceTime where OpenId=@OpenId";

            int res = Convert.ToInt32(conn.ExecuteScalar(selectSql, new { OpenId = openId }));

            return res;
        }
    }
}
