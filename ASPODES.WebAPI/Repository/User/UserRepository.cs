using ASPODES.Database;
using ASPODES.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPODES.WebAPI.Repository
{
    /// <summary>
    /// 对用户表进行相关的操作
    /// </summary>
    public class UserRepository
    {
        private AspodesDB _context;
        public UserRepository(AspodesDB context)
        {
            this._context = context;
        }

        /// <summary>
        /// 通过用户名和密码获取一个合法用户的信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User GetLegalUser(string name, string password)
        {
            return _context.Users.FirstOrDefault(
                u => (
                    u.Person.EnglishName == name || 
                    u.UserId == name
                ) && 
                u.Password == password && 
                u.Person.Status != "D");
        }

        /// <summary>
        /// 获取某单位的所有管理员用户ID列表
        /// </summary>
        /// <param name="instituteId"></param>
        /// <returns></returns>
        public List<string> GetInstituteManagersUserId(int instituteId)
        {
            var instRolePerson = from authorize in _context.Authorizes
                                 join user in _context.Users on authorize.UserId equals user.UserId
                                 where user.InstituteId == instituteId && authorize.RoleId == 2
                                 select user.UserId;
            return instRolePerson.ToList();
        }

        /// <summary>
        /// 获取某用户所在的单位ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetInstIdOfUser(string userId)
        {
            return _context.Users.Find(userId).InstituteId.Value;
        }

        /// <summary>
        /// 通过申请书ID获取牵头人的ID
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public string GetUserIdByApplicationId(string applicationId)
        {
            var persionId = (from application in _context.Applications
                             where application.ApplicationId == applicationId
                             select application.LeaderId.Value).FirstOrDefault();
            return _context.Users.FirstOrDefault(u => u.PersonId.Value == persionId).UserId;
        }

        /// <summary>
        /// 通过申请书领域种类的ID获取在此领域院管理员的Id列表
        /// </summary>
        /// <param name="ProjectTypeId"></param>
        /// <returns></returns>
        public List<string> GetDepartManagerIdByProjectTypeId(int ProjectTypeId)
        {
            var deptManagerId = from applicationAssignment in _context.ApplicationAssignments
                                 where applicationAssignment.ProjectTypeId == ProjectTypeId
                                 select applicationAssignment.UserId;
            return deptManagerId.ToList();
        }

        /// <summary>
        /// 获取所有的院管理员Id列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllDeptManagerId()
        {
            var deptManagerList = from a in _context.Authorizes
                                where a.RoleId == 3
                                select a.UserId;
            return deptManagerList.ToList();
        }

        /// <summary>
        /// 通过persionId获取UserId
        /// </summary>
        /// <param name="persionId"></param>
        /// <returns></returns>
        public string GetUserIdByPersonId(int persionId)
        {
            return _context.Users.FirstOrDefault(u => u.PersonId == persionId).UserId;
        }

        /// <summary>
        /// 通过用户ID获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetUser(string userId)
        {
            return _context.Users.Find(userId);
        }

        /// <summary>
        /// 获取某单位的所有用户ID列表
        /// </summary>
        /// <param name="instId"></param>
        /// <returns></returns>
        public List<string> GetUserIdListByInstId(int instId)
        {
            return _context.Users.Where(u => u.InstituteId == instId)
                .Select(u => u.UserId).ToList();
        }

        /// <summary>
        /// 获取所有的用户Id列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllUserIdList()
        {
            return _context.Users.Select(u => u.UserId).ToList();
        }
    }
}