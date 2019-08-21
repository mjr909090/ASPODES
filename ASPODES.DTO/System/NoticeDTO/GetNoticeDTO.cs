using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;

namespace ASPODES.DTO.System
{
    /// <summary>
    /// 获取页面通知DTO
    /// </summary>
    public class GetNoticeDTO
    {
        /// <summary>
        /// 通知总数
        /// </summary>
        public int NoticeTotalNum { get; set; }

        /// <summary>
        /// 通知总页数
        /// </summary>
        public int NoticePageNum { get; set; }

        /// <summary>
        /// 通知当前页数
        /// </summary>
        public int NoticePage { get; set; }

        /// <summary>
        /// 当前页通知条数
        /// </summary>
        public int NoticeNum { get; set; }

        /// <summary>
        /// 通知列表
        /// </summary>
        public List<NoticeDTO> NoticeList { get; set; }
    }
}
