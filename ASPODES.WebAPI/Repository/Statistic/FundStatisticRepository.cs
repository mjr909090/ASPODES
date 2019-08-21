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
    /// 用于统计经费的repository层
    /// </summary>
    public class FundStatisticRepository
    {
        private AspodesDB _context;
        private int currentYear;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public FundStatisticRepository(AspodesDB context)
        {
            this._context = context;
            //为了有统计数据，暂时修改
            this.currentYear = DateTime.Now.Year;
        }

        /// <summary>
        /// 通过大类统计项目资金
        /// </summary>
        /// <param name="instituteId"></param>
        /// <returns></returns>
        public IEnumerable<FundStatisticByCate> GetFundStatisticByCate(int? instituteId = null)
        {
            SqlParameter[] param = new SqlParameter[]
            {

                new SqlParameter("@year", currentYear),
                instituteId.HasValue ? new SqlParameter("@instituteId", instituteId.Value)
                            : new SqlParameter("@instituteId", DBNull.Value)
            };

            var statistics = _context.Database.SqlQuery<FundStatisticByCate>
                ("dbo.spFundStatisticByCate @year,@instituteId", param);
            return statistics;
        }

        /// <summary>
        /// 通过年份和大类统计经费
        /// </summary>
        /// <param name="instituteId"></param>
        /// <param name="startYear"></param>
        /// <returns></returns>
        public IEnumerable<StatisticByYearAndCate<double>> GetFundStatisticByCateAndYear(int startYear, int? instituteId = null)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@startYear", startYear),
                new SqlParameter("@endYear", currentYear),
                instituteId.HasValue ? new SqlParameter("@instituteId", instituteId.Value)
                            : new SqlParameter("@instituteId", DBNull.Value)
            };

            var statistics = _context.Database.SqlQuery<StatisticByYearAndCate<double>>
                ("dbo.spFundStatisticByYearAndCate @startYear,@endYear,@instituteId", param);
            return statistics;
        }

        /// <summary>
        /// 通过单位统计经费
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StatisticKeyValue<double>> GetFundStatisticByInst()
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@year", currentYear)
            };

            var statistics = _context.Database.SqlQuery<StatisticKeyValue<double>>
                ("dbo.spFundStatisticByInst @year", param);
            return statistics;
        }

        /// <summary>
        /// 通过年份和大类统计申请书资助率
        /// </summary>
        /// <param name="startYear"></param>
        /// <param name="instituteId"></param>
        /// <returns></returns>
        public IEnumerable<StatisticByYearAndCate<double>> GetFundRatioStatisticByYearAndCate(int startYear, int? instituteId=null)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@startYear", startYear),
                new SqlParameter("@endYear", currentYear),
                instituteId.HasValue ? new SqlParameter("@instituteId", instituteId.Value)
                            : new SqlParameter("@instituteId", DBNull.Value)
            };

            var statistics = _context.Database.SqlQuery<StatisticByYearAndCate<double>>
                ("dbo.spFundRatioStatisticByYearAndCate @startYear,@endYear,@instituteId", param);
            return statistics;
        }

        /// <summary>
        /// 统计经费排名前十的已结题项目
        /// </summary>
        /// <param name="instituteId"></param>
        /// <returns></returns>
        public IEnumerable<Top10FundProject> GetTop10FundProject(int? instituteId = null)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                instituteId.HasValue ? new SqlParameter("@instituteId", instituteId.Value)
                            : new SqlParameter("@instituteId", DBNull.Value)
            };

            var statistics = _context.Database.SqlQuery<Top10FundProject>
                ("dbo.spTop10FundProject @instituteId", param);
            return statistics;
        }
    }
}