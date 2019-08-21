using ASPODES.Database;
using ASPODES.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASPODES.WebAPI.Common;
using ASPODES.DTO.AnnualTask;

namespace ASPODES.WebAPI.Repository
{
    public class AnnualTaskBudgetItemRepository: IAnnualTaskBudgetItemRepository
    {
        private AspodesDB _context;
        /// <summary>
        /// 出库项目年度预算操作类构造函数
        /// </summary>
        public AnnualTaskBudgetItemRepository(AspodesDB context)
        {
            _context = context;
        }

        public AnnualTaskBudgetItem Get( int id )
        {
            return _context.AnnualTaskBudgetItems.FirstOrDefault(atbi => atbi.AnnualTaskBudgetItemId == id);
        }

        ///// <summary>
        ///// 添加出库项目年度预算
        ///// </summary>
        ///// <param name="annualTaskBudgetItem">出库项目年度预算实体</param>
        //public AnnualTaskBudgetItem AddAnnualTaskBudgetItem(AnnualTaskBudgetItem annualTaskBudgetItem)
        //{
        //    var task = _context.AnnualTasks.FirstOrDefault(atb => atb.AnnualTaskId == annualTaskBudgetItem.AnnualTaskId);

        //    if (task == null) throw new NotFoundException("未找到年度任务");
        //    if (!task.Editable()) throw new OtherException("状态不允许修改");

        //    var addannualTaskBudgetItem = _context.AnnualTaskBudgetItems.Add(annualTaskBudgetItem);
        //    _context.SaveChanges();

        //    return addannualTaskBudgetItem;
        //}

        ///// <summary>
        ///// 修改出库项目年度预算
        ///// </summary>
        ///// <param name="annualTaskBudgetItem">出库项目年度预算实体</param>
        //public AnnualTaskBudgetItem UpdateAnnualTaskBudgetItem(AnnualTaskBudgetItem annualTaskBudgetItem)
        //{
        //    var updateannualTaskBudgetItem = _context.AnnualTaskBudgetItems
        //        .Include("AnnualTask")
        //        .FirstOrDefault(a => a.AnnualTaskBudgetItemId == annualTaskBudgetItem.AnnualTaskBudgetItemId);
        //    if (null == updateannualTaskBudgetItem) throw new NotFoundException("未找到要修改的科目预算");
        //    if (!updateannualTaskBudgetItem.AnnualTask.Editable()) throw new OtherException("状态不允许修改");

        //    //更新时不允许更改科目
        //    updateannualTaskBudgetItem.Reason = annualTaskBudgetItem.Reason;
        //    updateannualTaskBudgetItem.Amount = annualTaskBudgetItem.Amount;

        //    _context.SaveChanges();
        //    return updateannualTaskBudgetItem;
        //}

        /// <summary>
        /// 修改出库项目年度预算
        /// </summary>
        /// <param name="annualTaskBudgetItem">出库项目年度预算实体</param>
        public void UpdateAnnualTaskBudgetItem(List<UpdateAnnualTaskBudgetItemDTO>  updates)
        {
            foreach (var item in updates)
            {
                var update = _context.AnnualTaskBudgetItems.FirstOrDefault(atbi => atbi.AnnualTaskBudgetItemId == item.AnnualTaskBudgetItemId);
                if (null == update) throw new NotFoundException("没有找到该预算科目");
                if (!update.AnnualTask.Editable()) throw new OtherException("该状态的任务书不允许修改预算");
                update.Reason = item.Reason;
                update.Amount = item.Amount;
            }
            _context.SaveChanges();
        }

        ///// <summary>
        ///// 删除出库项目年度预算
        ///// </summary>
        ///// <param name="id">出库项目年度预算id</param>
        //public AnnualTaskBudgetItem DeleteAnnualTaskBudgetItem(int id)
        //{
        //    var removeannualTaskBudgetItem = _context.AnnualTaskBudgetItems.FirstOrDefault(a => a.AnnualTaskBudgetItemId == id);
        //    if (removeannualTaskBudgetItem == null) throw new NotFoundException("未找到要删除的科目预算");
        //    if (!removeannualTaskBudgetItem.AnnualTask.Editable()) throw new OtherException("状态不允许修改");

        //    _context.AnnualTaskBudgetItems.Remove(removeannualTaskBudgetItem);

        //    _context.SaveChanges();
        //    return removeannualTaskBudgetItem;
        //}

        /// <summary>
        /// 查询出库项目年度预算
        /// </summary>
        public IQueryable<AnnualTaskBudgetItem> GetAnnualTaskBudgetItemList()
        {
            var getannualTaskBudgetItem = _context.AnnualTaskBudgetItems;
            return getannualTaskBudgetItem;
        }

    }
}