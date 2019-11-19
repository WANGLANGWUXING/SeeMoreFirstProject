using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FristProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //΢�����ÿ�ʼ
            var isGLobalDebug = true;//����ȫ�� Debug ״̬
            var senparcSetting = SenparcSetting.BuildFromWebConfig(isGLobalDebug);
            var register = RegisterService.Start(senparcSetting).UseSenparcGlobal();//CO2NETȫ��ע�ᣬ���룡

            var isWeixinDebug = true;//����΢�� Debug ״̬
            var senparcWeixinSetting = SenparcWeixinSetting.BuildFromWebConfig(isWeixinDebug);
            register.UseSenparcWeixin(senparcWeixinSetting, senparcSetting);////΢��ȫ��ע�ᣬ���룡

            //ע�ṫ�ں�
            AccessTokenContainer.RegisterAsync(
                System.Configuration.ConfigurationManager.AppSettings["WeixinAppId"],
                System.Configuration.ConfigurationManager.AppSettings["WeixinAppSecret"],
                "���ں�");


            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
