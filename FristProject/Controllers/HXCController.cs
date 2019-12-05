using DAL;
using Newtonsoft.Json;
using Pinyin4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FristProject.Controllers
{
    public class HXCController : Controller
    {
        readonly GiftCountDAL giftCountDAL = new GiftCountDAL();
        readonly GiftDAL giftDAL = new GiftDAL();
        readonly Random random = new Random();
        readonly GiftLogDAL giftLogDAL = new GiftLogDAL();

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

        public string GetPrice2019Christmas(string openId,int score)
        {
            int id = 0;
            string msg = "";

            //1.是否中有记录 在giftlog中有记录
            // 有记录，向下执行2
            // 没记录，向下执行3

            //2.是否已经登记过 giftLog name和tel 有值，直接弹出值 
            // 有值，退出判断
            // 没值，向下执行3

            //3.是否礼物已经领完 giftcount 两种礼物数量Remainder是否已经都是0
            // 领完，退出判断
            // 没领完，向下执行4

            //4.查询是否有分数记录 GameScore
            //  有记录：5
            //  没记录：添加记录，并向下执行6


            //5.分数是否超过记录数
            //  没超过，退出判断
            //  超过，向下执行6

            //6.判断分数范围
            // 
            if (score > 1000)
            {
                //围脖手套

            }
            else if (score > 500)
            {
                //娃娃公仔

            }
            else
            {
                //没有礼品

            }

            //
        }


        public string AddRegInfo2019Christmas(string openId,string name,string tel)
        {
            int id = 0;
            string msg = "";
            string actName = "新影华翔城2019圣诞老人";

            // 是否需要判断两小时,不用
            if (giftLogDAL.SelGiftLog(openId, actName) != null)
            {
                if (giftLogDAL.EditGiftLog(openId, actName, name, tel) > 0)
                {
                    id = 1;
                    msg = "登记成功";
                }
            }
            return JsonConvert.SerializeObject(new { id, msg });
        }
        #endregion
        #region 华翔城2019圣诞助力
        ShareActivityUserDAL shareActivityUserDAL = new ShareActivityUserDAL();
        public string AddShareUser(string openId, string img, string nickName)
        {
            int id = 0;
            string msg = "";
            string actName = "新影华翔城2019圣诞助力";
            ShareActivityUser shareUser = shareActivityUserDAL.SelShareUser(openId, "新影华翔城2019圣诞助力");
            // 如果存在，不添加
            // 如果不存在，添加
            if (shareUser != null)
            {
                id = 0;
                msg = "已存在此分享人";
            }
            else
            {
                int res = shareActivityUserDAL.AddShareUser(new ShareActivityUser
                {
                    UserShareId = CreateGUID(),
                    OpenId = openId,
                    UserImg = img,
                    NickName = nickName,
                    ActivityName = actName
                });
                if (res > 0)
                {

                    id = 1;
                    msg = "添加分享人成功";
                }
                else
                {
                    id = 2;
                    msg = "添加分享人失败";
                }

            }
            return JsonConvert.SerializeObject(new { id, msg });


        }


        public string AddHelpUser(string shareId, string openId, string url)
        {
            // 添加助力用户
            // 参数 被助力人分享Id ， 助力人微信Id, 助力链接
            return "";
        }

        public string SelHelpRank()
        {
            //序号  头像   姓名  助力数
            //分页
            return "";
        }

        public string CreateGUID()
        {
            return Guid.NewGuid().ToString("N") + DateTime.Now.ToString("yyyyMMddHHmmss");
        }
        #endregion
    }
}