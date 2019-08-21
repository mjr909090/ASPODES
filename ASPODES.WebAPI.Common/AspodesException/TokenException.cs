using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.WebAPI.Common
{
    /// <summary>
    /// 自定义token异常
    /// </summary>
    public class TokenException:AspodesException
    {
        public TokenException( string message ="Token值错误")
            : base(HttpStatusCode.Unauthorized, AspodesExceptionCode.TOKEN_EXCEPTION, message)
        {

        }
    }
}
