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



        public List<HelpeRank> GetHelpRank(string actName)
        {
            string sql = "select count(shareId) HelpCount,c.OpenId,c.Headimgurl UserImg,c.Nickname NickName " +
                "from  (select a.UserShareId,a.OpenId,a.UserImg Headimgurl,d.Nickname,a.ActivityName ,b.UserShareId shareId " +
                "from ShareActivityUser a " +
                "left join CollectLike b on a.UserShareId=b.UserShareId " +
                "left join WXUser d on a.OpenId=d.OpenId where a.ActivityName=@actName ) c " +
                "group by c.OpenId,c.Headimgurl,c.NickName " +
                "order by HelpCount desc";
            return DapperHelper<HelpeRank>.Query(sql,new { actName });
         
        }
    }


    public class HelpeRank
    {
        public string OpenId { get; set; }
        public string UserImg { get; set; }
        public string NickName { get; set; }

        public int HelpCount { get; set; }
    }
}
