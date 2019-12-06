using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class GameScoreDAL
    {
        public int AddGameScore(GameScore gameScore)
        {
            string sql = "INSERT INTO [dbo].[GameScore] ([OpenId] ,[Score] ,[ActivityName] ,[AddTime]) VALUES (@OpenId,@Score ,@ActivityName ,GETDATE())";
            return DapperHelper<GameScore>.Execute(sql, gameScore);
        }

        public GameScore SelGameScore(string openId,string actName)
        {
            string sql = "SELECT * FROM GameScore WHERE OpenId=@openId and ActivityName=@actName";
            return DapperHelper<GameScore>.Query(sql, new { openId, actName }).FirstOrDefault();
        }

        public int EditGameScore(GameScore gameScore)
        {
            string sql = "UPDATE GameScore SET Score=@Score WHERE OpenId=@OpenId AND ActivityName=@ActivityName ";
            return DapperHelper<GameScore>.Execute(sql, gameScore);
        }
    }
}
