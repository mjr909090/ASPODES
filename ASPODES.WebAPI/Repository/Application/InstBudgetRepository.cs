using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity.Migrations;
using System.Net.Http;

using AutoMapper;

using ASPODES.WebAPI.Common;
using ASPODES.Database;
using ASPODES.Model;
using ASPODES.DTO.Application;
using System.Net;
using System.Web.Http;
using ASPODES.WebAPI.Security;

namespace ASPODES.WebAPI.Repository
{
    public class InstBudgetRepository
    {
        /// <summary>
        /// 添加单位预算
        /// </summary>
        /// <param name="budgetDTO">单位预算信息</param>
        /// <returns>
        /// 需要下一步修改
        /// </returns>
        public InstBudget AddInstBudget(AddInstTotalWithAnnualBudget budgetDTO, Func<Application, CurrentUser, bool> privilege)
        {

            using (var ctx = new AspodesDB())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    var application = ctx.Applications.FirstOrDefault(a => a.ApplicationId == budgetDTO.ApplicationId);
                    if (application == null) throw new NotFoundException("未找到申请书");
                    var userInfo = UserHelper.GetCurrentUser();
                    if (!privilege(application, userInfo))
                    {
                        throw new UnauthorizationException();
                    }
                    InstBudget instBudget = new InstBudget()
                    {
                        InstituteId = budgetDTO.InstituteId,
                        ApplicationId = budgetDTO.ApplicationId,
                        Amount = budgetDTO.Amount
                    };

                    try
                    {
                        //如果有旧的记录，删除
                        var old = ctx.InstBudgets.FirstOrDefault(ib => ib.ApplicationId == budgetDTO.ApplicationId && ib.InstituteId == budgetDTO.InstituteId);
                        if (old != null)
                        {
                            ctx.InstBudgets.Remove(old);
                        }
                        //添加新的单位预算
                        instBudget = ctx.InstBudgets.Add(instBudget);
                        ctx.SaveChanges();

                        //添加单位预算的年度预算
                        if (budgetDTO.AnnualBudgets != null)
                        {
                            int year = 1;
                            foreach (var ab in budgetDTO.AnnualBudgets)
                            {
                                if (ab < 0) throw new OtherException("预算额不能是负数");
                                InstAnnualBudget annual = new InstAnnualBudget()
                                {
                                    InstBudgetId = instBudget.InstBudgetId,
                                    Year = year++,
                                    Amount = ab
                                };
                                ctx.InstAnnualBudgets.Add(annual);
                            }
                        }
                        ctx.SaveChanges();
                        transaction.Commit();
                        return instBudget;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }
        /// <summary>
        /// 获取申请书某一单位的预算
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <param name="instId">单位ID</param>
        /// <returns>
        /// </returns>
        public InstBudget GetInstBudget(string applicationId, int instId)
        {
            using (var ctx = new AspodesDB())
            {
                var instBudget = ctx.InstBudgets.FirstOrDefault(ib => ib.ApplicationId == applicationId && ib.InstituteId == instId);
                if (null == instBudget) throw new NotFoundException();
                return instBudget;
            }
        }

        /// <summary>
        /// 获取申请书的单位预算列表
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <returns></returns>
        public List<GetInstBudgetDTO> GetInstBudgets(string applicationId)
        {
            using (var ctx = new AspodesDB())
            {
                //取出单位预算列表
                var ibs = ctx.InstBudgets.Include("InstAnnualBudgets")
                    .Where(ib => ib.ApplicationId == applicationId)
                    .Select(Mapper.Map<GetInstBudgetDTO>)
                    .ToList();

                return ibs;
            }
        }

        /// <summary>
        /// 删除单位预算
        /// </summary>
        /// <param name="instBudgetId">单位预算ID</param>
        /// <returns></returns>
        public void Delete(int instBudgetId, Func<Application, CurrentUser, bool> privilege)
        {
            using (var ctx = new AspodesDB())
            {
                InstBudget instBudget = ctx.InstBudgets.FirstOrDefault(ib => ib.InstBudgetId == instBudgetId);
                if (null == instBudget)
                    throw new NotFoundException("未找到预算内容");
                if (!privilege(instBudget.Application, UserHelper.GetCurrentUser()))
                {
                    throw new UnauthorizationException();
                }
                //单位年度预算设置为级联删除，单位预算和它的年度预算同时删除
                ctx.InstBudgets.Remove(instBudget);
                ctx.SaveChanges();
            }
        }
    }
}