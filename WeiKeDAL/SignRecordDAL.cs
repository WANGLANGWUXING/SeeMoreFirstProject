using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiKeDAL
{
    public class SignRecordDAL
    {

        public int AddSign(int UId)
        {
            string date = DateTime.Now.ToShortDateString();
            var insertSql = "INSERT INTO [dbo].[SignRecord] ([SId]  ,[SignDate] ,[SignTime]) VALUES(@UId, @date, GETDATE()) ";
            return DapperHelper<SignRecord>.Execute(insertSql, new { UId, date });
        }

        public SignRecord SelSignRecordByUId(int UId)
        {
            var selSql = "SELECT * FROM SignRecord where SId=@UId";
            return DapperHelper<SignRecord>.Query(selSql, new { UId }).FirstOrDefault();
        }

        public List<SignRecord> GetSignRecords()
        {
            var selSql = "SELECT * FROM SignRecord";
            return DapperHelper<SignRecord>.Query(selSql,null);
        }
    }
}
