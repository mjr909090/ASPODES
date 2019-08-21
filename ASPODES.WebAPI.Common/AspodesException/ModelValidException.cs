using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.WebAPI.Common
{
    /// <summary>
    /// 自定义模型校验异常
    /// </summary>
    public class ModelValidException:AspodesException
    {
        public ModelValidException(string message = "属性值验证错误")
            : base(HttpStatusCode.BadRequest, AspodesExceptionCode.MODEL_VALID_EXCEPTION, message)
        {

        }
    }
}
