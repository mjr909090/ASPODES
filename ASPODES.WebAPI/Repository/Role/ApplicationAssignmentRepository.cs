using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Http;
using System.Net.Http;
using ASPODES.Model;
using ASPODES.DTO.Role;
using ASPODES.Database;
using AutoMapper;
using ASPODES.WebAPI.Common;
using System.Security.Principal;
using ASPODES.WebAPI;
using System.Text;

namespace ASPODES.WebAPI.Repository
{
    /// <summary>
    /// 給院管理員分配申请书的项目分类的操作类
    /// </summary>
    public class ApplicationAssignmentRepository
    {
        /// <summary>
        ///  給院管理員添加申请书
        /// </summary>
        /// <param name="ReviewCommentDTO">申请书信息</param>
        /// <returns>
        /// 添加成功，返回GetApplicationAssignmentDTO
        /// 失败，返回ResponseStatus.unknown_error和错误信息
        /// </returns>
        public GetApplicationAssignmentDTO AddApplicationAssignment(AddApplicationAssignmentDTO dto)
        {
            ApplicationAssignment reco = Mapper.Map<ApplicationAssignment>(dto);
            using (var ctx = new AspodesDB())
            {
                var rec = ctx.ApplicationAssignments.Add(reco);
                ctx.SaveChanges();
                return Mapper.Map<GetApplicationAssignmentDTO>(rec);
            }
        }


        /// <summary>
        ///根据申请书ID获取分配結果
        /// </summary>
        /// <returns>
        /// 成功，返回ResponseStatus.success分配结果列表
        /// 未找到数据，返回ResponseStatus.parameter_error
        /// 失败，返回ResponseStatus.unknown_error和错误信息
        /// </returns>
        public List<GetApplicationAssignmentDTO> GetApplicationAssignment(int appAssId)
        {
            List<GetApplicationAssignmentDTO> appASS = null;
            using (var ctx = new AspodesDB())
            {
                appASS = ctx.ApplicationAssignments
                    .Where(a => a.ApplicationAssignmentId == appAssId)
                    .Select(Mapper.Map<GetApplicationAssignmentDTO>)
                    .ToList();
                return appASS;
            }

        }

        /// <summary>
        /// 删除申请书的分配記錄
        /// </summary>
        /// <param name="id">申请书分配記錄ID</param>
        /// <returns></returns>
        public void DeleteApplicationAssignment(int appAssId)
        {
            using (var ctx = new AspodesDB())
            {
                var reco = ctx.ApplicationAssignments.FirstOrDefault(a => a.ApplicationAssignmentId == appAssId);
                if (null == reco) throw new NotFoundException("未找到数据");
                ctx.ApplicationAssignments.Remove(reco);
                ctx.SaveChanges();
            }

        }


        /// <summary>
        /// 获取某院管理员所能处理的申请书项目分类
        /// </summary>
        /// <param name="userId">院管理员ID</param>
        /// <returns></returns>
        public List<ApplicationAssignmentDTO> GetUserApplicationAssignment(int personId)
        {
            List<ApplicationAssignmentDTO> appASSList = new List<ApplicationAssignmentDTO>();
            using (var db = new AspodesDB())
            {
                //根据PersonId获取User
                User u1 = db.Users.FirstOrDefault(c => c.PersonId == personId);

                if (null == u1)
                {
                    throw new NotFoundException("用户不存在!");
                }

                //判断用户是否是院管理员
                if (db.Authorizes.Any(c => c.UserId == u1.UserId && c.RoleId == 3))
                {
                    IQueryable<ApplicationAssignment> AssignmentList =
                        db.ApplicationAssignments.Where(c => c.UserId == u1.UserId && c.RoleId == 3);

                    IQueryable<ProjectType> ProjectTypeList = db.ProjectTypes.OrderBy(c => c.Name);

                    ApplicationAssignmentDTO appASS;
                    foreach (var temp in ProjectTypeList)
                    {
                        appASS = new ApplicationAssignmentDTO();
                        appASS.ProjectTypeName = temp.Name;
                        appASS.ProjectTypeId = temp.ProjectTypeId.Value;

                        if (AssignmentList.Any(c => c.ProjectTypeId == temp.ProjectTypeId))
                        {
                            appASS.Checked = true;
                        }
                        else
                        {
                            appASS.Checked = false;
                        }
                        appASSList.Add(appASS);
                    }
                }
                else
                {
                    throw new UnauthorizationException("该用户非有效的院管理员!");
                }
                return appASSList;
            }

        }

        /// <summary>
        /// 修改院管理员申请书项目分类
        /// </summary>
        /// <param name="personId">院管理员ID</param>
        /// <param name="AssignmentList">项目分类列表</param>
        /// <returns></returns>
        public List<GetApplicationAssignmentDTO> UpdateUserApplicationAssignment(int personId, List<ApplicationAssignmentDTO> AssignmentList)
        {
            using (var db = new AspodesDB())
            {
                //根据PersonId获取User
                User u1 = db.Users.FirstOrDefault(c => c.PersonId == personId);

                if (null == u1)
                {
                    throw new NotFoundException("用户不存在!");
                }

                //判断用户是否是院管理员
                if (db.Authorizes.Any(c => c.UserId == u1.UserId && c.RoleId == 3))
                {
                    //获取选中的项目类型列表
                    AssignmentList = AssignmentList.Where(c => c.Checked == true).ToList();

                    //删除之前的项目类型分配列表
                    IQueryable<ApplicationAssignment> oldAssignmentList =
                        db.ApplicationAssignments.Where(c => c.UserId == u1.UserId && c.RoleId == 3);
                    db.ApplicationAssignments.RemoveRange(oldAssignmentList);

                    List<GetApplicationAssignmentDTO> App = new List<GetApplicationAssignmentDTO>();
                    ApplicationAssignment appASS;
                    foreach (var temp in AssignmentList)
                    {
                        appASS = new ApplicationAssignment();
                        appASS.RoleId = 3;
                        appASS.UserId = u1.UserId;
                        appASS.ProjectTypeId = temp.ProjectTypeId;
                        App.Add(Mapper.Map<GetApplicationAssignmentDTO>(db.ApplicationAssignments.Add(appASS)));
                    }

                    db.SaveChanges();
                    return App;

                }
                else
                {
                    throw new UnauthorizationException("该用户非有效的院管理员!");
                }
            }

        }

    }

}