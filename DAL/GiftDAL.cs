using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public class GiftDAL
    {
        private static IDbConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);

        public List<Gift> GetGifts()
        {
            
            string selectSql = "SELECT [GiftId],[GiftName] ,[GiftRealName] ,[GiftPY] ,[GiftDesc],[GiftUrl] ,[StartTime] ,[EndTime] ,[Unit],[Probability] FROM  [dbo].[Gift]";
            List<Gift> giftList = conn.Query<Gift>(selectSql).ToList();
            return giftList;
        }


        public List<Gift> GetGifts(string starttime, string endtime)
        {
            
            string selectSql = "SELECT [GiftId],[GiftName] ,[GiftRealName] ,[GiftPY] ,[GiftDesc],[GiftUrl] ,[StartTime] ,[EndTime] ,[Unit],[Probability] FROM  [dbo].[Gift] WHERE EndTime=@EndTime";
            List<Gift> giftList = conn.Query<Gift>(selectSql, new { StartTime = starttime, EndTime = endtime }).ToList();
            return giftList;
        }

        public Gift GetGiftByGiftId(int giftId)
        {
          
            string selectSql = "SELECT [GiftId],[GiftName] ,[GiftRealName] ,[GiftPY] ,[GiftDesc],[GiftUrl] ,[StartTime] ,[EndTime] ,[Unit],[Probability] FROM  [dbo].[Gift] WHERE GiftId=@GiftId";
            Gift gift= conn.Query<Gift>(selectSql, new { GiftId = giftId }).FirstOrDefault();
            return gift;
        }

        public List<Gift> GetGiftsByAcitvityName(string ActivityName)
        {
            string selectSql = "SELECT * FROM  [dbo].[Gift] WHERE ActivityName=@ActivityName";
            List<Gift> giftList = conn.Query<Gift>(selectSql, new { ActivityName = ActivityName }).ToList();
            return giftList;
        }
        public List<Gift> GetGiftsByAcitvityNameAndType(string ActivityName,string Type)
        {
            string selectSql = "SELECT * FROM  [dbo].[Gift] WHERE ActivityName=@ActivityName and GiftDesc=@Type";
            List<Gift> giftList = conn.Query<Gift>(selectSql, new { ActivityName, Type }).ToList();
            return giftList;
        }
        public List<Gift> GetGiftsByAcitvityNameIsExist(string ActivityName)
        {
            string selectSql = "SELECT * FROM  [dbo].[Gift] WHERE ActivityName=@ActivityName and giftid in(select giftid from GiftCount where Remainder>0)";
            List<Gift> giftList = conn.Query<Gift>(selectSql, new { ActivityName = ActivityName }).ToList();
            return giftList;
        }


    }
}
