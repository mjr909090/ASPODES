//using ASPODES.Model;
//using ASPODES.WebAPI.Repository;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace ASPODES.WebAPI.Service
//{

//    /// <summary>
//    /// 委托，实现异步AddNoticeList
//    /// </summary>
//    /// <param name="receiveIds"></param>
//    /// <param name="templateId"></param>
//    /// <param name="dic"></param>
//    //public delegate void AddNoticeListHandler(List<string> receiveIds, int templateId, Dictionary<string, string> dic = null);

//    /// <summary>
//    /// 委托，实现异步AddNotice
//    /// </summary>
//    /// <param name="receiveId"></param>
//    /// <param name="templateId"></param>
//    /// <param name="dic"></param>
//    //public delegate void AddNoticeHandler(string receiveId, int templateId, Dictionary<string, string> dic = null);


//    /// <summary>
//    /// 系统形成通知的服务
//    /// </summary>
//    public class CreateNoticeService
//    {
//        private NoticeRepository _noticeRepository;
//        private NoticeTemplateRepository _noticeTemplateRepository;
//        private UserRepository _userRepository;
//        private ApplicationRepository _applicationRepository;
//        private InstRepository _instRepository;
//        private ReviewCommentRepository _reviewCommentRepository;
//        private ReviewAssignmentRepository _reviewAssignmentRepository;
//        private AnnualTaskRepository _annualTaskRepository;
//        private PersonRepository _personRepository;
//        private RecommendationRepository _recomendationRepository;
//        public CreateNoticeService(NoticeRepository noticeRepository,
//            UserRepository userRepository,
//            ApplicationRepository applicationRepository,
//            NoticeTemplateRepository noticeTemplateRepository,
//            InstRepository instRepository,
//            ReviewCommentRepository reviewCommentRepository,
//            ReviewAssignmentRepository reviewAssignmentRepository,
//            AnnualTaskRepository annualTaskRepository,
//            PersonRepository personRepository,
//            RecommendationRepository recomendationRepository)
//        {
//            this._noticeRepository = noticeRepository;
//            this._userRepository = userRepository;
//            this._applicationRepository = applicationRepository;
//            this._noticeTemplateRepository = noticeTemplateRepository;
//            this._instRepository = instRepository;
//            this._reviewCommentRepository = reviewCommentRepository;
//            this._reviewAssignmentRepository = reviewAssignmentRepository;
//            this._annualTaskRepository = annualTaskRepository;
//            this._personRepository = personRepository;
//            this._recomendationRepository = recomendationRepository;
//        }
        
//        /// <summary>
//        /// 向数据库中添加对一列人员的通知
//        /// </summary>
//        /// <param name="receiveIds">通知接收者ID列表</param>
//        /// <param name="templateId">通知模板ID</param>
//        /// <param name="dic">要替换的信息</param>
//        public void AddNoticeList(List<string> receiveIds, int templateId, Dictionary<string, string> dic = null)
//        {
//            string content = null;
//            // 判断是否有需要替换的字段
//            if (dic != null)
//            {
//                content = _noticeTemplateRepository.GetContentById(templateId);
//                foreach (KeyValuePair<string, string> pair in dic)
//                {
//                    content = content.Contains(pair.Key)
//                        ? content.Replace(pair.Key, pair.Value) : content;
//                }
//            }
//            List<Notice> notices = new List<Notice>();
//            foreach(string id in receiveIds)
//            {
//                notices.Add(
//                    new Notice {
//                        NoticeTemplateId = templateId,
//                        ReceiveId = id,
//                        Read = false,
//                        SendTime = DateTime.Now,
//                        Content = content
//                    });
//            }
//            _noticeRepository.SaveNoticeList(notices);
//        }

//        /// <summary>
//        /// 向数据库中添加对一个人员的通知
//        /// </summary>
//        /// <param name="receiveId">通知接收者ID</param>
//        /// <param name="templateId">通知模板ID</param>
//        /// <param name="dic">要替换的信息</param>
//        public void AddNotice(string receiveId, int templateId, Dictionary<string, string> dic = null)
//        {
//            string content = null;
//            // 判断是否有需要替换的字段
//            if (dic != null)
//            {
//                content = _noticeTemplateRepository.GetContentById(templateId);
//                foreach (KeyValuePair<string, string> pair in dic)
//                {
//                    content = content.Contains(pair.Key) 
//                        ? content.Replace(pair.Key, pair.Value) : content;
//                }
//            }
//            Notice notice = new Notice
//            {
//                NoticeTemplateId = templateId,
//                ReceiveId = receiveId,
//                Read = false,
//                SendTime = DateTime.Now,
//                Content = content
//            };
//            _noticeRepository.SaveNotice(notice);
//        }

//        //组合服务-----------------------------------------------------------------------------------

//        /// <summary>
//        /// 通过申请书获取所在单位的所有的单位管理员Id列表，由单服务组成
//        /// </summary>
//        /// <param name="applicationId"></param>
//        /// <returns></returns>
//        public List<string> GetInstManagerIdsbyAppId(string applicationId)
//        {
//            string userId = GetUserIdByApplicationId(applicationId);
//            int instId = GetInstituteIdOfUser(userId);
//            return GetInstituteManagerIdList(instId);
//        }
        
//        /// <summary>
//        /// 通过初审指派ID，获取申请书领域的院管理员ID列表
//        /// </summary>
//        /// <param name="assignmentCommentId"></param>
//        /// <returns></returns>
//        public List<string> GetDeptMIdListByAssignmentId(int assignmentCommentId)
//        {
//            var appId = GetApplicationIdByAssignmentId(assignmentCommentId);
//            return GetDepartManagerIdListByApplicationId(appId);
//        }

//        /// <summary>
//        /// 获取专家的用户ID列表（待评审的申请书数量以后天上）
//        /// </summary>
//        /// <returns></returns>
//        public List<string> GetExpertIdList()
//        {
//            List<string>AppIdList = GetReviewAppIdList(ApplicationStatus.REVIEW);
//            return _reviewAssignmentRepository.GetExpertIdList(AppIdList);

//        }

//        /// <summary>
//        /// 通过申请书Id，获取申请书所属领域的院管理员的Id列表
//        /// </summary>
//        /// <param name="applicationId"></param>
//        /// <returns></returns>
//        public List<string> GetDepartManagerIdListByApplicationId(string applicationId)
//        {
//            int projectTypeId = _applicationRepository.GetProjectTypeId(applicationId);
//            return GetDepartManagerIdListByProjectTypeId(projectTypeId);
//        }

//        /// <summary>
//        /// 通过年度任务书获取项目所在单位的单位管理员
//        /// </summary>
//        /// <param name="annualTaskId"></param>
//        /// <returns></returns>
//        public List<string> GetInstManagerListByAnnualTaskId(int annualTaskId)
//        {
//            int instId = GetInstIdByAnnualTask(annualTaskId);
//            return GetInstituteManagerIdList(instId);
//        }

//        /// <summary>
//        /// 通过年度任务书获取项目负责人UserId
//        /// </summary>
//        /// <param name="annualTaskId"></param>
//        /// <returns></returns>
//        public string GetUserIdByAnnualTaskId(int annualTaskId)
//        {
//            int personId = GetPersonIdByAnnualTask(annualTaskId);
//            return GetUserIdByPersonId(personId);
//        }

//        /// <summary>
//        /// 通过专家推荐表获取候选人所在单位的单位管理员
//        /// </summary>
//        /// <param name="recommendationId"></param>
//        /// <returns></returns>
//        public List<string> GetInstMListByAdoptRecommendation(int recommendationId)
//        {
//            int instId = GetInstMListByRecommendation(recommendationId);
//            return GetInstituteManagerIdList(instId);
//        }

//        /// <summary>
//        /// 通过personId获取所在单位的所有单位管理员
//        /// </summary>
//        /// <param name="personId"></param>
//        /// <returns></returns>
//        public List<string> GetInstMListByPersonId(int personId)
//        {
//            int recommendation = GetRecommendationByPersonId(personId);
//            return GetInstMListByAdoptRecommendation(recommendation);
//        }

//        /// <summary>
//        /// 通过专家推荐ID获取候选人名称
//        /// </summary>
//        /// <param name="recommendationId"></param>
//        /// <returns></returns>
//        public string GetCandidateNameByRecommendation(int recommendationId)
//        {
//            string userId = GetCandidateIdByRecommendation(recommendationId);
//            return GetNameByUserId(userId);
//        }

//        //单个服务-----------------------------------------------------------------------------------

//        /// <summary>
//        /// 根据personId获取专家推荐ID
//        /// </summary>
//        /// <param name="personId"></param>
//        /// <returns></returns>
//        public int GetRecommendationByPersonId(int personId)
//        {
//            return _recomendationRepository.GetRecommendation(personId).RecommendationId.Value;
//        }

//        /// <summary>
//        /// 通过年度任务书ID获取项目ID
//        /// </summary>
//        /// <param name="annualTaskId"></param>
//        /// <returns></returns>
//        public string GetProjectIdByAnnualTask(int annualTaskId)
//        {
//            return _annualTaskRepository.GetProjectIdByAnnualTask(annualTaskId);
//        }

//        /// <summary>
//        /// 通过年度任务书ID获取项目所在单位ID
//        /// </summary>
//        /// <param name="annualTaskId"></param>
//        /// <returns></returns>
//        public int GetInstIdByAnnualTask(int annualTaskId)
//        {
//            return _annualTaskRepository.GetInstIdByAnnualTask(annualTaskId);
//        }

//        /// <summary>
//        /// 通过年度任务书ID获取项目负责人personId
//        /// </summary>
//        /// <param name="annualTaskId"></param>
//        /// <returns></returns>
//        public int GetPersonIdByAnnualTask(int annualTaskId)
//        {
//            return _annualTaskRepository.GetPersonIdByAnnualTask(annualTaskId);
//        }

//        /// <summary>
//        /// 通过persionID获取UserID
//        /// </summary>
//        /// <param name="personId"></param>
//        /// <returns></returns>
//        public string GetUserIdByPersonId(int personId)
//        {
//            return _userRepository.GetUserIdByPersonId(personId);
//        }
        
//        /// <summary>
//        /// 获取单位的所有管理员ID列表
//        /// </summary>
//        /// <param name="instituteId"></param>
//        /// <returns></returns>
//        public List<string> GetInstituteManagerIdList(int instituteId)
//        {
//            return _userRepository.GetInstituteManagersUserId(instituteId);
//        }

//        /// <summary>
//        /// 通过单位ID获取单位的名称
//        /// </summary>
//        /// <param name="instituteId"></param>
//        /// <returns></returns>
//        public string GetInstituteNameById(int instituteId)
//        {
//            return _instRepository.GetInstName(instituteId);
//        }

//        /// <summary>
//        /// 通过申请书ID获取牵头人的ID
//        /// </summary>
//        /// <param name="applicationId"></param>
//        /// <returns></returns>
//        public string GetUserIdByApplicationId(string applicationId)
//        {
//            return _userRepository.GetUserIdByApplicationId(applicationId);
//        }

//        /// <summary>
//        /// 获取某用户所在的单位ID
//        /// </summary>
//        /// <param name="userId"></param>
//        /// <returns></returns>
//        public int GetInstituteIdOfUser(string userId)
//        {
//            return _userRepository.GetInstIdOfUser(userId);
//        }

//        /// <summary>
//        /// 通过项目类型获取所属领域的院管理员
//        /// </summary>
//        /// <param name="projectTypeId"></param>
//        /// <returns></returns>
//        public List<string> GetDepartManagerIdListByProjectTypeId(int projectTypeId)
//        {
//            return _userRepository.GetDepartManagerIdByProjectTypeId(projectTypeId);
//        }

//        /// <summary>
//        /// 获取所有的院管理员ID列表
//        /// </summary>
//        /// <returns></returns>
//        public List<string> GetAllDepartManagerIdList()
//        {
//            return _userRepository.GetAllDeptManagerId();
//        }

//        /// <summary>
//        /// 通过申请书Id获取申请书名称
//        /// </summary>
//        /// <param name="applicationID"></param>
//        /// <returns></returns>
//        public string GetApplicationNameById(string applicationID)
//        {
//            return _applicationRepository.GetApplicationNameById(applicationID);
//        }

//        /// <summary>
//        /// 通过初审评论ID获取申请书ID
//        /// </summary>
//        /// <param name="assignmentCommentId"></param>
//        /// <returns></returns>
//        public string GetApplicationIdByAssignmentId(int assignmentCommentId)
//        {
//            return _reviewCommentRepository.GetAppId(assignmentCommentId);
//        }

//        /// <summary>
//        /// 判断一封申请书所指派的专家是否全部评审完毕
//        /// </summary>
//        /// <param name="appId"></param>
//        /// <returns></returns>
//        public bool CompleteReview(string appId)
//        {
//            int num = _reviewCommentRepository.getReviewedNum(appId);
//            return num >= SystemConfig.ExpertReviewAmount;
//        }


//        /// <summary>
//        /// 获取属于某种状态的申请书Id列表
//        /// </summary>
//        /// <param name="status"></param>
//        /// <returns></returns>
//        public List<string> GetReviewAppIdList(ApplicationStatus status)
//        {
//            return _applicationRepository.GetAppIdListByStatus(status);
//        }

//        /// <summary>
//        /// 通过personId获取名字
//        /// </summary>
//        /// <param name="personId"></param>
//        /// <returns></returns>
//        public string GetNameByPersionId(int personId)
//        {
//            return _personRepository.GetPersion(personId).Name;
//        }

//        /// <summary>
//        /// 通过用户ID获取用户名字
//        /// </summary>
//        /// <param name="userId"></param>
//        /// <returns></returns>
//        public string GetNameByUserId(string userId)
//        {
//            return _userRepository.GetUser(userId).Name;
//        }

//        /// <summary>
//        /// 通过专家推荐ID获取单位ID
//        /// </summary>
//        /// <param name="recommendationId"></param>
//        /// <returns></returns>
//        public int GetInstMListByRecommendation(int recommendationId)
//        {
//            return _recomendationRepository.GetRecommendationById(recommendationId)
//                .InstituteId.Value;
//        }

//        /// <summary>
//        /// 通过专家推荐ID获取候选人的UserId
//        /// </summary>
//        /// <param name="recommendationId"></param>
//        /// <returns></returns>
//        public string GetCandidateIdByRecommendation(int recommendationId)
//        {
//            return _recomendationRepository.GetRecommendationById(recommendationId)
//                .CandidateId;
//        }

//        /// <summary>
//        /// 根据单位ID获取单位所有用户ID
//        /// </summary>
//        /// <param name="instId"></param>
//        /// <returns></returns>
//        public List<string> GetUserIdListByInstId(int instId)
//        {
//            return _userRepository.GetUserIdListByInstId(instId);
//        }

//        /// <summary>
//        /// 获取所有用户的Id
//        /// </summary>
//        /// <returns></returns>
//        public List<string> GetAllUserIdList()
//        {
//            return _userRepository.GetAllUserIdList();
//        }
//    }
//}