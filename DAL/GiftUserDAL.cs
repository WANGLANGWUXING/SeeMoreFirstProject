using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;


namespace DAL
{
    public class GiftUserDAL
    {
        private static IDbConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);

        public int AddGiftUser(GiftUser giftUser)
        {
            string insertSql = "INSERT INTO [dbo].[GiftUser] ([GiftId] ,[OpenId] ,[GiftShowId] ,[GetTime]) VALUES (@GiftId ,@OpenId ,@GiftShowId ,GETDATE())";
            var result = conn.Execute(insertSql, giftUser);
            return result;
        }

        public GiftUser SelGiftUserByOpenId(string openId,string startTime,string endTime)
        {
            string selectSql = "SELECT [GiftUserId] ,[GiftId] ,[OpenId] ,[GiftShowId] ,[GetTime] FROM [dbo].[GiftUser] where OpenId=@OpenId and GetTime between @StartTime and @EndTime";
            GiftUser giftUser = conn.Query<GiftUser>(selectSql, new { OpenId = openId , StartTime= startTime , EndTime = endTime }).FirstOrDefault();
            return giftUser;
        }

    }
}
