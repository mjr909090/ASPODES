using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.Model
{
    public class VisitLog
    {
        /// <summary>
        /// 主键 Id
        /// </summary>
        public int VisitId { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        [StringLength(256)]
        public string UserId { get; set; }

        /// <summary>
        /// 控制器名
        /// </summary>
        [StringLength(512)]
        public string ControllerName { get; set; }
        
        /// <summary>
        /// 操作action名
        /// </summary>
        [StringLength(512)]
        public string ActionName { get; set; }

        /// <summary>
        /// 操作action用途说明
        /// </summary>
        [StringLength(512)]
        public string ActionNameDetail { get; set; }

        /// <summary>
        /// 开始执行时间
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 结束执行时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 执行耗时
        /// </summary>
        public double? CostTime { get; set; }

        public string UserHostName { get; set; }

        public string UrlReferrer { get; set; }

        public string RequestUri { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string RequestParamaters { get; set; }

        /// <summary>
        /// 相应数据
        /// </summary>
        public string ResponseResult { get; set; }

        /// <summary>
        /// 用户UA
        /// </summary>
        public string UsersAgents { get; set; }

        [StringLength(128)]
        public string IPAddress { get; set; }

        [StringLength(2048)]
        public string UserDevice { get; set; }

        [StringLength(256)]
        public string UserPartform { get; set; }

        [StringLength(2048)]
        public string UserBrowser { get; set; }

        [StringLength(2048)]
        public string UserOS { get; set; }

    }
}
