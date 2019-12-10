using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class ZGTJVIPDAL
    {
        public int AddZGTJVIP(ZGTJVIP zgtjVIP)
        {
            string insertSql = "INSERT INTO [dbo].[ZGTJVIP] " +
                " ([OpenId] ,[Name] ,[Tel] ,[IdCard] ," +
                "[Area] ,[Referrer],[RefTel] ,[CreateTime]) " +
                "VALUES (@OpenId ,@Name  ,@Tel ,@IdCard ,@Area  ,@Referrer,@RefTel ,GETDATE())"; ;

            return DapperHelper<ZGTJVIP>.Execute(insertSql, zgtjVIP);
        }

        public ZGTJVIP SelZGTJVIPByOpenId(string openId)
        {
            string selSql = "select * from ZGTJVIP where OpenId=@openId";

            return DapperHelper<ZGTJVIP>.Query(selSql, new { openId }).FirstOrDefault();
        }

    }
}
