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
        /// <summary>
        /// 判断是否有这条记录
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="ActivityName"></param>
        /// <returns></returns>
        public GiftLog SelGiftLog(string openId, string ActivityName)
        {
            string selectSql = "SELECT * FROM [dbo].[GiftLog] where OpenId=@OpenId and ActivityName=@ActivityName";

            GiftLog giftLog = conn.Query<GiftLog>(selectSql, new { OpenId = openId, ActivityName = ActivityName }).FirstOrDefault();
            return giftLog;
        }
        /// <summary>
        /// 判断自定义ID是否存在
        /// </summary>
        /// <param name="customNum"></param>
        /// <param name="ActivityName"></param>
        /// <returns></returns>
        public GiftLog SelGiftLogByCustomNum(string customNum, string ActivityName)
        {
            string selectSql = "SELECT * FROM [dbo].[GiftLog] where  ActivityName=@ActivityName and GiftCustomNum=@CustomNum";

            GiftLog giftLog = conn.Query<GiftLog>(selectSql, new { ActivityName = ActivityName, CustomNum = customNum }).FirstOrDefault();
            return giftLog;
        }
        /// <summary>
        /// 获取超过两小时但没有登记的记录
        /// </summary>
        /// <param name="actName"></param>
        /// <returns></returns>
        public List<GiftLog> SelGiftLogAfterTwoHour(string actName)
        {
            string selSql = "select * from GiftLog where  DATEDIFF(hour,AddTime,GETDATE())>2 and Name is null and ActivityName=@actName ";
            return DapperHelper<GiftLog>.Query(selSql, new { actName });
        }

        /// <summary>
        /// 删除礼物记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DelGiftLogById(int id)
        {
            string del = "delete from GiftLog where id=@id";
            return DapperHelper<GiftLog>.Execute(del, new { id });
        }








    }
}
