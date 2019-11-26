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

        public string GetPrice1125(string openId)
        {
            int id = 0;
            Gift gift = new Gift();
            string actName = "新影华翔城1125";

            // 判断奖品记录中是否有超过两小时的抽奖但没有登记的记录
            // 有就删除
            DelGiftLog(actName);
            GiftLog giftLog = giftLogDAL.SelGiftLog(openId, actName);
            string msg;
            string customCode;
            int gitId;
            if (giftLog != null)
            {
                id = 2;
                msg = "当前用户已经抽过奖了";
                gitId = giftLog.GiftId;
                customCode = giftLog.GiftCustomNum;

                if(!string.IsNullOrWhiteSpace(giftLog.Name) && !string.IsNullOrWhiteSpace(giftLog.Telphone))
                {
                    id = 3;
                    msg = "当前用户已经登记了";
                }

            }
            else
            {
                gift = GetGiftRandom(actName);

                if (gift != null)
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
                        GiftDesc = gift.GiftDesc,
                        GiftCustomNum = customCode
                    }); 
                    id = 1;
                    msg = "用户抽中了将抽奖记录保存";
                    gitId = gift.GiftId;

                }
                else
                {
                    id = 0;
                    msg = "用户未抽中奖";
                    gitId = 0;
                    customCode = "";
                }


            }

            return JsonConvert.SerializeObject(new { id, gitId, msg, gift,giftLog, customCode });
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


        public Gift GetGiftRandom(string actName)
        {
            int sum = giftCountDAL.GetGiftCountSumByActName(actName);
            if (sum <= 0)
            {
                return null;
            }
            List<Gift> gifts = giftDAL.GetGiftsByAcitvityName(actName);
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

        public string AddRegInfo(string openId, string name, string tel)
        {

            int id = 0;
            string msg = "";
            string actName = "新影华翔城1125";

            // 是否需要判断两小时,不用
            if (giftLogDAL.SelGiftLog(openId, actName) != null)
            {
                if( giftLogDAL.EditGiftLog(openId, actName, name, tel) > 0)
                {
                    id = 1;
                    msg = "登记成功";
                }
            }
            return JsonConvert.SerializeObject(new { id, msg });
        }


    }
}