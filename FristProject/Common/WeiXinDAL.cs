using LitJson;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FristProject.Common
{
    public class WeiXinDAL
    {
        public string Title { get; set; }
        public string Appid { get; set; }
        public string Secret { get; set; }

        public string GetTicket()
        {
            string token = this.GetToken();
            string sQLString = "select * from weixinsign where name='" + this.Title + "'";
            DataTable dataTable = DbHelperSQL.Query(sQLString).Tables[0];
            string text;
            if (dataTable.Rows.Count > 0)
            {
                text = dataTable.Rows[0]["ticket"].ToString();
                DateTime dateTime = Convert.ToDateTime(dataTable.Rows[0]["addtime"].ToString());
                if (DateTime.Now >= dateTime.AddHours(1.0))
                {
                    sQLString = "delete from weixinsign where name='" + this.Title + "'";
                    DbHelperSQL.GetSingle(sQLString);
                    text = this.GetTicket();
                }
            }
            else
            {
                string json = Utils.getjson("https://api.weixin.qq.com/cgi-bin/ticket/getticket", "access_token=" + token + "&type=jsapi");
                JsonData jsonData = JsonMapper.ToObject(json);
                text = jsonData["ticket"].ToString();
                sQLString = string.Concat(new string[]
                {
                    "insert into weixinsign values('",
                    this.Title,
                    "','",
                    text,
                    "','",
                    DateTime.Now.ToString(),
                    "')"
                });
                DbHelperSQL.GetSingle(sQLString);
            }
            return text;
        }

        public string GetToken()
        {
            string sQLString = "select * from weixintoken where name='" + this.Title + "'";
            DataTable dataTable = DbHelperSQL.Query(sQLString).Tables[0];
            string text;
            if (dataTable.Rows.Count > 0)
            {
                text = dataTable.Rows[0]["token"].ToString();
                DateTime dateTime = Convert.ToDateTime(dataTable.Rows[0]["addtime"].ToString());
                if (DateTime.Now >= dateTime.AddHours(1.0))
                {
                    sQLString = "delete from weixintoken where name='" + this.Title + "'";
                    DbHelperSQL.GetSingle(sQLString);
                    text = this.GetToken();
                }
            }
            else
            {
                string json = Utils.getjson("https://api.weixin.qq.com/cgi-bin/token?", "grant_type=client_credential&appid=" + this.Appid + "&secret=" + this.Secret);
                JsonData jsonData = JsonMapper.ToObject(json);
                text = jsonData["access_token"].ToString();
                sQLString = string.Concat(new string[]
                {
                    "insert into weixintoken values('",
                    this.Title,
                    "','",
                    text,
                    "','",
                    DateTime.Now.ToString(),
                    "')"
                });
                DbHelperSQL.GetSingle(sQLString);
            }
            return text;
        }
    }
}