using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.Model
{
    /// <summary>
    /// 系统设置
    /// </summary>
    public class SysSetting
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        public int? SetId{get;set;}
        /// <summary>
        /// 设置变量名称
        /// </summary>
        [Required,StringLength(50)]
        public string SetName{get;set;}
        /// <summary>
        /// 设置变量值
        /// </summary>
        [Required,StringLength(150)]
        public string SetValue{get;set;}
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(150)]
        public string Remark { get; set; }
    }
}
