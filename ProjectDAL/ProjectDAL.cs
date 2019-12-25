
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProjectDAL
{
    public class ProjectDAL
    {
        //private static readonly IDbConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["connstr"]);

        public static List<ViewProject> SelProject(string sql,object o)
        {
            //string selectSql = "SELECT * FROM SMProject";


            List<ViewProject> viewProjects = DapperHelper<ViewProject>.Query(sql,o);
            return viewProjects;
        }


        public static List<ProType> SelType(string sql,object o)
        {
            List<ProType> proTypes = DapperHelper<ProType>.Query(sql, o);
            return proTypes;
        }


        public static List<ProOwners> SelOwner(string sql, object o)
        {
            List<ProOwners> proOwners = DapperHelper<ProOwners>.Query(sql, o);
            return proOwners;
        }
    }
}