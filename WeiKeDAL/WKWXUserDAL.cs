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
    public class WKWXUserDAL
    {
        private static IDbConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["WeiKeConnectionString"]);


        public int AddUser(WKWXUser wKWXUser)
        {

            string insertSql = "INSERT INTO [dbo].[WXUser] ([OpenId] ,[NickName] ,[Name]   ,[CreateTime])  VALUES (@OpenId ,@NickName  ,@Name ,GETDATE())";
            return conn.Execute(insertSql,wKWXUser);
        }

        public WKWXUser SelUserByOpenId(string OpenId)
        {
            string selectSql = "SELECT * FROM [dbo].[WXUser] where  OpenId=@OpenId";
            return conn.Query<WKWXUser>(selectSql, new { OpenId }).FirstOrDefault();
        }

    }
}
