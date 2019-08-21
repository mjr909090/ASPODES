using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ASPODES.WebAPI.Common
{
    /// <summary>
    /// 对返回信息的封装
    /// </summary>
    public class ResponseWrapper
    {
        /// <summary>
        /// 封装返回的异常信息
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static HttpResponseMessage ExceptionResponse( Exception e )
        {
            HttpResponseMessage response = new HttpResponseMessage();
            if (e is AspodesException)
            {
                AspodesException aspodes = (AspodesException)e;
                return new HttpResponseMessage 
                { 
                    StatusCode = aspodes.GetHttpCode(),
                    Content = new StringContent(aspodes.ToJsonString())
                };
            }
            return new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                Content = new StringContent( e.Message )
            };
        }

        /// <summary>
        /// 封装返回的警告信息
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static HttpResponseMessage WarningResponse(Exception e)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            if (e is AspodesException)
            {
                AspodesException aspodes = (AspodesException)e;
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.PreconditionFailed,
                    Content = new StringContent(aspodes.ToJsonString())
                };
            }
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.PreconditionFailed,
                Content = new StringContent(e.Message)
            };
        }

        /// <summary>
        /// 封装返回的正确信息
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static HttpResponseMessage SuccessResponse( object response = null)
        {
            return new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new { status=0,response=response} ) )
            };
        }
    }
}
