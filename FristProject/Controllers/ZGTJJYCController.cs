using DAL;
using FristProject.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FristProject.Controllers
{
    public class ZGTJJYCController : BaseController
    {
        // GET: ZGTJJYC
        /// <summary>
        /// 江语城2020年新年运势H5
        /// </summary>
        /// <returns></returns>
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index()
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
                Session["User"] = user;
                user.ShareId = Request.QueryString["shareId"];

            }
            UserInfoSaveNoSaveImg(user);
            // 添加访问记录
            AddPV(urlpath, user.Openid, "江语城2020年新年运势H5");
            return View(user);
        }

        // 礼物可以抽两次
        // 分享后可以再抽一次
        // 判断是否抽过一次，抽过才能获取一次抽奖机会

        // ? 分享给好友之后，直接再跳新的礼品出来
        //   两次奖品不能一样
        // 礼品：红包和礼品
        // 第二次进入判断是否领取过
        // 第二次分享判断是否分享过

        public string PrizeDraw(string openId)
        {
            int id = 0;
            string msg = "";
            string actName = "江语城2020年新年运势H5";
            List<Gift> gifts = giftDAL.GetGiftsByAcitvityNameIsExist(actName);
            Gift gift = new Gift();
            // 先判断openId是否存在
            if (!string.IsNullOrWhiteSpace(openId))
            {
                try
                {

                    //先判断openId是否存在 ,在一定程度上防止刷
                    if (userDAL.SelUserInfoByOpenId(openId) != null)
                    {
                        //礼物没有了就不允许分享
                        if (GetPriceSumCount(actName) <= 0)
                        {
                            id = 7;
                            msg = "礼物没有了";
                            return JsonConvert.SerializeObject(new { id, msg });
                        }
                        //判断是否是第一次
                        // 通过查询礼物记录里面的记录数量
                        List<GiftLog> giftLogs = giftLogDAL.SelGiftLogs(openId, actName);
                        if (giftLogs == null || giftLogs.Count == 0)
                        {
                            gift = GetRandomGift(new List<Gift>(), actName, 0);
                            GiftLog giftLog = new GiftLog() { OpenId = openId, ActivityName = actName, GiftId = gift.GiftId, GiftName = gift.GiftName, GiftDesc = "第一次抽"};
                            if (giftLogDAL.AddGiftLog(giftLog) > 0)
                            {
                                id = 5;
                                msg = "第一次抽奖，记录添加成功";
                            }
                            else
                            {
                                id = 6;
                                msg = "第一次抽奖，记录添加失败";
                            }
                        }
                        // 抽过两次
                        else if (giftLogs.Count == 2)
                        {
                            id = 1;
                            msg = "两次机会都用过了";
                        }
                        // 只抽过一次
                        else if (giftLogs.Count == 1)
                        {
                            if (giftLogs[0].GiftDesc.Contains("已选择此奖品"))
                            {
                                gift = giftDAL.GetGiftByGiftId(giftLogs[0].GiftId);
                                id = 8;
                                msg = "已经登记过了";
                                return JsonConvert.SerializeObject(new { id, msg, gift });
                            }
                            //判断是否分享过H5
                            if (isShareTableDAL.GetShareTable(openId, actName) != null)
                            {
                                // 判断此人之前的礼物是什么，如果抽到一样的，就再次抽取
                                // 当礼物只剩一种时，就选那一个
                                GiftLog giftLog = giftLogs[0];
                                gift = GetRandomGift(new List<Gift>(), actName, 0);
                                if (gift.GiftId == giftLog.GiftId && gifts.Count > 1)
                                {
                                    gift = GetDifferentGift(giftLog.GiftId, actName);
                                }
                                // 将礼物记录保存起来
                                giftLog = new GiftLog() { OpenId = openId, ActivityName = actName, GiftId = gift.GiftId, GiftName = gift.GiftName, GiftDesc = "第二次抽"};
                                if (giftLogDAL.AddGiftLog(giftLog) > 0)
                                {
                                    id = 2;
                                    msg = "正在使用第二次机会，记录添加成功";
                                }
                                else
                                {
                                    id = 3;
                                    msg = "正在使用第二次机会，记录添加失败";
                                }


                            }
                            else
                            {
                                id = 4;
                                msg = "未分享，请分享后再进行抽奖";
                            }
                        }
                        // 一次都没抽过
                        //else if (giftLogs.Count == 0)
                        //{

                        //}
                    }
                    else
                    {
                        msg = "非正常调用接口";
                    }

                }
                catch (Exception ex)
                {

                    id = 98;
                    msg = ex.Source + ";" + ex.Message+";"+ex.StackTrace;
                }

            }
            else
            {
                msg = "参数错误";
            }

            return JsonConvert.SerializeObject(new { id, msg, gift });
        }

        //获取指定id不同的礼物
        public Gift GetDifferentGift(int id, string actName)
        {
            Gift gift = GetRandomGift(new List<Gift>(), actName, 0);
            if (id == gift.GiftId)
            {
                gift = GetDifferentGift(id, actName);
            }
            return gift;
        }
        //添加礼物分享记录
        public string AddShareLog(string openId)
        {
            int id = 0;
            string msg = "";
            string actName = "江语城2020年新年运势H5";
            // 判断参数是否存在
            if (!string.IsNullOrWhiteSpace(openId))
            {
                // 判断是否分享过
                if (isShareTableDAL.GetShareTable(openId, actName) != null)
                {
                    id = 1;
                    msg = "已经分享过";
                }
                else
                {
                    if (GetPriceSumCount(actName) <= 0)
                    {

                        id = 4;
                        msg = "礼物没有了无法分享";
                        return JsonConvert.SerializeObject(new { id, msg });
                    }
                    //判断是否添加分享记录成功
                    if (isShareTableDAL.AddShareLog(openId, actName) > 0)
                    {
                        id = 2;
                        msg = "分享成功";
                    }
                    else
                    {
                        id = 3;
                        msg = "分享失败";
                    }
                }

            }
            else
            {
                msg = "参数错误";

            }
            return JsonConvert.SerializeObject(new { id, msg });
        }

        // 我的奖品
        public string GetMyGift(string openId)
        {
            // 没登记
            string actName = "江语城2020年新年运势H5";
            List<GiftLog> giftLogs = new List<GiftLog>();
            int flag = 0;
            if (!string.IsNullOrWhiteSpace(openId))
            {
                giftLogs = giftLogDAL.SelGiftLogs(openId, actName);
                for (int i = 0; i < giftLogs.Count; i++)
                {
                    // 是否已登记
                    if (giftLogs[i].GiftDesc.Contains("已选择此奖品"))
                    {
                        flag = 1;
                        GiftLog giftLog = giftLogs[i];
                        giftLogs.Clear();
                        giftLogs.Add(giftLog);
                    }
                }

                for (int i = 0; i < giftLogs.Count; i++)
                {
                    Gift gift = giftDAL.GetGiftByGiftId(giftLogs[i].GiftId);
                    giftLogs[i].Unit = gift.Unit;
                }
            }



            return JsonConvert.SerializeObject(new { flag, giftLogs });

        }

        // 登记
        public string JYC20200109Reg(string openId, int giftId, string name, string telphone)
        {
            int id = 0;
            string msg = "";
            string actName = "江语城2020年新年运势H5";
            if (!string.IsNullOrWhiteSpace(openId))
            {
                if (giftLogDAL.SelGiftLogs(openId, actName).Count > 0)
                {
                    // 判断数量是否还有（过时不候）
                    if (GetPriceSumCount("江语城2020年新年运势H5") > 0)
                    {
                        if (giftLogDAL.EditGiftLog(openId, actName, giftId, name, telphone, ",已选择此奖品") > 0)
                        {
                            id = 1;
                            msg = "登记成功";
                            // 礼物减少

                            if (giftCountDAL.EditGiftCountByGiftId(giftId) > 0)
                            {
                                Gift gift = giftDAL.GetGiftByGiftId(giftId);
                                if (gift.GiftDesc.Equals("奖金"))
                                {
                                    FHB("江语城新年运势", "中国铁建·江语城", "新年快乐", Convert.ToInt32(giftCountDAL.GetGiftCountModelByGiftId(giftId).Money * 100), openId);
                                }
                                msg += "礼物数量减少成功";

                            }
                            else
                            {
                                id = 2;
                                msg += "礼物数量减少失败";
                            }


                        }
                        else
                        {
                            id = 2;
                            msg = "登记失败";
                        }
                    }
                    else
                    {
                        id = 2;
                        msg = "礼物没有了";
                    }
                }
                else
                {
                    id = 3;
                    msg = "无法登记";
                }
            }
            else
            {
                msg = "参数错误";
            }
            return JsonConvert.SerializeObject(new { id, msg });
        }







    }
}