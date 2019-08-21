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
    /// 自定义异常
    /// </summary>
    public class AspodesException:Exception
    {
        private HttpStatusCode _httpCode;

        private AspodesExceptionCode _exceptionCode;

        public AspodesException( HttpStatusCode httpCode, AspodesExceptionCode exceptionCode, string message)
            : base( message )
        {
            _httpCode = httpCode;
            _exceptionCode = exceptionCode;
        }

        /// <summary>
        /// 获取http状态码
        /// </summary>
        /// <returns></returns>
        public HttpStatusCode GetHttpCode()
        {
            return _httpCode;
        }

        /// <summary>
        /// 获取自定义异常码
        /// </summary>
        /// <returns></returns>
        public AspodesExceptionCode GetExceptionCode()
        {
            return _exceptionCode;
        }

        /// <summary>
        /// 获取转为JSON格式的数据
        /// </summary>
        /// <returns>string</returns>
        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(new { status=_exceptionCode, errorMsg=Message});
        }
    }
}
