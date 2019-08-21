using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPODES.WebAPI.Security
{
    [Serializable]
    public class CurrentUser
    {
        /// <summary>
        /// 当前用户姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 当前用户UserId
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 当前用户PersionId
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// 用户所在单位Id
        /// </summary>
        public int InstId { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public string[] Roles { get; set; }

        /// <summary>
        /// 项目类型Id
        /// </summary>
        public int?[] ProjectTypeIds { get; set; }

        /// <summary>
        /// 还不知道有什么用
        /// </summary>
        public int[] InstIds { get; set; }
    }
}