using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.System
{
    /// <summary>
    /// 获取申请书设置DTO
    /// </summary>
    public class GetApplicationSettingDTO
    {
        /// <summary>
        /// 用户提交开始时间
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ApplicationSubmitBeginTime { get; set; }

        /// <summary>
        /// 用户提交截止日期
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ApplicationSubmitDeadline { get; set; }

        /// <summary>
        /// 单位管理员审核截止日期
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ApplicationVerifyDeadline { get; set; }

        /// <summary>
        /// 专家评分截止日期
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ApplicationExpertDeadline { get; set; }

        /// <summary>
        /// 项目开始年
        /// </summary>
        [Required]
        public int ApplicationStartYear { get; set; }
    }
}
