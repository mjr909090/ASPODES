using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;
using ASPODES.WebAPI.Jobs;
using ASPODES.WebAPI.App_Start.Inject;
using ASPODES.WebAPI.TypeMapping;
using Autofac;
using Autofac.Integration.WebApi;
using ASPODES.Database;
using System.Reflection;


namespace ASPODES.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public override void Init()
        {
            //开启session
            this.PostAuthenticateRequest += (sender, e) => HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            base.Init();
        }
        protected void Application_Start()
        {
            var configuration = GlobalConfiguration.Configuration;
            var inject = new InjectConfiguration();
            inject.Configuration(configuration);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfiguration.Configuration();

            //log4net
            log4net.Config.XmlConfigurator.Configure();

            //定时任务
            JobManage.Start();

            LogHelper.Debug("IIS进程池启动");
        }

        protected void Application_End(object sender, EventArgs e)
        {
            LogHelper.Debug("IIS进程池回收");
            JobManage.Stop();

            //发起请求重启启动iis进程
            //解决应用池回收问题   
            System.Threading.Thread.Sleep(30000);
            try
            {
                string strUrl = string.Format("http://{0}/api/Notice/StartIIS", "222.195.226.73");
                System.Net.HttpWebRequest _httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(strUrl);
                System.Net.HttpWebResponse _httpWebResponse = (System.Net.HttpWebResponse)_httpWebRequest.GetResponse();
                //System.IO.Stream _stream = _httpWebResponse.GetResponseStream();//得到回写的字节流 
                string str = _httpWebResponse.StatusDescription;
                LogHelper.Info(_httpWebResponse.StatusCode.ToString()+"："+_httpWebResponse.StatusDescription);
                //_stream.Dispose();
                _httpWebResponse.Dispose();
                
                //LogHelper.Info("IIS进程池重启请求成功");
            }
            catch (Exception exception)
            {
                //LogHelper.Error(exception.Message);
                //LogHelper.Info("IIS进程池重启请求失败");
            }
            
        }
    }
}
