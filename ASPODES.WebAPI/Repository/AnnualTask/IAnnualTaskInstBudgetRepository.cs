using ASPODES.DTO.AnnualTask;
using ASPODES.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.WebAPI.Repository
{
    public interface IAnnualTaskInstBudgetRepository
    {
        AnnualTaskInstBudget Get(int annualTaskId, int instId);
        //AnnualTaskInstBudget AddAnnualTaskInstBudget(AnnualTaskInstBudget annualTaskInstBudget);
        void UpdateAnnualTaskInstBudget(List<UpdateAnnualTaskInstBudgetDTO> updates );
        //AnnualTaskInstBudget DeleteAnnualTaskInstBudget(int annualTaskId, int instituteId);
        IQueryable<AnnualTaskInstBudget> GetAnnualTaskInstBudgetList();

    }
}
