using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.System
{
    /// <summary>
    /// 数量封装类
    /// </summary>
    public class NumberListDTO
    {
        /// <summary>
        /// 院待受理申请书数量
        /// </summary>
        public int DepartAppNum { get; set; }
        /// <summary>
        /// 院不受理申请书数量
        /// </summary>
        public int DepartRefuseAppNum { get; set; }
        /// <summary>
        /// 院待审核结题项目数量
        /// </summary>
        public int DepartProjectNum { get; set; }
        /// <summary>
        /// 院驳回结题项目数量
        /// </summary>
        public int DepartRejectProjectNum { get; set; }
        /// <summary>
        /// 院待审核任务书数量
        /// </summary>
        public int DepartATBookNum { get; set; }
        /// <summary>
        /// 院驳回任务书数量
        /// </summary>
        public int DepartRejectATBookNum { get; set; }
        /// <summary>
        /// 院待审核年度报告数量
        /// </summary>
        public int DepartATTalkNum { get; set; }
        /// <summary>
        /// 院驳回年度报告数量
        /// </summary>
        public int DepartRejectATTalkNum { get; set; }
        /// <summary>
        /// 单位待审核申请书数量
        /// </summary>
        public int IntAppNum { get; set; }
        /// <summary>
        /// 单位驳回申请书数量
        /// </summary>
        public int IntRejectAppNum { get; set; }
        /// <summary>
        /// 单位待审核结题项目数量
        /// </summary>
        public int IntProjectNum { get; set; }
        /// <summary>
        /// 单位驳回结题项目数量
        /// </summary>
        public int IntRejectProjectNum { get; set; }
        /// <summary>
        /// 单位待审核任务书数量
        /// </summary>
        public int IntATBookNum { get; set; }
        /// <summary>
        /// 单位驳回任务书数量
        /// </summary>
        public int IntRejectATBookNum { get; set; }
        /// <summary>
        /// 单位待审核年度报告数量
        /// </summary>
        public int IntATTalkNum { get; set; }
        /// <summary>
        /// 单位驳回年度报告数量
        /// </summary>
        public int IntRejectATTalkNum { get; set; }

        /// <summary>
        /// 专家待审核申请书数量
        /// </summary>
        public int CheckingAppNum { get; set; }
    }
}
