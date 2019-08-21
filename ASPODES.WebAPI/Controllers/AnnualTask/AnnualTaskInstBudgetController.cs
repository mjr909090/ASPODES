using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using ASPODES.DTO.AnnualTask;
using ASPODES.WebAPI.Repository;
using ASPODES.WebAPI.Common;
using ASPODES.Model;
using Newtonsoft.Json;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 年度任务单位预算控制器
    /// </summary>
    public class AnnualTaskInstBudgetController : ApiController
    {
        private IAnnualTaskInstBudgetRepository _repository;

        public AnnualTaskInstBudgetController( IAnnualTaskInstBudgetRepository repository )
        {
            _repository = repository;
        }
        /// <summary>
        /// 获得年度任务单位预算
        /// </summary>
        /// <param name="annualTaskId">年度任务Id</param>
        /// <returns></returns>
        [Route("api/annualTaskInstBudget/{annualTaskId}")]
        public HttpResponseMessage GetAnnualTaskInstBudgets(int annualTaskId)
        {
            try
            {
                var budgets = _repository.GetAnnualTaskInstBudgetList()
                    .Where(atib => atib.AnnualTaskId == annualTaskId)
                    .Select( Mapper.Map<GetAnnualTaskInstBudgetDTO> )
                    .ToList();
                return ResponseWrapper.SuccessResponse(budgets);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }


        //public HttpResponseMessage Post( AddAnnualTaskInstBudgetDTO itemDTO )
        //{
        //    try
        //    {
        //        var entity = Mapper.Map<AnnualTaskInstBudget>(itemDTO);
        //        return ResponseWrapper.SuccessResponse(_repository.AddAnnualTaskInstBudget(entity));
        //    }
        //    catch (Exception e)
        //    {
        //        return ResponseWrapper.ExceptionResponse(e);
        //    }
        //}

        /// <summary>
        /// 修改年度任务单位预算
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Put()
        {
            try
            {
                string content = Request.Content.ReadAsStringAsync().Result;
                var updates = JsonConvert.DeserializeObject<List<UpdateAnnualTaskInstBudgetDTO>>(content, new JsonSerializerSettings());
                _repository.UpdateAnnualTaskInstBudget(updates);
                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        //public HttpResponseMessage Delete(int annualTaskId, int instId)
        //{
        //    try
        //    {
        //        _repository.DeleteAnnualTaskInstBudget( annualTaskId, instId );
        //        return ResponseWrapper.SuccessResponse("删除成功");
        //    }
        //    catch (Exception e)
        //    {
        //        return ResponseWrapper.ExceptionResponse(e);
        //    }
        //}
    }
}
