using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Inst_Person_User
{
    /// <summary>
    /// 获取专家信息DTO
    /// </summary>
    public class GetExpertInfoDTO
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所属单位ID
        /// </summary>
        public int InstituteId { get; set; }

        /// <summary>
        /// 所属单位名称
        /// </summary>
        public string InstituteName { get; set; }

        /// <summary>
        /// 人员ID
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// 所属领域ID_1
        /// </summary>
        public int FirstExpertFieldId { get; set; }

        /// <summary>
        /// 子领域ID_1
        /// </summary>
        public string FirstSubFieldId { get; set; }

        /// <summary>
        /// 所属领域ID_2
        /// </summary>
        public int SecondExpertFieldId { get; set; }

        /// <summary>
        /// 子领域ID_2
        /// </summary>
        public string SecondSubFieldId { get; set; }
    }
}
