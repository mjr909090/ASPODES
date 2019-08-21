using ASPODES.Model;
using ASPODES.WebAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPODES.WebAPI.Service
{
    /// <summary>
    /// 院管理与统计申请书
    /// </summary>
    public class DeptStatisticService
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
        public DeptStatisticService(ApplicationStatisticRepository appRepository,
               ProjectStatisticRepository projectRepository,
               FundStatisticRepository fundRepository)
        {
            this._appRepository = appRepository;
            this._projectRepository = projectRepository;
            this._fundRepository = fundRepository;
        }

        /// <summary>
        /// 用于院管理员统计申请书，分八个类别和
        /// 三个状态（待审核、受理、不受理）统计。
        /// </summary>
        /// <returns></returns>
        public ApplicationStatistic[] GetAppStatisticByCateAndStatus()
        {
            ApplicationStatistic[] statisticArray = _appRepository.GetDeptStatisticApp().ToArray();
            return statisticArray;
        }

        /// <summary>
        /// 用于在研项目统计，分八个类别统计
        /// </summary>
        /// <returns></returns>
        public ProjectStatistic[] GetProjectStatistic()
        {
            ProjectStatistic[] statisticArray = _projectRepository.GetStatisticProject().ToArray();
            return statisticArray;
        }

        /// <summary>
        /// 用于在研项目项目统计，分单位进行统计
        /// </summary>
        /// <returns></returns>
        public ProjectStatisticByInst[] GetProjectStatisticByInst()
        {
            ProjectStatisticByInst[] statisticArray = _projectRepository.GetStatisticProjectByInst().ToArray();
            return statisticArray;
        }

        /// <summary>
        /// 通过年份和大类统计项目
        /// </summary>
        /// <param name="startYear"></param>
        /// <returns></returns>
        public StatisticByYearAndCate<int>[] GetProjectStatisticByYearAndCate(int startYear=2017)
        {
            StatisticByYearAndCate<int>[] statistic = _projectRepository.getProjectByYearAndCate(startYear).ToArray();
            return statistic;
        }

        /// <summary>
        /// 统计当年各个大类所有资金
        /// </summary>
        /// <returns></returns>
        public FundStatisticByCate[] GetFundStatisticByCate()
        {
            FundStatisticByCate[] statistic = _fundRepository.GetFundStatisticByCate().ToArray();
            return statistic;
        }

        /// <summary>
        /// 统计各个大类的年度经费变化
        /// </summary>
        /// <returns></returns>
        public StatisticByYearAndCate<double>[] GetFundStatisticByCateAndYear(int startYear)
        {
            StatisticByYearAndCate<double>[] statistic = _fundRepository.GetFundStatisticByCateAndYear(startYear).ToArray();
            return statistic;
        }

        /// <summary>
        /// 统计当年每个单位的经费
        /// </summary>
        /// <returns></returns>
        public StatisticKeyValue<double>[] GetFundStatisticByInst()
        {
            StatisticKeyValue<double>[] statistic = _fundRepository.GetFundStatisticByInst().ToArray();
            return statistic;
        }

        /// <summary>
        /// 通过年份和大类统计申请书资助率
        /// </summary>
        /// <returns></returns>
        public StatisticByYearAndCate<double>[] GetFundRatioStatisticByYearAndCate(int startYear)
        {
            StatisticByYearAndCate<double>[] statistic = _fundRepository.GetFundRatioStatisticByYearAndCate(startYear).ToArray();
            return statistic;
        }

        /// <summary>
        /// 统计经费排名前十的已结题项目
        /// </summary>
        /// <returns></returns>
        public Top10FundProject[] GetTop10FundProject()
        {
            Top10FundProject[] statistic = _fundRepository.GetTop10FundProject().ToArray();
            return statistic;
        }

        /// <summary>
        /// 通过八个大类统计申请书数量和资助数量
        /// </summary>
        /// <returns></returns>
        public FundAndAcceptByCate[] GetFundAndAcceptAppByCate(int year)
        {
            FundAndAcceptByCate[] statistic = _appRepository.GetFundAndAcceptAppByCate(year).ToArray();
            return statistic;
        }
    }
}