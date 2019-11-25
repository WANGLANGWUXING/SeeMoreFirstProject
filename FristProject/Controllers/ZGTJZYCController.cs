using DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FristProject.Controllers
{
    public class ZGTJZYCController : Controller
    {
        ZGTJVIPDAL zgtjDAL = new ZGTJVIPDAL();
        public string AddVIP(string openId, string name, string tel, string idCard, string area, string referrer)
        {
            int id = 0;
            string msg = "";
            // 先判断是否存在openId
            if (zgtjDAL.SelZGTJVIPByOpenId(openId) != null)
            {
                msg = "已存在记录";
            }
            else
            { //不存在添加
                zgtjDAL.AddZGTJVIP(new ZGTJVIP
                {
                    OpenId = openId,
                    Name = name,
                    Tel = tel,
                    IdCard = idCard,
                    Area = area,
                    Referrer = referrer
                });
                id = 1;
                msg = "添加成功";
            }

            return JsonConvert.SerializeObject(new { id, msg });

        }
    }
}