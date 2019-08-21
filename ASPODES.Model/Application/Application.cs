using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ASPODES.Model
{
    /// <summary>
    /// 申请书
    /// </summary>
    public class Application
    {
        public Application() 
        {
            Period = 1;
        }
        /// <summary>
        /// 申请书编号,采用一定编码规则自动生成
        /// </summary>
        [StringLength(64)]
        public string ApplicationId { get; set; }
        /// <summary>
        /// 工作任务名称
        /// </summary>
        [StringLength(128)]
        public string ProjectName { get; set; }
        /// <summary>
        /// 执行期限
        /// </summary>
        public int? Period { get; set; }
        /// <summary>
        /// 样品类型ID，外键，参照ProjectType
        /// </summary>
        public int? ProjectTypeId { get; set; }

        /// <summary>
        /// 资助类别
        /// </summary>
        public int? SupportCategoryId { get; set; }

        /// <summary>
        /// 承担单位
        /// </summary>
        public int? InstituteId { get; set; }
      
        /// <summary>
        /// 牵头负责人
        /// </summary>
        public int? LeaderId { get; set; }
        /// <summary>
        /// 牵头负责人手机
        /// </summary>
        [StringLength(16)]
        public string LeaderPhone { get; set; }
        /// <summary>
        /// 牵头负责人电子信箱
        /// </summary>
        [StringLength(64)]
        public string LeaderEmail { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public int? ContactId { get; set; }
        /// <summary>
        /// 联系人手机
        /// </summary>
        [StringLength(16)]
        public string ContactPhone { get; set; }
        /// <summary>
        /// 联系人电子信箱
        /// </summary>
        [StringLength(64)]
        public string ContactEmail { get; set; }
        /// <summary>
        /// 总预算额
        /// </summary>
        public double? TotalBudget { get; set; }
        /// <summary>
        /// 当年预算额
        /// </summary>
        public double? FirstYearBudget { get; set; }
        /// <summary>
        /// 首次申报年份
        /// </summary>
        public int? YearCreated { get; set; }
        /// <summary>
        /// 本次申报年份
        /// </summary>
        public int? CurrentYear { get; set; }
        /// <summary>
        /// 第几次申报
        /// </summary>
        public int? AppliyTimes { get;set;}

        /// <summary>
        /// 摘要
        /// </summary>
        [MaxLength]
        public string AbstractContent { get; set; }
        /// <summary>
        /// 最后编辑时间
        /// </summary>
        public DateTime EditTime { get; set; }

        /// <summary>
        /// 申请书初审评分
        /// </summary>
        public double? TotalScore { get; set; } 
        /// <summary>
        /// 内容摘要
        /// </summary>
        //[StringLength(512)]
        //public string AbstractContent { get; set; }
        /// <summary>
        /// 申请书的状态
        /// </summary>
        public ApplicationStatus Status { get; set; }

        /// <summary>
        /// 委托类型
        /// </summary>
        public DelegateType DeleageType { get; set; }
        /// <summary>
        /// 导航属性，项目类型
        /// </summary>
        public virtual ProjectType ProjectType { get; set; }
        /// <summary>
        /// 资助类别
        /// </summary>
        public virtual SupportCategory SupportCategory { get; set; }
        /// <summary>
        /// 导航属性，承担单位
        /// </summary>
        public virtual Institute Institute { get; set; }
        /// <summary>
        /// 导航属性，项目负责人
        /// </summary>
        public virtual Person Leader { get; set; }
        /// <summary>
        /// 导航属性，联系人
        /// </summary>
        public virtual Person Contact { get; set; }

        /// <summary>
        /// 导航属性，年度预算
        /// </summary>
        public virtual ICollection<AnnualBudget> AnnualBudgets { get; set; }

        ///// <summary>
        ///// 导航属性，参加人员
        ///// </summary>
        //public virtual ICollection<Member> Members { get; set; }

        ///// <summary>
        ///// 导航属性，各单位预算
        ///// </summary>
        //public virtual ICollection<InstBudget> InstituteBudgets { get; set; }
        
        ///// <summary>
        ///// 导航属性，申请书的文档
        ///// </summary>
        //public virtual ICollection<ApplicationDoc> ApplicationDocuments { get; set; }

        ///// <summary>
        ///// 导航属性，论文涉及的研究领域
        ///// </summary>
        //public virtual ICollection<ApplicationField> Fields { get; set; }
        

        /// <summary>
        /// 申请书的指派信息
        /// </summary>
        public virtual ICollection<ReviewAssignment> ReviewAssignments { get; set; }

        /// <summary>
        /// 申请书的评审信息
        /// </summary>
        public virtual ICollection<ReviewComment> ReviewComments { get; set; }
         
        public bool HasReview()
        {
            return Status >= ApplicationStatus.FINISH_REVIEW;
        }

    }

    /// <summary>
    /// 项目申请书状态
    /// </summary>
    public enum ApplicationStatus
    {
        /// <summary>
        /// 新建，填写第一部分
        /// </summary>
        NEW_ONE,
        /// <summary>
        /// 新建，填写第二部分
        /// </summary>
        NEW_TWO,
        /// <summary>
        /// 新建，填写第三部分
        /// </summary>
        NEW_THREE,
        /// <summary>
        /// 新建，填写第四部分
        /// </summary>
        NEW_FOUR,
        /// <summary>
        /// 新建
        /// </summary>
        NEW,
        /// <summary>
        /// 等待单位审核
        /// </summary>
        CHECK,//5
        /// <summary>
        /// 被单位管理员驳回
        /// </summary>
        REJECT,//6
        /// <summary>
        /// 被单位管理员撤回
        /// </summary>
        CANCEL,//7

        /// <summary>
        /// 等待院管理员受理
        /// </summary>
        ACCEPT,//8

        /// <summary>
        /// 院不受理
        /// </summary>
        REFUSE,//9

        /// <summary>
        /// 等待指派专家
        /// </summary>
        ASSIGNMENT,//10

        /// <summary>
        /// 等待手动指派
        /// </summary>
        MANUAL_ASSIGNMENT,//11

        /// <summary>
        /// 等待确认指派信息
        /// </summary>
        SEND_ASSIGNMENT,//12

        /// <summary>
        /// 评审中
        /// </summary>
        REVIEW,//13

        /// <summary>
        /// 评审完毕
        /// </summary>
        FINISH_REVIEW,//14

        /// <summary>
        /// 不资助
        /// </summary>
        UNSUPPORT,//15

        /// <summary>
        /// 在库
        /// </summary>
        STORAGE,//16

        /// <summary>
        /// 已出库,资助
        /// </summary>
        SUPPORT,//17

        /// <summary>
        /// 过期失效
        /// </summary>
        OVERDUE//18

    }
}