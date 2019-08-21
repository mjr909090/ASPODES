using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using ASPODES.DTO.Application;
using ASPODES.Model;
using ASPODES.Database;
using ASPODES.WebAPI.Common;
using AutoMapper;

namespace ASPODES.WebAPI.Repository
{
    /// <summary>
    /// 申请书日志操作类
    /// </summary>
    public class ApplicationLogRepository
    {
        /// <summary>
        /// 添加一条申请书操作日志
        /// </summary>
        /// <param name="ctx">数据库上下文对象</param>
        /// <param name="logDTO">需要添加的日志信息</param>
        /// <returns>
        /// 成功，返回TRUE
        /// 否则，抛出一个异常
        /// </returns>
        public static bool AddApplicationLog(AspodesDB ctx, AddApplicationLogDTO logDTO)
        {

            ApplicationLog log = Mapper.Map<ApplicationLog>(logDTO);
            log.Time = DateTime.Now;
            ctx.ApplicationLogs.Add(log);
            return true;
        }

        /// <summary>
        /// 获取applicationId申请书，actionType类型操作的最新日志
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <param name="actionType">操作类型</param>
        /// <returns>
        /// 成功返回ResponseStatus.success和日志信息
        /// 没有找到数据返回ResponseStatus.parameter_error,
        /// 出错返回ResponseStatus.unknown_error和错误信息
        /// </returns>
        public List<GetApplicationLogDTO> GetApplicationLog(string applicationId, int actionType)
        {
            List<GetApplicationLogDTO> logs = new List<GetApplicationLogDTO>();

            using (var ctx = new AspodesDB())
            {
                logs = ctx.ApplicationLogs
                    .Where(al => al.ApplicationId == applicationId && (int)al.Operation == actionType)
                    .OrderByDescending(al => al.Time)
                    .Select(Mapper.Map<GetApplicationLogDTO>)
                    .Take<GetApplicationLogDTO>(1)
                    .ToList();
            }
            return logs;
        }

    }
}