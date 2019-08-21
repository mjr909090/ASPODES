using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASPODES.Database;
using ASPODES.DTO.Application;
using ASPODES.Model;

using System.Data.Entity.Migrations;

using System.Net;
using System.Net.Http;
using AutoMapper;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Security;

namespace ASPODES.WebAPI.Repository
{
    /// <summary>
    /// 年度预算条目操作类
    /// </summary>
    public class AnnualBudgetItemRepository
    {
        /// <summary>
        /// 添加一条年度预算条目
        /// </summary>
        /// <param name="annualBudgetItemDTO">年度预算条目信息</param>
        /// <returns>
        /// 添加成功返回ResponseStatus.success,
        /// 否则返回ResponseStatus.unkown_error和错误信息
        /// </returns>
        public GetAnnualBudgetItemDTO AddOrUpdateAnnualBudgetItem(AddAnnualBudgetItemDTO annualBudgetItemDTO, Func<Application, CurrentUser, bool> privilege)
        {

            if (annualBudgetItemDTO.Amount < 0)
                throw new OtherException("额度值不能为负数");
            using (var ctx = new AspodesDB())
            {
                var application = ctx.Applications.FirstOrDefault(a => a.ApplicationId == annualBudgetItemDTO.ApplicationId);
                if (null == application) throw new NotFoundException();

                if (!privilege(application, UserHelper.GetCurrentUser()))
                    throw new UnauthorizationException("您没有权限修改或者超过了申请书提交日期");
                AnnualBudget budget = ctx.AnnualBudgets.FirstOrDefault(ab => ab.ApplicationId == annualBudgetItemDTO.ApplicationId && ab.Year == annualBudgetItemDTO.Year);
                //年度预算是否存在，不存在添加一个
                if (budget == null)
                {
                    budget = new AnnualBudget()
                    {
                        ApplicationId = annualBudgetItemDTO.ApplicationId,
                        Year = annualBudgetItemDTO.Year,
                        Amount = 0
                    };
                    budget = ctx.AnnualBudgets.Add(budget);
                }
                ctx.SaveChanges();

                AnnualBudgetItem item = Mapper.Map<AnnualBudgetItem>(annualBudgetItemDTO);

                //预算条目是否存在，不存在，添加，存在，更新数据
                var oldItem = ctx.AnnualBudgetItems.FirstOrDefault(abi => abi.AnnualBudgetId == budget.AnnualBudgetId && abi.SubjectId == annualBudgetItemDTO.SubjectId);
                if (oldItem == null)
                {
                    item.AnnualBudgetId = budget.AnnualBudgetId;
                    item = ctx.AnnualBudgetItems.Add(item);
                    budget.Amount += item.Amount;
                }
                else
                {
                    budget.Amount -= oldItem.Amount;
                    oldItem.Reason = annualBudgetItemDTO.Reason;
                    oldItem.Amount = annualBudgetItemDTO.Amount;
                    budget.Amount += oldItem.Amount;
                }
                ctx.SaveChanges();
                return Mapper.Map<GetAnnualBudgetItemDTO>(item);
            }

        }

        /// <summary>
        /// 获取一条AnnualBudget的所有预算条目
        /// </summary>
        /// <param name="annualBudgetId">AnnualBudgetId</param>
        /// <returns>
        /// 成功返回ResponseStatus.success和获取到的AnnualBudgetItem列表，
        /// 没有找到数据返回ResponseStatus.parameter_error，
        /// 出错返回ResponseStatus.unknown_error和错误信息
        /// </returns>
        public List<GetAnnualBudgetItemDTO> GetAnnualBudgetItems(int annualBudgetId)
        {
            using (var ctx = new AspodesDB())
            {
                var annualBudgetItems = ctx.AnnualBudgetItems
                                       .Where(abi => abi.AnnualBudgetId == annualBudgetId)
                                       .Select(Mapper.Map<GetAnnualBudgetItemDTO>)
                                       .ToList();
                if (annualBudgetItems == null || annualBudgetItems.Count == 0)
                    throw new NotFoundException();
                return annualBudgetItems;
            }
        }

        /// <summary>
        /// 根据AnnualBudgetItemId删除一条预算条目
        /// </summary>
        /// <param name="itemId">AnnualBudgetItemId</param>
        /// <returns>
        /// 成功返回ResponseStatus.success,
        /// 出错返回ResponseStatus.database_error和错误信息
        /// </returns>
        public void Delete(int itemId, Func<Application, CurrentUser, bool> privilege)
        {
            using (var ctx = new AspodesDB())
            {
                var item = ctx.AnnualBudgetItems.FirstOrDefault(iab => iab.AnnualBudgetItemId == itemId);
                if (null == item) throw new NotFoundException("未找到年度预算条目");

                if (!privilege(item.AnnualBudget.Application, UserHelper.GetCurrentUser()))
                {
                    throw new UnauthorizationException("您没有权限修改或者超过了申请书提交日期");
                }
                item.AnnualBudget.Amount -= item.Amount;
                if (item.AnnualBudget.Amount < 0)
                {
                    item.AnnualBudget.Amount = 0;
                }
                var annualBudget = item.AnnualBudget;
                ctx.AnnualBudgetItems.Remove(item);

                ctx.SaveChanges();

                if (!ctx.AnnualBudgetItems.Any(abi => abi.AnnualBudgetId == annualBudget.AnnualBudgetId))
                {
                    ctx.AnnualBudgets.Remove(annualBudget);
                }
                ctx.SaveChanges();
            }

        }
    }
}