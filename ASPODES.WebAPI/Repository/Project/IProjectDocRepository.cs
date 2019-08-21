using ASPODES.Database;
using ASPODES.DTO.Project;
using ASPODES.Model;
using ASPODES.WebAPI.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.WebAPI.Repository
{
    /// <summary>
    /// 项目文档操作接口
    /// </summary>
    public interface IProjectDocRepository
    {
        
        void UploadProjectDoc(ProjectDoc doc);

        IQueryable<ProjectDoc> GetProjectDocList(String projectId);

        HttpResponseMessage DownloadProjectDoc(int? projectDocId);

        ProjectDoc Delete(int projectDocId);

        ProjectDoc UploadFinishReport(ProjectDoc doc);
    }
}
