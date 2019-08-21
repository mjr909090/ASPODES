using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace ASPODES.WebAPI.Jobs
{
    public class JobManage
    {
        private static ISchedulerFactory sf = new StdSchedulerFactory();
        private static IScheduler scheduler;

        /// <summary>
        /// 开始任务
        /// </summary>
        public static void Start()
        {
            try
            {
                scheduler = sf.GetScheduler();

                IJobDetail jobDetail = JobBuilder.Create<TimedTask>()
                    .WithIdentity("TimedTaskJob", "定时任务")
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("TimedTaskTrigger", "TriggerGroup")
                    .ForJob(jobDetail.Key)
                    //.StartNow()
                    //.WithCronSchedule("/30 * * ? * *")  //每30秒运行
                    //.WithCronSchedule("0 0 1 * * ?")   //每天凌晨1点执行
                    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(1, 0))//每天凌晨1点0分执行
                    .Build();

                //告诉Quartz 用trigger这个触发器去执行TimedTask
                scheduler.ScheduleJob(jobDetail, trigger);
                //开始定时任务
                scheduler.Start();
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message);
            }
        }


        /// <summary>
        /// 结束任务
        /// </summary>
        public static void Stop()
        {
            if (scheduler != null && !scheduler.IsShutdown)
            {
                scheduler.Shutdown(true);
            }
        }
    }
}
