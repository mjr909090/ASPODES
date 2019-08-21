using System;
using System.Collections.Generic;
using System.Linq;
//using System.Net;
using System.Net.Http;
using System.Web.Http;
using ASPODES.Model;
using ASPODES.WebAPI.Repository;
using ASPODES.WebAPI.Common;
using AutoMapper;
using ASPODES.DTO.AnnualTask;
using Newtonsoft.Json;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 年度任务的科目预算控制器
    /// </summary>
    public class AnnualTaskBudgetItemController : ApiController
    {
        private IAnnualTaskBudgetItemRepository _reposittory;
        public AnnualTaskBudgetItemController( IAnnualTaskBudgetItemRepository repository )
        {
            _reposittory = repository;
        }
        /// <summary>
        /// 更新一条预算科目
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Put()
        {
            try
            {
                string content = Request.Content.ReadAsStringAsync().Result;
                List<UpdateAnnualTaskBudgetItemDTO> updates = 
                    JsonConvert.DeserializeObject<List<UpdateAnnualTaskBudgetItemDTO>>( content, new JsonSerializerSettings());
                if( updates.Count() > 0 )
                {
                    _reposittory.UpdateAnnualTaskBudgetItem(updates);
                }
                return ResponseWrapper.SuccessResponse();
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
        /// <summary>
        /// 获取年度任务的科目预算列表
        /// </summary>
        /// <param name="annualTaskId">年度任务Id</param>
        /// <returns></returns>
        [HttpGet, Route("api/annualTaskBudgetItem/{annualTaskId}")]
        public HttpResponseMessage GetAnnualTaskBudgetItem(int annualTaskId)
        {
            try
            {
                var taskDTOs = _reposittory.GetAnnualTaskBudgetItemList()
                    .Where(ati => ati.AnnualTaskId == annualTaskId)
                    .Select(Mapper.Map<GetAnnualTaskBudgetItemDTO>)
                    .ToList();
                return ResponseWrapper.SuccessResponse(taskDTOs);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        //public HttpResponseMessage Post( AddAnnualTaskBudgetItemDTO addItemDTO )
        //{
        //    try
        //    {
        //        var entity = Mapper.Map<AnnualTaskBudgetItem>( addItemDTO );
        //        //if (entity.Amount < 0) return ResponseWrapper.ExceptionResponse(new OtherException("预算参数不能为负数"));
        //        var invalid = _reposittory.GetAnnualTaskBudgetItemList()
        //            .FirstOrDefault( ati=>ati.AnnualTaskId == addItemDTO.AnnualTaskId && ati.SubjectId == addItemDTO.SubjectId );
        //        if( invalid != null )
        //        {
        //            entity.AnnualTaskBudgetItemId = invalid.AnnualTaskBudgetItemId;
        //            return ResponseWrapper.SuccessResponse(_reposittory.UpdateAnnualTaskBudgetItem( entity ));
        //        }
        //        return ResponseWrapper.SuccessResponse(_reposittory.AddAnnualTaskBudgetItem(entity));
        //    }
        //    catch( Exception e)
        //    {
        //        return ResponseWrapper.ExceptionResponse(e);
        //    }
        //}

        //public HttpResponseMessage Put( UpdateAnnualTaskBudgetItemDTO updateItemDTO )
        //{
        //    try
        //    {
        //        var entity = Mapper.Map<AnnualTaskBudgetItem>(updateItemDTO);
        //        if (entity.Amount < 0) return ResponseWrapper.ExceptionResponse(new OtherException("预算参数不能为负数"));
        //        var invalid = _reposittory.GetAnnualTaskBudgetItemList()
        //            .FirstOrDefault(ati => ati.AnnualTaskBudgetItemId == updateItemDTO.AnnualTaskBudgetItemId );
        //        if (invalid == null) return ResponseWrapper.ExceptionResponse(new NotFoundException());

        //        entity.SubjectId = invalid.SubjectId;
        //        entity.AnnualTaskId = invalid.AnnualTaskId;

        //        return ResponseWrapper.SuccessResponse(_reposittory.UpdateAnnualTaskBudgetItem(entity));
        //    }
        //    catch( Exception e )
        //    {
        //        return ResponseWrapper.ExceptionResponse(e);
        //    }
        //}

        //[HttpDelete, Route("api/annualTaskBudgetItem/{id}")]
        //public HttpResponseMessage Delete( int id)
        //{
        //    try
        //    {
        //        return ResponseWrapper.SuccessResponse(_reposittory.DeleteAnnualTaskBudgetItem(id));
        //    }
        //    catch (Exception e)
        //    {
        //        return ResponseWrapper.ExceptionResponse(e);
        //    }
        //}
        
    }
}
