using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.WebAPI.Common
{
    /// <summary>
    /// 自定义权限异常
    /// </summary>
    public class UnauthorizationException : AspodesException
    {
        public UnauthorizationException( string message = "未授权访问")
            : base(HttpStatusCode.Forbidden, AspodesExceptionCode.UNAUTHORIZE_EXCEPTION, message)
        {

        }
    }
}
