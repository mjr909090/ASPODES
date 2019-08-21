using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.DTO.Consultation
{
    /// <summary>
    /// 添加项目滚动库DTO
    /// </summary>
    public class AddConsultationDTO
    {
        private string _delegateType = "NORMAL";

        /// <summary>
        /// 委托类型
        /// </summary>
        public string DelegateType 
        { 
            get{ return _delegateType;} 
            set 
            {
                if ("定向委托".Equals(value))
                    _delegateType = "DIRECTIONAL";
                else
                    _delegateType = "NORMAL";
            }
        }

        /// <summary>
        /// 执行期限
        /// </summary>
        [Required(ErrorMessage="执行期不能为空")]
        public int? Period { get; set; }

        /// <summary>
        /// 总预算额度
        /// </summary>
        [Required(ErrorMessage="总预算额度不能为空")]
        public double? TotalBudget { get; set; }

        /// <summary>
        /// 本年度预算额度
        /// </summary>
        [Required(ErrorMessage="本年度预算额不能为空")]
        public double? CurrentBudget { get; set; }

    }

    /// <summary>
    /// 增加申请书咨询审议DTO
    /// </summary>
    public class AddApplicationConsultationDTO : AddConsultationDTO
    {
        /// <summary>
        /// 申请书ID
        /// </summary>
        [Required(ErrorMessage="申请书ID不能为空")]
        public string ApplicationId { get; set; }

        public string _result;

        /// <summary>
        /// 评审结果
        /// </summary>
        [Required(ErrorMessage="评审结果未填写或者填写错误")]
        public string Result 
        { 
            get{ return _result;} 
            set{ 
                if("入库".Equals(value))
                    _result = "STORAGE";
                else if("出库".Equals(value))
                    _result = "SUPPORT";
                else if( "不资助".Equals(value) )
                    _result = "UNSUPPORT";
                else
                    _result = null;
            }
        }

        //[Required(ErrorMessage="位次未填写")]
        //public int Ordinal { get; set; }
    }

    /// <summary>
    /// 增加项目咨询审议DTO
    /// </summary>
    public class AddProjectConsultationDTO : AddConsultationDTO
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        [Required(ErrorMessage="项目编号未填写")]
        public string ProjectId { get; set; }

        public string _result;
        /// <summary>
        /// 评审结果
        /// </summary>
        [Required(ErrorMessage = "评审结果未填写或者填写错误")]
        public string Result
        {
            get { return _result; }
            set
            {
                if ("中止".Equals(value))
                    _result = "SUSPEND";
                else if ("持续资助".Equals(value))
                    _result = "CONTINUE";
                else
                    _result = null;
            }
        }
        //public double? ArrivalBudget { get; set; }
    }
}
