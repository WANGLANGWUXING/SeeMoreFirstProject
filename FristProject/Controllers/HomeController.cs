using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FristProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public string GetData(string action)
        {
            bool IsSuccess = false;
            string Msg = "";
            dynamic viewProjects = null;
            if (action == "GetDataByWhereAndOrder")
            {
                // 页码
                string pageIndex = Request.Form["pageIndex"];

                int pageStart = 1 + (Convert.ToInt32(pageIndex) - 1) * 10;
                int pageEnd = Convert.ToInt32(pageIndex) * 10;
                string sql = "select * from (select row_number() over(order by  [CreateTime] ";
                // 时间排序方式
                string orderTime = Request.Form["orderTime"];
                if (!string.IsNullOrWhiteSpace(orderTime))
                {
                    if (orderTime == "desc")
                    {
                        sql += " desc";
                    }
                    else if (orderTime == "asc")
                    {
                        sql += " asc";
                    }
                    else
                    {
                        sql += " desc";
                    }
                }
                else
                {
                    sql += " desc";
                }
                sql += " ) as Rownumber,*from[SMProject] where 1 = 1 ";

                // 项目所有者
                string owner = Request.Form["owner"];
                if (!string.IsNullOrWhiteSpace(owner))
                {
                    sql += " and ProOwner=@owner";
                }
                //筛选条件
                string filter = Request.Form["filter"];
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    string[] keys = filter.Split(',');
                    sql += " and (";
                    for (int i = 0; i < keys.Length - 1; i++)
                    {
                        if (i == keys.Length - 2)
                        {
                            sql += " ProType like '%" + keys[i] + "%' ";
                        }
                        else
                        {
                            sql += " ProType like '%" + keys[i] + "%' or ";
                        }
                    }
                    sql += " ) ";
                    //filter = "%" + filter + "%";
                    //sql += " and ProType like @filter ";
                }
                // 搜索
                string keyWord = Request.Form["keyWord"];

                if (!string.IsNullOrWhiteSpace(keyWord))
                {

                    keyWord = "%" + keyWord + "%";
                    sql += " and ProTitle like @keyWord";
                }

                sql += " ) as temp where Rownumber between @pageStart and @pageEnd";
                viewProjects = ProjectDAL.ProjectDAL.SelProject(sql, new { pageStart, pageEnd, owner, filter, keyWord });
                IsSuccess = true;
                Msg = "查询成功！" + sql;

            }
            else if (action == "GetOwnerType")
            {
                string sql = "select * from SMProOwners order by OrderNum";
                viewProjects = ProjectDAL.ProjectDAL.SelOwner(sql);
                IsSuccess = true;
                Msg = "查询成功！" + sql;
            }
            else if (action == "GetProLabel")
            {
                string sql = "select * from [SMProType]  order by [OrderNum] ";
                viewProjects = ProjectDAL.ProjectDAL.SelType(sql);
                IsSuccess = true;
                Msg = "查询成功！" + sql;

            }
            else
            {

            }


            return JsonConvert.SerializeObject(new { Data = viewProjects, IsSuccess, Msg });
        }


        public ActionResult ProjectShow(string company)
        {
            ViewBag.Company = company;
            return View();
        }

        public string GetData(string action,string company)
        {
          

            bool IsSuccess = false;
            string Msg = "";
            dynamic viewProjects = null;
            if (string.IsNullOrWhiteSpace(company))
            {
                return JsonConvert.SerializeObject(new { Data = viewProjects, IsSuccess, Msg });
            }
          


            if (action == "GetDataByWhereAndOrder")
            {
                // 页码
                string pageIndex = Request.Form["pageIndex"];

                int pageStart = 1 + (Convert.ToInt32(pageIndex) - 1) * 10;
                int pageEnd = Convert.ToInt32(pageIndex) * 10;
                string sql = "select * from (select row_number() over(order by  [CreateTime] ";
                // 时间排序方式
                string orderTime = Request.Form["orderTime"];
                if (!string.IsNullOrWhiteSpace(orderTime))
                {
                    if (orderTime == "desc")
                    {
                        sql += " desc";
                    }
                    else if (orderTime == "asc")
                    {
                        sql += " asc";
                    }
                    else
                    {
                        sql += " desc";
                    }
                }
                else
                {
                    sql += " desc";
                }
                sql += " ) as Rownumber,* from [OtherProject] where 1 = 1 ";
                sql += " ";
                // 项目所有者
                string owner = Request.Form["owner"];
                if (!string.IsNullOrWhiteSpace(owner))
                {
                    sql += " and ProOwner=@owner";
                }
                //筛选条件
                string filter = Request.Form["filter"];
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    string[] keys = filter.Split(',');
                    sql += " and (";
                    for (int i = 0; i < keys.Length - 1; i++)
                    {
                        if (i == keys.Length - 2)
                        {
                            sql += " ProType like '%" + keys[i] + "%' ";
                        }
                        else
                        {
                            sql += " ProType like '%" + keys[i] + "%' or ";
                        }
                    }
                    sql += " ) ";
                    //filter = "%" + filter + "%";
                    //sql += " and ProType like @filter ";
                }
                // 搜索
                string keyWord = Request.Form["keyWord"];

                if (!string.IsNullOrWhiteSpace(keyWord))
                {

                    keyWord = "%" + keyWord + "%";
                    sql += " and ProTitle like @keyWord";
                }

                sql += " ) as temp where Rownumber between @pageStart and @pageEnd";
                viewProjects = ProjectDAL.ProjectDAL.SelProject(sql, new { pageStart, pageEnd, owner, filter, keyWord });
                IsSuccess = true;
                Msg = "查询成功！" + sql;

            }
            else if (action == "GetOwnerType")
            {
                string sql = "select * from OtherProOwners order by OrderNum";
                viewProjects = ProjectDAL.ProjectDAL.SelOwner(sql);
                IsSuccess = true;
                Msg = "查询成功！" + sql;
            }
            else if (action == "GetProLabel")
            {
                string sql = "select * from [OtherProType]  order by [OrderNum] ";
                viewProjects = ProjectDAL.ProjectDAL.SelType(sql);
                IsSuccess = true;
                Msg = "查询成功！" + sql;

            }
            else
            {

            }


            return JsonConvert.SerializeObject(new { Data = viewProjects, IsSuccess, Msg });
        }
    }
}