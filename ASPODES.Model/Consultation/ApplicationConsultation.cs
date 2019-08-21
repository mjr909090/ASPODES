using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ASPODES.Model
{
    /// <summary>
    /// 申请书咨询审议
    /// </summary>
    public class ApplicationConsultation : Consultation
    {

        /// <summary>
        /// 申请书ID,外键，参照Application
        /// </summary>
        [StringLength(64)]
        public string ApplicationId { get; set; }

        /// <summary>
        /// 导航属性，申请书
        /// </summary>
        [JsonIgnore]
        public virtual Application Application { get; set; }

        /// <summary>
        /// 申请书咨询审议结果
        /// </summary>
        public ApplicationConsultationResult Result { get; set; }
    }

    /// <summary>
    /// 申请书咨询审议结果
    /// </summary>
    public enum ApplicationConsultationResult
    {
        /// <summary>
        /// 入库
        /// </summary>
        STORAGE,

        /// <summary>
        /// 资助
        /// </summary>
        SUPPORT,

        /// <summary>
        /// 不资助
        /// </summary>
        UNSUPPORT
    }
     
}
