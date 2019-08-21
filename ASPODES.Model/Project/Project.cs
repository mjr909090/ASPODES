using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.Model
{
    /// <summary>
    /// 已资助项目，出库项目
    /// </summary>
    public class Project
    {

        public Project()
        {
            Status = ProjectStatus.ACTIVE;
            ProjectId = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 资助编号（根据一定规则自动生成，也即项目编号），主键
        /// </summary>
        [StringLength(64)]
        public string ProjectId { get; set; }
        /// <summary>
        /// 申请书ID，外键，参照Application
        /// </summary>
        [StringLength(64)]
        public string ApplicationId { get; set; }

        /// <summary>
        /// 申请书编码
        /// </summary>
        public string ProjectCode { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [Required,StringLength(256)]
        public string Name { get; set; }
        /// <summary>
        /// 负责人ID，参考Person的外键，可能和在库时不同
        /// </summary>
        public int? LeaderId { get; set; }
        /// <summary>
        /// 承担单位ID，参考Institute的外键
        /// </summary>
        public int? InstituteId { get; set; }

        private int? _period = 0;
        /// <summary>
        /// 执行期限
        /// </summary>
        [Required]
        public int? Period 
        { 
            get{return _period;} 
            set
            { 
                _period = value;
                //EndDate = StartDate.AddYears(_period.Value);
            }
        }
        /// <summary>
        /// 项目开始执行的时间
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 项目执行结束的时间
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 资助额度，咨询评议时确定的资助金额，可能和在库时不同
        /// </summary>
        [Required]
        public double? TotalBudget { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        public int? ProjectTypeId { get; set; }
        /// <summary>
        /// 委托类型
        /// </summary>
        [Required]
        public DelegateType DelegateType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 出库项目状态,默认是新建
        /// </summary>
        [Required]
        public ProjectStatus Status { get; set; }
        /// <summary>
        /// 导航属性，申请书
        /// </summary>
        public virtual Application Application { get; set; }
        /// <summary>
        /// 导航属性，项目负责人
        /// </summary>
        public virtual Person Leader { get; set; }
        /// <summary>
        /// 导航属性，承担单位
        /// </summary>
        public virtual Institute Institude { get; set; }
      
        /// <summary>
        /// 导航属性，项目类型
        /// </summary>
        public virtual ProjectType ProjectType { get; set; }

        /// <summary>
        /// 导航属性，项目文档
        /// </summary>
        public virtual ICollection<ProjectDoc> Docs { get; set; }

        /// <summary>
        /// 导航属性，项目成员
        /// </summary>
        public virtual ICollection<ProjectMember> Members { get; set; }

        /// <summary>
        /// 导航属性，项目类型
        /// </summary>
        public virtual ICollection<AnnualTask> AnnualTasks { get; set; }

        public bool Terminable()
        {
            return Status == ProjectStatus.ACTIVE
                || Status == ProjectStatus.INST_REJECT
                || Status == ProjectStatus.DEPART_REJECT;
        }

    }
    
    /// <summary>
    /// 出库项目状态
    /// </summary>
    public enum ProjectStatus
    {
        /// <summary>
        /// 正在执行
        /// </summary>
        ACTIVE = 0,

        /// <summary>
        /// 申请结题成功，等待单位管理员审核
        /// </summary>
        INST_REVIEW = 2,

        /// <summary>
        /// 单位管理员驳回结题申请
        /// </summary>
        INST_REJECT = 3,

        /// <summary>
        /// 院管理员审核结题申请
        /// </summary>
        DEPART_REVIEW = 4,

        /// <summary>
        /// 院管理员驳回结题申请
        /// </summary>
        DEPART_REJECT = 5,

        /// <summary>
        /// 项目结题
        /// </summary>
        FINISH = 1,

        /// <summary>
        /// 项目中止
        /// </summary>
        //ERRUPTED
    }
     
}
