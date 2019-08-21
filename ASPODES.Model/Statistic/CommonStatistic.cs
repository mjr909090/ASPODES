using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.Model
{
    /// <summary>
    /// 最普通的键值对单元，用于饼图，单线图，单柱状图的基本单元
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StatisticKeyValue<T>
    {
        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public T Value { get; set; }
    }

    /// <summary>
    /// 通过年份和大类分组统计模型
    /// </summary>
    public class StatisticByYearAndCate<T>
    {
        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 1.联盟重点工作项目
        /// </summary>
        public T AllianceFocusWork { get; set; }

        /// <summary>
        /// 2.应急性科研工作项目
        /// </summary>
        public T EmergencyResearchWork { get; set; }

        /// <summary>
        /// 3.科技基础性工作项目
        /// </summary>
        public T BasicScienceAndTechnologyWork { get; set; }

        /// <summary>
        /// 4.基础研究引导计划项目
        /// </summary>
        public T BasicResearchGuidancePlan { get; set; }

        /// <summary>
        /// 5.重大成果培育计划项目
        /// </summary>
        public T MajorResultsDevelopmentProgram { get; set; }

        /// <summary>
        /// 6.重大平台推进计划项目
        /// </summary>
        public T MajorPlatformPromotionPlan { get; set; }

        /// <summary>
        /// 7.重大项目储备计划项目
        /// </summary>
        public T MajorProjectReservePlan { get; set; }

        /// <summary>
        /// 8.农业智库建设计划项目
        /// </summary>
        public T AgriculturalThinkTankConstructionPlan { get; set; }

        /// <summary>
        /// 合计数量
        /// </summary>
        public T Total { get; set; }
    }
}
