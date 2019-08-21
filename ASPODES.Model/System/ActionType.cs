using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.Model
{
    /// <summary>
    /// 用户操作操作类型
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// 登录
        /// </summary>
        LOGIN,
        /// <summary>
        /// 注销
        /// </summary>
        LOGOUT,//1
        /// <summary>
        /// 提交信息
        /// </summary>
        SUBMIT_MESSAGE,
        /// <summary>
        /// 提交审核
        /// </summary>
        SUBMIT_EVALUATION,
        /// <summary>
        /// 修改记录
        /// </summary>
        UPDATE_RECORD,
        /// <summary>
        /// 删除记录
        /// </summary>
        DELETE_RECORD,//5
        /// <summary>
        /// 提交全文
        /// </summary>
        SUBMIT_ALL,
        /// <summary>
        /// 删除全文
        /// </summary>
        DELETE_ALL,
        /// <summary>
        /// 单位管理员驳回申请书
        /// </summary>
        REJECT_APPLICATION,
        /// <summary>
        /// 院管理员不受理申请书
        /// </summary>
        REFUSE_APPLICATION
    }
}
