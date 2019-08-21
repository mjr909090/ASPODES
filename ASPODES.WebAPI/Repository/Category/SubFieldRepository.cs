using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Net.Http;

using AutoMapper;

using ASPODES.WebAPI.Common;
using ASPODES.Database;
using ASPODES.Model;
using ASPODES.DTO.Application;
using ASPODES.DTO.Category;
using System.Net;

namespace ASPODES.WebAPI.Repository
{
    /// <summary>
    /// 研究子领域操作类
    /// </summary>
    public class SubFieldRepository
    {
        /*
        /// <summary>
        /// 获取研究子领域列表
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetSubFields()
        {
            IEnumerable<GetSubFieldDTO> subFields = null;
            try
            {
                using (var ctx = new AspodesDB())
                {
                    subFields = ctx.SubFields
                        .Select( Mapper.Map<GetSubFieldDTO> )
                        .ToList();
                    if (subFields == null)
                        return Response.CreateRecordNotFoundResponse();
                }
            }
            catch (Exception e)
            {
                return Response.CreateUnkownErrorReponse(e.Message);
            }
            return Response.CreateSuccessResponse( subFields);
        }
        /// <summary>
        /// 获取研究领域的子领域
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public HttpResponseMessage GetSubFields(string fieldName)
        {
            IEnumerable<GetSubFieldDTO> subFields = null;
            try
            {
                using (var ctx = new AspodesDB())
                {
                    subFields = ctx.SubFields
                        .Where(sf => sf.ParentName == fieldName)
                        .Select( Mapper.Map<GetSubFieldDTO> )
                        .ToList();
                    if (subFields == null )
                        return Response.CreateRecordNotFoundResponse();
                }
            }
            catch (Exception e)
            {
                return Response.CreateUnkownErrorReponse(e.Message);
            }
            return Response.CreateSuccessResponse( subFields);
        }
        */

        public IEnumerable<GetSubFieldDTO> GetSubFields(Func<SubField, bool> predicate)
        {
            using (var ctx = new AspodesDB())
            {
                return ctx.SubFields
                    .Where(predicate)
                    .Select(Mapper.Map<GetSubFieldDTO>)
                    .ToList();
            }
        }

    }
}