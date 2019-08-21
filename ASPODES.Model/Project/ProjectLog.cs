using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASPODES.Model
{
    /// <summary>
    /// 项目日志
    /// </summary>
    public class ProjectLog
    {
        /// <summary>
        /// 项目日志ID
        /// </summary>
        public int ProjectLogId { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        [StringLength(64)]
        public string ProjectId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int AssociateId { get; set; }

        public AssociateType AssociateType { get; set; }

        public ProjectOpreationType OperationType { get; set; }

        [StringLength(0124)]
        public string Comment { get; set; }

        public virtual Project Project { get; set; }
    }


    public enum ProjectOpreationType
    {
        INSTITUTE_REJECT,

        DEPART_REJECT
    }

    public enum AssociateType
    {
        ANNUAL_TASK,

        ATTATCHMENT,

        CONCLUDING_REPORT
    }
}
