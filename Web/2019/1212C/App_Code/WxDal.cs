using LitJson;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

/// <summary>
/// WxDal 的摘要说明
/// </summary>
public class WxDal
{
    public WxDal()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    public string appid = "";

    public string secret = "";

    public string title = "";

    public string key = "";

    public string mch_id = "";

    public WxDal(string wt)
    {
        this.title = wt;
        this.appid = ConfigurationManager.AppSettings["appid_" + wt].ToString();
        this.secret = ConfigurationManager.AppSettings["secret_" + wt].ToString();
        try
        {
            this.key = ConfigurationManager.AppSettings["key_" + wt].ToString();
            this.mch_id = ConfigurationManager.AppSettings["password_" + wt].ToString();
        }
        catch (Exception)
        {
        }
    }

    public WxUser Auth(string type)
    {
        WxUser result = new WxUser();
        if (type != null)
        {
            if (!(type == "base"))
            {
                if (type == "userinfo")
                {
                    result = this.AuthUserInfo();
                }
            }
            else
            {
                result = this.AuthBase();
            }
        }
        return result;
    }

    public WxUser AuthBase()
    {
        WxUser result = new WxUser();
        if (HttpContext.Current.Session["openid_" + this.title] == null)
        {
            string text = HttpContext.Current.Request["code"];
            if (!string.IsNullOrEmpty(text))
            {
                result = this.GetWeixinBase(text);
            }
            else
            {
                HttpContext.Current.Response.Redirect(string.Concat(new string[]
                {
                        "https://open.weixin.qq.com/connect/oauth2/authorize?appid=",
                        this.appid,
                        "&redirect_uri=",
                        HttpContext.Current.Request.Url.AbsoluteUri,
                        "&response_type=code&scope=snsapi_base&state=1#wechat_redirect"
                }));
            }
        }
        else
        {
            result = (WxUser)HttpContext.Current.Session["openid_" + this.title];
        }
        return result;
    }

    public WxUser GetWeixinBase(string code)
    {
        WxUser WxUser = new WxUser();
        try
        {
            string json = common.getjson("https://api.weixin.qq.com/sns/oauth2/access_token", string.Concat(new string[]
            {
                    "appid=",
                    this.appid,
                    "&secret=",
                    this.secret,
                    "&code=",
                    code,
                    "&grant_type=authorization_code"
            }));
            JsonData jsonData = JsonMapper.ToObject(json);
            string text = jsonData["access_token"].ToString();
            string openId = jsonData["openid"].ToString();
            WxUser.OpenId = openId;
            HttpContext.Current.Session["openid_" + this.title] = WxUser;
        }
        catch (Exception var_5_BB)
        {
            string text2 = HttpContext.Current.Request.Url.AbsoluteUri;
            text2 = text2.Substring(0, text2.IndexOf("code="));
            HttpContext.Current.Response.Redirect(text2);
        }
        return WxUser;
    }

    public WxUser AuthUserInfo()
    {
        WxUser result = new WxUser();
        if (HttpContext.Current.Session["weixin_" + this.title] == null)
        {
            string text = HttpContext.Current.Request["code"];
            if (!string.IsNullOrEmpty(text))
            {
                result = this.GetWeixinInfo(text);
            }
            else
            {
                HttpContext.Current.Response.Redirect(string.Concat(new string[]
                {
                        "https://open.weixin.qq.com/connect/oauth2/authorize?appid=",
                        this.appid,
                        "&redirect_uri=",
                        HttpContext.Current.Request.Url.AbsoluteUri,
                        "&response_type=code&scope=snsapi_userinfo&state=1#wechat_redirect"
                }));
            }
        }
        else
        {
            result = (WxUser)HttpContext.Current.Session["weixin_" + this.title];
        }
        return result;
    }

    public WxUser GetWeixinInfo(string code)
    {
        WxUser WxUser = new WxUser();
        string text = "";
        try
        {
            string json = common.getjson("https://api.weixin.qq.com/sns/oauth2/access_token", string.Concat(new string[]
            {
                    "appid=",
                    this.appid,
                    "&secret=",
                    this.secret,
                    "&code=",
                    code,
                    "&grant_type=authorization_code"
            }));
            JsonData jsonData = JsonMapper.ToObject(json);
            string str = jsonData["access_token"].ToString();
            text = jsonData["openid"].ToString();
            string json2 = common.getjson("https://api.weixin.qq.com/sns/userinfo", "access_token=" + str + "&openid=" + text);
            JsonData jsonData2 = JsonMapper.ToObject(json2);
            WxUser.OpenId = text;
            WxUser.NickName = jsonData2["nickname"].ToString().Replace("'", "’");
            WxUser.Headimgurl = jsonData2["headimgurl"].ToString();
            try
            {
                WxUser.Subscribe = new int?(int.Parse(jsonData2["subscribe"].ToString()));
            }
            catch (Exception)
            {
            }
            try
            {
                WxUser.Sex = new int?(int.Parse(jsonData2["sex"].ToString()));
            }
            catch (Exception)
            {
            }
            try
            {
                WxUser.Language = jsonData2["language"].ToString();
            }
            catch (Exception ex)
            {
                WxUser.Language = ex.Message;
            }
            try
            {
                WxUser.City = jsonData2["city"].ToString();
            }
            catch (Exception ex2)
            {
                WxUser.City = ex2.Message;
            }
            try
            {
                WxUser.Province = jsonData2["province"].ToString();
            }
            catch (Exception ex3)
            {
                WxUser.Province = ex3.Message;
            }
            try
            {
                WxUser.Country = jsonData2["country"].ToString();
            }
            catch (Exception ex4)
            {
                WxUser.Country = ex4.Message;
            }
            try
            {
                WxUser.Subscribe_Time = new DateTime?(common.ConvertIntDateTime(jsonData2["Subscribe_Time"].ToString()));
            }
            catch (Exception var_11_23F)
            {
                WxUser.Subscribe_Time = new DateTime?(DateTime.Now);
            }
            try
            {
                WxUser.Remark = jsonData2["remark"].ToString();
            }
            catch (Exception ex5)
            {
                WxUser.Remark = ex5.Message;
            }
            try
            {
                WxUser.GroupId = new int?(int.Parse(jsonData2["groupid"].ToString()));
            }
            catch (Exception)
            {
            }
            try
            {
                WxUser.Title = this.title;
            }
            catch (Exception ex6)
            {
                WxUser.Title = ex6.Message;
            }
            HttpContext.Current.Session["weixin_" + this.title] = WxUser;
            if (WxUser.Exists(text))
            {
                WxUser.Update();
            }
            else
            {
                WxUser.Add();
            }
        }
        catch (Exception var_14_322)
        {
            string text2 = HttpContext.Current.Request.Url.AbsoluteUri;
            text2 = text2.Substring(0, text2.IndexOf("code="));
            HttpContext.Current.Response.Redirect(text2);
        }
        return WxUser;
    }

    public string GetToken()
    {
        string sQLString = "select * from weixintoken where name='" + this.title + "'";
        DataTable dataTable = DbHelperSQL.Query(sQLString).Tables[0];
        string text;
        if (dataTable.Rows.Count > 0)
        {
            text = dataTable.Rows[0]["token"].ToString();
            DateTime dateTime = Convert.ToDateTime(dataTable.Rows[0]["addtime"].ToString());
            if (DateTime.Now >= dateTime.AddHours(1.0))
            {
                sQLString = "delete from weixintoken where name='" + this.title + "'";
                DbHelperSQL.GetSingle(sQLString);
                text = this.GetToken();
            }
        }
        else
        {
            string json = common.getjson("https://api.weixin.qq.com/cgi-bin/token?", "grant_type=client_credential&appid=" + this.appid + "&secret=" + this.secret);
            JsonData jsonData = JsonMapper.ToObject(json);
            text = jsonData["access_token"].ToString();
            sQLString = string.Concat(new string[]
            {
                    "insert into weixintoken values('",
                    this.title,
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

    public string GetTicket()
    {
        string token = this.GetToken();
        string sQLString = "select * from weixinsign where name='" + this.title + "'";
        DataTable dataTable = DbHelperSQL.Query(sQLString).Tables[0];
        string text;
        if (dataTable.Rows.Count > 0)
        {
            text = dataTable.Rows[0]["ticket"].ToString();
            DateTime dateTime = Convert.ToDateTime(dataTable.Rows[0]["addtime"].ToString());
            if (DateTime.Now >= dateTime.AddHours(1.0))
            {
                sQLString = "delete from weixinsign where name='" + this.title + "'";
                DbHelperSQL.GetSingle(sQLString);
                text = this.GetTicket();
            }
        }
        else
        {
            string json = common.getjson("https://api.weixin.qq.com/cgi-bin/ticket/getticket", "access_token=" + token + "&type=jsapi");
            JsonData jsonData = JsonMapper.ToObject(json);
            text = jsonData["ticket"].ToString();
            sQLString = string.Concat(new string[]
            {
                    "insert into weixinsign values('",
                    this.title,
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

    public JsonData GetSign(string url)
    {
        JsonData jsonData = new JsonData();
        string ticket = this.GetTicket();
        string text = common.md5str16(Guid.NewGuid().ToString());
        int num = common.ConvertDateTimeInt(DateTime.Now);
        string pwd = string.Concat(new object[]
        {
                "jsapi_ticket=",
                ticket,
                "&noncestr=",
                text,
                "&timestamp=",
                num,
                "&url=",
                url
        });
        string sHA = common.GetSHA(pwd);
        jsonData["timestamp"] = num;
        jsonData["noncestr"] = text;
        jsonData["sign"] = sHA;
        return jsonData;
    }

    public bool CheckGZ(string openid)
    {
        string token = this.GetToken();
        string text = common.getjson("https://api.weixin.qq.com/cgi-bin/user/info", "access_token=" + token + "&openid=" + openid);
        if (!text.Contains("subscribe"))
        {
            string sQLString = "delete from weixintoken where name='" + this.title + "'";
            DbHelperSQL.GetSingle(sQLString);
            token = this.GetToken();
            text = common.getjson("https://api.weixin.qq.com/cgi-bin/user/info", "access_token=" + token + "&openid=" + openid);
        }
        JsonData jsonData = JsonMapper.ToObject(text);
        string a = jsonData["subscribe"].ToString();
        bool result;
        if (a == "1")
        {
            string sQLString = string.Concat(new string[]
            {
                    "update WxUser set subscribe=1 where openid='",
                    openid,
                    "' and title='",
                    this.title,
                    "'"
            });
            DbHelperSQL.GetSingle(sQLString);
            result = true;
        }
        else
        {
            result = false;
        }
        return result;
    }

    public string SendMoney(string openid, string name, int money, string wishing, string act_name, string remark)
    {
        Random random = new Random();
        return this.PayMoney(openid, name, name, money, money, money, wishing, act_name, random.Next(1000, 9999).ToString(), remark, "", "", "", "");
    }

    public string PayMoney(string re_openid, string nick_name, string send_name, int total_amount, int min_value, int max_value, string wishing, string act_name, string act_id, string remark, string logo_imgurl, string share_content, string share_url, string share_imgurl)
    {
        string posturl = "https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack";
        string text = common.md5str32(Guid.NewGuid().ToString());
        string text2 = this.Get_BillNo(this.mch_id);
        string text3 = this.appid;
        int num = 1;
        string userHostAddress = HttpContext.Current.Request.UserHostAddress;
        string text4 = "act_name=" + act_name;
        text4 = text4 + "&client_ip=" + userHostAddress;
        text4 = text4 + "&max_value=" + max_value.ToString();
        text4 = text4 + "&mch_billno=" + text2;
        text4 = text4 + "&mch_id=" + this.mch_id;
        text4 = text4 + "&min_value=" + min_value.ToString();
        text4 = text4 + "&nick_name=" + nick_name;
        text4 = text4 + "&nonce_str=" + text;
        text4 = text4 + "&re_openid=" + re_openid;
        text4 = text4 + "&remark=" + remark;
        text4 = text4 + "&send_name=" + send_name;
        text4 = text4 + "&total_amount=" + total_amount;
        text4 = text4 + "&total_num=" + num;
        text4 = text4 + "&wishing=" + wishing;
        text4 = text4 + "&wxappid=" + text3;
        string s = text4 + "&key=" + this.key;
        MD5 mD = MD5.Create();
        byte[] array = mD.ComputeHash(Encoding.UTF8.GetBytes(s));
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < array.Length; i++)
        {
            stringBuilder.Append(array[i].ToString("X2"));
        }
        string str = stringBuilder.ToString();
        StringBuilder stringBuilder2 = new StringBuilder();
        stringBuilder2.Append("<xml>");
        stringBuilder2.Append("<sign><![CDATA[" + str + "]]></sign>");
        stringBuilder2.Append("<mch_billno><![CDATA[" + text2 + "]]></mch_billno>");
        stringBuilder2.Append("<mch_id><![CDATA[" + this.mch_id + "]]></mch_id>");
        stringBuilder2.Append("<wxappid><![CDATA[" + text3 + "]]></wxappid>");
        stringBuilder2.Append("<nick_name><![CDATA[" + nick_name + "]]></nick_name>");
        stringBuilder2.Append("<send_name><![CDATA[" + send_name + "]]></send_name>");
        stringBuilder2.Append("<re_openid><![CDATA[" + re_openid + "]]></re_openid>");
        stringBuilder2.Append("<total_amount><![CDATA[" + total_amount + "]]></total_amount>");
        stringBuilder2.Append("<min_value><![CDATA[" + min_value + "]]></min_value>");
        stringBuilder2.Append("<max_value><![CDATA[" + max_value + "]]></max_value>");
        stringBuilder2.Append("<total_num><![CDATA[" + num + "]]></total_num>");
        stringBuilder2.Append("<wishing><![CDATA[" + wishing + "]]></wishing>");
        stringBuilder2.Append("<client_ip><![CDATA[" + userHostAddress + "]]></client_ip>");
        stringBuilder2.Append("<act_name><![CDATA[" + act_name + "]]></act_name>");
        stringBuilder2.Append("<remark><![CDATA[" + remark + "]]></remark>");
        stringBuilder2.Append("<nonce_str><![CDATA[" + text + "]]></nonce_str>");
        stringBuilder2.Append("</xml>");
        string value = stringBuilder2.ToString();
        string text5 = this.PostPage(posturl, stringBuilder2.ToString());
        string sQLString = "insert into weixin_pay (openid,orderno,sign,res,addtime,ip,result) values(@openid,@orderno,@sign,@res,@addtime,@ip,@result)";
        SqlParameter[] array2 = new SqlParameter[]
        {
                new SqlParameter("@openid", SqlDbType.NVarChar, 50),
                new SqlParameter("@orderno", SqlDbType.NVarChar, 50),
                new SqlParameter("@sign", SqlDbType.Text),
                new SqlParameter("@res", SqlDbType.Text),
                new SqlParameter("@addtime", SqlDbType.DateTime),
                new SqlParameter("@ip", SqlDbType.NVarChar, 50),
                new SqlParameter("@result", SqlDbType.NVarChar, 50)
        };
        array2[0].Value = re_openid;
        array2[1].Value = text2;
        array2[2].Value = value;
        array2[3].Value = text5;
        array2[4].Value = DateTime.Now;
        array2[5].Value = HttpContext.Current.Request.UserHostAddress;
        string text6;
        if (text5.Contains("发放成功"))
        {
            text6 = "红包发放成功";
        }
        else
        {
            text6 = "红包发放失败";
        }
        array2[6].Value = text6;
        DbHelperSQL.GetSingle(sQLString, array2);
        return text6;
    }

    public string Get_BillNo(string mch_id2)
    {
        Random random = new Random();
        return mch_id2 + DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next(1000, 9999).ToString();
    }

    /// <summary>
    /// 获取支付配置
    /// </summary>
    /// <param name="attach">说明</param>
    /// <param name="body">商品或支付单简要描述 </param>
    /// <param name="openid">用户的Openid</param>
    /// <param name="total_fee">订单总金额</param>
    /// <param name="notify_url">回调地址</param>
    /// <param name="out_trade_no">订单号</param>
    /// <returns></returns>
    public JsonData GetPayCofig(string attach, string body, string openid, int total_fee, string notify_url, string out_trade_no)
    {
        var url = "https://api.mch.weixin.qq.com/pay/unifiedorder";

        var key = this.key;
        var appid = this.appid;
        //var attach = "微信支付测试";
        //var body = "支付测试";
        var mch_id = this.mch_id;
        var nonce_str = common.md5str32(Guid.NewGuid().ToString());
        if (notify_url == "")
        {
            notify_url = "http://wxsx.swwlotus.com/dal/backPayMoney.aspx";
        }
        if (out_trade_no == "")
        {
            out_trade_no = Get_BillNo(mch_id);
        }
        var spbill_create_ip = HttpContext.Current.Request.UserHostAddress;
        var timeStamp = common.ConvertDateTimeInt(DateTime.Now);
        //var total_fee = 1;
        var trade_type = "JSAPI";


        //var stringA = "act_id=" + act_id;
        var stringA = "appid=" + appid;
        stringA += "&attach=" + attach;
        stringA += "&body=" + body;
        stringA += "&mch_id=" + mch_id;
        stringA += "&nonce_str=" + nonce_str;
        stringA += "&notify_url=" + notify_url;
        stringA += "&openid=" + openid;
        stringA += "&out_trade_no=" + out_trade_no;
        //stringA += "&product_id =123";
        stringA += "&spbill_create_ip=" + spbill_create_ip;
        stringA += "&timeStamp=" + timeStamp;
        stringA += "&total_fee=" + total_fee.ToString();
        stringA += "&trade_type=" + trade_type;
        var stringSignTemp = stringA + "&key=" + key;

        var sign = "";
        MD5 md5 = MD5.Create();
        byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(stringSignTemp));

        StringBuilder results = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            results.Append(bytes[i].ToString("X2"));
        }
        sign = results.ToString().ToUpper();
        //sign = SiteFun.MD5str32(stringSignTemp);

        var strXml = new StringBuilder();

        strXml.Append("<xml>");
        strXml.Append("<appid><![CDATA[" + appid + "]]></appid>");
        strXml.Append("<attach><![CDATA[" + attach + "]]></attach>");
        strXml.Append("<body><![CDATA[" + body + "]]></body>");
        strXml.Append("<mch_id><![CDATA[" + mch_id + "]]></mch_id>");
        strXml.Append("<nonce_str><![CDATA[" + nonce_str + "]]></nonce_str>");
        strXml.Append("<notify_url><![CDATA[" + notify_url + "]]></notify_url>");
        strXml.Append("<openid><![CDATA[" + openid + "]]></openid>");
        strXml.Append("<out_trade_no><![CDATA[" + out_trade_no + "]]></out_trade_no>");
        //strXml.Append("<product_id><![CDATA[123]]></product_id>");
        strXml.Append("<spbill_create_ip><![CDATA[" + spbill_create_ip + "]]></spbill_create_ip>");
        strXml.Append("<timeStamp><![CDATA[" + timeStamp + "]]></timeStamp>");
        strXml.Append("<total_fee><![CDATA[" + total_fee + "]]></total_fee>");
        strXml.Append("<trade_type><![CDATA[" + trade_type + "]]></trade_type>");
        strXml.Append("<sign><![CDATA[" + sign + "]]></sign>");
        strXml.Append("</xml>");

        //var xml = strXml.ToString();

        //var xml = new String(strXml.ToString().getBytes(), "ISO8859-1");
        var result = PostPage(url, strXml.ToString());


        var return_code = result.Substring(result.IndexOf("<return_code><![CDATA[") + 22, result.IndexOf("]]></return_code>") - (result.IndexOf("<return_code>") + 22));
        var prepay_id = "";
        if (return_code == "SUCCESS")
        {
            prepay_id = result.Substring(result.IndexOf("<prepay_id><![CDATA[") + 20, result.IndexOf("]]></prepay_id>") - (result.IndexOf("<prepay_id>") + 20));
        }

        var noncestr = common.md5str16(Guid.NewGuid().ToString());
        var timestamp = common.ConvertDateTimeInt(DateTime.Now);

        var stringB = "appId=" + this.appid;
        stringB += "&nonceStr=" + noncestr;
        stringB += "&package=prepay_id=" + prepay_id;
        stringB += "&signType=MD5";
        stringB += "&timeStamp=" + timestamp;
        //var stringSignTemp2 = stringB + "&key=a7e36089a1d936f57e61eca68a7d5beb";


        var stringSignTemp2 = "appId=" + this.appid + "&nonceStr=" + noncestr + "&package=prepay_id=" + prepay_id + "&signType=MD5&timeStamp=" + timestamp + "&key=" + this.key;
        var sign2 = "";
        //MD5 md5 = MD5.Create();
        byte[] bytes2 = md5.ComputeHash(Encoding.UTF8.GetBytes(stringSignTemp2));

        StringBuilder results2 = new StringBuilder();
        for (int i = 0; i < bytes2.Length; i++)
        {
            results2.Append(bytes2[i].ToString("X2"));
        }
        sign2 = results2.ToString();

        JsonData jd = new JsonData();
        jd["prepay_id"] = prepay_id;
        jd["nonceStr"] = noncestr;
        jd["timeStamp"] = timestamp;
        jd["paySign"] = sign2;
        return jd;
    }

    public string PostPage(string posturl, string postData)
    {
        Encoding uTF = Encoding.UTF8;
        byte[] bytes = uTF.GetBytes(postData);
        string fileName = ConfigurationManager.AppSettings["certPath_" + this.title].ToString();
        string password = ConfigurationManager.AppSettings["password_" + this.title].ToString();
        X509Certificate2 value = new X509Certificate2(fileName, password, X509KeyStorageFlags.MachineKeySet);
        HttpWebRequest httpWebRequest = WebRequest.Create(posturl) as HttpWebRequest;
        CookieContainer cookieContainer = new CookieContainer();
        httpWebRequest.CookieContainer = cookieContainer;
        httpWebRequest.AllowAutoRedirect = true;
        httpWebRequest.Method = "POST";
        httpWebRequest.ContentType = "text/xml";
        httpWebRequest.ContentLength = (long)bytes.Length;
        httpWebRequest.ClientCertificates.Add(value);
        Stream requestStream = httpWebRequest.GetRequestStream();
        requestStream.Write(bytes, 0, bytes.Length);
        requestStream.Close();
        HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
        Stream responseStream = httpWebResponse.GetResponseStream();
        StreamReader streamReader = new StreamReader(responseStream, uTF);
        string result = streamReader.ReadToEnd();
        string empty = string.Empty;
        return result;
    }


    public string EnterprisePay(string openid,int money)
    {
        var url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers";

        return "";
    }
}