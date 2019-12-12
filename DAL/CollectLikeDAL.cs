using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class CollectLikeDAL
    {
        public int AddHelperUser(CollectLike collectLike)
        {
            string sql = "INSERT INTO [dbo].[CollectLike] " +
                "([UserShareId] ,[HelpOpenId] ,[Url] ,[ActivityName] ,[AddTime]) " +
                "VALUES (@UserShareId ,@HelpOpenId,@Url ,@ActivityName,GETDATE())";
            return DapperHelper<CollectLike>.Execute(sql, collectLike);
        }

        public CollectLike SelHelperUser(string helpOpenId,string userShareId,string actName)
        {
            string sql = "SELECT * FROM CollectLike " +
                "WHERE UserShareId=@userShareId AND HelpOpenId=@helpOpenId AND ActivityName=@actName";

            return DapperHelper<CollectLike>.Query(sql, new { helpOpenId, userShareId, actName }).FirstOrDefault();
        }

        public int SelHelperCount(string userShareId,string actName)
        {
            string sql = "SELECT * FROM CollectLike WHERE UserShareId=@userShareId AND ActivityName=@actName";
            return DapperHelper<CollectLike>.Query(sql, new {  userShareId, actName }).Count;
        }

        public List<HelpeRank> GetHelpRank()
        {
            string sql = "SELECT * FROM CollectLike a,ShareActivityUser b " +
                "WHERE a.UserShareId=b.UserShareId";
            return null;
         
        }
    }


    public class HelpeRank
    {
        public string Img { get; set; }
        public string UserShareId { get; set; }
        public int HelperCount { get; set; }
    }
}
