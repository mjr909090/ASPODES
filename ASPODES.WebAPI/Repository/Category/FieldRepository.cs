using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Net.Http;

using AutoMapper;
using ASPODES.DTO.Category;
using ASPODES.WebAPI.Common;
using ASPODES.Database;
using ASPODES.Model;
using ASPODES.DTO.Application;
using System.Net;

namespace ASPODES.WebAPI.Repository
{
    /// <summary>
    /// 研究领域操作类
    /// </summary>
    public class FieldRepository
    {
        /// <summary>
        /// 获取研究领域列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GetFieldDTO> GetFields()
        {

            using (var ctx = new AspodesDB())
            {
                return ctx.Fields.Select(Mapper.Map<GetFieldDTO>).ToList();

            }

        }
    }
}