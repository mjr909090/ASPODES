using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Web;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;

namespace ASPODES.Common
{
    /// <summary>
    /// 文件助手
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="path"></param>
        /// <param name="rename"></param>
        /// <returns></returns>
        public static string Upload(HttpContext httpContext, string path, string rename = null )
        {
            //只能上传一个文件
            if (httpContext.Request.Files.Count <= 0)
                throw new Exception("未发现上传文件");
            HttpPostedFile file = httpContext.Request.Files.Get(0);

            string filename = string.Format("{0}_{1}", DateTime.Now.ToFileTime(), rename == null ? file.FileName: rename );
            string fullName = Path.Combine(path.Replace("/", @"\"), filename);
            file.SaveAs(fullName);
            return filename;
        }

        /// <summary>
        /// 上传评分说明的文件
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="path"></param>
        /// <param name="rename"></param>
        /// <returns></returns>
        public static string UploadReview(HttpContext httpContext, string path)
        {
            //先删除原来文件夹中的文件
            foreach (string d in Directory.GetFileSystemEntries(path))
            {
                if (File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d);//直接删除其中的文件  
                }
            }
            //只能上传一个文件
            if (httpContext.Request.Files.Count <= 0)
                throw new Exception("未发现上传文件");
            HttpPostedFile file = httpContext.Request.Files.Get(0);

            string filename = string.Format(file.FileName);
            string fullName = Path.Combine(path.Replace("/", @"\"), filename);
            file.SaveAs(fullName);
            return filename;
        }

        /// <summary>
        /// 获取文件夹的目录结构
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetReviewDocList(HttpContext httpContext, string path)
        {
            var a = Directory.GetDirectories(System.Web.Hosting.HostingEnvironment.MapPath(path));
            Dictionary<string, string> dict = new Dictionary<string, string>();
            path = System.Web.Hosting.HostingEnvironment.MapPath(path);
            DirectoryInfo theDir = new DirectoryInfo(path);
            DirectoryInfo[] subDirectories = theDir.GetDirectories();
            foreach (DirectoryInfo dirinfo in subDirectories)
            {
                dict.Add(dirinfo.Name, dirinfo.GetFiles()[0].Name);
            }
            return dict;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="relativePath"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static HttpResponseMessage Download(HttpContext httpContext, string relativePath, string filename)
        {
            var browser = String.Empty;
            if (HttpContext.Current.Request.UserAgent != null)
            {
                browser = HttpContext.Current.Request.UserAgent.ToUpper();
            }
            string filePath = System.Web.Hosting.HostingEnvironment.MapPath(relativePath);
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            FileStream fileStream = File.OpenRead(filePath);
            httpResponseMessage.Content = new StreamContent(fileStream);
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = !browser.Contains("FIREFOX") ? filename : HttpUtility.UrlEncode(filename)
            };
            return httpResponseMessage;
        }

        /// <summary>
        /// 下载评分说明文件
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="relativePath"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static HttpResponseMessage DownloadReview(HttpContext httpContext, string directoryPath)
        {
            var browser = String.Empty;
            if (HttpContext.Current.Request.UserAgent != null)
            {
                browser = HttpContext.Current.Request.UserAgent.ToUpper();
            }
            directoryPath = System.Web.Hosting.HostingEnvironment.MapPath(directoryPath);
            string fileName = Directory.GetFileSystemEntries(directoryPath)[0];
            FileInfo fi = new FileInfo(fileName);
            
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            FileStream fileStream = fi.OpenRead();
            httpResponseMessage.Content = new StreamContent(fileStream);
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = !browser.Contains("FIREFOX") ? fileName : HttpUtility.UrlEncode(fileName)
            };
            return httpResponseMessage;
        }

    }
}
