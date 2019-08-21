using ASPODES.Model;
using ASPODES.WebAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPODES.WebAPI.Service
{
    /// <summary>
    /// 关于单位的统计模块service层
    /// </summary>
    public class InstStatisticService
    {
        private ApplicationStatisticRepository _appRepository;
        private ProjectStatisticRepository _projectRepository;
        private FundStatisticRepository _fundRepository;

        /// <summary>
        /// 构造函数，注入三种统计方式的repository层
        /// </summary>
        /// <param name="appRepository"></param>
        /// <param name="projectRepository"></param>
        /// <param name="fundRepository"></param>
        public InstStatisticService(ApplicationStatisticRepository appRepository,
               ProjectStatisticRepository projectRepository,
               FundStatisticRepository fundRepository)
        {
            this._appRepository = appRepository;
            this._projectRepository = projectRepository;
            this._fundRepository = fundRepository;
        }

        /// <summary>
        /// 用于单位管理员统计申请书，分八个类别和
        /// 三个状态（待审核、单位管理员审核通过、驳回）统计。
        /// </summary>
        /// <returns></returns>
        public ApplicationStatistic[] GetAppStatisticByCateAndStatus()
        {
            int instId = UserHelper.GetCurrentUser().InstId;
            ApplicationStatistic[] statisticArray = _appRepository.GetInstStatisticApp(instId).ToArray();
            return statisticArray;
        }

        /// <summary>
        /// 用于项目统计，分八个类别统计
        /// </summary>
        /// <returns></returns>
        public ProjectStatistic[] GetProjectStatistic()
        {
            int instituteId = UserHelper.GetCurrentUser().InstId;
            ProjectStatistic[] statisticArray = _projectRepository.GetStatisticProject(instituteId).ToArray();
            return statisticArray;
        }

        /// <summary>
        /// 统计当年各个大类所有资金
        /// </summary>
        /// <returns></returns>
        public FundStatisticByCate[] GetFundStatisticByCate()
        {
            int instituteId = UserHelper.GetCurrentUser().InstId;
            FundStatisticByCate[] statistic = _fundRepository.GetFundStatisticByCate(instituteId).ToArray();
            return statistic;
        }

        /// <summary>
        /// 统计本单位各个大类的年度经费变化
        /// </summary>
        /// <returns></returns>
        public StatisticByYearAndCate<double>[] GetFundStatisticByCateAndYear(int startYear)
        {
            int instituteId = UserHelper.GetCurrentUser().InstId;
            StatisticByYearAndCate<double>[] statistic = _fundRepository.GetFundStatisticByCateAndYear(startYear, instituteId).ToArray();
            return statistic;
        }

        /// <summary>
        /// 通过年份和大类统计申请书资助率
        /// </summary>
        /// <returns></returns>
        public StatisticByYearAndCate<double>[] GetFundRatioStatisticByYearAndCate(int startYear)
        {
            int instituteId = UserHelper.GetCurrentUser().InstId;
            StatisticByYearAndCate<double>[] statistic = _fundRepository.GetFundRatioStatisticByYearAndCate(startYear, instituteId).ToArray();
            return statistic;
        }

        /// <summary>
        /// 统计经费排名前十的已结题项目
        /// </summary>
        /// <returns></returns>
        public Top10FundProject[] GetTop10FundProject()
        {
            int instituteId = UserHelper.GetCurrentUser().InstId;
            Top10FundProject[] statistic = _fundRepository.GetTop10FundProject(instituteId).ToArray();
            return statistic;
        }

        /// <summary>
        /// 通过八个大类统计申请书数量和资助数量
        /// </summary>
        /// <returns></returns>
        public FundAndAcceptByCate[] GetFundAndAcceptAppByCate(int year)
        {
            int instituteId = UserHelper.GetCurrentUser().InstId;
            FundAndAcceptByCate[] statistic = _appRepository.GetFundAndAcceptAppByCate(year, instituteId).ToArray();
            return statistic;
        }
    }
}