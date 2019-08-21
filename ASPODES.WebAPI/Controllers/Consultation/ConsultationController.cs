using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ASPODES.WebAPI.Repository;
using ASPODES.WebAPI.Common;
using ASPODES.Model;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 咨询审议控制器
    /// </summary>
    public class ConsultationController : ApiController
    {
        private IConsultationRepository _repository;

        public ConsultationController( IConsultationRepository repository )
        {
            _repository = repository;
        }

        /// <summary>
        /// 获取咨询审议结果
        /// </summary>
        /// <param name="importYear"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetConsultations( [FromUri]int importYear )
        {
            try
            {
                var consultaions = _repository.GetConsultaion( importYear );
                return ResponseWrapper.SuccessResponse(new { 
                         Applications = consultaions.OfType<ApplicationConsultation>().Select( c=>ConsultationRepository.TypeConverter(c)),
                         Projects= consultaions.OfType<ProjectConsultation>().Select( c=>ConsultationRepository.TypeConverter(c))
               });
            }
            catch( Exception e )
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 下载咨询审议模板
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/consultation/template")]
        public HttpResponseMessage DownloadConsultationTemplate()
        {
            return _repository.DownloadConsultationTemplate();
        }

        /// <summary>
        /// 上传咨询审议结果
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("api/consultation/result")]
        public HttpResponseMessage UploadConsultationResult()
        {
            try
            {
                List<Consultation> consultationList = _repository.UploadConsultationResult();
                return ResponseWrapper.SuccessResponse("上传成功！");
            }
            catch( Exception e )
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
    }
}
