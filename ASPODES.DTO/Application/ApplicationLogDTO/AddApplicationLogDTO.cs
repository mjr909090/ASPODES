using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using ASPODES.Model;

namespace ASPODES.DTO.Application
{
    /// <summary>
    /// 添加申请书操作记录DTO
    /// </summary>
    public class AddApplicationLogDTO
    {
        /// <summary>
        /// 申请书ID
        /// </summary>
        [Required]
        public string ApplicationId { get; set; }

        /// <summary>
        /// 操作用户的ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public ActionType Operation { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Required]
        public string Comment { get; set; }
    }
}
