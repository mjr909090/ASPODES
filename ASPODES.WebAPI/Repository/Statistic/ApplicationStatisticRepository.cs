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
    /// 用于统计申请书的repository层
    /// </summary>
    public class ApplicationStatisticRepository
    {
        private AspodesDB _context;
        private int currentYear;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public ApplicationStatisticRepository(AspodesDB context)
        {
            this._context = context;
            this.currentYear = DateTime.Now.Year;
        }

        /// <summary>
        /// 单位管理员通过八个大类和三个状态查询对申请书分组统计
        /// </summary>
        /// <param name="instituteId">单位ID</param>
        /// <returns></returns>
        public IEnumerable<ApplicationStatistic> GetInstStatisticApp
            (int instituteId)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@instituteId", instituteId),
                new SqlParameter("@currentYear", currentYear)
            };

            var statistics = _context.Database.SqlQuery<ApplicationStatistic>
                ("dbo.spInstStatisticApp @instituteId,@currentYear", param);
            return statistics;
        }

        /// <summary>
        /// 院管理员通过八个大类和三个状态查询对申请书分组统计
        /// </summary>
        /// <param name="instituteId">单位ID</param>
        /// <returns></returns>
        public IEnumerable<ApplicationStatistic> GetDeptStatisticApp()
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@currentYear", currentYear)
            };

            var statistics = _context.Database.SqlQuery<ApplicationStatistic>
                ("dbo.spDeptStatisticApp @currentYear", param);
            return statistics;
        }

        /// <summary>
        /// 通过八个大类统计申请书数量和资助数量
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="instituteId">单位ID</param>
        /// <returns></returns>
        public IEnumerable<FundAndAcceptByCate> GetFundAndAcceptAppByCate(int year, int? instituteId=null)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@year", year),
                instituteId.HasValue ? new SqlParameter("@instituteId", instituteId.Value)
                            : new SqlParameter("@instituteId", DBNull.Value)
            };

            var statistics = _context.Database.SqlQuery<FundAndAcceptByCate>
                ("dbo.spStatisticAcceptAndFundApp @year,@instituteId", param);
            return statistics;
        }
    }
}