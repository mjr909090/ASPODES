using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ASPODES.DTO.Profile;
using ASPODES.WebAPI.Authorize;
using ASPODES.WebAPI.Filter;
using ASPODES.WebAPI.Repository;
using ASPODES.WebAPI.Common;

namespace ASPODES.WebAPI.Controllers
{
    /// <summary>
    /// 用户个人信息
    /// </summary>
    [AspodesAuthorize]
    [ActionTrack]
    public class ProfileController : ApiController
    {
        // GET: Profile
        private ProfileRepository repository = new ProfileRepository();

        /// <summary>
        /// 获取用户个人信息
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get()
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetProfile());
            }
            catch(Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 修改用户个人信息
        /// </summary>
        /// <param name="profile">个人信息</param>
        /// <returns></returns>
        [Validation]
        public HttpResponseMessage Put(EditProfileDTO profile)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.EditProfile(profile));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取用户教育经历
        /// </summary>
        /// <returns></returns>
        [Route("api/Profile/Education")]
        [Validation]
        public HttpResponseMessage GetEducation()
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetEducation());
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 添加个人教育经历
        /// </summary>
        /// <param name="education"></param>
        /// <returns></returns>
        [Route("api/Profile/Education")]
        [Validation]
        public HttpResponseMessage Post(EducationDTO education)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.AddEducation(education));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
        

        /// <summary>
        /// 修改个人教育经历
        /// </summary>
        /// <param name="education"></param>
        /// <returns></returns>
        [Route("api/Profile/Education")]
        [Validation]
        public HttpResponseMessage Put(EducationDTO education)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.EditEducation(education));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 删除个人教育经历
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/Profile/Education/{id}")]
        [Validation]
        public HttpResponseMessage DeleteEducation(string id)
        {
            try
            {
                repository.DeleteEducation(id);
                return ResponseWrapper.SuccessResponse("删除成功！");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取用户工作经历
        /// </summary>
        /// <returns></returns>
        [Route("api/Profile/Working")]
        [Validation]
        public HttpResponseMessage GetWorking()
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetWorking());
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 添加个人工作经历
        /// </summary>
        /// <param name="working"></param>
        /// <returns></returns>
        [Route("api/Profile/Working")]
        [Validation]
        public HttpResponseMessage Post(WorkingDTO working)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.AddWorking(working));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 修改个人工作经历
        /// </summary>
        /// <param name="working">工作经历</param>
        /// <returns></returns>
        [Route("api/Profile/Working")]
        [Validation]
        public HttpResponseMessage Put(WorkingDTO working)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.EditWorking(working));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 删除个人工作经历
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/Profile/Working/{id}")]
        [Validation]
        public HttpResponseMessage DeleteWorking(string id)
        {
            try
            {
                repository.DeleteWorking(id);
                return ResponseWrapper.SuccessResponse("删除成功！");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 获取项目成果经历
        /// </summary>
        /// <returns></returns>
        [Route("api/Profile/Achievement")]
        [Validation]
        public HttpResponseMessage GetAchievement()
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.GetAchievement());
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 添加个人项目成果
        /// </summary>
        /// <param name="achievement">项目成果</param>
        /// <returns></returns>
        [Route("api/Profile/Achievement")]
        [Validation]
        public HttpResponseMessage PostAchievement(AchievementDTO achievement)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.AddAchievement(achievement));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 修改个人项目成果
        /// </summary>
        /// <param name="achievement">项目成果</param>
        /// <returns></returns>
        [Route("api/Profile/Achievement")]
        [Validation]
        public HttpResponseMessage PutAchievement(AchievementDTO achievement)
        {
            try
            {
                return ResponseWrapper.SuccessResponse(repository.EditAchievement(achievement));
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }

        /// <summary>
        /// 删除项目成果
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/Profile/Achievement/{id}")]
        [Validation]
        public HttpResponseMessage DeleteAchievement(string id)
        {
            try
            {
                repository.DeleteAchievement(id);
                return ResponseWrapper.SuccessResponse("删除成功！");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="pwd">密码信息</param>
        /// <returns></returns>
        [Route("api/Profile/Password")]
        [HttpPost]
        [Validation]
        public HttpResponseMessage ChangePassword(ChangePasswordDTO pwd)
        {
            try
            {
                repository.ChangePassword(pwd);
                return ResponseWrapper.SuccessResponse("修改成功！");
            }
            catch (Exception e)
            {
                return ResponseWrapper.ExceptionResponse(e);
            }
        }
    }
}