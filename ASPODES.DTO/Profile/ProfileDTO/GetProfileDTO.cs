using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Profile
{
    /// <summary>
    /// 获取个人信息DTO
    /// </summary>
    public class GetProfileDTO
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 单位名
        /// </summary>
        public string InstituteName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 职务
        /// </summary>
        public string Duty { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 通讯地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 是否为专家
        /// </summary>
        public bool Expert { get; set; }

        /// <summary>
        /// 人员ID
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// 教育经历列表
        /// </summary>
        public List<EducationDTO> EducationList { get; set; }

        /// <summary>
        /// 工作经历列表
        /// </summary>
        public List<WorkingDTO> WorkingList { get; set; }

        /// <summary>
        /// 成果列表
        /// </summary>
        public List<AchievementDTO> AchievementList { get; set; }
    }

    /// <summary>
    /// 教育经历DTO
    /// </summary>
    public class EducationDTO
    {
        /// <summary>
        /// 教育经历ID
        /// </summary>
        [Required]
        public string EducationId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Required]
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Required]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 地点
        /// </summary>
        [Required]
        public string Location { get; set; }

        /// <summary>
        /// 专业
        /// </summary>
        [Required]
        public string Major { get; set; }

    }

    /// <summary>
    /// 工作经历DTO
    /// </summary>
    public class WorkingDTO
    {
        /// <summary>
        /// 工作经历ID
        /// </summary>
        [Required]
        public string WorkingId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Required]
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Required]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 地点
        /// </summary>
        [Required]
        public string Location { get; set; }

        /// <summary>
        /// 专业
        /// </summary>
        [Required]
        public string Major { get; set; }
    }

    /// <summary>
    /// 成果DTO
    /// </summary>
    public class AchievementDTO
    {
        /// <summary>
        /// 成果ID
        /// </summary>
        [Required]
        public string AchievementId { get; set; }

        /// <summary>
        /// 成果时间
        /// </summary>
        [Required]
        public DateTime AchievementTime { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Depart
        /// </summary>
        [Required]
        public string Depart { get; set; }

        /// <summary>
        /// 注释
        /// </summary>
        public string Note { get; set; }

    }
}
