using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity.Migrations;

using System.Net;
using System.Net.Http;
using AutoMapper;

using ASPODES.WebAPI.Common;
using ASPODES.Database;
using ASPODES.Model;
using ASPODES.DTO.Application;

namespace ASPODES.WebAPI.Repository
{
    /// <summary>
    /// 年度预算操作类
    /// </summary>
    public class AnnualBudgetRepository
    {
        /// <summary>
        /// 获取申请书的年度预算
        /// </summary>
        /// <param name="applicationId">申请书的ID</param>
        /// <returns>
        /// 成功返回ResponseStatus.success和年度预算列表，年度预算对象内含有预算条目列表
        /// 失败返回ResponseStatus.unkown_error和错误信息
        /// </returns>
        public ICollection<GetAnnualBudgetDTO> GetApplicationAnnualBudgets(string applicationId)
        {
            ICollection<GetAnnualBudgetDTO> annualBudgetDTOs = null;

            using (var ctx = new AspodesDB())
            {
                var annualBudgets = ctx.AnnualBudgets.Where(ab => ab.ApplicationId == applicationId);
                annualBudgetDTOs = new List<GetAnnualBudgetDTO>();
                foreach (var ab in annualBudgets)
                {
                    var annualBudgetDTO = Mapper.Map<GetAnnualBudgetDTO>(ab);
                    annualBudgetDTO.Items.AddRange(ab.Items.Select(Mapper.Map<GetAnnualBudgetItemDTO>));
                    annualBudgetDTOs.Add(annualBudgetDTO);
                }
            }
            return annualBudgetDTOs;
        }

        /// <summary>
        /// 获取一条年度预算信息
        /// </summary>
        /// <param name="applicationId">申请书ID</param>
        /// <param name="year">年度，第几年</param>
        /// <returns>
        /// 成功，返回ResponseStatus.success和一条年度预算信息，年度预算对象内含有预算条目列表
        /// 未找到数据，返回 ResponseStatus.parameter_error
        /// 失败，返回ResponseStatus.unkown_error和错误信息
        /// </returns>
        public GetAnnualBudgetDTO GetOneAnnualBudget(string applicationId, int year)
        {
            GetAnnualBudgetDTO annualBudgetDTO = null;
            using (var ctx = new AspodesDB())
            {
                var annualBudget = ctx.AnnualBudgets.Include("Items")
                    .FirstOrDefault(ab => ab.ApplicationId == applicationId && ab.Year == year);

                //if (null == annualBudget) throw new NotFoundException("未找到预算信息");

                annualBudgetDTO = Mapper.Map<GetAnnualBudgetDTO>(annualBudget);
                //annualBudgetDTO.Items.AddRange(annualBudget.Items.Select(Mapper.Map<GetAnnualBudgetItemDTO>));
            }
            return annualBudgetDTO;
        }
    }
}