using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;

namespace ASPODES.DTO.Application
{
    /// <summary>
    /// 获取申请书操作记录DTO
    /// </summary>
    public class GetApplicationLogDTO
    {
        /// <summary>
        /// 申请书ID
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 操作用户的ID
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public ActionType Operation { get; set; }
    }
}
