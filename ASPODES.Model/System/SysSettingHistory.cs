using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.Model
{
    public class SysSettingHistory
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// 个人提交开始时间
        /// </summary>
        public DateTime? ApplicationSubmitBeginTime { get; set; }

        /// <summary>
        /// 个人提交截止时间
        /// </summary>
        public DateTime? ApplicationSubmitDeadline { get; set; }

        /// <summary>
        /// 单位管理员审核截止时间
        /// </summary>
        public DateTime? ApplicationVerifyDeadline { get; set; }

        /// <summary>
        /// 专家评分截止时间
        /// </summary>
        public DateTime? ApplicationExpertDeadline { get; set; }

        /// <summary>
        /// 项目开始年
        /// </summary>
        public int? ApplicationStartYear { get; set; }

        /// <summary>
        /// 设置更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        
        /// <summary>
        /// 设置更新IP
        /// </summary>
        [StringLength(40)]
        public string UpdateIp { get; set; }

        /// <summary>
        /// 设置更新ID
        /// </summary>
        [StringLength(256)]
        public string UpdateId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(150)]
        public string Remark { get; set; }
    }
}
