using ASPODES.WebAPI.Repository;
using ASPODES.WebAPI.Service;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPODES.WebAPI.App_Start.Inject
{
    public class NoticeModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // service层向controller层的依赖注入
            //builder.RegisterType<CreateNoticeService>().AsSelf().InstancePerRequest();
            builder.RegisterType<OperateNoticeService>().AsSelf().InstancePerRequest();

            // repository层向service层的依赖注入
            builder.RegisterType<UserRepository>().AsSelf().InstancePerRequest();
            builder.RegisterType<NoticeRepository>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationRepository>().AsSelf().InstancePerRequest();
            builder.RegisterType<NoticeTemplateRepository>().AsSelf().InstancePerRequest();
            builder.RegisterType<InstRepository>().AsSelf().InstancePerRequest();
            builder.RegisterType<ReviewCommentRepository>().AsSelf().InstancePerRequest();
            builder.RegisterType<ReviewAssignmentRepository>().AsSelf().InstancePerRequest();
            builder.RegisterType<PersonRepository>().AsSelf().InstancePerRequest();
            builder.RegisterType<RecommendationRepository>().AsSelf().InstancePerRequest();
        }
    }
}