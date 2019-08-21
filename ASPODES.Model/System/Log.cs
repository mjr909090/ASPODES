using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.Model
{
    /// <summary>
    /// 日志表
    /// </summary>
    public class Log
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int? LogId { get; set; }
        /// <summary>
        /// 修改表名
        /// </summary>
        [StringLength(64)]
        public string TableName{get;set;}
        /// <summary>
        /// 修改记录编号
        /// </summary>
        public int? EditRecordId{get;set;}
        /// <summary>
        /// 操作类型功能
        /// </summary>
        public ActionType Type{get;set;}
        /// <summary>
        /// 修改前内容
        /// </summary>
        [Required,StringLength(256)]
        public string OldRecord{get;set;}
        /// <summary>
        /// 修改后内容
        /// </summary>
        [Required,StringLength(256)]
        public string NewRecord{get;set;}
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime EditTime{get;set;}
        /// <summary>
        /// 操作人ID
        /// </summary>
        [Required,StringLength(256)]
        public string UserId{get;set;}
        /// <summary>
        /// 用户名
        /// </summary>
        [StringLength(64)]
        public string UserName{get;set;}
        /// <summary>
        /// 操作终端IP地址
        /// </summary>
        [StringLength(256)]
        public string IpAddress{get;set;}
        /// <summary>
        /// 导航属性，操作人
        /// </summary>
        public User User { get; set; }
    }
}
