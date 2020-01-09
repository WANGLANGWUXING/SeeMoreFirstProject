using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class IsShareTableDAL
    {
        public IsShareTable GetShareTable(string openId, string actName)
        {
            string sql = "select * from IsShareTable where openId=@openId and ActivityName=@actName";
            return DapperHelper<IsShareTable>.Query(sql, new { openId, actName }).First();
        }

        public int AddShareLog(string openId,string actName)
        {
            string sql = "insert into IsShareTable(OpenId,IsShare,ActivityName,CreateTime) value(@openId,1,@actName,getdate())";
            return DapperHelper<IsShareTable>.Execute(sql, new { openId, actName });
        }
    }
}
