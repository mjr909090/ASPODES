using ASPODES.DTO;
using ASPODES.Model;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPODES.WebAPI.TypeMapping
{
    /// <summary>
    /// 关于统计模块的对象映射
    /// </summary>
    public class StatisticProfile : Profile
    {
        /// <summary>
        /// 配置映射规则
        /// </summary>
        protected override void Configure()
        {
            CreateMap<StatisticKeyValue<double>, InstAndFundDTO>()
                .ForMember(t => t.InstName, config => config.MapFrom(r => r.Key))
                .ForMember(t => t.Amount, config => config.MapFrom(r => r.Value));
        }
    }
}