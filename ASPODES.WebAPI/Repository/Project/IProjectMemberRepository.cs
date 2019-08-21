using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;

namespace ASPODES.WebAPI.Repository
{
    public interface IProjectMemberRepository
    {
        IOrderedQueryable<ProjectMember> GerProjectMemberList(string projectId);

        ProjectMember Add(ProjectMember member);

        ProjectMember Update(ProjectMember member);

        void Delete(string projectId, int? personId);
    }
}
