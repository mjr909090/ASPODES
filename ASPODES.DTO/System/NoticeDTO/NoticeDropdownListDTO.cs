using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.System
{
    /// <summary>
    /// 通知下拉列表DTO
    /// </summary>
    public class NoticeDropdownListDTO
    {
        /// <summary>
        /// 未读消息数量
        /// </summary>
        public int UnReadNum { get; set; }

        /// <summary>
        /// 通知列表
        /// </summary>
        public List<NoticeDTO> Notices { get; set; }
    }
}
