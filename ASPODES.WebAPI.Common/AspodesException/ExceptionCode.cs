using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.WebAPI.Common
{
    /// <summary>
    /// 异常码集合
    /// </summary>
    public enum AspodesExceptionCode
    {
        // token异常
        TOKEN_EXCEPTION = 100,

        // 模型校验异常
        MODEL_VALID_EXCEPTION = 200,

        // 其他异常
        OTHER_EXCEPTION = 300,

        // 权限异常
        UNAUTHORIZE_EXCEPTION = 400,

        // 没有找到相关资源的异常
        NOTFOUN_EXCEPTION = 500,

        // 未知异常
        UNKOWN_EXEPTION = 999
    }
}
