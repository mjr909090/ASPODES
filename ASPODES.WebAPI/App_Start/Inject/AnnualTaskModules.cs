using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASPODES.WebAPI.Repository;
using Autofac;
namespace ASPODES.WebAPI.App_Start.Inject
{
    /// <summary>
    /// 年度任务书模块依赖注入
    /// </summary>
    public class AnnualTaskModules:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AnnualTaskRepository>().As<IAnnualTaskRepository>();
            //年度任务书向自身的注入
            builder.RegisterType<AnnualTaskRepository>().AsSelf().InstancePerRequest();
            builder.RegisterType<AnnualTaskDocRepository>().As<IAnnualTaskDocRepository>();
            builder.RegisterType<AnnualTaskBudgetItemRepository>().As<IAnnualTaskBudgetItemRepository>();
            builder.RegisterType<AnnualTaskInstBudgetRepository>().As<IAnnualTaskInstBudgetRepository>();
        }
    }
}