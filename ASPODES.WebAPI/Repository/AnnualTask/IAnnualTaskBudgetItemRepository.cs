using ASPODES.DTO.AnnualTask;
using ASPODES.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.WebAPI.Repository
{
    public interface IAnnualTaskBudgetItemRepository
    {
        AnnualTaskBudgetItem Get(int id);

        //AnnualTaskBudgetItem AddAnnualTaskBudgetItem(AnnualTaskBudgetItem annualTaskBudgetItem);
        //AnnualTaskBudgetItem UpdateAnnualTaskBudgetItem(AnnualTaskBudgetItem annualTaskBudgetItem);
        //AnnualTaskBudgetItem DeleteAnnualTaskBudgetItem(int id);

        void UpdateAnnualTaskBudgetItem(List<UpdateAnnualTaskBudgetItemDTO> annualTaskBudgetItems);
        IQueryable<AnnualTaskBudgetItem> GetAnnualTaskBudgetItemList();

    }
}
