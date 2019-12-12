
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

        public int AddUserS(WXUser wXUser)
        {
            string sql = "SELECT * FROM WXUser WHERE OpenId=@openId";

            WXUser userModel = DapperHelper<WXUser>.Query(sql, new { openId = wXUser.OpenId }).FirstOrDefault();

            if (userModel != null)
            {
                // 如果已经添加过，查看图片和用户名是否相同
                // 相同返回1
                if (wXUser.Nickname.Equals(userModel.Nickname)
                    && wXUser.Headimgurl.Equals(userModel.Headimgurl)) return 1;
                // 不同返回2
                sql = "UPDATE [dbo].[WXUser] SET  [Nickname] =@Nickname ,[Headimgurl] =@Headimgurl WHERE [OpenId]=@OpenId";
                return DapperHelper<WXUser>.Execute(sql, wXUser);

            }
            else
            {
                //没有添加过，进行添加
                sql = "INSERT INTO [dbo].[WXUser] ([OpenId] ,[Nickname] ,[Headimgurl] ,[CreateTime]) VALUES (@OpenId,@Nickname,@Headimgurl,GETDATE())";
                return DapperHelper<WXUser>.Execute(sql, wXUser);

            }

        }


        public int SelUserByOpenId(string openId)
        {

            string selectSql = "SELECT UserId,OpenId,Nickname,Phone FROM [dbo].[WXUser] WHERE OpenId = @OpenId";
            List<WXUser> wXUserList = conn.Query<WXUser>(selectSql, new { OpenId = openId }).ToList();
            return wXUserList.Count;
        }


        public int AddPhone(string openId, string phone)
        {
            string updateSql = "UPDATE [dbo].[WXUser] SET [Phone] = @Phone WHERE [OpenId] =@OpenId";
            var result = conn.Execute(updateSql, new { Phone = phone, OpenId = openId });
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
