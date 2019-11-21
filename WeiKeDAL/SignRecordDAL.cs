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
            string date = DateTime.Now.ToShortDateString();
            var selSql = "SELECT * FROM SignRecord where SId=@UId and  SignDate=@date";
            return DapperHelper<SignRecord>.Query(selSql, new { UId,date }).FirstOrDefault();
        }

        public List<SignRecord> GetSignRecords()
        {
            var selSql = "SELECT ROW_NUMBER() over(order by SignDate desc ) RowIndex,* FROM SignRecord";
            return DapperHelper<SignRecord>.Query(selSql,null);
        }

        public List<SignRecord> GetSignRecords(int UId)
        {
            var selSql = "SELECT ROW_NUMBER() over(order by SignDate desc ) RowIndex,* FROM SignRecord where SId=@UId";
            return DapperHelper<SignRecord>.Query(selSql, new { UId });
        }



    }
}
