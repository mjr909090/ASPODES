using ASPODES.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.DTO.AnnualTask;

namespace ASPODES.WebAPI.Repository
{
    public interface IAnnualTaskRepository
    {
        AnnualTask Get(int? annualTaskId);
        AnnualTask AddAnnualTask(AnnualTask annualTask);

        //AnnualTask UpdateAnnualTask(AnnualTask annualTask);

        //AnnualTask DeleteAnnualTask(int id);

        IQueryable<AnnualTask> GetAnnualTaskList();

        IQueryable<AnnualTask> GetAnnualTaskList(int leaderId);

        IQueryable<AnnualTask> GetAnnualTaskList(int instId, AnnualTaskStatus status, int year);

        IQueryable<AnnualTask> GetAnnualTaskDetailList();

        AnnualTask ChangeAnnualTaskStatus(AnnualTask task, AnnualTaskStatus from, AnnualTaskStatus to, string commnet = "");

        AnnualTask SubmitAnnualTask(int id);

        void CreatePDF(int id);
    }
}
