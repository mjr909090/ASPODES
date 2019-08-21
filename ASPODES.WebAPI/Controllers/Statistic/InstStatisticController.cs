using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 单位管理员统计模块控制器
    /// </summary>
    public class InstStatisticController : ApiController
    {
        private InstStatisticService _StatisticService;

        public InstStatisticController(InstStatisticService instStatisticService)
        {
            this._StatisticService = instStatisticService;
        }

        /// <summary>
        /// 单位管理员统计申请书
        /// </summary>
        /// <returns></returns>
        [Route("api/InstStatistic/GetAppStatistic")]
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
        /// 单位管理员统计项目
        /// </summary>
        /// <returns></returns>
        [Route("api/InstStatistic/GetProjectStatistic")]
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
        /// 单位管理员统计当年各个大类的经费
        /// </summary>
        /// <returns></returns>
        [Route("api/InstStatistic/GetFundStatisticByCate")]
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
        /// 单位管理员统计各个大类年度经费的变化
        /// </summary>
        /// <returns></returns>
        [Route("api/InstStatistic/GetFundStatisticByCateAndYear")]
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
        /// 通过年份和大类统计申请书资助率
        /// </summary>
        /// <returns></returns>
        [Route("api/InstStatistic/GetFundRatioStatisticByYearAndCate")]
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
        /// 单位管理员统计经费排名前十的已结题项目
        /// </summary>
        /// <returns></returns>
        [Route("api/InstStatistic/GetTop10FundProject")]
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
        /// 单位管理员通过八个大类统计申请书数量和资助数量
        /// </summary>
        /// <returns></returns>
        [Route("api/InstStatistic/GetFundAndAcceptAppByCate/{year}")]
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