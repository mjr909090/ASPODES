using ASPODES.WebAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPODES.WebAPI.Service
{
    /// <summary>
    /// 申请书服务
    /// </summary>
    public class ApplicationService
    {
        private ApplicationRepository _applicationRepository;
        private ExportExcelRepository _exportExcelRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="applicationRepository"></param>
        /// <param name="exportExcelRepository"></param>
        public ApplicationService(ApplicationRepository applicationRepository, ExportExcelRepository exportExcelRepository)
        {
            this._applicationRepository = applicationRepository;
            this._exportExcelRepository = exportExcelRepository;
        }

        /// <summary>
        /// 是否存在申请书所在领域的未指派专家的申请书
        /// </summary>
        /// <returns></returns>
        public bool ExistUnAssignmentApplication(string applicationId)
        {
            return _applicationRepository.ExistUnAssignmentApplication(applicationId);
        }


    }
}