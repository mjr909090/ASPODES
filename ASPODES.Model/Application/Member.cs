using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace ASPODES.Model
{
    /// <summary>
    /// 项目成员
    /// </summary>
    public class Member
    {
       
        /// <summary>
        /// 所属申请书，组合主键
        /// </summary>
        [StringLength(64)]
        public string ApplicationId { get; set; }
        /// <summary>
        /// 人员，组合主键
        /// </summary>
        public int? PersonId { get; set; }
        
        /// <summary>
        /// 成员责任分工
        /// </summary>
        [StringLength(1024)]
        public string Task { get; set; }

        /// <summary>
        /// 导航属性，申请书
        /// </summary>
        public virtual Application Application { get; set; }
        /// <summary>
        /// 导航属性，人员
        /// </summary>
        public virtual Person Person { get; set; }
    }
}