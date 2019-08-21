using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.System
{
    /// <summary>
    /// 添加日志DTO
    /// </summary>
    public class AddLogDTO
    {
        /// <summary>
        /// 日志ID
        /// </summary>
        public int? LogId { get; set; }

        /// <summary>
        /// 修改表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 修改记录编号
        /// </summary>
        public int? EditRecordId { get; set; }

        /// <summary>
        /// 修改前内容
        /// </summary>
        public string OldRecord { get; set; }

        /// <summary>
        /// 修改后内容
        /// </summary>
        public string NewRecord { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime EditTime { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 操作终端IP地址
        /// </summary>
        public string IpAddress { get; set; }
    }
}
