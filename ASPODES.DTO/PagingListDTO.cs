using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO
{
    /// <summary>
    /// 分页DTO
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagingListDTO<T>
    {
        public PagingListDTO()
        {
            ItemDTOs = new List<T>();
            TotalPageNum = 1;
            NowPage = 1;
        }

        /// <summary>
        /// 申请书总数
        /// </summary>
        public int TotalNum { get; set; }

        /// <summary>
        /// 申请书总页数
        /// </summary>
        public int TotalPageNum { get; set; }

        /// <summary>
        /// 申请书当前页数
        /// </summary>
        public int NowPage { get; set; }

        /// <summary>
        /// 申请书当前页的数量
        /// </summary>
        public int NowNum { get; set; }
        public List<T> ItemDTOs{ get; set; }
    }
}
