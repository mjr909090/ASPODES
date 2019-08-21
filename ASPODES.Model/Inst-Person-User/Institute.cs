using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.Model
{
    /// <summary>
    /// 单位
    /// </summary>
    public class Institute
    {
        public Institute()
        {
            ParentId = 1;
            Status = "C";
        }
        /// <summary>
        /// 自增主键
        /// </summary>
        public int InstituteId { get; set; }

        /// <summary>
        /// 机构编码
        /// </summary>
        [StringLength(64)]
        public string Code { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        [Required,StringLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        [StringLength(256)]
        public string ShortName { get; set; }
        
        /// <summary>
        /// 上级单位
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// 机构类型
        /// </summary>
        public InstituteType Type { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        [StringLength(32)]
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
        [Required]
        public string Status { get; set; }

        public string ContactId { get; set; }

        /// <summary>
        /// 单位联系人
        /// </summary>
        public virtual User Contact { get; set; }
    }

    /// <summary>
    /// 单位类型
    /// </summary>
    public enum InstituteType
    {
        /// <summary>
        /// 外单位
        /// </summary>
        PARTNER,
        /// <summary>
        /// 研究所
        /// </summary>
        INSTITUTE,
        /// <summary>
        /// 院机关
        /// </summary>
        HEADQUATER
    }
}
