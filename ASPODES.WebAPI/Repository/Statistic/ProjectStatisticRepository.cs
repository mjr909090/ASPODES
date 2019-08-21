using ASPODES.Database;
using ASPODES.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ASPODES.WebAPI.Repository
{
    /// <summary>
    /// 用于统计项目的repository层
    /// </summary>
    public class ProjectStatisticRepository
    {
        private AspodesDB _context;
        private int currentYear;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public ProjectStatisticRepository(AspodesDB context)
        {
            this._context = context;
            this.currentYear = DateTime.Now.Year;
        }

        /// <summary>
        /// 管理员统计在研项目，通过项目类别分类
        /// </summary>
        /// <param name="instituteId">单位ID,为null时全院查询</param>
        /// <returns></returns>
        public IEnumerable<ProjectStatistic> GetStatisticProject(int? instituteId = null)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                instituteId.HasValue ? new SqlParameter("@instituteId", instituteId.Value)
                            : new SqlParameter("@instituteId", DBNull.Value),
            };

            var statistics = _context.Database.SqlQuery<ProjectStatistic>
                ("dbo.spStatisticProject @instituteId", param);
            return statistics;
        }

        /// <summary>
        /// 院管理员统计在研项目，通过项目主持单位分类
        /// </summary>
        /// <param name="instituteId">单位ID,为null时全院查询</param>
        /// <returns></returns>
        public IEnumerable<ProjectStatisticByInst> GetStatisticProjectByInst()
        {
            var statistics = _context.Database.SqlQuery<ProjectStatisticByInst>
                ("dbo.spStatisticProjectByInst");
            return statistics;
        }

        /// <summary>
        /// 统计在研项目，通过年份和大类分组
        /// </summary>
        /// <param name="startYear"></param>
        /// <returns></returns>
        public IEnumerable<StatisticByYearAndCate<int>> getProjectByYearAndCate(int startYear)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@startYear", startYear),
                new SqlParameter("@endYear", currentYear),
            };

            var statistics = _context.Database.SqlQuery<StatisticByYearAndCate<int>>
                ("dbo.spProjectStatisticByYearAndCate @startYear,@endYear", param);
            return statistics;
        }
    }
}