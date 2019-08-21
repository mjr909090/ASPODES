using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO
{
    /// <summary>
    /// 公司和经费
    /// </summary>
    public class InstAndFundDTO
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        public string InstName { get; set; }

        /// <summary>
        /// 经费额度
        /// </summary>
        public double Amount { get; set; }
    }
}
