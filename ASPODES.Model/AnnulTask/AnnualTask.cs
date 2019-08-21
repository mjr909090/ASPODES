using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPODES.Model
{
    /// <summary>
    /// 年度任务书
    /// </summary>
    public class AnnualTask
    {
        public AnnualTask()
        {
            EditTime = DateTime.Now;
            Status = AnnualTaskStatus.SAVE;
        }
        /// <summary>
        /// 年度任务书编号，主键，自增
        /// </summary>
        public int? AnnualTaskId { get; set; }
        /// <summary>
        /// 年度任务书名称
        /// </summary>
        public string Name { get; set; }
       
        /// <summary>
        /// 项目ID，外键，参考Project
        /// </summary>
        [StringLength(64)]
        public string ProjectId { get; set; }
      
        /// <summary>
        /// 牵头负责人
        /// </summary>
        public int? LeaderId { get; set; }

        /// <summary>
        /// 承担单位ID
        /// </summary>
        public int? InstituteId { get; set; }

        /// <summary>
        /// 当年预算额
        /// </summary>
        public double? CurrentBudget { get; set; }

        /// <summary>
        /// 年份,和申请年度相同，即System.ApplyYear
        /// </summary>
       [Required]
        public int? Year { get; set; }

        /// <summary>
        /// 填报时间
        /// </summary>
        public DateTime EditTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Comment { get; set; }

        //public DateTime CreateTime{get;set;}

        //public DateTime SubmitTime{get;set;}


        /// <summary>
        /// 绩效目标及年度目标
        /// </summary>
       // [StringLength(2048)]
       // public string AnnaulTarget { get; set; }

        /// <summary>
        /// 任务书状态
        /// </summary>
        public AnnualTaskStatus Status { get; set; }

        /// 导航属性，项目
        /// </summary>
        public virtual Project Project { get; set; }

        /// <summary>
        /// 导航属性，项目负责人
        /// </summary>
        public virtual Person Leader { get; set; }

        /// <summary>
        /// 导航属性，承担单位
        /// </summary>
        public virtual Institute Institute { get; set; }

        /// <summary>
        /// 文档
        /// </summary>
        public virtual ICollection<AnnualTaskDoc> AnnualTaskDocs { get;set; }

        /// <summary>
        /// 科目预算
        /// </summary>
        public virtual ICollection<AnnualTaskBudgetItem> AnnualTaskBudgetItems { get; set; }

        /// <summary>
        /// 单位预算
        /// </summary>
        public virtual ICollection<AnnualTaskInstBudget> AnnualTaskInstBudgets { get; set; }

        public bool Editable()
        {
            return Status == AnnualTaskStatus.SAVE
                || Status == AnnualTaskStatus.INST_REJECT 
                || Status == AnnualTaskStatus.DEPART_REJECT;
        }

        public bool Terminable()
        {
            return Status == AnnualTaskStatus.UPLOAD_ANNUAL_REPORT
                || Status == AnnualTaskStatus.INST_REJECT_ANNUAL_REPORT
                || Status == AnnualTaskStatus.DEPART_REJECT_ANNUAL_REPORT;
        }
    }
    /// <summary>
    /// 年度任务书状态
    /// </summary>
    public enum AnnualTaskStatus
    {
        /// <summary>
        /// 保存
        /// </summary>
        SAVE,//0

        /// <summary>
        /// 单位审核任务书
        /// </summary>
        INST_CHECK,//1

        /// <summary>
        /// 单位驳回任务书
        /// </summary>
        INST_REJECT,//2

        /// <summary>
        /// 院审核任务书
        /// </summary>
        DEPART_CHECK,//3

        /// <summary>
        /// 院驳回任务书
        /// </summary>
        DEPART_REJECT,//4

        //PASS,
        /// <summary>
        /// 上传总结报告
        /// </summary>
        UPLOAD_ANNUAL_REPORT, //5

        /// <summary>
        /// 单位审核总结报告
        /// </summary>
        INST_REVIEW_ANNUAL_REPORT,//6

        /// <summary>
        /// 单位驳回总结报告
        /// </summary>
        INST_REJECT_ANNUAL_REPORT,//7

        /// <summary>
        /// 院审核总结报告
        /// </summary>
        DEPART_REVIEW_ANNUAL_REPORT,//8

        /// <summary>
        /// 院驳回总结报告
        /// </summary>
        DEPART_REJECT_ANNUAL_REPORT,//9

        /// <summary>
        /// 通过
        /// </summary>
        FINISH //10
    }
}