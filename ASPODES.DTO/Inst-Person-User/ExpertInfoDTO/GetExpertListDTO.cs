using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Inst_Person_User
{
    /// <summary>
    /// 获取专家列表DTO
    /// </summary>
    public class GetExpertListDTO
    {
        /// <summary>
        /// 专家信息ID
        /// </summary>
        public string ExpertInfoId { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDCard { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 教育经历
        /// </summary>
        public string EducationHistor { get; set; }

        /// <summary>
        /// 工作经历
        /// </summary>
        public string WorkingHistory { get; set; }

        /// <summary>
        /// 成果
        /// </summary>
        public string Achievements { get; set; }
    }
}
