using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ASPODES.Model
{
    /// <summary>
    /// 用户类
    /// </summary>
    public class User
    {
        public User()
        {
            ReviewAmount = 0;
        }
        /// <summary>
        /// 邮箱，用户名，主键
        /// </summary>
        [StringLength(256)]
        public string UserId { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required,StringLength(32)]
        public string Password { get; set; }
        /// <summary>
        /// 用户关联的人员ID，外键
        /// </summary>
        //public int? PersonId { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        [Required,StringLength(32)]
        public string IDCard { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [Required,StringLength(64)]
        public string Name { get; set; }
        /// <summary>
        /// sessionId
        /// 计划用session存储登录信息
        /// </summary>
        [StringLength(64)]
        public string SessionId { get; set; }
        /// <summary>
        /// 用户所属单位，外键，参照Institute表
        /// </summary>
        public int? InstituteId { get; set; }
        /// <summary>
        /// 登录状态
        /// </summary>
        public bool? Login { get; set; }
        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime? LastLogin { get; set; }

        /// <summary>
        /// 人员ID
        /// </summary>
        public int? PersonId { get; set; }

        /// <summary>
        /// 分配给专家的申请书数目
        /// </summary>
        
        public int? ReviewAmount { get; set; }
        /// <summary>
        /// 导航属性，人员
        /// </summary>
        public virtual Person Person { get; set; }
        /// <summary>
        /// 导航属性,单位
        /// </summary>
        public virtual Institute Institute{ get; set; }

        /// <summary>
        /// 导航属性,研究领域
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<ExpertField> ExpertFields { get; set; }
        
   }
}
