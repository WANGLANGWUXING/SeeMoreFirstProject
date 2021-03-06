﻿using DAL;
using FristProject.Models;
using Newtonsoft.Json;
using Pinyin4net;
using Senparc.CO2NET.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace FristProject.Controllers
{
    public class HXCController : BaseController
    {

        #region 华翔城事事如意20191123
        public string GetGiftCustomCode(string name)
        {
            string customCode = "";

            if (!string.IsNullOrWhiteSpace(name))
            {
                char py = PinyinHelper.ToHanyuPinyinStringArray(name[0])[0][0];
                string date = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                customCode = py.ToString().ToUpper() + date;
            }

            return customCode;

        }

        public string GetPrice1125(string openId, string type)
        {

            if (string.IsNullOrEmpty(openId))
            {
                return "未正确请求此接口";
            }
            int id = 0;

            string actName = "新影华翔城1125";

            // 判断奖品记录中是否有超过两小时的抽奖但没有登记的记录
            // 有就删除
            DelGiftLog(actName);
            // 一星 33  金龙鱼油  35  煮蛋器
            // 二星 32  锅具三件套 34  被子
            // 4种情况

            //0 奖品没有了
            //1 奖品此次抽中（不同类型的抽中和第一次的抽中）
            //2 奖品抽中过同类型
            //3 奖品全部但是没有登记
            //4 奖品抽中登记了


            List<GiftLog> giftLogs = giftLogDAL.SelGiftLogs(openId, actName);
            Gift gift = GetGiftRandom(actName, type);
            GiftLog giftLog = new GiftLog();

            string msg = "";
            string customCode = "";
            int gitId = 0;
            // 两个等级的游戏都玩过了
            if (giftLogs.Count == 2)
            {


                //id = 3;
                //msg = "当前用户已经抽过奖了";
                for (int i = 0; i < giftLogs.Count; i++)
                {
                    if (type == giftLogs[i].GiftDesc)
                    {
                        if (!string.IsNullOrWhiteSpace(giftLogs[i].Name) &&
                            !string.IsNullOrWhiteSpace(giftLogs[i].Telphone))
                        {
                            id = 4;
                            msg = "用户抽中且登记了";
                        }
                        else
                        {
                            id = 3;
                            msg = "用户抽中了但是没有登记";

                        }
                        gitId = giftLogs[i].GiftId;
                        customCode = giftLogs[i].GiftCustomNum;
                        giftLog = giftLogs[i];
                    }
                }
            }
            //
            else if (gift != null && giftLogs.Count == 1)
            {
                if (type == giftLogs[0].GiftDesc)
                {
                    id = 2;
                    msg = "用户抽中过同类型的奖品";
                    gitId = giftLogs[0].GiftId;
                    customCode = giftLogs[0].GiftCustomNum;
                    giftLog = giftLogs[0];
                }
                else
                {
                    customCode = GetGiftCustomCode(gift.GiftName);

                    //奖品数量减一
                    giftCountDAL.EditGiftCountByGiftId(gift.GiftId);
                    giftLogDAL.AddGiftLog(new
                    GiftLog
                    {
                        OpenId = openId,
                        NickName = "",
                        ActivityName = actName,
                        GiftId = gift.GiftId,
                        GiftName = gift.GiftName,
                        GiftDesc = type,
                        GiftCustomNum = customCode
                    });
                    id = 1;
                    msg = "用户抽中了将抽奖记录保存";
                    gitId = gift.GiftId;
                }
            }
            else if (gift != null && giftLogs.Count == 0)
            {
                customCode = GetGiftCustomCode(gift.GiftName);

                //奖品数量减一
                giftCountDAL.EditGiftCountByGiftId(gift.GiftId);
                giftLogDAL.AddGiftLog(new
                GiftLog
                {
                    OpenId = openId,
                    NickName = "",
                    ActivityName = actName,
                    GiftId = gift.GiftId,
                    GiftName = gift.GiftName,
                    GiftDesc = type,
                    GiftCustomNum = customCode
                });
                id = 1;
                msg = "用户抽中了将抽奖记录保存";
                gitId = gift.GiftId;
            }
            else if (gift == null)
            {
                id = 0;
                msg = "用户未抽中奖,奖品没有了";
                gitId = 0;
                customCode = "";
            }
            #region 之前的代码
            //// 添加过礼物
            //if (giftLog != null)
            //{
            //    // 一个类型的礼物
            //    if (type == giftLog.GiftDesc)
            //    {

            //    }
            //    // 不是一个类型的
            //    else
            //    {
            //        // 109
            //        // 120
            //        gift = GetGiftRandom(actName, type);




            //    }




            //    if (!string.IsNullOrWhiteSpace(giftLog.Name) && !string.IsNullOrWhiteSpace(giftLog.Telphone))
            //    {
            //        id = 3;
            //        msg = "当前用户已经登记了";
            //    }

            //}
            //else
            //{
            //    gift = GetGiftRandom(actName);
            //    // 抽中
            //    if (gift != null)
            //    {
            //        customCode = GetGiftCustomCode(gift.GiftName);

            //        //奖品数量减一
            //        giftCountDAL.EditGiftCountByGiftId(gift.GiftId);
            //        giftLogDAL.AddGiftLog(new
            //        GiftLog
            //        {
            //            OpenId = openId,
            //            NickName = "",
            //            ActivityName = actName,
            //            GiftId = gift.GiftId,
            //            GiftName = gift.GiftName,
            //            GiftDesc = gift.GiftDesc,
            //            GiftCustomNum = customCode
            //        });
            //        id = 1;
            //        msg = "用户抽中了将抽奖记录保存";
            //        gitId = gift.GiftId;

            //    }

            //    //礼品没有了
            //    else
            //    {
            //        id = 0;
            //        msg = "用户未抽中奖";
            //        gitId = 0;
            //        customCode = "";
            //    }


            //}
            #endregion


            return JsonConvert.SerializeObject(new { id, gitId, msg, gift, giftLog, customCode });
        }


        public string GetPriceInfo(string openId)
        {
            return JsonConvert.SerializeObject(giftLogDAL.SelGiftLogs(openId, "新影华翔城1125"));
        }


        public void DelGiftLog(string actName)
        {
            // 判断奖品记录中是否有超过两小时的抽奖但没有登记的记录
            // 有就删除
            List<GiftLog> giftLogs = giftLogDAL.SelGiftLogAfterTwoHour(actName);

            if (giftLogs.Count > 0)
            {
                for (int i = 0; i < giftLogs.Count; i++)
                {
                    giftLogDAL.DelGiftLogById(giftLogs[i].Id);
                    giftCountDAL.EditGiftCountByGiftIdAdd1(giftLogs[i].GiftId);

                }
            }


        }


        public Gift GetGiftRandom(string actName, string type)
        {
            int sum = giftCountDAL.GetGiftCountSumByActNameAndType(actName, type);
            if (sum <= 0)
            {
                return null;
            }
            List<Gift> gifts = giftDAL.GetGiftsByAcitvityNameAndType(actName, type);
            for (int i = 0; i < gifts.Count; i++)
            {
                if (giftCountDAL.GetGiftCountByGiftId(gifts[i].GiftId) <= 0)
                {
                    gifts.RemoveAt(i);
                }
            }
            // 开始随机
            return GiftRandom(gifts);
        }


        public Gift GiftRandom(List<Gift> gifts)
        {
            int r = random.Next(0, gifts.Count);
            return gifts[r];
        }

        public string AddRegInfo(string openId, string name, string tel, string type)
        {

            int id = 0;
            string msg = "";
            string actName = "新影华翔城1125";

            // 是否需要判断两小时,不用
            if (giftLogDAL.SelGiftLog(openId, actName, type) != null)
            {
                if (giftLogDAL.EditGiftLog(openId, actName, name, tel, type) > 0)
                {
                    id = 1;
                    msg = "登记成功";
                }
            }
            return JsonConvert.SerializeObject(new { id, msg });
        }


        #endregion


        #region 华翔城2019拯救圣诞老人
        readonly GameScoreDAL gameScoreDAL = new GameScoreDAL();

        //public int Get
        // 高分 手套
        readonly int bigLength = 200;
        // 低分 公仔
        readonly int smaliLength = 100;
        //  0 礼物领完了
        //  1 分数没有达到领奖标准
        //  2 已登记
        //  3 没有超过上一次的分数
        //  4 礼物一样的
        //  5 添加记录成功
        public string GetPrice2019Christmas(string openId, string nickName, string img, int score)
        {
            string actName = "新影华翔城2019圣诞老人";
            GiftLog giftLog = giftLogDAL.SelGiftLog(openId, actName);
            int isHaveLog = 0;

            int id;
            string msg;
            if (!string.IsNullOrWhiteSpace(openId))
            {
                try
                {
                    //1.是否中有记录 在giftlog中有记录
                    // 有记录，向下执行2
                    if (giftLog != null)
                    {
                        isHaveLog = 1;
                        //2.是否已经登记过 giftLog name和tel 有值，直接弹出值 
                        // 有值，退出判断
                        if (!string.IsNullOrWhiteSpace(giftLog.Name) && !string.IsNullOrWhiteSpace(giftLog.Telphone))
                        {
                            id = 2;
                            msg = "已登记";
                            return JsonConvert.SerializeObject(new { id, msg });
                        }

                        // 没值，向下执行3
                    }

                    // 没领完，向下执行4
                    //4.查询是否有分数记录 GameScore
                    GameScore gameScore = gameScoreDAL.SelGameScore(openId, actName);
                    if (gameScore != null)
                    {
                        //  有记录：5
                        //5.分数是否超过记录数
                        //  没超过，退出判断
                        if (score <= gameScore.Score)
                        {
                            id = 3;
                            msg = "分数没有超过";
                            return JsonConvert.SerializeObject(new { id, msg });
                        }
                        //  超过，向下执行6
                        // 先修改分数，再执行6
                        gameScore.Score = score;
                        //gameScore.WeiXinImg = img;
                        gameScoreDAL.EditGameScore(gameScore);
                    }
                    else
                    {
                        // 图片下载，位置保存到数据库
                        string temp = SaveWxImg(img, openId);

                        //  没记录：添加记录，并向下执行6
                        gameScoreDAL.AddGameScore(
                            new GameScore
                            {
                                OpenId = openId,
                                WeiXinImg = openId + ".jpg",
                                Score = score,
                                ActivityName = actName
                            });
                    }

                    // 没记录，向下执行3
                    //3.是否礼物已经领完 giftcount 两种礼物数量Remainder是否已经都是0
                    int giftSum = giftCountDAL.GetGiftCountSumByActName(actName);
                    if (giftSum == 0)
                    {
                        // 领完，退出判断
                        id = 0;
                        msg = "礼物领完了";
                        return JsonConvert.SerializeObject(new { id, msg });
                    }
                    //围脖手套
                    Gift gift1 = giftDAL.GetGiftsByAcitvityNameAndType(actName, "大于1000").FirstOrDefault();
                    //娃娃公仔
                    Gift gift2 = giftDAL.GetGiftsByAcitvityNameAndType(actName, "小于1000").FirstOrDefault();
                    int count1 = giftCountDAL.GetGiftCountByGiftId(gift1.GiftId);
                    int count2 = giftCountDAL.GetGiftCountByGiftId(gift2.GiftId);
                    Gift selGift = null;
                    int liwu2 = 0;
                    //6.判断分数范围
                    if (score > bigLength)
                    {
                        // 两个礼物都没了
                        if (count1 <= 0 && count2 <= 0)
                        {
                            selGift = null;
                        }
                        // 娃娃公仔没有，围脖手套还有
                        else if (count1 <= 0)
                        {
                            selGift = gift2;
                        }
                        // 娃娃公仔还有
                        else if (count1 > 0)
                        {
                            selGift = gift1;
                        }
                    }


                    else if (score > smaliLength)
                    {
                        if (count2 > 0)
                        {

                            selGift = gift2;
                        }
                    }
                    // 7.分数是否达到有礼物的标准
                    // 没有达到标准，跳出
                    else
                    {
                        id = 1;
                        msg = "分数没有达到领奖标准";
                        return JsonConvert.SerializeObject(new { id, msg });
                    }
                    // 得到低分 高分礼物还有，低分礼物没有的情况
                    if (selGift == null)
                    {
                        // 领完，退出判断
                        id = 0;
                        msg = "礼物领完了";
                        return JsonConvert.SerializeObject(new { id, msg });
                    }
                    // 达到标准
                    // 8.礼物是否和记录里面的礼物相等
                    // 8.1 是否有记录
                    // 有记录，开始比较
                    if (isHaveLog == 1)
                    {
                        // 礼物一样 退出判断
                        if (selGift.GiftId == giftLog.GiftId)
                        {
                            id = 4;
                            msg = "礼物一样的";
                            return JsonConvert.SerializeObject(new { id, msg });
                        }
                        // 礼物不一样 修改礼物记录
                        else
                        {
                            giftLogDAL.EditGiftLog(
                                openId,
                                actName,
                                selGift.GiftId,
                                selGift.GiftName
                                );
                        }
                    }
                    // 没记录，直接添加记录
                    else
                    {
                        giftLogDAL.AddGiftLog(new GiftLog
                        {
                            OpenId = openId,
                            NickName = nickName,
                            ActivityName = actName,
                            GiftId = selGift.GiftId,
                            GiftName = selGift.GiftName,
                            GiftCustomNum = DateTime.Now.ToString("yyyyMMddHHmmssms")

                        });
                    }
                    id = 5;
                    msg = "添加记录成功";

                    if (liwu2 == 1)
                    {
                        msg = msg + "礼物2还有：" + count2;
                    }
                    return JsonConvert.SerializeObject(new { id, msg });
                }
                catch (Exception ex)
                {
                    id = 99;
                    msg = ex.Message;
                    return JsonConvert.SerializeObject(new { id, msg });
                }
            }
            else
            {
                id = 98;
                msg = "openId为空";
                return JsonConvert.SerializeObject(new { id, msg });
            }


        }
        public Image UrlToImage(string url)
        {
            WebClient mywebclient = new WebClient();
            byte[] Bytes = mywebclient.DownloadData(url);
            using (MemoryStream ms = new MemoryStream(Bytes))
            {
                Image outputImg = Image.FromStream(ms);
                return outputImg;
            }
        }

        public string SaveWxImg(string imgNetWork, string openId)
        {

            string path = Path.Combine("E:", "www", "wx", "2019", "1212", "WxImgs", openId + ".jpg");
            if (System.IO.File.Exists(path))
            {
                return path;
            }

            HttpDownload(imgNetWork, path);

            return path;

        }


        /// <summary>
        /// http下载文件
        /// </summary>
        /// <param name="url">下载文件地址</param>
        /// <param name="path">文件存放地址，包含文件名</param>
        /// <returns></returns>
        public static bool HttpDownload(string url, string path)
        {
            string tempPath = System.IO.Path.GetDirectoryName(path) + @"\temp";
            System.IO.Directory.CreateDirectory(tempPath);  //创建临时文件目录
            string tempFile = tempPath + @"\" + System.IO.Path.GetFileName(path) + ".temp"; //临时文件
            if (System.IO.File.Exists(tempFile))
            {
                System.IO.File.Delete(tempFile);    //存在则删除
            }
            try
            {
                FileStream fs = new FileStream(tempFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                // 设置参数
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                Stream responseStream = response.GetResponseStream();
                //创建本地文件写入流
                //Stream stream = new FileStream(tempFile, FileMode.Create);
                byte[] bArr = new byte[1024];
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    //stream.Write(bArr, 0, size);
                    fs.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                }
                //stream.Close();
                fs.Close();
                responseStream.Close();
                System.IO.File.Move(tempFile, path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        ///<summary>
        /// 下载文件
        /// </summary>
        /// <param name="URL">下载文件地址</param>
        /// <param name="Filename">下载后另存为（全路径）</param>
        public static bool DownloadFile(string URL, string filename)
        {
            if (System.IO.File.Exists(filename))
            {
                System.IO.File.Delete(filename);    //存在则删除
            }
            try
            {
                HttpWebRequest Myrq = (System.Net.HttpWebRequest)WebRequest.Create(URL);
                HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
                Stream st = myrp.GetResponseStream();
                Stream so = new System.IO.FileStream(filename, System.IO.FileMode.Create);
                byte[] by = new byte[1024];
                int osize = st.Read(by, 0, (int)by.Length);
                while (osize > 0)
                {
                    so.Write(by, 0, osize);
                    osize = st.Read(by, 0, (int)by.Length);
                }
                so.Close();
                st.Close();
                myrp.Close();
                Myrq.Abort();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 登记并将礼物数量减少
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="name"></param>
        /// <param name="tel"></param>
        /// <returns></returns>
        public string AddRegInfo2019Christmas(string openId, string name, string tel)
        {
            int id = 0;
            string msg = "";
            string actName = "新影华翔城2019圣诞老人";


            if (!string.IsNullOrWhiteSpace(openId))
            {
                // 
                GiftLog giftLog = giftLogDAL.SelGiftLog(openId, actName);
                // 判断是否有礼物记录
                // 无礼物记录
                if (giftLog == null)
                {
                    id = 0;
                    msg = "没有礼物记录";
                    return JsonConvert.SerializeObject(new { id, msg });
                }


                // 礼物记录已经登记过
                if (!string.IsNullOrWhiteSpace(giftLog.Name) && !string.IsNullOrWhiteSpace(giftLog.Telphone))
                {
                    id = 2;
                    msg = "已登记";
                    return JsonConvert.SerializeObject(new { id, msg });
                }
                // 未登记
                if (giftLogDAL.EditGiftLog(openId, actName, name, tel) > 0)
                {
                    giftCountDAL.EditGiftCountByGiftId(giftLog.GiftId);

                    id = 1;
                    msg = "登记成功";
                }
                return JsonConvert.SerializeObject(new { id, msg });
            }
            else
            {
                id = 98;
                msg = "openId为空";
                return JsonConvert.SerializeObject(new { id, msg });
            }

        }


        public string GetWeiXinInfo2019Christmas(string openId)
        {
            string actName = "新影华翔城2019圣诞老人";
            GiftLog giftLog = giftLogDAL.SelGiftLog(openId, actName);
            GameScore gameScore = gameScoreDAL.SelGameScore(openId, actName);

            if (giftLog != null && string.IsNullOrWhiteSpace(giftLog.Name))
            {
                int count3 = giftCountDAL.GetGiftCountByGiftId(giftLog.GiftId);
                if (count3 <= 0)
                {
                    return JsonConvert.SerializeObject(new { id = 0, msg = "当前礼物没有了", giftLog, gameScore });
                }
            }


            return JsonConvert.SerializeObject(
                new
                {
                    id = 1,
                    msg = "礼物还有",
                    giftLog,
                    gameScore
                });
        }
        #endregion

        #region 华翔城2019圣诞助力
        readonly ShareActivityUserDAL shareActivityUserDAL = new ShareActivityUserDAL();
        public string AddShareUser(string openId)
        {
            string shareId = "";
            string actName = "新影华翔城2019圣诞助力";

            int id;
            string msg;
            if (!string.IsNullOrWhiteSpace(openId))
            {
                ShareActivityUser shareUser = shareActivityUserDAL.SelShareUser(openId, "新影华翔城2019圣诞助力");
                // 如果存在，不添加
                // 如果不存在，添加
                if (shareUser != null)
                {
                    id = 0;
                    msg = "已存在此分享人";
                    shareId = shareUser.UserShareId;
                }
                else
                {
                    WXUser wXUser = userDAL.SelUserInfoByOpenId(openId);

                    int res = shareActivityUserDAL.AddShareUser(new ShareActivityUser
                    {
                        UserShareId = CreateGUID(),
                        OpenId = openId,
                        UserImg = wXUser.Headimgurl,
                        NickName = wXUser.Nickname,
                        ActivityName = actName
                    });
                    if (res > 0)
                    {

                        id = 1;
                        msg = "添加分享人成功";
                        shareUser = shareActivityUserDAL.SelShareUser(openId, "新影华翔城2019圣诞助力");

                        shareId = shareUser.UserShareId;
                    }
                    else
                    {
                        id = 2;
                        msg = "添加分享人失败";
                    }

                }
            }
            else
            {
                id = 4;
                msg = "微信用户信息不存在";
            }


            return JsonConvert.SerializeObject(new { id, msg, shareId });


        }

        readonly CollectLikeDAL collectLikeDAL = new CollectLikeDAL();

        public string AddHelpUser(string shareId, string openId, string url)
        {
            //string actName = "新影华翔城2019圣诞助力";
            return JsonConvert.SerializeObject(new { id = 99, msg = "活动已结束" });


            //// 添加助力用户
            //// 参数 被助力人分享Id ， 助力人微信Id, 助力链接
            //int id;
            //string msg;
            //if (!string.IsNullOrWhiteSpace(openId))
            //{
            //    CollectLike collectLike = collectLikeDAL.SelHelperUser(openId, shareId, actName);
            //    if (collectLike != null)
            //    {
            //        id = 4;
            //        // 助力过了
            //        msg = "此人已经助力过此用户";
            //    }
            //    else
            //    {
            //        // 第一次助力
            //        if (collectLikeDAL.AddHelperUser(new CollectLike()
            //        {
            //            UserShareId = shareId,
            //            HelpOpenId = openId,
            //            Url = url,
            //            ActivityName = actName
            //        }) > 0)
            //        {
            //            id = 1;
            //            msg = "助力成功";
            //        }
            //        else
            //        {
            //            id = 0;
            //            msg = "助力出现错误，接口需调整";
            //        }

            //    }
            //}
            //else
            //{
            //    id = 3;
            //    msg = "微信用户信息不存在";
            //}





            //return JsonConvert.SerializeObject(new { id, msg });
        }

        public string GetHelpCount(string shareId)
        {
            return JsonConvert.SerializeObject(collectLikeDAL.SelHelperCount(shareId, "新影华翔城2019圣诞助力"));
        }

        public string SelHelpRank()
        {

            string actName = "新影华翔城2019圣诞助力";
            //序号  头像   姓名  助力数
            //分页
            var rankList = collectLikeDAL.GetHelpRank(actName);

            return JsonConvert.SerializeObject(rankList);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult HXC2019SHZL()
        {
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetExpires(DateTime.Today.AddYears(-2));
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
            UserInfoSave(user);
            // 添加访问记录
            AddPV(urlpath, user.Openid);
            return View(user);
        }



        public string CreateGUID()
        {
            return Guid.NewGuid().ToString("N") + DateTime.Now.ToString("yyyyMMddHHmmss");
        }
        #endregion
    }
}