using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public class GiftLogDAL
    {

        private static IDbConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);

        public int AddGiftLog(GiftLog giftLog)
        {
            string insertSql = "INSERT INTO [dbo].[GiftLog] ([OpenId] ,[NickName] ,[ActivityName],[GiftId],[GiftName] ,[GiftDesc],[AddTime],[Name],[Telphone],[GiftCustomNum]) VALUES (@OpenId ,@NickName,@ActivityName,@GiftId,@GiftName,@GiftDesc,Getdate(),@Name,@Telphone,@GiftCustomNum)";
            var result = conn.Execute(insertSql, giftLog);
            return result;
        }


        public int EditGiftLog(string openId, string activityName, string name, string telphone)
        {
            string editSql = "UPDATE [dbo].[GiftLog] SET [Name]=@Name ,[Telphone]=@Telphone WHERE OpenId=@OpenId and ActivityName=@ActivityName";
            var result = conn.Execute(editSql, new { Name = name, Telphone = telphone, OpenId = openId, ActivityName = activityName });
            return result;
        }

        public GiftLog SelGiftLog(string openId, string ActivityName)
        {
            string selectSql = "SELECT * FROM [dbo].[GiftLog] where OpenId=@OpenId and ActivityName=@ActivityName";

            GiftLog giftLog = conn.Query<GiftLog>(selectSql, new { OpenId = openId, ActivityName = ActivityName }).FirstOrDefault();
            return giftLog;
        }

        public GiftLog SelGiftLogByCustomNum(string customNum, string ActivityName)
        {
            string selectSql = "SELECT * FROM [dbo].[GiftLog] where  ActivityName=@ActivityName and GiftCustomNum=@CustomNum";

            GiftLog giftLog = conn.Query<GiftLog>(selectSql, new { ActivityName = ActivityName, CustomNum = customNum }).FirstOrDefault();
            return giftLog;
        }



    }
}
