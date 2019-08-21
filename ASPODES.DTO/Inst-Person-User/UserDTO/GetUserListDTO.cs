using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Inst_Person_User
{
    /// <summary>
    /// 获取用户列表DTO
    /// </summary>
    public class GetUserListDTO
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 人员ID
        /// </summary>
        public int? PersonId { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDCard { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所属单位ID
        /// </summary>
        public int? InstituteId { get; set; }

        /// <summary>
        /// 是否登录
        /// </summary>
        public bool? Login { get; set; }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime LastLogin { get; set; }
    }
}
