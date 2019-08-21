using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.WebAPI.Common
{
    /// <summary>
    /// 自定义其他异常
    /// </summary>
    public class OtherException:AspodesException
    {
        public OtherException(string message = "其他错误")
            : base(HttpStatusCode.BadRequest, AspodesExceptionCode.OTHER_EXCEPTION, message)
        {

        }
    }
}
