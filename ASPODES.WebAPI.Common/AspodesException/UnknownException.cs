using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.WebAPI.Common
{
    /// <summary>
    /// 自定义未知异常
    /// </summary>
    public class UnknownException : AspodesException
    {
        public UnknownException( string message = "服务器未知错误")
            : base(HttpStatusCode.InternalServerError, AspodesExceptionCode.UNKOWN_EXEPTION, message)
        {

        }
    }
}
