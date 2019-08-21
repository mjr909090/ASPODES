using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
using System.Web;
namespace ASPODES.Common
{
    public class Util
    {
        public static bool SaveUploadFile( HttpContext httpContext, string path )
        {
            HttpFileCollection reqFiles = httpContext.Request.Files;
            for (int i = 0; i < reqFiles.Count; i++)
            {
                HttpPostedFile file = reqFiles[i];
                string fname = httpContext.Server.MapPath(path);
                file.SaveAs(fname);
            }
            return true;
        }
    }
}
