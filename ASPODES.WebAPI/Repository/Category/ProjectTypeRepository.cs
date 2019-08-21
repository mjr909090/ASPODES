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
using System.Net;
using ASPODES.DTO.Category;

namespace ASPODES.WebAPI.Repository
{
    /// <summary>
    /// 项目类型操作类
    /// </summary>
    public class ProjectTypeRepository
    {
        /// <summary>
        /// 获取项目类型列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GetProjectTypeDTO> GetProjectTypes(Func<ProjectType, bool> predicate)
        {
            using (var ctx = new AspodesDB())
            {
                return ctx.ProjectTypes.Where(predicate)
                    .Select(Mapper.Map<GetProjectTypeDTO>)
                    .ToList();
            }
        }

    }
}