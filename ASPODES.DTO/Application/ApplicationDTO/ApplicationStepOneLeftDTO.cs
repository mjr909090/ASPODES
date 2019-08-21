using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;
using ASPODES.DTO.Category;

namespace ASPODES.DTO.Application
{
    public class ApplicationStepOneLeftDTO
    {
        public ApplicationStepOneLeftDTO()
        {
            ApplicationFields = new List<ApplicationFieldVO>();
        }

        public string ApplicationId { get; set; }
        public string ProjectName { get; set; }
        public string AbstractContent { get; set; }
        public int? Period { get; set; }

        public int? ProjectTypeId { get; set; }
        public string ProjectTypeName { get; set; }

        public int? SupportCategoryId { get; set; }
        public string SupportCategoryName { get; set; }

        public double? TotalBudget { get; set; }
        public double? FirstYearBudget { get; set; }
        /// <summary>
        /// 委托类型
        /// </summary>
        public DelegateType DeleageType { get; set; }

        public ICollection<Field> Fields { get; set; }
        public ICollection<GetProjectTypeDTO> ProjectTypes { get; set; }
        public ICollection<GetSupportCategoryDTO> SupportCategorys { get; set; }

        public ICollection<ApplicationFieldVO> ApplicationFields { get; set; }

    }

    public class ApplicationFieldVO
    {
        public int? ApplicationFieldId { get; set; }
        public string FieldId { get; set; }
        public string SubFieldId { get; set; }
        public string KeyWordsCN { get; set; }
        public string KeyWordsEN { get; set; }

        public ICollection<GetSubFieldDTO> SubFields { get; set; }
    }
}
