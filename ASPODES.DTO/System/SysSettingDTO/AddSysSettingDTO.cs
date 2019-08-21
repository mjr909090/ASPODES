using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.System
{
    /// <summary>
    /// 添加系统设置DTO
    /// </summary>
    public class AddSysSettingDTO
    {
        /// <summary>
        /// 设置变量名称
        /// </summary>
        public string SetName { get; set; }

        /// <summary>
        /// 设置变量值
        /// </summary>
        public string SetValue { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
