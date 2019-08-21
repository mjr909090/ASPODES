using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using log4net.Repository.Hierarchy;

namespace ASPODES.WebAPI
{
    public class SystemConfig
    {
        //public static int ApplicationStartYear = 2017;

        public static string ProjectPathWin = "View\\Upload\\Project";
        public static string ProjectPathWeb = "View/Upload/Project";
        public static string AnnualTaskPathWin = "View\\Upload\\AnnualTask";
        public static string AnnualTaskPathWeb = "View/Upload/AnnualTask";
        public static string UploadFilePathWeb = "View/Upload";
        public static string UploadFilePathWin = "View\\Upload\\";
        /// <summary>
        /// 管理员上传评分说明的文件地址
        /// </summary>
        public static string UploadFileReview = "View\\Upload\\Review\\";
        public static string UploadFileReviewPathWeb = "/View/Upload/Review/";
        /// <summary>
        /// 存放临时压缩文件的地址
        /// </summary>
        public static string ZipFileAddress = "/View/Upload/Zip/";
        public static string DeleteZipFileAddress = "View\\Upload\\Zip\\";
        public static string ConsultationPath = "View\\Upload\\Consultation";
        public static string ExportReviewCommentPath = "View\\Upload\\ReviewComment";
        public static string ExportReviewCommentExcelTemplate = "View\\Upload\\ReviewComment\\ReviewComment.xls";
        public static string ExportConsultationExcelTemplate = "View\\Upload\\Consultation\\Consultation.xls";
        public static int ApplicationFieldAmount = 2;
        public static int ExpertFieldAmount = 2;
        /// <summary>
        /// 存放要导出的excel表格的地址
        /// </summary>
        public static string ExportExcel = "View\\Upload\\Export\\";
        //public static string SMTPHost = "smtp.ouc.edu.cn";
        public static string SMTPHost = "smtp.163.com";
        public static string SMTPUserName = "hdcaohaowei";
        public static string SenderAddress = "hdcaohaowei@163.com";
        public static string SMTPPassword = "cao321";

        public static bool AutoAssignmentExpert
        {
            get
            {
                return TryGetValueFromConfig<bool>( bool.Parse, ()=>false);
            }
        }
        /// <summary>
        /// 用户提交开始时间
        /// </summary>
        public static DateTime ApplicationSubmitBeginTime
        {
            get
            {
                return TryGetValueFromConfig(DateTime.Parse, () => new DateTime(2017, 7, 1, 00, 00, 00));                                
            }
        }

        /// <summary>
        /// 用户提交截止日期
        /// </summary>
        private static DateTime _applicationDeadline = new DateTime( 2017, 10, 30);
        public static DateTime ApplicationSubmitDeadline
        {
            get
            {
                return TryGetValueFromConfig(DateTime.Parse, () => new DateTime(2017, 10, 30, 23, 59, 59));                
            }
        }

        /// <summary>
        /// 单位管理员审核截止日期
        /// </summary>
        public static DateTime ApplicationVerifyDeadline
        {
            get
            {
                return TryGetValueFromConfig(DateTime.Parse, () => new DateTime(2017, 11, 20, 23, 59, 59));                
            }
        }

        /// <summary>
        /// 专家评分截止日期
        /// </summary>
        public static DateTime ApplicationExpertDeadline
        {
            get
            {
                return TryGetValueFromConfig(DateTime.Parse, () => new DateTime(2017,11,30,23,59,59));
            }
        }

        
        /// <summary>
        /// 项目开始年
        /// </summary>
        public static int ApplicationStartYear
        {
            get
            {
                return TryGetValueFromConfig(int.Parse, () => 2017);
            }
        }
        
        /// <summary>
        /// 初审专家评审的申请书数目
        /// </summary>
        private static int _expertReviewAmount = 5;
        public static int ExpertReviewAmount
        {
            get { return _expertReviewAmount; }
            set { _expertReviewAmount = value; }
        }

        //每份申请书的评审专家数目
        private static int _applicationExpertAmount = 5;
        public static int ApplicationExpertAmount
        {
            get { return _applicationExpertAmount; }
            set { _applicationExpertAmount = value; }
        }

        /// <summary>
        /// 人员分页，每页数目
        /// </summary>
        private static int _personPageCount = 20;
        public static int PersonPageCount 
        {
            get { return _personPageCount; }
            set { _personPageCount = value; }
        }

        /// <summary>
        /// 申请书每页的数目
        /// </summary>
        private static int _applicationPageCount = 20;
        public static int ApplicationPageCount
        {
            get { return _applicationPageCount; }
            set { _applicationPageCount = value; }
        }

        /// <summary>
        /// 项目每页的数目
        /// </summary>
        private static int _projectPageCount = 20;
        public static int ProjectPageCount
        {
            get { return _projectPageCount; }
            set { _projectPageCount = value; }
        }

        /// <summary>
        /// 通知每页的数目
        /// </summary>
        private static int _noticePageCount = 20;
        public static int NoticePageCount
        {
            get { return _noticePageCount; }
            set { _noticePageCount = value; }
        }

        /// <summary>
        /// 专家指派结果页面每页条目数
        /// </summary>
        private static int _reviewAssignmentPageCount = 20;
        public static int ReviewAssignmentPageCount
        {
            get { return _reviewAssignmentPageCount; }
            set { _reviewAssignmentPageCount = value; }
        }

        /// <summary>
        /// 专家推荐的每页显示条目数
        /// </summary>
        private static int _recommendationPageCount = 20;
        public static int RecommendationPageCount 
        {
            get { return _recommendationPageCount; }
            set { _recommendationPageCount = value; }
        }

        /// <summary>
        /// 评审意见每页显示条目数
        /// </summary>
        private static int _reviewCommentPageCount = 20;
        public static int ReviewCommentPageCount
        {
            get { return _reviewCommentPageCount; }
            set{_reviewCommentPageCount = value;}
        }

        /// <summary>
        /// 每页显示年度任务数目
        /// </summary>
        private static int _annualTaskPageCount = 20;
        public static int AnnualTaskPageCount
        {
            get { return _announcementPageCount; }
            set { _announcementPageCount = value; }
        }

        /// <summary>
        /// 每页显示公告数目
        /// </summary>
        private static int _announcementPageCount = 20;
        public static int AnnouncementPageCount
        {
            get { return _announcementPageCount; }
            set { _announcementPageCount = value; }
        }

        private static T TryGetValueFromConfig<T>(Func<string, T> parseFunc, Func<T> defaultTValueFunc,
            [CallerMemberName]string key = "", string supressKey = "")
        {
            try
            {
                if (!supressKey.IsNullOrWhiteSpace())
                {
                    key = supressKey;
                }

                var node = ConfigurationManager.AppSettings[key];
                return !string.IsNullOrEmpty(node) ? parseFunc(node) : defaultTValueFunc();
            }
            catch (Exception ex)
            {
                LogHelper.Error(string.Format("Error Reading web.config on AppSettings node: {0},EROR:{1}", key, ex.Message));
                return default(T);
            }
        }


    }
}