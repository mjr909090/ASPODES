using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASPODES.Model;

namespace ASPODES.WebAPI.Security
{
    /// <summary>
    /// 权限验证类
    /// </summary>
    public class Privilege
    {
        /// <summary>
        /// 用户修改申请书的权限验证
        /// </summary>
        public Func<Application, CurrentUser,bool> UserEditApplication = (a, u) 
                => a.LeaderId ==  u.PersonId//自己的申请书
                && a.CurrentYear == SystemConfig.ApplicationStartYear //本年度的申请书
                //&& DateTime.Now <= SystemConfig.ApplicationSubmitDeadline //对于申请书的操作，不需要时间限制。提交需要限制
                && (a.Status <= ApplicationStatus.NEW || a.Status == ApplicationStatus.CANCEL || a.Status == ApplicationStatus.REJECT); //申请书的状态是允许修改的

        /// <summary>
        /// 用户上传项目文档的权限验证
        /// </summary>
        public static Func<Project, CurrentUser, bool> UserEditProject = (a, u)
                 => a.LeaderId == u.PersonId//自己负责的项目
                 && a.Status == ProjectStatus.ACTIVE;//已经结题或终止的项目不用上传文档

        public static bool UserEditAnnualTask( CurrentUser user, AnnualTask task )
        {
            return true;
        }

        //public static bool UserReadResource( CurrentUser user, )
    }
}