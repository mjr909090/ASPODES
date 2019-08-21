using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ASPODES.Common.Util;
using ASPODES.Database;
using ASPODES.DTO.Inst_Person_User;
using ASPODES.DTO.Profile;
using ASPODES.Model;
using ASPODES.WebAPI.Common;
using ASPODES.WebAPI.Models;
using AutoMapper;
using Newtonsoft.Json;

namespace ASPODES.WebAPI.Repository
{
    public class ProfileRepository
    {
        /// <summary>
        /// 获取用户个人信息
        /// </summary>
        public GetProfileDTO GetProfile()
        {
            GetProfileDTO profileDTO = null;
            var userid = HttpContext.Current.User.Identity.Name;
            using (var db = new AspodesDB())
            {
                var user = db.Users.FirstOrDefault(c => c.UserId == userid);
                if (user != null)
                {
                    profileDTO = Mapper.Map<GetProfileDTO>(user.Person);

                    if (!string.IsNullOrEmpty(user.Person.EducationHistor))
                    {
                        profileDTO.EducationList = JsonConvert
                            .DeserializeObject<List<EducationDTO>>(user.Person.EducationHistor)
                            .OrderBy(c => c.BeginTime)
                            .ThenBy(c => c.EndTime)
                            .ToList();
                    }
                    if (!string.IsNullOrEmpty(user.Person.WorkingHistory))
                    {
                        profileDTO.WorkingList = JsonConvert
                            .DeserializeObject<List<WorkingDTO>>(user.Person.WorkingHistory)
                            .OrderBy(c => c.BeginTime)
                            .ThenBy(c => c.EndTime)
                            .ToList();
                    }
                    if (!string.IsNullOrEmpty(user.Person.Achievements))
                    {
                        profileDTO.AchievementList = JsonConvert
                            .DeserializeObject<List<AchievementDTO>>(user.Person.Achievements)
                            .OrderBy(c => c.AchievementTime)
                            .ThenBy(c => c.Name)
                            .ToList();
                    }

                    //判断是否为专家
                    var expertInfo = db.Authorizes.FirstOrDefault(c => c.UserId == userid && c.RoleId == 5);
                    if (expertInfo != null)
                    {
                        profileDTO.Expert = true;
                    }
                    else
                    {
                        profileDTO.Expert = false;
                    }
                }
                else
                {
                    throw new NotFoundException("找不到用户！");
                }
                return profileDTO;
            }

        }

        /// <summary>
        /// 修改用户个人信息
        /// </summary>
        /// <param name="profile">个人信息</param>
        public GetProfileDTO EditProfile(EditProfileDTO profile)
        {
            var userid = HttpContext.Current.User.Identity.Name;
            using (var db = new AspodesDB())
            {
                var user = db.Users.FirstOrDefault(c => c.UserId == userid);
                if (user != null)
                {
                    //user.UserId = profile.Email;
                    //user.Person.Email = profile.Email;
                    user.Person.Phone = profile.Phone;
                    user.Person.Address = profile.Address;


                    //判断是否为专家
                    //var expertInfo = db.ExpertInfos.FirstOrDefault(c => c.ExpertInfoId == userid);
                    //if (expertInfo != null)
                    //{
                    //    expertInfo.ExpertInfoId = profile.Email;
                    //}

                    db.SaveChanges();
                    return Mapper.Map<GetProfileDTO>(user.Person);
                }
                else
                {
                    throw new NotFoundException("找不到用户！");
                }
            }
        }

        /// <summary>
        /// 获取用户教育经历
        /// </summary>
        public List<EducationDTO> GetEducation()
        {
            List<EducationDTO> EducationList = new List<EducationDTO>();
            var userid = HttpContext.Current.User.Identity.Name;

            using (var db = new AspodesDB())
            {
                //判断是否为专家
                var userInfo = db.Users.FirstOrDefault(c => c.UserId == userid);
                if (userInfo != null)
                {
                    if (!string.IsNullOrEmpty(userInfo.Person.EducationHistor))
                    {
                        EducationList = JsonConvert
                            .DeserializeObject<List<EducationDTO>>(userInfo.Person.EducationHistor)
                            .OrderBy(c => c.BeginTime)
                            .ThenBy(c => c.EndTime)
                            .ToList();
                    }
                    return EducationList;
                }
                else
                {
                    throw new NotFoundException("找不到用户！");
                }
            }
        }

        /// <summary>
        /// 添加用户教育经历
        /// </summary>
        /// <param name="education">教育经历</param>
        public List<EducationDTO> AddEducation(EducationDTO education)
        {
            var userid = HttpContext.Current.User.Identity.Name;

            using (var db = new AspodesDB())
            {
                var userInfo = db.Users.FirstOrDefault(c => c.UserId == userid);
                if (userInfo != null)
                {
                    education.EducationId = Guid.NewGuid().ToString("D");
                    List<EducationDTO> educationList = new List<EducationDTO>();
                    if (!string.IsNullOrEmpty(userInfo.Person.EducationHistor))
                    {
                        educationList = JsonConvert.DeserializeObject<List<EducationDTO>>(userInfo.Person.EducationHistor);
                    }
                    educationList.Add(education);

                    userInfo.Person.EducationHistor = JsonConvert.SerializeObject(educationList);

                    db.SaveChanges();
                    return educationList;
                }
                else
                {
                    throw new NotFoundException("找不到用户！");

                }
            }

        }

        /// <summary>
        /// 修改用户教育经历
        /// </summary>
        /// <param name="education">教育经历</param>
        public List<EducationDTO> EditEducation(EducationDTO education)
        {
            var userid = HttpContext.Current.User.Identity.Name;

            using (var db = new AspodesDB())
            {
                var userInfo = db.Users.FirstOrDefault(c => c.UserId == userid);
                if (userInfo != null)
                {
                    ;
                    if (!string.IsNullOrEmpty(userInfo.Person.EducationHistor))
                    {
                        List<EducationDTO> educationList = JsonConvert.DeserializeObject<List<EducationDTO>>(userInfo.Person.EducationHistor);
                        var educationdata =
                            educationList.FirstOrDefault(c => c.EducationId == education.EducationId);
                        if (educationdata != null)
                        {
                            educationList.Remove(educationdata);
                            educationList.Add(education);

                            userInfo.Person.EducationHistor = JsonConvert.SerializeObject(educationList);

                            db.SaveChanges();
                            return educationList;
                        }
                        else
                        {
                            throw new NotFoundException("该教育经历不存在！");
                        }
                    }
                    else
                    {
                        throw new NotFoundException("无教育经历！");
                    }
                }
                else
                {
                    throw new NotFoundException("找不到用户！");
                }
            }

        }

        /// <summary>
        /// 删除用户教育经历
        /// </summary>
        /// <param name="educationId">教育经历id</param>
        public void DeleteEducation(string educationId)
        {
            if (string.IsNullOrEmpty(educationId))
            {
                throw new ModelValidException("教育经历编号不能为空！");
            }
            var userid = HttpContext.Current.User.Identity.Name;

            using (var db = new AspodesDB())
            {
                var userInfo = db.Users.FirstOrDefault(c => c.UserId == userid);
                if (userInfo != null)
                {
                    ;
                    if (!string.IsNullOrEmpty(userInfo.Person.EducationHistor))
                    {
                        List<EducationDTO> educationList = JsonConvert.DeserializeObject<List<EducationDTO>>(userInfo.Person.EducationHistor);
                        var educationdata =
                            educationList.FirstOrDefault(c => c.EducationId == educationId);
                        if (educationdata != null)
                        {
                            educationList.Remove(educationdata);

                            if (educationList.Count > 0)
                            {
                                userInfo.Person.EducationHistor = JsonConvert.SerializeObject(educationList);
                            }
                            else
                            {
                                userInfo.Person.EducationHistor = "";
                            }
                            db.SaveChanges();
                        }
                        else
                        {
                            throw new NotFoundException("该教育经历不存在！");
                        }
                    }
                    else
                    {
                        throw new NotFoundException("无教育经历！");
                    }
                }
                else
                {
                    throw new NotFoundException("找不到用户！");
                }
            }

        }

        /// <summary>
        /// 获取用户工作经历
        /// </summary>
        public List<WorkingDTO> GetWorking()
        {
            List<WorkingDTO> WorkingList = new List<WorkingDTO>();
            var userid = HttpContext.Current.User.Identity.Name;

            using (var db = new AspodesDB())
            {
                //判断是否为专家
                var userInfo = db.Users.FirstOrDefault(c => c.UserId == userid);
                if (userInfo != null)
                {
                    if (!string.IsNullOrEmpty(userInfo.Person.WorkingHistory))
                    {
                        WorkingList = JsonConvert
                            .DeserializeObject<List<WorkingDTO>>(userInfo.Person.WorkingHistory)
                            .OrderBy(c => c.BeginTime)
                            .ThenBy(c => c.EndTime)
                            .ToList();
                    }
                    return WorkingList;
                }
                else
                {
                    throw new NotFoundException("找不到用户！");
                }
            }
        }

        /// <summary>
        /// 添加用户工作经历
        /// </summary>
        /// <param name="working">工作经历</param>
        public List<WorkingDTO> AddWorking(WorkingDTO working)
        {
            var userid = HttpContext.Current.User.Identity.Name;

            using (var db = new AspodesDB())
            {
                var userInfo = db.Users.FirstOrDefault(c => c.UserId == userid);
                if (userInfo != null)
                {
                    working.WorkingId = Guid.NewGuid().ToString("D"); ;
                    List<WorkingDTO> workingList = new List<WorkingDTO>();
                    if (!string.IsNullOrEmpty(userInfo.Person.WorkingHistory))
                    {
                        workingList = JsonConvert.DeserializeObject<List<WorkingDTO>>(userInfo.Person.WorkingHistory);
                    }
                    workingList.Add(working);

                    userInfo.Person.WorkingHistory = JsonConvert.SerializeObject(workingList);

                    db.SaveChanges();
                    return workingList;
                }
                else
                {
                    throw new NotFoundException("找不到用户！");
                }
            }
        }

        /// <summary>
        /// 修改用户工作经历
        /// </summary>
        /// <param name="working">工作经历</param>
        public List<WorkingDTO> EditWorking(WorkingDTO working)
        {

            var userid = HttpContext.Current.User.Identity.Name;

            using (var db = new AspodesDB())
            {
                var userInfo = db.Users.FirstOrDefault(c => c.UserId == userid);
                if (userInfo != null)
                {
                    ;
                    if (!string.IsNullOrEmpty(userInfo.Person.WorkingHistory))
                    {
                        List<WorkingDTO> workingList = JsonConvert.DeserializeObject<List<WorkingDTO>>(userInfo.Person.WorkingHistory);
                        var workingdata =
                            workingList.FirstOrDefault(c => c.WorkingId == working.WorkingId);
                        if (workingdata != null)
                        {
                            workingList.Remove(workingdata);
                            workingList.Add(working);

                            userInfo.Person.WorkingHistory = JsonConvert.SerializeObject(workingList);

                            db.SaveChanges();
                            return workingList;
                        }
                        else
                        {
                            throw new NotFoundException("该工作经历不存在！");

                        }
                    }
                    else
                    {
                        throw new NotFoundException("无工作经历！");
                    }
                }
                else
                {
                    throw new NotFoundException("找不到用户！");
                }
            }
        }

        /// <summary>
        /// 删除用户工作经历
        /// </summary>
        /// <param name="workingId">工作经历Id</param>
        public void DeleteWorking(string workingId)
        {
            if (string.IsNullOrEmpty(workingId))
            {
                throw new ModelValidException("工作经历编号不能为空！");
            }
            var userid = HttpContext.Current.User.Identity.Name;

            using (var db = new AspodesDB())
            {
                var userInfo = db.Users.FirstOrDefault(c => c.UserId == userid);
                if (userInfo != null)
                {
                    ;
                    if (!string.IsNullOrEmpty(userInfo.Person.WorkingHistory))
                    {
                        List<WorkingDTO> workingList = JsonConvert.DeserializeObject<List<WorkingDTO>>(userInfo.Person.WorkingHistory);
                        var workingdata =
                            workingList.FirstOrDefault(c => c.WorkingId == workingId);
                        if (workingdata != null)
                        {
                            workingList.Remove(workingdata);

                            if (workingList.Count > 0)
                            {
                                userInfo.Person.WorkingHistory = JsonConvert.SerializeObject(workingList);
                            }
                            else
                            {
                                userInfo.Person.WorkingHistory = "";
                            }
                            db.SaveChanges();
                        }
                        else
                        {
                            throw new NotFoundException("该工作经历不存在！");

                        }
                    }
                    else
                    {
                        throw new NotFoundException("无工作经历！");
                    }
                }
                else
                {
                    throw new NotFoundException("找不到用户！");
                }
            }
        }

        /// <summary>
        /// 获取用户成果
        /// </summary>
        public List<AchievementDTO> GetAchievement()
        {
            List<AchievementDTO> achievementList = new List<AchievementDTO>();
            var userid = HttpContext.Current.User.Identity.Name;

            using (var db = new AspodesDB())
            {
                //判断是否为专家
                var userInfo = db.Users.FirstOrDefault(c => c.UserId == userid);
                if (userInfo != null)
                {
                    if (!string.IsNullOrEmpty(userInfo.Person.Achievements))
                    {
                        achievementList = JsonConvert
                            .DeserializeObject<List<AchievementDTO>>(userInfo.Person.Achievements)
                            .OrderBy(c => c.AchievementTime)
                            .ThenBy(c => c.Name)
                            .ToList();
                    }
                    return achievementList;
                }
                else
                {
                    throw new NotFoundException("找不到用户！");
                }
            }

        }

        /// <summary>
        /// 添加用户成果
        /// </summary>
        /// <param name="achievement">成果</param>
        public List<AchievementDTO> AddAchievement(AchievementDTO achievement)
        {
            var userid = HttpContext.Current.User.Identity.Name;

            using (var db = new AspodesDB())
            {
                var userInfo = db.Users.FirstOrDefault(c => c.UserId == userid);
                if (userInfo != null)
                {
                    achievement.AchievementId = Guid.NewGuid().ToString("D"); ;
                    List<AchievementDTO> achievementList = new List<AchievementDTO>();
                    if (!string.IsNullOrEmpty(userInfo.Person.Achievements))
                    {
                        achievementList = JsonConvert.DeserializeObject<List<AchievementDTO>>(userInfo.Person.Achievements);
                    }
                    achievementList.Add(achievement);

                    userInfo.Person.Achievements = JsonConvert.SerializeObject(achievementList);

                    db.SaveChanges();
                    return achievementList;
                }
                else
                {
                    throw new NotFoundException("找不到用户！");
                }
            }
        }

        /// <summary>
        /// 修改用户成果
        /// </summary>
        /// <param name="achievement">成果</param>
        public List<AchievementDTO> EditAchievement(AchievementDTO achievement)
        {
            var userid = HttpContext.Current.User.Identity.Name;

            using (var db = new AspodesDB())
            {
                var userInfo = db.Users.FirstOrDefault(c => c.UserId == userid);
                if (userInfo != null)
                {
                    ;
                    if (!string.IsNullOrEmpty(userInfo.Person.Achievements))
                    {
                        List<AchievementDTO> achievementList = JsonConvert.DeserializeObject<List<AchievementDTO>>(userInfo.Person.Achievements);
                        var achievementdata =
                            achievementList.FirstOrDefault(c => c.AchievementId == achievement.AchievementId);
                        if (achievementdata != null)
                        {
                            achievementList.Remove(achievementdata);
                            achievementList.Add(achievement);

                            userInfo.Person.Achievements = JsonConvert.SerializeObject(achievementList);

                            db.SaveChanges();
                            return achievementList;
                        }
                        else
                        {
                            throw new NotFoundException("该项目成果不存在！");

                        }
                    }
                    else
                    {
                        throw new NotFoundException("无项目成果！");
                    }
                }
                else
                {
                    throw new NotFoundException("找不到用户！");
                }
            }
        }

        /// <summary>
        /// 删除用户成果
        /// </summary>
        /// <param name="achievementId">成果Id</param>
        public void DeleteAchievement(string achievementId)
        {
            if (string.IsNullOrEmpty(achievementId))
            {
                throw new ModelValidException("项目成功编号不能为空！");
            }
            var userid = HttpContext.Current.User.Identity.Name;

            using (var db = new AspodesDB())
            {
                var userInfo = db.Users.FirstOrDefault(c => c.UserId == userid);
                if (userInfo != null)
                {
                    ;
                    if (!string.IsNullOrEmpty(userInfo.Person.Achievements))
                    {
                        List<AchievementDTO> achievementList = JsonConvert.DeserializeObject<List<AchievementDTO>>(userInfo.Person.Achievements);
                        var achievementdata =
                            achievementList.FirstOrDefault(c => c.AchievementId == achievementId);
                        if (achievementdata != null)
                        {
                            achievementList.Remove(achievementdata);

                            if (achievementList.Count > 0)
                            {
                                userInfo.Person.Achievements = JsonConvert.SerializeObject(achievementList);
                            }
                            else
                            {
                                userInfo.Person.Achievements = "";
                            }
                            db.SaveChanges();
                        }
                        else
                        {
                            throw new NotFoundException("该项目成果不存在！");

                        }
                    }
                    else
                    {
                        throw new NotFoundException("无项目成果！");
                    }
                }
                else
                {
                    throw new NotFoundException("找不到用户！");
                }
            }
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="pwd">密码</param>
        public void ChangePassword(ChangePasswordDTO pwd)
        {
            var userid = HttpContext.Current.User.Identity.Name;
            using (var db = new AspodesDB())
            {
                string pwdmd5 = HashHelper.IntoMd5(pwd.OldPassword);
                User user = db.Users.FirstOrDefault(u => u.UserId == userid && u.Password == pwdmd5);
                if (null == user)
                {
                    throw new ModelValidException("原密码错误！");
                }
                else
                {
                    user.Password = HashHelper.IntoMd5(pwd.NewPassword);
                    db.SaveChanges();
                }
            }
        }


    }
}
