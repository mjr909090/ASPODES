using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.Model
{
    /// <summary>
    /// 申请书操作记录
    /// </summary>
    public class ApplicationLog
    {
        /// <summary>
        /// 主键，自增
        /// </summary>
        public int? ApplicationLogId { get; set; }
        /// <summary>
        /// 申请书ID，外键，参考Application
        /// </summary>
        [StringLength(64)]
        public string ApplicationId { get; set; }
        /// <summary>
        /// 操作用户的ID，外键，参考User表
        /// </summary>
        [StringLength(256)]
        public string UserId { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        [Required]
        public ActionType Operation { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        [Required]
        public DateTime Time { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(1024)]
        public string Comment { get; set; }
        /// <summary>
        /// 导航属性，申请书
        /// </summary>
        public virtual Application Application { get; set; }
        /// <summary>
        /// 导航属性，用户
        /// </summary>
        public virtual User User { get; set; }

    }
}
