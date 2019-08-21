using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Inst_Person_User
{
    /// <summary>
    /// 添加单位DTO
    /// </summary>
    public class AddInstituteDTO
    {
        /// <summary>
        /// 单位ID
        /// </summary>
        public int? InstituteId { get; set; }

        /// <summary>
        /// 机构编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 上级单位ID
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// 机构类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// 网址
        /// </summary>
        public string WebSite { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        ///  C 正常、L-锁定、D-删除， 默认值C
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 单位联系人ID
        /// </summary>
        public string ContactId { get; set; }
    }
}
