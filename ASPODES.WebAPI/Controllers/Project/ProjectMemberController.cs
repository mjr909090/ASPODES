using ASPODES.DTO.Project;
using ASPODES.Model;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Repository;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ASPODES.WebAPI.Controllers.Project
{
    /// <summary>
    /// 项目成员构造函数
    /// </summary>
    public class ProjectMemberController: ApiController
    {

        private IProjectMemberRepository _projectMemberRepository;

        /// <summary>
        /// 项目成员构造函数
        /// </summary>
        public ProjectMemberController(IProjectMemberRepository projectMemberRepository)
        {

            _projectMemberRepository = projectMemberRepository;
        }

        /// <summary>
        /// 获得项目的参与成员信息
        /// </summary>
        /// <param name="ProjectId">项目id</param>
        [Route("api/projectmember/{ProjectId}")]
        public HttpResponseMessage GetProjectMamberList(string ProjectId)
        {
            try
            {
                var membres = _projectMemberRepository.GerProjectMemberList(ProjectId).ToList();
                GetProjectMemberDTO getProjectMemberDTO = new GetProjectMemberDTO();
                getProjectMemberDTO.HostDepartMember = membres
                                                      .Where(a =>a.Person.InstituteId == a.Project.InstituteId)
                                                      .Select(Mapper.Map<GetProjectMemberVO>).ToList();
                getProjectMemberDTO.OtherDepartMember = membres
                                                      .Where(a =>a.Person.InstituteId != a.Project.InstituteId)
                                                      .Select(Mapper.Map<GetProjectMemberVO>).ToList();

                return ResponseWrapper.SuccessResponse(getProjectMemberDTO);
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
    }
}