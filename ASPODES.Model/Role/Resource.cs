using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.Model
{
    /// <summary>
    /// 资源类
    /// </summary>
    public class Resource
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        public int? ResourceId { get; set; }
        /// <summary>
        /// 资源URL
        /// </summary>
        [Required,StringLength(256)]
        public string URL { get; set; }
        /// <summary>
        /// 资源名
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }
    }
}
