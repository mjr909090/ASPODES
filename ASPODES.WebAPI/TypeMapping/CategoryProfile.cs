using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using ASPODES.DTO.Category;
using ASPODES.Model;

namespace ASPODES.WebAPI
{
    public class CategoryProfile:Profile
    {
        protected override void Configure()
        {
            CreateMap<AccountingSubject, GetAccountingSubjectDTO>();
            CreateMap<AddAccountingSubjectDTO, AccountingSubject>();

 

            CreateMap<SubField, GetSubFieldDTO>()
                .ForMember(DTO => DTO.SubFieldId, config => config.MapFrom(sf => sf.SubFieldName));

            CreateMap<AddSubFieldDTO, SubField>()
                .ForMember(sf => sf.SubFieldName, config => config.MapFrom(DTO => DTO.SubFieldId));

            CreateMap<Field, GetFieldDTO>()
                .ForMember( DTO=>DTO.FieldId, config=>config.MapFrom(f=>f.FieldName));

            //ProjectType
            CreateMap<ProjectType, GetProjectTypeDTO>();

            //SupportCategory
            CreateMap<SupportCategory, GetSupportCategoryDTO>();
        }
    }
}