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
    /// 资助类别操作类
    /// </summary>
    public class SupportCategoryRepository
    {

        public List<GetSupportCategoryDTO> GetSupportCategorys(Func<SupportCategory, bool> predicate)
        {
            using (var ctx = new AspodesDB())
            {
                return ctx.SupportCategorys
                   .Where(predicate)
                   .Select(Mapper.Map<GetSupportCategoryDTO>)
                   .ToList();
            }
        }
    }
}