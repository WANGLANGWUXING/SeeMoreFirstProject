using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiKeDAL
{
    public class SelKTableDAL
    {
        public int AddSelKTable(int KId,int UId)
        {
            var insertSql = "INSERT INTO [dbo].[SelKTable] ([UId]  ,[KId] ,[CreateTime]) VALUES(@UId, @KId, GETDATE())";

            return DapperHelper<SelKTable>.Execute(insertSql, new { UId, KId });
        }

        public List<SelKTable> GetSelKTables(int UId)
        {
            var selSql = "SELECT ROW_NUMBER() over(order by a.id ) RowIndex,a.*,b.KName,b.KType FROM SelKTable a ,KTable b where a.KId=b.Id and a.UId =@UId";
            return DapperHelper<SelKTable>.Query(selSql, new { UId });


        }
    }
}
