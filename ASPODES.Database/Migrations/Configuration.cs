namespace ASPODES.Database.Migrations
{
    using Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ASPODES.Database.AspodesDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "ASPODES.Database.AspodesDB";
        }

        protected override void Seed(ASPODES.Database.AspodesDB context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            // 对通知模板表进行数据填充
            context.NoticeTemplates.AddOrUpdate(
              p => p.Id,
              //new NoticeTemplate { Id = 1, Content = "您的一封申请书已提交，正等待单位管理员审核", NoticeType = NoticeTypes.Success, URL = "applicationSubmit" , ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 2, Content = "您的一封申请书已被单位管理员审核通过，正在等待院管理员受理", NoticeType = NoticeTypes.Success, URL = "applicationSubmit", ReceiverType = ReceiverType.User },
              new NoticeTemplate { Id = 3, Content = "您的申请书未通过单位管理员审核，请尽快修改后重新提交", NoticeType = NoticeTypes.Error, URL = "applicationSaved", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 4, Content = "您有一封申请书已被单位管理员撤回，请尽快修改后重新提交", NoticeType = NoticeTypes.Error, URL = "applicationSaved", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 5, Content = "您的一封申请书未通过院管理员受理", NoticeType = NoticeTypes.Error, URL = "applicationSubmit", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 6, Content = "您的一封申请书已被院管理员受理", NoticeType = NoticeTypes.Success, URL = "applicationSubmit", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 7, Content = "您的一封申请书已被成功指派专家", NoticeType = NoticeTypes.Success, URL = "applicationSubmit", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 8, Content = "您的一封申请书已经评审完毕", NoticeType = NoticeTypes.Success, URL = "applicationSubmit", ReceiverType = ReceiverType.User },
              new NoticeTemplate { Id = 9, Content = "您有Number封申请书需要审核，请尽快审核", NoticeType = NoticeTypes.Warning, URL = "divisionApplication", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 10, Content = "您单位有Number封申请书未通过院管理员审核", NoticeType = NoticeTypes.Error, URL = "divisionApplicationSubmit", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 11, Content = "您单位有Number封申请书已通过院管理员审核", NoticeType = NoticeTypes.Success, URL = "divisionApplicationSubmit", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 12, Content = "您所在的单位有申请书已被成功指派专家", NoticeType = NoticeTypes.Success, URL = "divisionApplicationSubmit", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 13, Content = "您所在的单位有申请书已经评审完毕", NoticeType = NoticeTypes.Success, URL = "divisionApplicationSubmit", ReceiverType = ReceiverType.InstManager },
              new NoticeTemplate { Id = 14, Content = "您有Number封申请书需要审核，请尽快审核", NoticeType = NoticeTypes.Warning, URL = "applicationUnhandled", ReceiverType = ReceiverType.DeptManager },
              //new NoticeTemplate { Id = 15, Content = "DepartmentName单位提交的ApplicationName已被单位管理员撤回", NoticeType = NoticeTypes.Warning, ReceiverType = ReceiverType.DeptManager },
              new NoticeTemplate { Id = 16, Content = "您有Number封申请书等待手动指派专家，请尽快指派", NoticeType = NoticeTypes.Warning, URL = "assignmentRecommendation", ReceiverType = ReceiverType.DeptManager },
              new NoticeTemplate { Id = 17, Content = "全部申请书已指派专家，请尽快确认", NoticeType = NoticeTypes.Warning, ReceiverType = ReceiverType.DeptManager },
              //new NoticeTemplate { Id = 18, Content = "全部申请书已指派专家，请尽快确认", NoticeType = NoticeTypes.Warning, URL = "assignmentRecommendation", ReceiverType = ReceiverType.DeptManager },
              new NoticeTemplate { Id = 19, Content = "您有Number封申请书已评审完毕，您现在可以导出专家评审结果和咨询审议模板", NoticeType = NoticeTypes.Warning, URL = "firstReviewResult", ReceiverType = ReceiverType.DeptManager },
              //new NoticeTemplate { Id = 20, Content = "待定", ReceiverType = ReceiverType.DeptManager },
              new NoticeTemplate { Id = 21, Content = "您有Number封申请书需要评审，请尽快评审", NoticeType = NoticeTypes.Warning, URL = "preReview", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 22, Content = "您的ApplicationName最终被评为不予资助", NoticeType = NoticeTypes.Error, ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 23, Content = "您的ApplicationName最终被评为出库,并已生成项目，请前往项目页面进行查看", NoticeType = NoticeTypes.Success, URL = "project_host", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 24, Content = "您的ApplicationName最终被评为入库" , NoticeType = NoticeTypes.Error, ReceiverType = ReceiverType.User },
              new NoticeTemplate { Id = 25, Content = "您有新的任务书待填写，请尽快填写", NoticeType = NoticeTypes.Warning, URL = "project_host", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 26, Content = "您有一封任务书已提交，正在等待单位管理员审核", NoticeType = NoticeTypes.Success, URL = "project_host", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 27, Content = "您的一封任务书已被单位管理员审核通过，正在等待院管理员审核", NoticeType = NoticeTypes.Success, URL = "project_host", ReceiverType = ReceiverType.User },
              new NoticeTemplate { Id = 28, Content = "您的任务书未通过单位管理员审核，请尽快修改后重新提交", NoticeType = NoticeTypes.Error, URL = "project_host", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 29, Content = "您的一封任务书已被院管理员审核通过", NoticeType = NoticeTypes.Success, URL = "project_host", ReceiverType = ReceiverType.User },
              new NoticeTemplate { Id = 30, Content = "您的任务书未通过院管理员审核，请尽快修改后重新提交", NoticeType = NoticeTypes.Error, URL = "project_host", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 31, Content = "您的项目需提交今年的年度报告", NoticeType = NoticeTypes.Warning, URL = "project_host", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 32, Content = "您今年的年度报告已提交，正在等待单位管理员审核", NoticeType = NoticeTypes.Success, URL = "project_host", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 33, Content = "您今年的年度报告已被单位管理员审核通过，正在等待院管理员审核", NoticeType = NoticeTypes.Success, URL = "project_host", ReceiverType = ReceiverType.User },
              new NoticeTemplate { Id = 34, Content = "您今年的年度报告未通过单位管理员审核，请尽快修改后重新提交", NoticeType = NoticeTypes.Error, URL = "project_host", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 35, Content = "您今年的年度报告已被院管理员审核通过", NoticeType = NoticeTypes.Success, URL = "project_host", ReceiverType = ReceiverType.User },
              new NoticeTemplate { Id = 36, Content = "您今年的年度报告未通过院管理员审核，请尽快修改后重新提交", NoticeType = NoticeTypes.Error, URL = "project_host", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 37, Content = "您的项目需提交结题报告", NoticeType = NoticeTypes.Warning, URL = "project_host", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 38, Content = "您的结题报告已提交，正在等待单位管理员审核", NoticeType =NoticeTypes.Success, URL = "project_host", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 39, Content = "您提交的ApplicationName的结题申请已被单位管理员审核通过", NoticeType = NoticeTypes.Success, URL = "project_host", ReceiverType = ReceiverType.User },
              new NoticeTemplate { Id = 40, Content = "你的结题申请未通过单位管理员审核，请尽快修改后重新提交", NoticeType = NoticeTypes.Error, URL = "project_host", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 41, Content = "您提交的ApplicationName的结题申请已被院管理员审核通过", NoticeType = NoticeTypes.Success, URL = "project_host", ReceiverType = ReceiverType.User },
              new NoticeTemplate { Id = 42, Content = "您的结题申请未通过院管理员审核，请尽快修改后重新提交", NoticeType = NoticeTypes.Error, URL = "project_host", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 43, Content = "本年度申请书的评审阶段已经结束，请点击此消息查看详情", NoticeType = NoticeTypes.Normal, URL = "divisionApplicationSubmit", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 44, Content = "本年度申请书的评审阶段已经结束，请点击此消息查看详情", NoticeType = NoticeTypes.Normal, URL = "divisionApplicationSubmit", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 45, Content = "本年度申请书的评审阶段已经结束，请点击此消息查看详情", NoticeType = NoticeTypes.Normal, URL = "divisionApplicationSubmit", ReceiverType = ReceiverType.InstManager },
              new NoticeTemplate { Id = 46, Content = "您有Number封任务书需要审核，请尽快审核", NoticeType = NoticeTypes.Warning, URL = "auditAssignBook", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 47, Content = "您单位有Number封任务书已通过院管理员审核", NoticeType = NoticeTypes.Success, URL = "divisionHost", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 48, Content = "您单位有Number封任务书未通过院管理员审核", NoticeType = NoticeTypes.Error, URL = "divisionHost", ReceiverType = ReceiverType.InstManager },
              new NoticeTemplate { Id = 49, Content = "您有Number封年度报告需要审核，请尽快审核", NoticeType = NoticeTypes.Warning, URL = "auditAnnualSummary", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 50, Content = "您单位有Number封年度报告已通过院管理员审核", NoticeType = NoticeTypes.Success, URL = "divisionHost", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 51, Content = "您单位有Number封年度报告未通过院管理员审核", NoticeType = NoticeTypes.Error, URL = "divisionHost", ReceiverType = ReceiverType.InstManager },
              new NoticeTemplate { Id = 52, Content = "您有Number封结题申请需要审核，请尽快审核", NoticeType = NoticeTypes.Warning, URL = "auditConcludeReport", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 53, Content = "您单位有Number封结题申请已通过院管理员审核", NoticeType = NoticeTypes.Success, URL = "divisionHost", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 54, Content = "您单位有Number封结题申请未通过院管理员审核", NoticeType = NoticeTypes.Error, URL = "divisionHost", ReceiverType = ReceiverType.InstManager },
              new NoticeTemplate { Id = 55, Content = "您有Number封任务书需要审核，请尽快审核", NoticeType = NoticeTypes.Warning, URL = "superAdmin_auditAssignBook", ReceiverType = ReceiverType.DeptManager },
              new NoticeTemplate { Id = 56, Content = "您有Number封年度报告需要审核，请尽快审核", NoticeType = NoticeTypes.Warning, URL = "superAdmin_auditAnnualSummary", ReceiverType = ReceiverType.DeptManager },
              new NoticeTemplate { Id = 57, Content = "您有Number封结题申请需要审核，请尽快审核", NoticeType = NoticeTypes.Warning, URL = "superAdmin_auditConcludeReport", ReceiverType = ReceiverType.DeptManager },
              //new NoticeTemplate { Id = 101, Content = "您所在单位的单位管理员已经修改了您的个人信息，请尽快查看并核对", NoticeType = NoticeTypes.Warning, URL = "personal_setting", ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 102, Content = "您所在单位的单位管理员已经重置了您的登录密码", NoticeType = NoticeTypes.Success, ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 103, Content = "您已被您所在单位的单位管理员推荐成为专家", NoticeType = NoticeTypes.Success, ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 104, Content = "您所在单位的单位管理员已经撤销了您的专家推荐", NoticeType = NoticeTypes.Error, ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 105, Content = "院管理员已经通过了对您的专家推荐，您现在可以以专家的身份登录系统", NoticeType = NoticeTypes.Success, ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 106, Content = "院管理员已经取消了您的专家资格，您现在将无法执行专家相关的事务", NoticeType = NoticeTypes.Error, ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 107, Content = "您已被系统管理员任命为院管理员", NoticeType = NoticeTypes.Normal, ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 108, Content = "您已被系统管理员任命为院管理员", NoticeType = NoticeTypes.Normal, ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 109, Content = "您已被系统管理员取消院管理员资格", NoticeType = NoticeTypes.Error, ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 110, Content = "系统管理员更改了您的项目分管类型", NoticeType = NoticeTypes.Warning, ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 111, Content = "您已被系统管理员任命为单位管理员", NoticeType = NoticeTypes.Success, ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 112, Content = "您已被系统管理员取消单位管理员资格", NoticeType = NoticeTypes.Error, ReceiverType = ReceiverType.User },
              //new NoticeTemplate { Id = 113, Content = "您对UserName的专家推荐已被院管理员驳回", NoticeType = NoticeTypes.Error, URL = "expertsRecommend", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 114, Content = "您对UserName的专家推荐已被院管理员通过", NoticeType = NoticeTypes.Success, URL = "expertsRecommend", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 115, Content = "您所在单位的UserName已被院管理员取消了专家资格", NoticeType = NoticeTypes.Error, URL = "expertsRecommend", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 116, Content = "您所在单位的联系人已被替换为UserName", NoticeType = NoticeTypes.Warning, URL = "infoMaintain", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 117, Content = "系统管理员修改了您所在单位的单位信息，请及时进行核对", NoticeType = NoticeTypes.Warning, URL = "infoMaintain", ReceiverType = ReceiverType.InstManager },
              //new NoticeTemplate { Id = 118, Content = "DepartmentName单位的UserName添加了一个新的院外单位——NewDepartmentName（单位名）", NoticeType = NoticeTypes.Warning, ReceiverType = ReceiverType.DeptManager },
              //new NoticeTemplate { Id = 119, Content = "DepartmentName1单位的UserName1为DepartmentName2单位添加了一个新的人员——UserName2", NoticeType = NoticeTypes.Warning, ReceiverType = ReceiverType.DeptManager },
              new NoticeTemplate { Id = 120, Content = "您有Number条新的专家推荐需要审核，请尽快审核", NoticeType = NoticeTypes.Warning, URL = "exportRecommendAudited", ReceiverType = ReceiverType.DeptManager }
              //new NoticeTemplate { Id = 121, Content = "DepartmentName单位提交的对UserName的专家推荐已被单位管理员撤回", NoticeType = NoticeTypes.Warning, ReceiverType = ReceiverType.DeptManager }
              );
            //
        }
    }
}
