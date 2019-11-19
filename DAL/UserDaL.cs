
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL
{
    public class UserDAL
    {
        private static IDbConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);
        public int AddUser(WXUser wXUser)
        {
            
            string insertSql = "INSERT INTO [dbo].[WXUser]([OpenId],[Nickname],[Phone],[CreateTime])VALUES(@OpenId,@Nickname,@Phone,GETDATE())";       
            var result = conn.Execute(insertSql, wXUser);
            return result;
        }


        public int SelUserByOpenId(string openId)
        {
            
            string selectSql = "SELECT UserId,OpenId,Nickname,Phone FROM [dbo].[WXUser] WHERE OpenId = @OpenId";
            List<WXUser> wXUserList = conn.Query<WXUser>(selectSql, new { OpenId = openId }).ToList();
            return wXUserList.Count;
        }


        public int AddPhone(string openId,string phone)
        {  
            string updateSql = "UPDATE [dbo].[WXUser] SET [Phone] = @Phone WHERE [OpenId] =@OpenId";
            var result = conn.Execute(updateSql, new { Phone=phone,OpenId=openId});
            return result;
        }


        public string SelUserPhoneByOpenId(string openId)
        {
            string selectSql = "SELECT UserId,OpenId,Nickname,Phone FROM [dbo].[WXUser] WHERE OpenId = @OpenId";
            List<WXUser> wXUserList = conn.Query<WXUser>(selectSql, new { OpenId = openId }).ToList();
            return wXUserList.FirstOrDefault().Phone;
        }
    }
}
