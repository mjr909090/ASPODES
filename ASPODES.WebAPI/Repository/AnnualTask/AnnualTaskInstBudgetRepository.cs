using ASPODES.Database;
using ASPODES.DTO.AnnualTask;
using ASPODES.Model;
using ASPODES.WebAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPODES.WebAPI.Repository
{
    public class AnnualTaskInstBudgetRepository: IAnnualTaskInstBudgetRepository
    {
        private AspodesDB _context;
        /// <summary>
        /// 年度任务书单位预算操作类构造函数
        /// </summary>
        public AnnualTaskInstBudgetRepository(AspodesDB context)
        {
            _context = context;
        }

        public AnnualTaskInstBudget Get( int annualTaskId, int instId )
        {
            return _context.AnnualTaskInstBudgets.FirstOrDefault(atib => atib.AnnualTaskId == annualTaskId && atib.InstituteId == instId);
        }

        /// <summary>
        /// 查询年度任务书单位预算
        /// </summary>
        public IQueryable<AnnualTaskInstBudget> GetAnnualTaskInstBudgetList()
        {
            var getannualTaskInstBudget = _context.AnnualTaskInstBudgets.Include("Institute");
            return getannualTaskInstBudget;
        }

        /// <summary>
        /// 修改年度任务书单位预算
        /// </summary>
        /// <param name="annualTaskInstBudget">年度任务书单位预算实体</param>
        public void UpdateAnnualTaskInstBudget(List<UpdateAnnualTaskInstBudgetDTO> updates)
        {
            foreach( var item in updates )
            {
                var updateannualTaskInstBudget = _context.AnnualTaskInstBudgets
                .Include("AnnualTask")
                .FirstOrDefault( a => a.AnnualTaskId == item.AnnualTaskId && a.InstituteId == item.InstituteId);

                if (updateannualTaskInstBudget == null) throw new NotFoundException("未找到年度任务");
                if (!updateannualTaskInstBudget.AnnualTask.Editable()) throw new OtherException("状态不允许修改");

                updateannualTaskInstBudget.Amount = item.Amount;
            }
            
            _context.SaveChanges();
        }

        ///// <summary>
        ///// 修改年度任务书单位预算
        ///// </summary>
        ///// <param name="annualTaskInstBudget">年度任务书单位预算实体</param>
        //public void UpdateAnnualTaskInstBudget(AnnualTaskInstBudget annualTaskInstBudget)
        //{
        //    var updateannualTaskInstBudget = _context.AnnualTaskInstBudgets
        //        .Include("AnnualTask")
        //        .FirstOrDefault(
        //        a => a.AnnualTaskId == annualTaskInstBudget.AnnualTaskId
        //        && a.InstituteId == annualTaskInstBudget.InstituteId );

        //    if (updateannualTaskInstBudget == null) throw new NotFoundException("未找到年度任务");
        //    if (!updateannualTaskInstBudget.AnnualTask.Editable()) throw new OtherException("状态不允许修改");

        //    updateannualTaskInstBudget.Amount = annualTaskInstBudget.Amount;
        //    _context.SaveChanges();
        //    return updateannualTaskInstBudget;
        //}
    }
}