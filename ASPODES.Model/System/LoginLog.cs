using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPODES.Model
{
    /// <summary>
    /// 用户登录日志
    /// </summary>
    public class LoginLog
    {
        /// <summary>
        /// 主键，UUID
        /// </summary>
        [StringLength(64)]
        public string LoginLogId { get; set; }

        /// <summary>
        /// 外键，参照User表
        /// </summary>
        [StringLength(256)]
        public string UserId { get; set; }

        /// <summary>
        /// 登录角色，如有多个用，分隔
        /// </summary>
        [StringLength(256)]
        public string Roles { get; set; }

        /// <summary>
        /// 登录的IP
        /// </summary>
        [StringLength(64)]
        public string LoginIP { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime? LoginTime { get; set; }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime? LastActiveTime { get; set; }

        /// <summary>
        /// 是否注销
        /// </summary>
        public bool IsLogout { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] TimeStamp { get; set; }

        /// <summary>
        /// 导航属性，用户
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// 登录时间戳
        /// </summary>
        public double LoginTimeStamp { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        [StringLength(512)]
        public string Token { get; set; }
    }
}
