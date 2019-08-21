using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ASPODES.DTO.Inst_Person_User;
using ASPODES.WebAPI.Authorize;
using ASPODES.WebAPI.Repository;
using ASPODES.Model;
using ASPODES.WebAPI.Common;
using System.Text.RegularExpressions;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 人员控制器
    /// </summary>
    [AspodesAuthorize]
    [ASPODES.WebAPI.Filter.ActionTrack]
    public class PersonController : ApiController
    {
        private PersonRepository repository = new PersonRepository();
        
        /// <summary>
        /// 获取人员信息
        /// </summary>
        /// <param name="id">人员ID</param>
        /// <returns></returns>
        public HttpResponseMessage Get(int id)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetOnePerson(id));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// combobox获取指定单位的人员信息
        /// </summary>
        /// <param name="instId"></param>
        /// <returns></returns>
        [Route("api/person/combo/inst/{instId}")]
        public HttpResponseMessage GetInstComboPersons(int instId )
        {
            try
            {
                if (instId <= 0) instId = UserHelper.GetCurrentUser().InstId;
                return ResponseWrapper.SuccessResponse(repository.GetNormalPersonComboList(p => p.InstituteId == instId));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }

        /// <summary>
        /// 获取全部外单位人员combobox
        /// </summary>
        /// <returns></returns>
        [Route("api/person/combo/partnersinst")]
        public HttpResponseMessage GetPartnersInstComboPersons()
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetPartnersPersonComboList());
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }

        /// <summary>
        /// 获取全部外单位人员列表
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "院管理员")]
        [Route("api/person/partnersinst")]
        public HttpResponseMessage GetPartnersInstPersons()
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetPartnersPersonList());
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }

        }

        /// <summary>
        /// 添加外单位人员
        /// </summary>
        /// <param name="personDTO">添加人员信息</param>
        /// <returns></returns>
        [Route("api/person/partners")]
        public HttpResponseMessage Post(AddPersonDTO personDTO)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.AddPerson(personDTO, p => true));
            }
            catch (DbEntityValidationException ex)
            {
                return ResponseWrapper.ExceptionResponse(ex);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }


        /// <summary>
        /// 验证身份证
        /// </summary>
        /// <param name="IDCard">添加人员信息</param>
        /// <returns></returns>
        [HttpPost,Route("api/person/validateidcard/{idcard}")]
        public HttpResponseMessage PostValidateIDCard(string idcard)
        {
            try
            {
                var personList = repository.GetPagingPersonList(p => p.IDCard == idcard, -1);
                if(personList.ItemDTOs.Count() != 0)
                {
                    return ResponseWrapper.SuccessResponse("该身份证已存在，若找不到该人员，请检查该人员状态");
                }

                if ((!Regex.IsMatch(idcard, @"^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$", RegexOptions.IgnoreCase)))
                {
                    return ResponseWrapper.SuccessResponse("身份证格式错误");
                }

                return ResponseWrapper.SuccessResponse("身份证验证通过");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
    }
}
