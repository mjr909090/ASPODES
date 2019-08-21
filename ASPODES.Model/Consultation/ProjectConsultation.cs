using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.Model
{
    /// <summary>
    /// 项目咨询审议
    /// </summary>
    public class ProjectConsultation : Consultation
    {
        /// <summary>
        /// 已到达预算
        /// </summary>
        public double? ArrivalBudget { get; set; }
        /// <summary>
        /// 项目咨询审议结果
        /// </summary>
        public ProjectConsultationResult Result { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// 项目导航属性
        /// </summary>
        [JsonIgnore]
        public virtual Project Project { get; set; }

        
    }
    
    /// <summary>
    /// 项目咨询审议结果
    /// </summary>
    public enum ProjectConsultationResult
    {
        /// <summary>
        /// 继续执行
        /// </summary>
        CONTINUE,

        /// <summary>
        /// 中止
        /// </summary>
        SUSPEND
    }
}
