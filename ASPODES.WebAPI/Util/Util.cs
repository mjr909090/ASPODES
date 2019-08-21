using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;
using System.Web;
using System.Net;
using System.Web.Http;
using System.Security.Principal;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using ASPODES.WebAPI.Security;
using System.Security.Cryptography;
using ASPODES.DTO;
using System.Web.Security;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using System.Text.RegularExpressions;

namespace ASPODES.WebAPI
{
    public class UserHelper
    {
        public static bool UnnormalUser(User user)
        {
            return user == null || user.Person.Status != "C";
        }

        public static bool NormalUser(User user)
        {
            return user != null && user.Person.Status == "C";
        }

        public static string SerializeUserInfo(CurrentUser info)
        {
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, info);
            stream.Position = 0;
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            stream.Flush();
            stream.Close();
            return Convert.ToBase64String(buffer);
        }

        public static CurrentUser DeserializeUserInfo(string text)
        {
            byte[] buffer = Convert.FromBase64String(text);
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(buffer);
            return (CurrentUser)formatter.Deserialize(stream);
        }

        public static CurrentUser GetCurrentUser()
        {
            try
            {
                UserPrincipal principal = (UserPrincipal)HttpContext.Current.User;
                return principal.UserInfo;
            }
            catch (Exception e)
            {
                throw new UnauthorizedAccessException("未登录或者登录信息缺失");
            }
            
        }

        public static string GetReviewPassword()
        {
            var userInfo = GetCurrentUser();
            StringBuilder password = new StringBuilder();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            
            var byteBuffer = Encoding.Default.GetBytes( userInfo.UserId + SystemConfig.ApplicationStartYear + userInfo.PersonId + "" );
            var encryByte = md5.ComputeHash(byteBuffer);
            var encryStr = BitConverter.ToString(encryByte);
            var value = FormsAuthentication.HashPasswordForStoringInConfigFile(encryStr, "MD5");
            return value.Substring(16);
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="DirectoryToZip">需要压缩的文件路径的数组</param>
        /// <param name="ZipedPath">压缩后的文件路径</param>
        /// <param name="ZipedFileName">压缩后的名称</param>
        /// <param name="isPathMap">传入的地址数组是否需要转换</param>
        /// <param name="IsEncrypt">是否加密（默认不加密）</param>
        /// <param name="password">加密的密码（默认密码123456）</param>
        public static void ZipFiles(string []DirectoryToZip, string ZipedPath, string ZipedFileName, bool isPathMap=true,bool IsEncrypt = false, string password = "123456")
        {
            ZipedPath = System.Web.Hosting.HostingEnvironment.MapPath(ZipedPath);
            //如果目录不存在就创建
            if (!Directory.Exists(ZipedPath)) Directory.CreateDirectory(ZipedPath);

            string ZipFileName = ZipedPath + ZipedFileName;
            
            using (FileStream ZipFile = File.Create(ZipFileName))
            {
                using (ZipOutputStream s = new ZipOutputStream(ZipFile))
                {
                    if(IsEncrypt)
                    {
                        s.Password = password;
                    }
                    Crc32 crc = new Crc32();
                    foreach(string file in DirectoryToZip)
                    {
                        string fileAddress = "";
                        if (isPathMap)
                        {
                            fileAddress = System.Web.Hosting.HostingEnvironment.MapPath(file);
                        }
                        using (FileStream fs = File.OpenRead(fileAddress))
                        {
                            byte[] buffer = new byte[fs.Length];
                            fs.Read(buffer, 0, buffer.Length);
                            string[] pathCells = fileAddress.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                            ZipEntry entry = new ZipEntry(pathCells[pathCells.Length-1]);
                            entry.DateTime = DateTime.Now;
                            entry.Size = fs.Length;
                            fs.Close();
                            crc.Reset();
                            crc.Update(buffer);
                            entry.Crc = crc.Value;
                            s.PutNextEntry(entry);
                            s.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 申请书状态的映射
        /// </summary>
        public static Dictionary<int, string> ApplicationStatusTransform = new Dictionary<int, string> {
            {(int)ApplicationStatus.NEW_ONE,"新建填写第一部分"},
            {(int)ApplicationStatus.NEW_TWO,"新建填写第二部分"},
            {(int)ApplicationStatus.NEW_THREE,"新建填写第三部分"},
            {(int)ApplicationStatus.NEW_FOUR,"新建填写第四部分"},
            {(int)ApplicationStatus.NEW,"新建"},
            {(int)ApplicationStatus.CHECK,"等待单位审核"},
            {(int)ApplicationStatus.REJECT,"单位管理员驳回"},
            {(int)ApplicationStatus.CANCEL,"单位管理员撤回"},
            {(int)ApplicationStatus.ACCEPT,"等待院管理员受理"},
            {(int)ApplicationStatus.REFUSE,"院不受理"},
            {(int)ApplicationStatus.ASSIGNMENT,"等待指派专家"},
            {(int)ApplicationStatus.MANUAL_ASSIGNMENT,"等待手动指派"},
            {(int)ApplicationStatus.SEND_ASSIGNMENT,"等待确认指派信息"},
            {(int)ApplicationStatus.REVIEW,"评审中"},
            {(int)ApplicationStatus.FINISH_REVIEW,"评审完毕"},
            {(int)ApplicationStatus.UNSUPPORT,"不资助"},
            {(int)ApplicationStatus.STORAGE,"在库"},
            {(int)ApplicationStatus.SUPPORT,"已出库资助"},
            {(int)ApplicationStatus.OVERDUE,"过期失效"}
        };

        /// <summary>
        /// 申请书大类的映射
        /// </summary>
        public static Dictionary<int, string> ProjectTypqTransform = new Dictionary<int, string> {
            {1, "联盟重点工作"},
            {2, "应急性科研工作"},
            {3, "科技基础性工作"},
            {4, "基础研究引导计划"},
            {5, "重大成果培育计划"},
            {6, "重大平台推进计划"},
            {7, "重大项目储备计划"},
            {8, "农业智库建设计划"}
        };

        /// <summary>
        /// 导出申请书综合查询时的表头
        /// </summary>
        public static Dictionary<int, string> ApplicationExportTableHeader = new Dictionary<int, string>
        {
            {1, "申请书ID"},
            {2, "任务名称"},
            {3, "执行期限"},
            {4, "申请书大类"},
            {5, "资助类别"},
            {6, "承担单位"},
            {7, "项目负责人"},
            {8, "负责人电话"},
            {9, "负责人邮箱"},
            {10, "联系人电话"},
            {11, "联系人邮箱"},
            {12, "总预算额"},
            {13, "当年预算额"},
            {14, "首次申报年份"},
            {15, "本次申报年份"},
            {16, "摘要"},
            {17, "委托类型"},
            {18, "申请书状态"}
        };

    }
}
