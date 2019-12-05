using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class ShareActivityUserDAL
    {
        public int AddShareUser(ShareActivityUser shareActivityUser)
        {
            string sql = "INSERT INTO [dbo].[ShareActivityUser] ([UserShareId] ,[OpenId] ,[UserImg] ,[NickName] ,[ActivityName] ,[AddTime]) VALUES (@UserShareId ,@OpenId ,@UserImg ,@NickName ,@ActivityName ,GETDATE())";
            return DapperHelper<ShareActivityUser>.Execute(sql, shareActivityUser);
        }


        public int EditShareUser(string openId,string img,string name,string actName)
        {
            string sql= "UPDATE [dbo].[ShareActivityUser] SET [UserImg] = @img ," +
                "[NickName] = @name  WHERE OpenId=@openId AND ActivityName=@actName";

            return DapperHelper<ShareActivityUser>.Execute(sql, new { img, name, openId, actName });
        }


        public ShareActivityUser SelShareUser(string openId,string actName)
        {
            string sql = "select * from ShareActivityUser WHERE OpenId=@openId AND ActivityName=@actName";
            return DapperHelper<ShareActivityUser>.Query(sql, new { openId, actName }).FirstOrDefault();
        }
        
    }
}
