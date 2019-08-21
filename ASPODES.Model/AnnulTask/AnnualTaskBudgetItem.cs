
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ASPODES.Model
{
    /// <summary>
    /// 出库项目年度预算
    /// </summary>
    public class AnnualTaskBudgetItem
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        public int? AnnualTaskBudgetItemId { get; set; }
        /// <summary>
        /// 所属出库项目的ID，外键
        /// </summary>
        public int? AnnualTaskId { get; set; }
        /// <summary>
        /// 支出科目
        /// </summary>
        [Required,StringLength(30)]
        public string SubjectId { get; set; }
        /// <summary>
        /// 年度预算额度
        /// </summary>
        [Required]
        public double? Amount { get; set; }
        /// <summary>
        /// 经费支出依据
        /// </summary>
        [StringLength(1024)]
        public string Reason { get; set; }
        /// <summary>
        /// 导航属性，所属出库项目
        /// </summary>
        [JsonIgnore]
        public virtual AnnualTask AnnualTask { get; set; }
        /// <summary>
        /// 导航属性，会计科目
        /// </summary>
        [JsonIgnore]
        public virtual AccountingSubject Subject { get; set; }
    }
}