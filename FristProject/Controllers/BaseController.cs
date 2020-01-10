using DAL;
using FristProject.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Senparc.CO2NET.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using WxPayAPI;
using WxPayAPI.lib;

namespace FristProject.Controllers
{
    public class BaseController : Controller
    {

        public static readonly string Token = WebConfigurationManager.AppSettings["WeixinToken"];//与微信公众账号后台的Token设置保持一致，区分大小写。
        public static readonly string EncodingAESKey = WebConfigurationManager.AppSettings["WeixinEncodingAESKey"];//与微信公众账号后台的EncodingAESKey设置保持一致，区分大小写。
        public static readonly string AppId = WebConfigurationManager.AppSettings["WeixinAppId"];//与微信公众账号后台的AppId设置保持一致，区分大小写。
        public static readonly string WeixinAppSecret = WebConfigurationManager.AppSettings["WeixinAppSecret"];
        public UserDAL userDAL = new UserDAL();
        public GiftDAL giftDAL = new GiftDAL();
        public GiftUserDAL giftUserDAL = new GiftUserDAL();
        public GiftLogDAL giftLogDAL = new GiftLogDAL();
        public GiftCountDAL giftCountDAL = new GiftCountDAL();
        public PVTableDAL pVTableDAL = new PVTableDAL();
        public IsShareTableDAL isShareTableDAL = new IsShareTableDAL();
        public Random random = new Random();
        // GET: Base
        /// <summary>
        /// 根据code获取用户openid
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static string GetOpenidByCode(string Code, out string access_token)
        {
            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", AppId, WeixinAppSecret, Code);
            string ReText = WebRequestPostOrGet(url, "");//post/get方法获取信息
            JObject DicText = (JObject)JsonConvert.DeserializeObject(ReText);
            access_token = "";
            if (DicText.ContainsKey("access_token"))
                access_token = DicText["access_token"].ToString();
            if (!DicText.ContainsKey("openid"))
                return "";
            return DicText["openid"].ToString();
        }

        public void AddPV(string url, string openId, string actName = "")
        {
            pVTableDAL.AddPV(new PVTable { Url = url, OpenId = openId, ActName = actName });
        }
        public static string WebRequestPostOrGet(string sUrl, string sParam)
        {
            byte[] bt = System.Text.Encoding.UTF8.GetBytes(sParam);
            Uri uriurl = new Uri(sUrl);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(uriurl);
            req.Method = "Post";
            req.Timeout = 120 * 1000;
            req.ContentType = "application/x-www-form-urlencoded;";
            req.ContentLength = bt.Length;

            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(bt, 0, bt.Length);
                reqStream.Flush();
            }
            try
            {
                using (WebResponse res = req.GetResponse())
                {
                    Stream resStream = res.GetResponseStream();
                    StreamReader resStreamReader = new StreamReader(resStream, System.Text.Encoding.UTF8);
                    string resLine;
                    System.Text.StringBuilder resStringBuilder = new System.Text.StringBuilder();
                    while ((resLine = resStreamReader.ReadLine()) != null)
                    {
                        resStringBuilder.Append(resLine + System.Environment.NewLine);
                    }
                    resStream.Close();
                    resStreamReader.Close();
                    return resStringBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public void GetWeixinCode(string urlpath)
        {
            string state = Guid.NewGuid().ToString("N");
            string url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=" + state + "#wechat_redirect", AppId, urlpath);

            Response.Redirect(url);
        }

        public WXModel GetUser(string urlpath)
        {
            WXModel model = null;
            var code = Request.Params["code"];
            urlpath = urlpath.UrlEncode();
            if (code == null || code == "")
            {
                GetWeixinCode(urlpath);
            }
            else
            {
                model = GetUserInfoByCode(code);
            }
            return model;
        }


        public WXModel GetUserInfoByCode(string code)
        {
            string openid = GetOpenidByCode(code, out string access_token);
            string userinfo = WebRequestPostOrGet("https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token + "&openid=" + openid + "&lang=zh_CN", "");
            WXModel model = JsonConvert.DeserializeObject<WXModel>(userinfo);
            return model;
        }




        #region 微信授权接口

        string RequestUrl = "";
        public void MyAuthorization(string url)
        {
            //pVTableDAL.AddPV(new PVTable { Url = url, OpenId = "" });
            RequestUrl = url;
            GetWeixinCode(url);
        }

        /// <summary>
        /// 版本1
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string MyGetUserInfoByCode(string code)
        {
            string openid = GetOpenidByCode(code, out string access_token);
            string userinfo = WebRequestPostOrGet("https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token + "&openid=" + openid + "&lang=zh_CN", "");
            WXModel model = JsonConvert.DeserializeObject<WXModel>(userinfo);
            if (model != null && !string.IsNullOrWhiteSpace(model.Openid))
            {
                // 添加用户信息到数据库 有则查看是否需要修改
                userDAL.AddUserS(new WXUser { OpenId = model.Openid, Nickname = model.Nickname, Headimgurl = model.Headimgurl });

                // 添加访问记录
                pVTableDAL.AddPV(new PVTable { Url = RequestUrl, OpenId = model.Openid });
            }

            return JsonConvert.SerializeObject(model);
        }
        /// <summary>
        /// 版本二
        /// </summary>
        /// <param name="code"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public string MyGetUserInfoByCodeOrUrl(string code, string url)
        {
            string openid = GetOpenidByCode(code, out string access_token);
            string userinfo = WebRequestPostOrGet("https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token + "&openid=" + openid + "&lang=zh_CN", "");
            WXModel model = JsonConvert.DeserializeObject<WXModel>(userinfo);
            if (model != null && !string.IsNullOrWhiteSpace(model.Openid))
            {
                // 添加用户信息到数据库 有则查看是否需要修改
                userDAL.AddUserS(new WXUser { OpenId = model.Openid, Nickname = model.Nickname, Headimgurl = model.Headimgurl });
                // 添加访问记录
                AddPV(url, model.Openid);
            }

            return JsonConvert.SerializeObject(model);
        }
        /// <summary>
        /// 版本3
        /// </summary>
        /// <param name="code"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public string MyGetUserInfoByCodeVer03(string code, string url)
        {
            string openid = GetOpenidByCode(code, out string access_token);
            string userinfo = WebRequestPostOrGet("https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token + "&openid=" + openid + "&lang=zh_CN", "");
            WXModel model = JsonConvert.DeserializeObject<WXModel>(userinfo);
            if (model != null && !string.IsNullOrWhiteSpace(model.Openid))
            {

                // 添加用户信息到数据库 有则查看是否需要修改
                userDAL.AddUserS(new WXUser
                {
                    OpenId = model.Openid,
                    Nickname = model.Nickname,
                    Headimgurl = DateTime.Now.ToString("yyyyMMdd") + "/" + model.Openid + ".jpg"
                });
                // 保存用户图片
                string path = SaveUserImg(model.Openid, model.Headimgurl);
                // 按照日期保存
                // 如果日期
                // 添加访问记录
                AddPV(url, model.Openid);
            }

            return JsonConvert.SerializeObject(model);
        }


        /// <summary>
        /// 2019年12月18日 用户信息保存
        /// </summary>
        /// <param name="model"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public string UserInfoSave(WXModel model)
        {
            int id = 0;
            string msg = "";
            if (model != null && !string.IsNullOrWhiteSpace(model.Openid))
            {
                string path = SaveUserImgs(model.Openid, model.Headimgurl);

                // 添加用户信息到数据库 有则查看是否需要修改
                userDAL.AddUserS(new WXUser
                {
                    OpenId = model.Openid,
                    Nickname = model.Nickname,
                    Headimgurl = DateTime.Now.ToString("yyyyMMdd") + "/" + model.Openid + ".jpg"
                });
                // 保存用户图片

                // 按照日期保存
                // 如果日期

                id = 1;
                msg = path;
            }

            return JsonConvert.SerializeObject(new { id, msg });
        }

        public string UserInfoSaveNoSaveImg(WXModel model)
        {
            int id = 0;
            string msg = "";
            if (model != null && !string.IsNullOrWhiteSpace(model.Openid))
            {
                // 添加用户信息到数据库 有则查看是否需要修改
                if (userDAL.AddUserS(new WXUser
                {
                    OpenId = model.Openid,
                    Nickname = model.Nickname,
                    Headimgurl = model.Headimgurl
                }) > 0)
                {
                    id = 1;
                    msg = "添加成功";
                }
                else
                {
                    id = 2;
                    msg = "添加失败";
                }

            }

            return JsonConvert.SerializeObject(new { id, msg });
        }

        public string SaveUserImgs(string openId, string url)
        {
            string path = Path.Combine("E:", "www", "test", "Content", "Images", "WeiXinHeadimgurl", DateTime.Now.ToString("yyyyMMdd"), openId + ".jpg");
            //string resPath = Path.Combine(DateTime.Now.ToString("yyyyMMdd"), openId + ".jpg");
            if (System.IO.File.Exists(path))
            {
                return path;
            }
            HXCController.HttpDownload(url, path);
            return path;
        }

        public string SaveUserImg(string openId, string url)
        {
            string path = Path.Combine("E:", "www", "wx", "WxImgs", DateTime.Now.ToString("yyyyMMdd"), openId + ".jpg");

            string resPath = Path.Combine(DateTime.Now.ToString("yyyyMMdd"), openId + ".jpg");
            if (System.IO.File.Exists(path))
            {
                return path;
            }

            HXCController.HttpDownload(url, path);

            return path;

        }




        #endregion


        /// <summary>
        /// 获取对应活动的礼物数量
        /// </summary>
        /// <param name="activityName"></param>
        /// <returns></returns>
        public int GetPriceSumCount(string activityName)
        {
            return giftCountDAL.GetGiftCountSumByActName(activityName);
        }

        /// <summary>
        /// 获取随机的礼物，最普通的随机
        /// </summary>
        /// <param name="gifts"></param>
        /// <param name="actName"></param>
        /// <param name="flag">是否递归</param>
        /// <returns></returns>
        public Gift GetRandomGift(List<Gift> gifts, string actName, int flag)
        {
            if (gifts.Count == 0 && flag == 0)
            {
                gifts = giftDAL.GetGiftsByAcitvityNameIsExist(actName);
            }
            if (gifts.Count > 0)
            {
                int i = random.Next(0, gifts.Count);
                Gift res = gifts[i];
                if (giftCountDAL.GetGiftCountModelByGiftId(gifts[i].GiftId).Remainder > 0)
                {
                    return res;
                }
                else
                {
                    gifts.RemoveAt(i);
                    return GetRandomGift(gifts, actName, 1);
                }

            }
            else
            {
                return null;
            }
        }


        #region 微信发红包
        /// <summary>
        /// 微信发红包
        /// </summary>
        /// <param name="activityName">活动名称</param>
        /// <param name="sendName">红包名称 展示在红包上</param>
        /// <param name="wishing">红包描述 展示在红包上</param>
        /// <param name="totalAmount">红包金额（分）</param>
        /// <returns></returns>
        public string FHB(string activityName, string sendName, string wishing, int totalAmount, string re_openId)
        {

            // 记得将此方法设置为私有的，防止刷红包
            //string resstr = "";
            //string rescode = "0";
            // 活动名称
            // 红包名称 展示在红包上
            // 红包描述 展示在红包上
            // 红包金额 


            string strData = GetJsApiParameters(activityName, sendName, wishing, totalAmount, re_openId);
            string strUrl = "https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack";//这个就是发送红包的API接口了

            string strResult = WxRedPackPost(strUrl, strData);


            //XmlDocument xmldoc = new XmlDocument();
            //xmldoc.LoadXml(strResult);
            //XmlNode msg = xmldoc.SelectSingleNode("/xml/return_msg");
            //if (msg.InnerText == "<![CDATA[发放成功]]>")
            //{
            //    resstr = "红包发送成功";
            //    rescode = "1";
            //}
            //else
            //{
            //    resstr = strResult;
            //}
            // 返回发生成功时将数据库里的礼物红包数量减一
            // 添加一条红包发送的礼物记录
            return strResult;
        }
        /**
         * 生成随机串，随机串包含字母或数字
         * @return 随机串
         */
        public static string GenerateNonceStr()
        {
            RandomGenerator randomGenerator = new RandomGenerator();
            return randomGenerator.GetRandomUInt().ToString();
        }
        /// <summary>
        /// 构造调用的参数
        /// </summary>
        /// <returns></returns>
        public string GetJsApiParameters(string activityName, string sendName, string wishing, int totalAmount, string re_openId)
        {
            int iMin = 1000;
            int iMax = 9999;
            string MCHID = "1507709561";// 商户号
            string openId = re_openId;
            //Random rd = new Random();//构造随机数
            string strMch_billno = MCHID + DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next(iMin, iMax).ToString();
            WxPayData jsApiParam = new WxPayData();
            jsApiParam.SetValue("nonce_str", GenerateNonceStr());//随机字符串，不长于32位
            jsApiParam.SetValue("mch_billno", strMch_billno);//商户订单号,商户订单号（每个订单号必须唯一）组成：mch_id+yyyymmdd+10位一天内不能重复的数字。 接口根据商户订单号支持重入，如出现超时可再调用。
            jsApiParam.SetValue("mch_id", MCHID);//商户号,微信支付分配的商户号
            jsApiParam.SetValue("wxappid", AppId);//公众账号appid,微信分配的公众账号ID（企业号corpid即为此appId）。接口传入的所有appid应该为公众号的appid（在mp.weixin.qq.com申请的），不能为APP的appid（在open.weixin.qq.com申请的）。
            jsApiParam.SetValue("send_name", sendName);//商户名称,红包发送者名称 显示在红包上的文字 西墨传媒红包 ，和红包上的标题
            jsApiParam.SetValue("re_openid", openId);//接收者的openid
            jsApiParam.SetValue("total_amount", totalAmount);//红包金额，单位分
            jsApiParam.SetValue("total_num", 1);//红包发放总人数
            jsApiParam.SetValue("wishing", wishing);//红包祝福语  显示在红包中间的文字
            jsApiParam.SetValue("client_ip", "39.104.81.240");//这里填写的是我本机的内网ip，实际应用不知道需不需要改。
            jsApiParam.SetValue("act_name", activityName);//活动名称 最长32个字符
            jsApiParam.SetValue("remark", "备注信息:test");//备注信息
            jsApiParam.SetValue("scene_id", "PRODUCT_2");//备注信息

            jsApiParam.SetValue("sign", jsApiParam.MakeSign());//签名,切记，这个签名参数必须放在最后，因为他生成的签名，跟前面的参数有关系

            string parameters = jsApiParam.ToXml();
            return parameters;
        }



        /// <summary>
        /// 提交请求
        /// </summary>
        /// <param name="posturl"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public string WxRedPackPost(string posturl, string postData)
        {
            Encoding encoding = Encoding.UTF8;
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...  
            try
            {
                //CerPath证书路径，这里是本机的路径，实际应用中，按照实际情况来填写
                string certPath = @"E:\zs\apiclient_cert.p12";
                //证书密码
                string password = "1507709561";
                X509Certificate2 cert = new X509Certificate2(certPath, password, X509KeyStorageFlags.MachineKeySet);
                // 设置参数  
                HttpWebRequest request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;//不可少（个人理解为，返回的时候需要验证）
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "text/xml";
                request.ContentLength = data.Length;
                request.ClientCertificates.Add(cert);//添加证书请求
                Stream outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据  
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求  
                Stream instream = response.GetResponseStream();
                StreamReader sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码  
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;

            }
            catch (Exception)
            {
                //string err = ex.Message;
                return string.Empty;
            }
        }
        #endregion
    }
}