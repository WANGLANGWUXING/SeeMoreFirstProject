using DAL;
using FristProject.Common;
using FristProject.Models;
using LitJson;
using Newtonsoft.Json;
using Pinyin4net;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WeiKeDAL;

namespace FristProject.Controllers
{
    public class SeeMoreController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }

        //用户登录，登录过或未登录过都会请求
        public string AddUser()
        {
            string resstr;
            string rescode;
            string openid = Request.Form["openid"].Trim();
            string nickname = Request.Form["nickname"].Trim();
            if (!string.IsNullOrWhiteSpace(openid) || !string.IsNullOrWhiteSpace(nickname))
            {

                if (userDAL.SelUserByOpenId(openid) == 0)
                {
                    int res = userDAL.AddUser(new WXUser { OpenId = openid, Nickname = nickname, Phone = "" });
                    if (res > 0)
                    {
                        resstr = "用户添加成功";
                        rescode = "1";
                    }
                    else
                    {
                        resstr = "用户添加失败";
                        rescode = "0";
                    }
                }
                else
                {
                    resstr = "此用户不是第一次登录";
                    rescode = "1";
                }

            }
            else
            {
                resstr = "不是从微信打开";
                rescode = "0";
            }
            return JsonConvert.SerializeObject(new { id = rescode, msg = resstr });

        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public string GetUserInfo()
        {
            WXModel user = null;
            string resstr = "";
            string rescode = "0";
            if (Session["User"] != null)
            {
                rescode = "1";
                resstr = "当前用户授权过";
                user = (WXModel)Session["User"];

            }

            return JsonConvert.SerializeObject(new { id = rescode, msg = resstr, user });
        }

        #region 知语城人生无限店
        /// <summary>
        /// 知语城H5首页
        /// </summary>
        /// <returns></returns>
        public ActionResult ZYCIndex()
        {
            WXModel user;
            if (Session["User"] != null)
            {
                user = (WXModel)Session["User"];
            }
            else
            {
                string urlpath = Request.Url.AbsoluteUri;
                user = GetUser(urlpath);
                Session["User"] = user;
            }

            return View();
        }



        /// <summary>
        /// 知语城H5专用领取礼物方法
        /// </summary>
        /// <param name="telephone"></param>
        /// <param name="giftCode"></param>
        /// <returns></returns>
        public string ReceiveGift(string telephone, string giftCode)
        {
            WXModel wXModel = (WXModel)Session["User"];
            string activityName = "中铁建知语城H5";

            // 添加礼物记录

            //如果已经领取过
            GiftLog giftLog = giftLogDAL.SelGiftLog(wXModel.Openid, activityName);

            string alertcode;
            string rescode;
            string resstr;
            if (giftLog == null)
            {

                string giftNameDesc = GetGiftName(giftCode);
                alertcode = GetGiftCustomCode(giftNameDesc, activityName);
                giftLogDAL.AddGiftLog(new GiftLog()
                {
                    OpenId = wXModel.Openid,
                    NickName = wXModel.Nickname,
                    ActivityName = activityName,
                    GiftName = giftCode,
                    GiftDesc = giftNameDesc,
                    Telphone = telephone,
                    GiftCustomNum = alertcode
                });
                resstr = "领取成功，礼物" + giftCode + ":" + giftNameDesc;
                rescode = "1";

            }
            else
            {
                giftCode = giftLog.GiftName;
                alertcode = giftLog.GiftCustomNum;
                resstr = "礼物已领取";
                rescode = "2";
            }

            return JsonConvert.SerializeObject(new { id = rescode, msg = resstr, giftCode, alertCode = alertcode });



        }

        public string GetGiftName(string giftCode)
        {
            string giftName = "";
            switch (giftCode)
            {
                case "1":
                    giftName = "食用油";
                    break;
                case "2":
                    giftName = "有机果蔬";
                    break;
                case "3":
                    giftName = "星巴克";
                    break;
                case "4":
                    giftName = "鲜花";
                    break;

            }
            return giftName;
        }


        public string GetGiftCustomCode(string name, string activityName)
        {
            string customCode = "";

            if (!string.IsNullOrWhiteSpace(name))
            {
                char py = PinyinHelper.ToHanyuPinyinStringArray(name[0])[0][0];
                string date = DateTime.Now.ToString("yyyyMMdd");
                // 6位随机数
                int rNum = random.Next(100000, 1000000);
                string randStr = rNum.ToString();

                customCode = py.ToString().ToUpper() + date + randStr;
                //查询当前活动是否存在此自定义id
                GiftLog giftLog = giftLogDAL.SelGiftLogByCustomNum(customCode, activityName);
                // 如果存在就继续调用此方法
                if (giftLog != null)
                {
                    customCode = GetGiftCustomCode(name, activityName);
                }
            }

            return customCode;

        }

        public string GetGiftLog()
        {
            //获取当前用户礼物记录
            string resstr = "";
            string rescode = "0";
            string tel = "";
            string alertcode = "";
            WXModel wXModel = (WXModel)Session["User"];
            string activityName = "中铁建知语城H5";
            GiftLog giftLog = giftLogDAL.SelGiftLog(wXModel.Openid, activityName);
            if (giftLog == null)
            {
                rescode = "0";
                resstr = "当前用户没有领取过礼物";
            }
            else
            {
                rescode = "1";
                resstr = giftLog.GiftName;
                tel = giftLog.Telphone;
                alertcode = giftLog.GiftCustomNum;

            }

            return JsonConvert.SerializeObject(new { id = rescode, msg = resstr, tel, alertCode = alertcode });

        }

        #endregion

        #region 光和创谷1108发红包
        public ActionResult GHCGIndex()
        {
            WXModel user;
            if (Session["User"] != null)
            {
                user = (WXModel)Session["User"];
            }
            else
            {
                string urlpath = Request.Url.AbsoluteUri;
                user = GetUser(urlpath);
                Session["User"] = user;
            }

            return View();
        }


        public string GHCGFHB(string name, string tel, string activityName)
        {
            string resstr = "";
            string rescode = "0";
            string resname = "";
            string restel = "";
            WXModel wXModel = (WXModel)Session["User"];
            //string activityName = "光和创谷发红包1108";
            // 判断用户是否领取过
            GiftLog giftLog = giftLogDAL.SelGiftLog(wXModel.Openid, activityName);
            if (giftLog != null)
            {
                //领过红包的
                // 领取过不允许领取
                resstr = "该用户已领取";
                resname = giftLog.Name;
                restel = giftLog.Telphone;
                rescode = "1";
            }
            else
            {
                // 判断红包数量
                // 1.查询当前获取的红包Id
                List<Gift> gifts = giftDAL.GetGiftsByAcitvityName(activityName);
                if (gifts.Count > 0)
                {
                    Gift gift = gifts[0];
                    // 2.获取礼物id的礼物数量
                    int giftCount = giftCountDAL.GetGiftCountByGiftId(gift.GiftId);
                    if (giftCount > 0)
                    {
                        if (giftCountDAL.EditGiftCountByGiftId(gift.GiftId) > 0)
                        {
                            //就设置2500人吧，金额0.88-3.88
                            GiftCount giftCountModel = giftCountDAL.GetGiftCountModelByGiftId(gift.GiftId);
                            int hbMoney = new Random().Next(giftCountModel.MinMoney, giftCountModel.MaxMoney);
                            giftLogDAL.AddGiftLog(new GiftLog()
                            {
                                OpenId = wXModel.Openid,
                                NickName = wXModel.Nickname,
                                ActivityName = activityName,
                                GiftId = gift.GiftId,
                                GiftName = "红包",
                                GiftDesc = "在" + giftCountModel.MinMoney + "~" + giftCountModel.MaxMoney + "之间：" + hbMoney + "分",
                                Name = name,
                                Telphone = tel
                            });
                            //红包领取
                            //调用红包领取方法
                            //FHB();
                            resstr = "领取成功" + ":" + FHB("光和创谷", "光和创谷", "恭喜发财", hbMoney, wXModel.Openid);
                            resname = name;
                            restel = tel;
                            rescode = "1";
                        }
                    }
                    else
                    {
                        resstr = "红包已领取完！";
                        resname = name;
                        restel = tel;
                        rescode = "0";
                    }
                }
            }
            return JsonConvert.SerializeObject(new { id = rescode, msg = resstr, name = resname, tel = restel });
        }
        public string GetGHCGLog(string activityName)
        {
            string resstr = "";
            string rescode = "0";
            string resname = "";
            string restel = "";
            WXModel wXModel = (WXModel)Session["User"];
            //string activityName = "光和创谷发红包1108";
            // 判断用户是否领取过
            GiftLog giftLog = giftLogDAL.SelGiftLog(wXModel.Openid, activityName);

            if (giftLog != null)
            {
                //领过红包的
                //领取过不允许领取
                resstr = "该用户已领取";
                resname = giftLog.Name;
                restel = giftLog.Telphone;
                rescode = "1";
            }
            else
            {
                // 未领取过红包
                resstr = "该用户未领取";
                rescode = "0";
            }

            return JsonConvert.SerializeObject(new { id = rescode, msg = resstr, name = resname, tel = restel });
        }



        #endregion

        #region 江语城1118抽奖

        public ActionResult JYCCJIndex()
        {
            WXModel user;
            string urlpath = Request.Url.AbsoluteUri;
            if (Session["User"] != null)
            {
                user = (WXModel)Session["User"];
            }
            else
            {

                user = GetUser(urlpath);
                if (user != null)
                {
                    if (userDAL.SelUserByOpenId(user.Openid) == 0)
                    {
                        userDAL.AddUser(new WXUser { OpenId = user.Openid, Nickname = user.Nickname, Phone = "" });
                    }
                }
                Session["User"] = user;
            }
            pVTableDAL.AddPV(new PVTable { Url = urlpath, OpenId = user.Openid });

            return View();
        }

        //public string MyAuthorization()
        //{
        //    //return Request.Url.AbsoluteUri;
        //    string urlpath = Request.Url.AbsoluteUri;
        //    WXModel user = GetUser(urlpath);
        //    if (user != null)
        //    {
        //        if (userDAL.SelUserByOpenId(user.Openid) == 0)
        //        {
        //            userDAL.AddUser(new WXUser { OpenId = user.Openid, Nickname = user.Nickname, Phone = "" });
        //        }
        //        pVTableDAL.AddPV(new PVTable { Url = urlpath, OpenId = user.Openid });
        //    }
        //    return JsonConvert.SerializeObject(user);
        //}

        readonly JYCPriceTimeDAL jYCPriceTime = new JYCPriceTimeDAL();
        readonly IsReceiveTableDAL isReceiveTableDAL = new IsReceiveTableDAL();
        public string JYCGetPrice(string openId, int last)
        {
            string activityName = "中铁建江语城H51118";
            string customCode = "";
            int prizeNum;
            string msg = "";
            string desc = "";
            GiftLog giftLog = giftLogDAL.SelGiftLog(openId, activityName);
            if (GetPriceSumCount("中铁建江语城H51118") == 0 && giftLog == null)
            {
                prizeNum = 0;
                msg = "奖品没有了";
            }
            else
            {
                // 判断是否领取过礼物
                if (giftLog == null)
                {
                    int count = jYCPriceTime.SelJYCPriceTime(openId);
                    // 次数是2,那么这一次必中
                    if (last == 0 || count == 2)
                    {
                        Gift gift = JYCCJ1118();
                        if (gift != null)
                        {
                            prizeNum = 1;
                            // 减少奖品数量
                            int res = giftCountDAL.EditGiftCountByGiftId(gift.GiftId);
                            // 添加奖品记录
                            if (res > 0)
                            {
                                customCode = GetGiftCustomCode(gift.GiftName, activityName);
                                desc = msg;
                                msg = gift.GiftId.ToString();
                                res = giftLogDAL.AddGiftLog(new GiftLog
                                {
                                    OpenId = openId,
                                    NickName = "",
                                    ActivityName = activityName,
                                    GiftId = gift.GiftId,
                                    GiftName = gift.GiftName,
                                    GiftDesc = gift.GiftName,
                                    GiftCustomNum = customCode
                                });
                            }
                        }
                        else
                        {
                            prizeNum = 0;
                            msg = "奖品没有了";
                        }
                    }
                    else
                    {
                        int isExistPrice = random.Next(0, 2);
                        if (isExistPrice == 1)
                        {
                            Gift gift = JYCCJ1118();
                            if (gift != null)
                            {
                                prizeNum = 1;
                                // 减少奖品数量
                                int res = giftCountDAL.EditGiftCountByGiftId(gift.GiftId);
                                // 添加奖品记录
                                if (res > 0)
                                {
                                    customCode = GetGiftCustomCode(gift.GiftName, activityName);
                                    desc = msg;
                                    msg = gift.GiftId.ToString();
                                    res = giftLogDAL.AddGiftLog(new GiftLog
                                    {
                                        OpenId = openId,
                                        NickName = "",
                                        ActivityName = activityName,
                                        GiftId = gift.GiftId,
                                        GiftName = gift.GiftName,
                                        GiftDesc = gift.GiftName,
                                        GiftCustomNum = customCode
                                    });

                                }
                            }
                            else
                            {
                                prizeNum = 0;
                                msg = "奖品没有了";
                            }
                        }
                        else
                        {
                            jYCPriceTime.AddJYCPriceTime(openId);
                            prizeNum = 2;
                            msg = "奖品不在这里";
                        }
                    }

                }
                else
                {
                    prizeNum = 1;
                    msg = giftLog.GiftId.ToString();
                    customCode = giftLog.GiftCustomNum;
                    if (isReceiveTableDAL.SelIsReceiveTable(openId) != null)
                    {
                        prizeNum = 3;
                        msg = giftLog.GiftId.ToString();
                        desc = "已经抽到了";
                    }
                }
            }


            return JsonConvert.SerializeObject(new { prizeNum, msg, customCode, desc });
        }


        public string IsFirst(string openId)
        {
            string activityName = "中铁建江语城H51118";
            string customCode = "";
            string msg = "";

            string isFirst = "0";
            // 判断是否领取过礼物
            GiftLog giftLog = giftLogDAL.SelGiftLog(openId, activityName);

            if (giftLog == null)
            {
                msg = "未领取过";
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(giftLog.Telphone) && !string.IsNullOrWhiteSpace(giftLog.Name))
                {
                    isFirst = "1";
                    msg = giftLog.GiftId.ToString();
                    customCode = giftLog.GiftCustomNum;
                }
                else
                {
                    msg = "礼物记录添加过，却未提交登记信息";
                }

            }
            return JsonConvert.SerializeObject(new { isFirst, msg, customCode });
        }

        public string AddUserInfo(string name, string tel, string openId)
        {
            int id = 0;
            string msg = "";
            string activityName = "中铁建江语城H51118";
            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(tel))
            {

                GiftLog giftLog = giftLogDAL.SelGiftLog(openId, activityName);

                if (giftLog != null)
                {

                    if (giftLogDAL.EditGiftLog(openId, activityName, name, tel) > 0)
                    {
                        id = 1;
                        msg = "添加成功";

                        if (isReceiveTableDAL.SelIsReceiveTable(openId) != null)
                        {

                        }
                        else
                        {
                            isReceiveTableDAL.AddIsReceiveTable(openId);

                            if (giftLog.GiftId >= 26 && giftLog.GiftId <= 31)
                            {
                                int hbTotalAmount = giftCountDAL.GetGiftMoneyByGiftId(giftLog.GiftId);
                                msg = msg + "," + FHB("江语城红包抽奖", "中国铁建·江语城", "恭喜发财", hbTotalAmount, openId);
                            }
                        }

                    }
                }
                else
                {
                    giftLogDAL.AddGiftLog(new GiftLog
                    {
                        OpenId = openId,
                        NickName = "",
                        ActivityName = activityName,
                        GiftId = 0,
                        GiftName = "",
                        GiftDesc = "",
                        Name = name,
                        Telphone = tel,
                        GiftCustomNum = ""
                    });
                    isReceiveTableDAL.AddIsReceiveTable(openId);
                    id = 2;
                    msg = "添加成功,奖品已领完";
                }

            }
            else
            {
                id = 0;
                msg = "参数错误";
            }
            return JsonConvert.SerializeObject(new { id, msg });

        }

        public Gift JYCCJ1118()
        {
            List<Gift> gifts = giftDAL.GetGiftsByAcitvityNameIsExist("中铁建江语城H51118");
            if (gifts.Count > 0)
            {
                int i = random.Next(0, gifts.Count);

                return gifts[i];
            }
            else
            {
                return null;
            }
        }


        #endregion

        #region 华翔城






        #endregion


        #region 微信授权
        /// <summary>
        /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url填写如：http://sdk.weixin.senparc.com/weixin
        /// </summary>
        [HttpGet]
        [ActionName("Index")]
        public ActionResult Get(PostModel postModel, string echostr)
        {
            if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
            {
                return Content(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                return Content("failed:" + postModel.Signature + "," + Senparc.Weixin.MP.CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, Token) + "。" +
                   "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
            }
        }


        public string GetAccessToken()
        {
            return Senparc.Weixin.MP.Containers.AccessTokenContainer.GetAccessToken(AppId);
        }


      
        
        #endregion

        

       


        #region 暂时用不到的代码


        public string GetPrize()
        {
            string openId = ((WXModel)Session["User"]).Openid;
            Gift gift;
            string resstr = "";
            string rescode;
            if (string.IsNullOrWhiteSpace(openId))
            {
                resstr = "用户未授权";
                rescode = "0";
                gift = null;
                return JsonConvert.SerializeObject(new { id = rescode, msg = resstr, gift });
            }
            string startTime;
            string endTime;
            try
            {
                // 抽奖时间
                if (CompareDate(DateTime.Now.ToString("yyyy-MM-dd"), "2019-11-03") <= 0)//日期在2019-11-03之前
                {
                    startTime = DateTime.Now.ToString("yyyy-MM-dd");
                    endTime = "2019-11-03";
                }
                else
                {
                    startTime = GetThisWeekMonday();
                    endTime = GetThisWeekSunday();
                }
                // 先判断该用户是否抽过奖
                if (giftUserDAL.SelGiftUserByOpenId(openId, startTime, endTime) != null)
                {
                    int giftId = giftUserDAL.SelGiftUserByOpenId(openId, startTime, endTime).GiftId;
                    gift = giftDAL.GetGiftByGiftId(giftId);
                    resstr = "用户抽过奖";
                    rescode = "1";
                }
                else
                {
                    List<Gift> gifts;
                    if (CompareDate(DateTime.Now.ToString("yyyy-MM-dd"), "2019-11-03") <= 0)//日期在2019-11-03之前
                    {
                        gifts = giftDAL.GetGifts("", "2019-11-03");
                    }
                    else
                    {
                        gifts = giftDAL.GetGifts("", GetThisWeekSunday());
                    }

                    if (gifts.Count == 1)
                    {
                        gift = gifts.First();
                        resstr = "奖品只有一份";
                        rescode = "1";


                        // 添加礼物记录
                        int num = giftUserDAL.AddGiftUser(new GiftUser
                        {
                            GiftId = gift.GiftId,
                            OpenId = openId,
                            GiftShowId = gift.GiftPY[0] + "201911" + string.Format("{0:D5}", new Random().Next(1000, 10000))
                        });

                        if (num > 0)
                        {
                            resstr += "，礼品记录添加成功";
                        }
                        else
                        {
                            resstr += "，礼品记录添加失败";
                            rescode = "0";
                        }


                    }
                    else if (gifts.Count > 1)
                    {
                        int temp = gifts.Count;
                        // 获取抽奖结果
                        double[] area = new double[temp];
                        for (int i = 0; i < temp; i++)
                        {
                            area[i] = Convert.ToDouble(gifts[i].Probability) / 100.00;
                        }
                        int r = GetRes(area);

                        for (int i = 0; i < temp; i++)
                        {
                            resstr += area[i].ToString() + ",";
                        }

                        gift = gifts[r];
                        resstr = "奖品有" + temp + "份,概率分别为" + resstr + ",随机到的数字" + r;
                        rescode = "1";

                        // 添加礼物记录
                        int num = giftUserDAL.AddGiftUser(new GiftUser
                        {
                            GiftId = gift.GiftId,
                            OpenId = openId,
                            GiftShowId = gift.GiftPY[0] + "201911" + string.Format("{0:D5}", new Random().Next(1000, 10000))
                        });

                        if (num > 0)
                        {
                            resstr += "，礼品记录添加成功";
                        }
                        else
                        {
                            resstr += "，礼品记录添加失败";
                            rescode = "0";
                        }

                    }
                    else
                    {
                        gift = null;
                        resstr = "无奖品";
                        rescode = "0";
                    }
                }

            }
            catch (Exception ex)
            {
                gift = null;
                resstr = ex.StackTrace;
                rescode = "0";
            }
            return JsonConvert.SerializeObject(new { id = rescode, msg = resstr, gift });
        }

        /// <summary>
        /// 比较两个日期大小
        /// </summary>
        /// <param name="dateStr1">日期1</param>
        /// <param name="dateStr2">日期2</param>
        /// <param name="msg">返回信息</param>
        public int CompareDate(string dateStr1, string dateStr2)
        {
            //将日期字符串转换为日期对象
            DateTime t1 = Convert.ToDateTime(dateStr1);
            DateTime t2 = Convert.ToDateTime(dateStr2);
            //DateTime.Compare()进行比较（）
            return DateTime.Compare(t1, t2);

            ////t1> t2
            //if (compNum > 0)
            //{
            //    msg = "t1:(" + dateStr1 + ")大于" + "t2(" + dateStr2 + ")";
            //}
            ////t1= t2
            //if (compNum == 0)
            //{
            //    msg = "t1:(" + dateStr1 + ")等于" + "t2(" + dateStr2 + ")";
            //}
            ////t1< t2
            //if (compNum < 0)
            //{
            //    msg = "t1:(" + dateStr1 + ")小于" + "t2(" + dateStr2 + ")";
            //}
        }

        /// <summary>
        /// 获取本周的周日日期
        /// </summary>
        /// <returns></returns>
        public string GetThisWeekSunday()
        {
            DateTime date = DateTime.Now;
            DateTime firstDate = System.DateTime.Now;
            switch (date.DayOfWeek)
            {
                case System.DayOfWeek.Monday:
                    firstDate = date.AddDays(6);
                    break;
                case System.DayOfWeek.Tuesday:
                    firstDate = date.AddDays(5);
                    break;
                case System.DayOfWeek.Wednesday:
                    firstDate = date.AddDays(4);
                    break;
                case System.DayOfWeek.Thursday:
                    firstDate = date.AddDays(3);
                    break;
                case System.DayOfWeek.Friday:
                    firstDate = date.AddDays(2);
                    break;
                case System.DayOfWeek.Saturday:
                    firstDate = date.AddDays(1);
                    break;
                case System.DayOfWeek.Sunday:
                    firstDate = date;
                    break;
            }
            return firstDate.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 获取本周的周一日期
        /// </summary>
        /// <returns></returns>
        public static string GetThisWeekMonday()
        {
            DateTime date = DateTime.Now;
            DateTime firstDate = System.DateTime.Now;
            switch (date.DayOfWeek)
            {
                case System.DayOfWeek.Monday:
                    firstDate = date;
                    break;
                case System.DayOfWeek.Tuesday:
                    firstDate = date.AddDays(-1);
                    break;
                case System.DayOfWeek.Wednesday:
                    firstDate = date.AddDays(-2);
                    break;
                case System.DayOfWeek.Thursday:
                    firstDate = date.AddDays(-3);
                    break;
                case System.DayOfWeek.Friday:
                    firstDate = date.AddDays(-4);
                    break;
                case System.DayOfWeek.Saturday:
                    firstDate = date.AddDays(-5);
                    break;
                case System.DayOfWeek.Sunday:
                    firstDate = date.AddDays(-6);
                    break;
            }
            return firstDate.ToString("yyyy-MM-dd");
        }

        public string AddPhone()
        {
            string openId = ((WXModel)Session["User"]).Openid;

            string phone = Request.Form["phone"].Trim();

            string resstr;
            string rescode;

            if (!string.IsNullOrWhiteSpace(openId) || !string.IsNullOrWhiteSpace(phone))
            {
                if (string.IsNullOrWhiteSpace(SelUserPhone()))
                {
                    int res = userDAL.AddPhone(openId, phone);
                    if (res > 0)
                    {
                        resstr = "添加手机号成功";
                        rescode = "1";
                    }
                    else
                    {
                        resstr = "添加手机号失败";
                        rescode = "0";
                    }
                }
                else
                {
                    resstr = "该用户为老用户";
                    rescode = "1";
                }

            }
            else
            {
                resstr = "openId为空或手机号为空";
                rescode = "0";
            }


            return JsonConvert.SerializeObject(new { id = rescode, msg = resstr });
        }


        public string SelUserPhone()
        {

            string openId = ((WXModel)Session["User"]).Openid;
            string res = "";
            if (!string.IsNullOrWhiteSpace(openId))
            {
                res = userDAL.SelUserPhoneByOpenId(openId);
            }
            return res;

        }
        /// <summary>
        /// 获取抽奖结果
        /// </summary>
        /// <param name="prob">各物品的抽中概率</param>
        /// <returns>返回抽中的物品所在数组的位置</returns>
        public int GetRes(double[] prob)
        {
            int result = 0;
            int n = (int)(prob.Sum() * 1000);           //计算概率总和，放大1000倍
            Random r = new Random();
            float x = (float)r.Next(0, n) / 1000;       //随机生成0~概率总和的数字

            for (int i = 0; i < prob.Count(); i++)
            {
                double pre = prob.Take(i).Sum();         //区间下界
                double next = prob.Take(i + 1).Sum();    //区间上界
                if (x >= pre && x < next)               //如果在该区间范围内，就返回结果退出循环
                {
                    result = i;
                    break;
                }
            }
            return result;
        }

        #endregion



        #region 光合创谷抽奖活动(test)
        /// <summary>
        /// 光合创谷抽奖活动(test)
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            WXModel user;
            if (Session["User"] != null)
            {
                user = (WXModel)Session["User"];
            }
            else
            {
                string urlpath = Request.Url.AbsoluteUri;
                user = GetUser(urlpath);
                Session["User"] = user;
            }

            return View();
        }


        int num = 0;//递归次数
        public string GetPrize2()
        {
            string resstr = "";
            string rescode = "0";
            WXModel wXModel = (WXModel)Session["User"];
            //int r = Cj();

            // 查询礼物日志，找到对应活动和当前用户是否存在记录，如果存在
            GiftLog giftLog = giftLogDAL.SelGiftLog(wXModel.Openid, "西墨测试");
            if (giftLog == null)
            {
                num = 0;
                Gift gift = Cj2();
                //如果递归次数过多，说明礼物领完,或者一直没有随机到礼物
                if (gift.GiftId == 0)
                {
                    //添加一条礼物记录
                    giftLogDAL.AddGiftLog(new GiftLog
                    {
                        OpenId = wXModel.Openid,
                        NickName = wXModel.Nickname,
                        ActivityName = "西墨测试",
                        GiftName = "0",
                        GiftDesc = "随机次数过多，直接默认为礼物1",
                        GiftId = gift.GiftId
                    });
                    rescode = "0";
                }
                else
                {
                    //礼物数量减一              
                    int res = giftCountDAL.EditGiftCountByGiftId(gift.GiftId);
                    if (res > 0)
                    {
                        //添加一条礼物记录
                        giftLogDAL.AddGiftLog(new GiftLog
                        {
                            OpenId = wXModel.Openid,
                            NickName = wXModel.Nickname,
                            ActivityName = "西墨测试",
                            GiftName = gift.GiftName,
                            GiftDesc = "礼物图片" + gift.GiftName,
                            GiftId = gift.GiftId
                        });
                        rescode = gift.GiftName;
                    }
                }

            }
            else
            {
                rescode = giftLog.GiftName;

                resstr = "您已经参与过";
            }
            return JsonConvert.SerializeObject(new { id = rescode, msg = resstr });
        }

        /// <summary>
        ///  固定概率抽奖
        /// </summary>
        /// <returns></returns>
        public int Cj()
        {
            double[] area = new double[4] { 0.1, 0.2, 0.3, 0.4 };

            int r = GetRes(area) + 1;
            return r;
        }



        public Gift Cj2()
        {
            num++;
            List<Gift> gifts = giftDAL.GetGiftsByAcitvityName("西墨测试");

            int r = random.Next(0, gifts.Count);
            //如果礼物数量为0，重新调用此方法
            int giftCount = giftCountDAL.GetGiftCountByGiftId(gifts[r].GiftId);
            if (giftCount == 0)
            {
                return Cj2();
            }
            // 递归次数过多
            if (num >= 21)
            {
                return new Gift() { GiftId = 0 };
            }

            return gifts[r];
        }
        #endregion



      



        public string GetSignature(string url)
        {

            WeiXinDAL wx = new WeiXinDAL
            {
                Title = "SeeMore",
                Appid = AppId,
                Secret = WeixinAppSecret
            };
            JsonData jd = new JsonData();
            var ticket = wx.GetTicket();

            var noncestr = Utils.md5str16(Guid.NewGuid().ToString());

            var timestamp = Utils.ConvertDateTimeInt(DateTime.Now);

            var temp = "jsapi_ticket=" + ticket + "&noncestr=" + noncestr + "&timestamp=" + timestamp + "&url=" + url;

            var sign = Utils.GetSHA(temp); //转换成为字符串的显示
            jd["timestamp"] = timestamp;
            jd["noncestr"] = noncestr;
            jd["sign"] = sign;
            jd["appid"] = AppId;

            //return JsonConvert.SerializeObject(new { timestamp, noncestr, sign, appid = AppId });
            return jd.ToJson();
        }


        #region 微课平台
        public ActionResult WeiKe()
        {
            WXModel user;
            if (Session["User"] != null)
            {
                user = (WXModel)Session["User"];
            }
            else
            {
                string urlpath = Request.Url.AbsoluteUri;
                user = GetUser(urlpath);
                Session["User"] = user;
            }
            return View();
        }

        readonly KTableDAL kTableDAL = new KTableDAL();
        readonly WKWXUserDAL wKWXUserDAL = new WKWXUserDAL();
        readonly KCommentDAL commentDAL = new KCommentDAL();
        readonly SelKTableDAL selKTableDAL = new SelKTableDAL();
        readonly SignRecordDAL signRecordDAL = new SignRecordDAL();
        readonly QuestionTableDAL questionTableDAL = new QuestionTableDAL();
        readonly AnswerStatusDAL answerStatusDAL = new AnswerStatusDAL();
        public string WeiKeAddUser(string openid, string nickname)
        {
            string resstr;
            string rescode;
            int UId = 0;

            if (!string.IsNullOrWhiteSpace(openid) || !string.IsNullOrWhiteSpace(nickname))
            {
                WKWXUser wKWXUser = wKWXUserDAL.SelUserByOpenId(openid);
                if (wKWXUserDAL.SelUserByOpenId(openid) == null)
                {
                    int res = wKWXUserDAL.AddUser(new WKWXUser { OpenId = openid, NickName = nickname });
                    if (res > 0)
                    {
                        resstr = "用户添加成功";
                        rescode = "1";
                        UId = wKWXUserDAL.SelUserByOpenId(openid).Id;
                    }
                    else
                    {
                        resstr = "用户添加失败";
                        rescode = "0";
                    }
                }
                else
                {
                    UId = wKWXUser.Id;
                    resstr = "此用户不是第一次登录";
                    rescode = "1";
                }

            }
            else
            {
                resstr = "不是从微信打开";
                rescode = "0";
            }
            return JsonConvert.SerializeObject(new { id = rescode, msg = resstr, UId });

        }


        public string KList()
        {
            return JsonConvert.SerializeObject(kTableDAL.GetKTables());
        }

        public string GetSelKList(int UId)
        {
            return JsonConvert.SerializeObject(selKTableDAL.GetSelKTables(UId));
        }

        public string AddSelKTable(int KId, int UId)
        {
            int res = selKTableDAL.AddSelKTable(KId, UId);
            string msg;
            int id;
            if (res > 0)
            {
                id = 1;
                msg = "添加成功";
            }
            else
            {
                id = 0;
                msg = "添加失败";
            }
            return JsonConvert.SerializeObject(new { id, msg });
        }

        public string OnSign(int UId)
        {
            string msg;
            int id;
            if (signRecordDAL.AddSign(UId) > 0)
            {
                id = 1;
                msg = "签到成功";
            }
            else
            {
                id = 0;
                msg = "签到失败";
            }
            return JsonConvert.SerializeObject(new { id, msg });
        }

        public string IsSign(int UId)
        {
            string msg;
            int id;
            if (signRecordDAL.SelSignRecordByUId(UId) != null)
            {
                id = 1;
                msg = "已签到";
            }
            else
            {
                id = 0;
                msg = "未签到";
            }
            return JsonConvert.SerializeObject(new { id, msg });
        }

        public ActionResult SignRecord()
        {
            return View();
        }
        public string GetSign()
        {
            string str = Request.QueryString["UId"];
            List<SignRecord> signRecords = new List<SignRecord>();
            if (!string.IsNullOrWhiteSpace(str))
            {
                signRecords = signRecordDAL.GetSignRecords(Convert.ToInt32(str));
            }
            else
            {
                signRecords = signRecordDAL.GetSignRecords();
            }
            return JsonConvert.SerializeObject(signRecords);
        }


        public string AddComment(int KId, string Comment)
        {
            string resstr = "";
            string rescode = "0";
            KComment comment = new KComment();
            comment.KId = KId;
            comment.Comment = Comment;
            if (Session["User"] != null)
            {
                resstr += "session非空";
                WXModel user = (WXModel)Session["User"];
                comment.UserName = user.Nickname;
                comment.UId = wKWXUserDAL.SelUserByOpenId(user.Openid).Id;
                if (commentDAL.AddComment(comment) > 0)
                {
                    rescode = "1";
                    resstr += "添加成功";
                }
            }
            return JsonConvert.SerializeObject(new { id = rescode, msg = resstr, name = comment.UserName, desc = comment.Comment, time = DateTime.Now.ToShortDateString() });
        }

        public string CommentShow(string kid)
        {
            return JsonConvert.SerializeObject(commentDAL.SelKCommentByKTableId(kid));
        }


        public ActionResult WeiKeInfo()
        {
            return View();
        }

        public string GetWeike(string id)
        {
            return JsonConvert.SerializeObject(kTableDAL.GetKTableByKid(id));
        }


        public string GetQuestion(string kid, string UId)
        {
            return JsonConvert.SerializeObject(questionTableDAL.GetQuestionTables(Convert.ToInt32(kid), Convert.ToInt32(UId)));
        }

        public string AddAnswer(int QId, int UId, string answer)
        {
            string msg = "";
            int id;
            if (answerStatusDAL.AddAnswer(UId, QId, answer) > 0)
            {
                id = 1;
                msg = "添加成功";
            }
            else
            {
                id = 0;
                msg = "添加失败";
            }
            return JsonConvert.SerializeObject(new { id, msg });
        }
        #endregion



    }



}