using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Integration.WebApi;
using System.Web.Http;
using System.Reflection;
using ASPODES.Database;

namespace ASPODES.WebAPI.App_Start.Inject
{
    /// <summary>
    /// 依赖注入配置
    /// </summary>
    public class InjectConfiguration
    {
        private ContainerBuilder _container;

        public ContainerBuilder GetContainer()
        {
            return _container;
        }

        public void Configuration(HttpConfiguration httpConfiguration)
        {
            _container = new ContainerBuilder();
            _container.RegisterApiControllers(Assembly.GetExecutingAssembly());
            _container.RegisterWebApiFilterProvider(httpConfiguration);
            _container.RegisterWebApiModelBinderProvider();

            _container.RegisterType<AspodesDB>().AsSelf().InstancePerRequest();

            // 注册依赖注入的模块
            _container.RegisterModule(new ProjectModules());
            _container.RegisterModule(new AnnualTaskModules());
            _container.RegisterModule(new ConsultationModules());
            _container.RegisterModule(new NoticeModules());
            _container.RegisterModule(new ApplicationModules());
            _container.RegisterModule(new StatisticModules());
            _container.RegisterModule(new SystemModules());

            var container = _container.Build();
            httpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}