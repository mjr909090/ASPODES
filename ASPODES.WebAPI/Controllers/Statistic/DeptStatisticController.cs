using ASPODES.WebAPI.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using ASPODES.WebAPI.Common;
using ASPODES.DTO;
using AutoMapper;

namespace ASPODES.WebAPI.Controllers.Statistic
{
    /// <summary>
    /// 院管理员统计模块控制器
    /// </summary>
    public class DeptStatisticController : ApiController
    {
        private DeptStatisticService _StatisticService;

        public DeptStatisticController(DeptStatisticService deptStatisticService)
        {
            this._StatisticService = deptStatisticService;
        }

        /// <summary>
        /// 院管理员统计申请书
        /// </summary>
        /// <returns></returns>
        [Route("api/DeptStatistic/GetAppStatistic")]
        [HttpGet]
        public HttpResponseMessage GetAppStatistic()
        {
            try
            {
                return ResponseWrapper.SuccessResponse(
                    _StatisticService.GetAppStatisticByCateAndStatus());
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 院管理员通过项目类别统计在研项目
        /// </summary>
        /// <returns></returns>
        [Route("api/DeptStatistic/GetProjectStatistic")]
        [HttpGet]
        public HttpResponseMessage GetProjectStatistic()
        {
            try
            {
                return ResponseWrapper.SuccessResponse(
                    _StatisticService.GetProjectStatistic());
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 院管理员通过主持单位统计在研项目
        /// </summary>
        /// <returns></returns>
        [Route("api/DeptStatistic/GetProjectStatisticByInst")]
        [HttpGet]
        public HttpResponseMessage GetProjectStatisticByInst()
        {
            try
            {
                return ResponseWrapper.SuccessResponse(
                    _StatisticService.GetProjectStatisticByInst());
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 院管理员通过年份和大类统计在研项目
        /// </summary>
        /// <returns></returns>
        [Route("api/DeptStatistic/GetProjectStatisticByYear/{startYear}")]
        [HttpGet]
        public HttpResponseMessage GetProjectStatisticByYear(int startYear=2017)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(
                    _StatisticService.GetProjectStatisticByYearAndCate(startYear));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 院管理员统计当年各个大类的经费
        /// </summary>
        /// <returns></returns>
        [Route("api/DeptStatistic/GetFundStatisticByCate")]
        [HttpGet]
        public HttpResponseMessage GetFundStatisticByCate()
        {
            try
            {
                return ResponseWrapper.SuccessResponse(
                    _StatisticService.GetFundStatisticByCate());
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 院管理员统计各个大类年度经费的变化
        /// </summary>
        /// <returns></returns>
        [Route("api/DeptStatistic/GetFundStatisticByCateAndYear")]
        [HttpGet]
        public HttpResponseMessage GetFundStatisticByCateAndYear()
        {
            try
            {
                return ResponseWrapper.SuccessResponse(
                    _StatisticService.GetFundStatisticByCateAndYear(2017));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 院管理员统计本年各个单位的经费
        /// </summary>
        /// <returns></returns>
        [Route("api/DeptStatistic/GetFundStatisticByInst")]
        [HttpGet]
        public HttpResponseMessage GetFundStatisticByInst()
        {
            try
            {
                return ResponseWrapper.SuccessResponse(
                    Mapper.Map<InstAndFundDTO[]>(_StatisticService.GetFundStatisticByInst()));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 通过年份和大类统计申请书资助率
        /// </summary>
        /// <returns></returns>
        [Route("api/DeptStatistic/GetFundRatioStatisticByYearAndCate")]
        [HttpGet]
        public HttpResponseMessage GetFundRatioStatisticByYearAndCate()
        {
            try
            {
                return ResponseWrapper.SuccessResponse(
                    _StatisticService.GetFundRatioStatisticByYearAndCate(2017));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 院管理员统计经费排名前十的已结题项目
        /// </summary>
        /// <returns></returns>
        [Route("api/DeptStatistic/GetTop10FundProject")]
        [HttpGet]
        public HttpResponseMessage GetTop10FundProject()
        {
            try
            {
                return ResponseWrapper.SuccessResponse(
                    _StatisticService.GetTop10FundProject());
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 院管理员通过八个大类统计申请书数量和资助数量
        /// </summary>
        /// <returns></returns>
        [Route("api/DeptStatistic/GetFundAndAcceptAppByCate/{year}")]
        [HttpGet]
        public HttpResponseMessage GetFundAndAcceptAppByCate(int year)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(
                    _StatisticService.GetFundAndAcceptAppByCate(year));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
    }
}