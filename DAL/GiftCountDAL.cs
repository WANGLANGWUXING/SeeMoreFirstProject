using Dapper;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL
{
    public class GiftCountDAL
    {
        private static IDbConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);

        /// <summary>
        /// 减少礼物实际数量
        /// </summary>
        /// <param name="GiftId">礼物id</param>
        /// <returns></returns>
        public int EditGiftCountByGiftId(int GiftId)
        {
            string updateSql = "UPDATE [dbo].[GiftCount] SET [Remainder] =  [Remainder] -1 WHERE [GiftId]=@GiftId";
            var result = conn.Execute(updateSql, new { GiftId = GiftId});
            return result;
        }


        public int GetGiftCountByGiftId(int GiftId)
        {
            string selSql = "select Remainder from GiftCount where GiftId=@GiftId";
            int res =  Convert.ToInt32( conn.ExecuteScalar(selSql, new { GiftId = GiftId }));
            return res;
        }


        public int GetGiftMoneyByGiftId(int GiftId)
        {
            string selSql = "select convert(int,Money*100) num from GiftCount where GiftId=@GiftId";
            int res = Convert.ToInt32(conn.ExecuteScalar(selSql, new { GiftId = GiftId }));
            return res;
        }

        public GiftCount GetGiftCountModelByGiftId(int GiftId)
        {
            string selSql = "select * from GiftCount where GiftId=@GiftId";
            GiftCount giftCount = conn.Query<GiftCount>(selSql, new { GiftId = GiftId }).FirstOrDefault();
            return giftCount;
        }

        public int GetGiftCountSumByActName(string actName)
        {
            string selSql = "select SUM(Remainder) c from GiftCount a,Gift b where a.GiftId=b.GiftId and ActivityName=@actName";
            int res = Convert.ToInt32(conn.ExecuteScalar(selSql, new { actName }));
            return res;
        }

    }
}
