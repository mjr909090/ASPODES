using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;

namespace ASPODES.DTO.Category
{
    public class SysSupportCategoryGetDTO
    {
        /// <summary>
        /// 资助类别ID
        /// </summary>
        public int SupportCategoryId { get; set; }

        /// <summary>
        /// 资助类别名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 资助类别列表
        /// </summary>
        public List<SupportCategory> Categorys { get; set; }
    }
}
