using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPODES.Model
{
    /// <summary>
    /// 项目滚动库
    /// </summary>
    public abstract class Consultation
    {
        public Consultation()
        {
            ImportTime = DateTime.Now;
        }
        /// <summary>
        /// 在库项目的ID，主键.
        /// 项目对应申请书的ID，外键
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None), StringLength(64)]
        public string ConsultationId { get; set; }

        /// <summary>
        /// 执行期限
        /// </summary>
        [Required]
        public int? Period { get; set; }

        /// <summary>
        /// 初审后确定的总预算额度，和申请书的可能不相等
        /// </summary>
        [Required]
        public double? Budget { get; set; }
        
        /// <summary>
        /// 初审后确定的首年度预算额度，和申请书的可能不相等
        /// </summary>
        [Required]
        public double? CurrentYearBudget { get; set; }

        /// <summary>
        /// 委托类型
        /// </summary>
        public DelegateType DelegateType { get; set; }

        /// <summary>
        /// 导入时间
        /// </summary>
        public DateTime ImportTime { get; set; }
    }
}
