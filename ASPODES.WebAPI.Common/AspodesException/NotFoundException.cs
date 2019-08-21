using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.WebAPI.Common
{
    /// <summary>
    /// 自定义未找到访问资源异常
    /// </summary>
    public class NotFoundException : AspodesException
    {
        public NotFoundException(string message = "未找到访问资源")
            : base(HttpStatusCode.Gone, AspodesExceptionCode.NOTFOUN_EXCEPTION, message)
        {

        }
    }
}
