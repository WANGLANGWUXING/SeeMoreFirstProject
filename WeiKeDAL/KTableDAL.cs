﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiKeDAL
{
    public class KTableDAL
    {
        private static IDbConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["WeiKeConnectionString"]);


        public List<KTable> GetKTables()
        {

            string selectSql = "select ROW_NUMBER() over(order by id ) RowIndex,* from [KTable]";
            List<KTable> list = DapperHelper<KTable>.Query(selectSql,null);
            return list;
        }

        public KTable GetKTableByKid(string id)
        {

            string selectSql = "select * from [KTable] where id=@id";
            KTable t = DapperHelper<KTable>.Query(selectSql, new { id }).FirstOrDefault();
            return t;
        }

    }
}
