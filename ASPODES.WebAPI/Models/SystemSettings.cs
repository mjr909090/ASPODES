using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.WebAPI.Models
{
    public class SystemSettings
    {
        /// <summary>
        /// 通知每页条数
        /// </summary>
        /// <returns></returns>
        public static int NoticeCount()
        {
            return 20;
        }

        /// <summary>
        /// 单位管理员申请书每页条数
        /// </summary>
        /// <returns></returns>
        public static int instApplicationPageCount()
        {
            return 20;
        }

        /// <summary>
        /// 系统管理员申请书每页条数
        /// </summary>
        /// <returns></returns>
        public static int departApplicationPageCount()
        {
            return 20;
        }

        /// <summary>
        /// 单位管理员人员每页条数
        /// </summary>
        /// <returns></returns>
        public static int instPersonPageCount()
        {
            return 10;
        }

        /// <summary>
        /// 系统管理员人员每页条数
        /// </summary>
        /// <returns></returns>
        public static int departPersonPageCount()
        {
            return 20;
        }
    }
}
