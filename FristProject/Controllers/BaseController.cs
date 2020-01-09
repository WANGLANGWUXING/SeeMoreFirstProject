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
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

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
    }
}