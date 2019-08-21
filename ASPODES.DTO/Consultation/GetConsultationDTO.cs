using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Consultation
{
    /// <summary>
    /// 获取项目滚动库DTO
    /// </summary>
    public class GetConsultationDTO
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 委托类型
        /// </summary>
        public string DelegateType { get; set; }

        /// <summary>
        /// 项目类型名
        /// </summary>
        public string ProjectTypeName { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string InstituteName { get; set; }

        /// <summary>
        /// 领导人名称
        /// </summary>
        public string LeaderName { get; set; }

        /// <summary>
        /// 执行期限
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// 总预算额度
        /// </summary>
        public double? Budget { get; set; }

        /// <summary>
        /// 首年度预算额度
        /// </summary>
        public double? CurrentYearBudget { get; set; }
    }

    /// <summary>
    /// 获取申请书咨询审议DTO
    /// </summary>
    public class GetApplicationConsultationDTO:GetConsultationDTO
    {
        /// <summary>
        /// 申请书ID
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// 总分
        /// </summary>
        public double? TotalScore { get; set; }

        /// <summary>
        /// 申请书咨询审议结果
        /// </summary>
        public int Result { get; set; }
    }

    /// <summary>
    /// 获取项目咨询审议DTO
    /// </summary>
    public class GetProjectConsultationDTO : GetConsultationDTO
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// 项目咨询审议结果
        /// </summary>
        public int Result { get; set; }

        /// <summary>
        /// 已到达预算
        /// </summary>
        public double? ArrivalBudget { get; set; }

        /// <summary>
        /// 申请书ID
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// year第几年的申请书，或者说第几份任务书，和申请书年度预算的年度对应，第一年，第二年...
        /// </summary>
        public int Year { get; set; }
    }
}
