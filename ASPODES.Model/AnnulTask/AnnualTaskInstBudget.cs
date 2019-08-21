using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ASPODES.Model
{
    /// <summary>
    /// 年度任务书单位预算
    /// </summary>
    public class AnnualTaskInstBudget
    {
        /// <summary>
        /// 年度任务书ID，外键，参照AnnualTask表
        /// 组合主键
        /// </summary>
        public int? AnnualTaskId { get; set; }
        /// <summary>
        /// 单位ID，外键，参照Institute表
        /// 组合主键
        /// </summary>
        public int? InstituteId { get; set; }

        /// <summary>
        /// 预算额度
        /// </summary>
        [Required]
        public double? Amount { get; set; }
        /// <summary>
        /// 导航属性，年度任务书
        /// </summary>
        [JsonIgnore]
        public AnnualTask AnnualTask { get; set; }
        /// <summary>
        /// 导航属性，单位
        /// </summary>
        [JsonIgnore]
        public Institute Institute { get; set; }
    }
}
