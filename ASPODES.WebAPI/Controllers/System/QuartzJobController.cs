using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Web.Http;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Filter;
using ASPODES.WebAPI.Jobs;
using log4net.Repository.Hierarchy;
using Quartz;
using Quartz.Impl;

namespace ASPODES.WebAPI.Controllers.System
{
    /// <summary>
    /// 定时任务控制器
    /// </summary>
    [ActionTrack]
    public class QuartzJobController : ApiController
    {
        /// <summary>
        /// 获取定时任务信息
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get()
        {
            return ResponseWrapper.SuccessResponse(string.Format("Job Key:{0},计划时间:{1},开始时间:{2},下次计划时间:{3}"
                , TryGetValueFromConfig(_ => _, () => "未知", supressKey: "JobKey")
                , TryGetValueFromConfig(_ => _, () => "未知", supressKey: "JobScheduledTime")
                , TryGetValueFromConfig(_ => _, () => "未知", supressKey: "JobTime")
                , TryGetValueFromConfig(_ => _, () => "未知", supressKey: "JobNextTime")
                ));
        }


        private T TryGetValueFromConfig<T>(Func<string, T> parseFunc, Func<T> defaultTValueFunc,
            [CallerMemberName]string key = "", string supressKey = "")
        {
            try
            {
                if (!supressKey.IsNullOrWhiteSpace())
                {
                    key = supressKey;
                }

                var node = ConfigurationManager.AppSettings[key];
                return !string.IsNullOrEmpty(node) ? parseFunc(node) : defaultTValueFunc();
            }
            catch (Exception ex)
            {
                LogHelper.Error(string.Format("Error Reading web.config on AppSettings node: {0},EROR:{1}", key, ex.Message));
                return default(T);
            }
        }
    }
}
