using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.Model
{
    /// <summary>
    /// 项目类型
    /// </summary>
    public class ProjectType
    {

        /// <summary>
        /// 自增主键
        /// </summary>
        public int? ProjectTypeId { get; set; }
        /// <summary>
        /// 项目类型名
        /// </summary>
        [Required,StringLength(128)]
        public string Name { get; set; }
 
        /// <summary>
        /// 限项数目
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// 项目类型是否启用
        /// </summary>
        public bool Enable { get; set; }
    }
}