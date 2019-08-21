/// <reference path="home.js" />
var indexApp = angular.module('ngIndexApp', ['ngAnimate', 'ui.router', 'oc.lazyLoad', 'ngFileUpload', 'ngMessages', 'ngCookies', 'angucomplete-alt', 'setDebuggerService', 'selfPagination', 'multiPagination', 'interceptorFactory']);

indexApp.config(["$provide", "$compileProvider", "$controllerProvider", "$filterProvider",
    function ($provide, $compileProvider, $controllerProvider, $filterProvider, $cookieStore) {
        indexApp.controller = $controllerProvider.register;
        indexApp.directive = $compileProvider.directive;
        indexApp.filter = $filterProvider.register;
        indexApp.factory = $provide.factory;
        indexApp.service = $provide.service;
        indexApp.constant = $provide.constant;
    }
]);


indexApp.constant('Modules_Config', [{
    serie: true,
    files: [
        "plugins/echarts.min.js"
    ]
}]);


indexApp.config(["$ocLazyLoadProvider", "Modules_Config", routeFn]);

indexApp.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptor');
})

function routeFn($ocLazyLoadProvider, Modules_Config) {
    $ocLazyLoadProvider.config({
        debug: false,
        events: false,
        modules: Modules_Config
    });
};


indexApp.config(["$stateProvider",
	function ($stateProvider) {
	    $stateProvider
            /****************************************首页******************************************************/
            //欢迎页
            .state('home', {
                url: '/home',
                templateUrl: 'Views/home.html',
                controller: "ngHomeCtrl",
                resolve: {
                    deps: ["$ocLazyLoad", function ($ocLazyLoad) {
                        return $ocLazyLoad.load("js/Views/home.js");
                    }]
                }
            })
            //统计
            .state('statistics', {
                url: '/statistics',
                templateUrl: 'Views/statistics.html',
                controller: "ngStatisticsCtrl",
                resolve: {
                    deps: ["$ocLazyLoad", function ($ocLazyLoad) {
                        return $ocLazyLoad.load("js/Views/statistics.js");
                    }]
                }
            })
            //申请书查询
            .state('applicationInquire', {
                url: '/applicationInquire',
                templateUrl: 'Views/applicationInquire.html',
                controller: "ngApplicationInquireCtrl",
                resolve: {
                    deps: ["$ocLazyLoad", function ($ocLazyLoad) {
                        return $ocLazyLoad.load("js/Views/applicationInquire.js");
                    }]
                }
            })
            //单位公告
            .state('instAnnounce', {
                url: '/instAnnounce',
                templateUrl: 'Views/announce/instAnnounce.html',
                controller: "ngInstAnnounceCtrl",
                resolve: {
                    deps: ["$ocLazyLoad", function ($ocLazyLoad) {
                        return $ocLazyLoad.load("js/Views/announce/instAnnounce.js");
                    }]
                }
            })
            //全院公告
            .state('departAnnounce', {
                url: '/departAnnounce',
                templateUrl: 'Views/announce/departAnnounce.html',
                controller: "ngDepartAnnounceCtrl",
                resolve: {
                    deps: ["$ocLazyLoad", function ($ocLazyLoad) {
                        return $ocLazyLoad.load("js/Views/announce/departAnnounce.js");
                    }]
                }
            })


			/****************************************申请人******************************************************/
			//申请书草稿
			.state('applicationSaved', {
			    url: '/applicationSaved',
			    templateUrl: 'Views/proposer/myApplications/application_saved.html',
			    controller: "ngApplicationSavedCtrl",
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/proposer/myApplications/application_saved.js");
			        }]
			    }
			})
			//已提交申请书
			.state('applicationSubmit', {
			    url: '/applicationSubmit',
			    templateUrl: 'Views/proposer/myApplications/application_submited.html',
			    controller: 'ngApplicationSubmitCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/proposer/myApplications/application_submited.js");
			        }]
			    }
			})
			//既往申请书
			.state('applicationInBase', {
			    url: '/applicationInBase',
			    templateUrl: 'Views/proposer/myApplications/application_inBase.html',
			    controller: 'ngApplicationInBaseCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/proposer/myApplications/application_inBase.js");
			        }]
			    }
			})
			//我主持的项目
			.state('project_host', {
			    url: '/project_host',
			    templateUrl: 'Views/proposer/myProject/project_host.html',
			    controller: 'ngProjectHostCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/proposer/myProject/project_host.js");
			        }]
			    }
			})
			//我参与的项目
			.state('project_partake', {
			    url: '/project_partake',
			    templateUrl: 'Views/proposer/myProject/project_partake.html',
			    controller: 'ngProjectPartakeCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/proposer/myProject/project_partake.js");
			        }]
			    }
			})
			//被驳回的结题申请
			.state('conclusion_rejected', {
			    url: '/conclusion_rejected',
			    templateUrl: 'Views/proposer/myProject/conclusion_rejected.html',
			    controller: 'ngConclusionRejectedCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/proposer/myProject/conclusion_rejected.js");
			        }]
			    }
			})
			//被驳回的任务书
			.state('assignment_rejected', {
			    url: '/assignment_rejected',
			    templateUrl: 'Views/proposer/myProject/assignment_rejected.html',
			    controller: 'ngAssignmentRejectedCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/proposer/myProject/assignment_rejected.js");
			        }]
			    }
			})
			//被驳回的文档
			.state('document_rejected', {
			    url: '/document_rejected',
			    templateUrl: 'Views/proposer/myProject/document_rejected.html',
			    controller: 'ngDocumentRejectedCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/proposer/myProject/document_rejected.js");
			        }]
			    }
			})
			/****************************************专家******************************************************/
			//待评审申请书
			.state('preReview', {
			    url: '/preReview',
			    templateUrl: 'Views/Expert/ApplicationReview/preReview.html',
			    controller: 'ngReviewCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/Expert/ApplicationReview/preReview.js");
			        }]
			    }
			})
			//既往评审记录
			.state('finishedReview', {
			    url: '/finishedReview',
			    templateUrl: 'Views/Expert/FinishedReview/finishedReview.html',
			    controller: 'ngFinishedReviewCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/Expert/FinishedReview/finishedReview.js");
			        }]
			    }
			})
            //本年度评审记录
			.state('currentReview', {
			    url: '/currentReview',
			    templateUrl: 'Views/Expert/CurrentReview/currentReview.html',
			    controller: 'ngCurrentReviewCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/Expert/CurrentReview/currentReview.js");
			        }]
			    }
			})
			//项目信息
			.state('projectInfo', {
			    url: '/projectInfo',
			    templateUrl: 'Views/Expert/ProjectInfo/projectInfo.html',
			    controller: 'ngProjectInfoCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/Expert/projectInfo/projectInfo.js");
			        }]
			    }
			})
			//专家信息
			.state('expertInfo', {
			    url: '/expertInfo',
			    templateUrl: 'Views/Expert/ExpertInfo/expertInfo.html',
			    controller: 'ngExpertInfoCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/Expert/ExpertInfo/expertInfo.js");
			        }]
			    }
			})
			/****************************************单位管理员******************************************************/
			//待审核申请书
			.state('divisionApplication', {
			    url: '/divisionApplication',
			    templateUrl: 'Views/divisionAdmin/applicationManagement/division_application.html',
			    controller: 'ngApplyUnitCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/divisionAdmin/applicationManagement/division_application.js");
			        }]
			    }
			})
			//已驳回申请书
			.state('divisionApplicationRejected', {
			    url: '/divisionApplicationRejected',
			    templateUrl: 'Views/divisionAdmin/applicationManagement/application_rejected.html',
			    controller: 'ngApplyRejectCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/divisionAdmin/applicationManagement/application_rejected.js");
			        }]
			    }
			})
			//已通过申请书
			.state('divisionApplicationSubmit', {
			    url: '/divisionApplicationSubmit',
			    templateUrl: 'Views/divisionAdmin/applicationManagement/application_submited.html',
			    controller: 'ngApplySubmitCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/divisionAdmin/applicationManagement/application_submited.js");
			        }]
			    }
			})
            //定向委托申请书
			.state('divisionDirectedDelegation', {
			    url: '/divisionDirectedDelegation',
			    templateUrl: 'Views/divisionAdmin/applicationManagement/delegation_application.html',
			    controller: 'ngDelegationApplicationCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/divisionAdmin/applicationManagement/delegation_application.js");
			        }]
			    }
			})
			//既往申请书
			.state('finishedApplication', {
			    url: '/finishedApplication',
			    templateUrl: 'Views/divisionAdmin/applicationManagement/finished_application.html',
			    controller: 'ngFinishedApplicationCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/divisionAdmin/applicationManagement/finished_application.js");
			        }]
			    }
			})
			//本单位主持项目
			.state('divisionHost', {
			    url: '/divisionHost',
			    templateUrl: 'Views/divisionAdmin/projectManage/division_host.html',
			    controller: 'ngDivisionHostCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/divisionAdmin/projectManage/division_host.js");
			        }]
			    }
			})
			//本单位参与项目
			.state('divisionParticipate', {
			    url: '/divisionParticipate',
			    templateUrl: 'Views/divisionAdmin/projectManage/division_participate.html',
			    controller: 'ngDivisionPaticipateCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/divisionAdmin/projectManage/division_participate.js");
			        }]
			    }
			})
			//待审核任务书
			.state('auditAssignBook', {
			    url: '/auditAssignBook',
			    templateUrl: 'Views/divisionAdmin/projectManage/audit_assignBook.html',
			    controller: 'ngAuditAssignBookCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/divisionAdmin/projectManage/audit_assignBook.js");
			        }]
			    }
			})
			//待审核文档
			.state('auditDocument', {
			    url: '/auditDocument',
			    templateUrl: 'Views/divisionAdmin/projectManage/audit_document.html',
			    controller: 'ngAuditDocumentCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/divisionAdmin/projectManage/audit_document.js");
			        }]
			    }
			})
            //待审中期报告
			.state('auditMidTermReport', {
			    url: '/auditMidTermReport',
			    templateUrl: 'Views/divisionAdmin/projectManage/audit_midtermReport.html',
			    controller: 'ngAuditMidtermReportCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/divisionAdmin/projectManage/audit_midtermReport.js");
			        }]
			    }
			})
            //待审核年度总结
			.state('auditAnnualSummary', {
			    url: '/auditAnnualSummary',
			    templateUrl: 'Views/divisionAdmin/projectManage/audit_annualSummary.html',
			    controller: 'ngAuditAnnualSummaryCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/divisionAdmin/projectManage/audit_annualSummary.js");
			        }]
			    }
			})
			//待审核结题申请
			.state('auditConcludeReport', {
			    url: '/auditConcludeReport',
			    templateUrl: 'Views/divisionAdmin/projectManage/audit_concludeReport.html',
			    controller: 'ngAuditConcludeReportCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/divisionAdmin/projectManage/audit_concludeReport.js");
			        }]
			    }
			})
			//用户列表
			.state('divisionUserList', {
			    url: '/divisionUserList',
			    templateUrl: 'Views/divisionAdmin/staffManagement/userList.html',
			    controller: 'ngUserListCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/divisionAdmin/staffManagement/userList.js");
			        }]
			    }
			})
			//批量导入
			.state('batchImport', {
			    url: '/batchImport',
			    templateUrl: 'Views/divisionAdmin/staffManagement/batchImport.html',
			    controller: 'ngBatchImportCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/divisionAdmin/staffManagement/batchImport.js");
			        }]
			    }
			})
			//已删除用户
			.state('userDelete', {
			    url: '/userDelete',
			    templateUrl: 'Views/divisionAdmin/staffManagement/user_deleted.html',
			    controller: 'ngUserDeletedCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/divisionAdmin/staffManagement/user_deleted.js");
			        }]
			    }
			})
			//专家列表
			.state('expertsList', {
			    url: '/expertsList',
			    templateUrl: 'Views/divisionAdmin/expert/expertsList.html',
			    controller: 'ngExpertsListCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/divisionAdmin/expert/expertsList.js");
			        }]
			    }
			})
			//已推荐专家
			.state('expertsRecommend', {
			    url: '/expertsRecommend',
			    templateUrl: 'Views/divisionAdmin/expert/experts_recommend.html',
			    controller: 'ngERCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/divisionAdmin/expert/experts_recommend.js");
			        }]
			    }
			})
			//申请期限设置
			.state('timeSetting', {
			    url: '/timeSetting',
			    templateUrl: 'Views/divisionAdmin/timeSetting/timeSetting.html',
			    controller: 'ngTimeSettingCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/divisionAdmin/timeSetting/timeSetting.js");
			        }]
			    }
			})
			//单位信息维护
			.state('infoMaintain', {
			    url: '/infoMaintain',
			    templateUrl: 'Views/divisionAdmin/infoMaintain/infoMaintain.html',
			    controller: 'ngInfoMaintainCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/divisionAdmin/infoMaintain/infoMaintain.js");
			        }]
			    }
			})
			//通知
			.state('inform', {
			    url: '/inform',
			    templateUrl: 'Views/divisionAdmin/inform/inform.html',
			    controller: 'ngInformCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/divisionAdmin/inform/inform.js");
			        }]
			    }
			})
            //公告
			.state('announcement', {
			    url: '/announcement',
			    templateUrl: 'Views/divisionAdmin/announcement/announcement.html',
			    controller: 'ngAnnouncementCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/divisionAdmin/announcement/announcement.js");
			        }]
			    }
			})
			/****************************************院管理员******************************************************/
			//待受理申请书
			.state('applicationUnhandled', {
			    url: '/applicationUnhandled',
			    templateUrl: 'Views/superAdmin/applicationManagement/application_unhandled.html',
			    controller: 'ngApplication_UnhandledCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/applicationManagement/application_unhandled.js");
			        }]
			    }
			})
			//已受理申请书
			.state('applicationAccepted', {
			    url: '/applicationAccepted',
			    templateUrl: 'Views/superAdmin/applicationManagement/application_accepted.html',
			    controller: 'ngApplication_AcceptedCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/applicationManagement/application_accepted.js");
			        }]
			    }
			})
            //定向委托申请书
			.state('directedDelegation', {
			    url: '/directedDelegation',
			    templateUrl: 'Views/superAdmin/applicationManagement/application_directedDelegation.html',
			    controller: 'ngDirectedDelegationCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/applicationManagement/application_directedDelegation.js");
			        }]
			    }
			})
			//不受理申请书
			.state('applicationRejeccted', {
			    url: '/applicationRejeccted',
			    templateUrl: 'Views/superAdmin/applicationManagement/application_rejected.html',
			    controller: 'ngApplication_RejectedCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/applicationManagement/application_rejected.js");
			        }]
			    }
			})
			//既往申请书
			.state('applicationPassed', {
			    url: '/applicationPassed',
			    templateUrl: 'Views/superAdmin/applicationManagement/application_passed.html',
			    controller: 'ngApplication_PassedCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/applicationManagement/application_passed.js");
			        }]
			    }
			})
			//专家列表
			.state('expertList', {
			    url: '/expertList',
			    templateUrl: 'Views/superAdmin/expertManagement/expertList.html',
			    controller: 'ngExpertListCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/expertManagement/expertList.js");
			        }]
			    }
			})
			//专家推荐列表
			.state('exportRecommendAudited', {
			    url: '/exportRecommendAudited',
			    templateUrl: 'Views/superAdmin/expertManagement/expertRecommend_Audited.html',
			    controller: 'ngExpertReCommend_AuditedCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/expertManagement/expertRecommend_Audited.js");
			        }]
			    }
			})
			//专家指派
			.state('assignmentRecommendation', {
			    url: '/assignmentRecommendation',
			    templateUrl: 'Views/superAdmin/expertAssignment/assignment_recommendation.html',
			    controller: 'ngAssignment_RecommendationCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/expertAssignment/assignment_recommendation.js");
			        }]
			    }
			})
			//被拒绝指派
			.state('assignmentRefused', {
			    url: '/assignmentRefused',
			    templateUrl: 'Views/superAdmin/expertAssignment/assignment_refused.html',
			    controller: 'ngAssignment_RefusedCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/expertAssignment/assignment_refused.js");
			        }]
			    }
			})
			//初审结果
			.state('firstReviewResult', {
			    url: '/firstReviewResult',
			    templateUrl: 'Views/superAdmin/reviewManagement/firstReviewResult.html',
			    controller: 'ngFirstReviewResultCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/reviewManagement/firstReviewResult.js");
			        }]
			    }
			})
			//咨询审议结果
			.state('consultReviewResult', {
			    url: '/consultReviewResult',
			    templateUrl: 'Views/superAdmin/reviewManagement/consultReviewResult.html',
			    controller: 'ngConsultReviewResultCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/reviewManagement/consultReviewResult.js");
			        }]
			    }
			})
			//既往初审
			.state('firstReviewPassed', {
			    url: '/firstReviewPassed',
			    templateUrl: 'Views/superAdmin/reviewManagement/firstReview_passed.html',
			    controller: 'ngFirstReview_PassedCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/reviewManagement/firstReview_passed.js");
			        }]
			    }
			})
			//既往咨询审议
			.state('consultReviewPassed', {
			    url: '/consultReviewPassed',
			    templateUrl: 'Views/superAdmin/reviewManagement/consultReview_passed.html',
			    controller: 'ngConsultReview_PassedCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/reviewManagement/consultReview_passed.js");
			        }]
			    }
			})
			//分管主持项目
			.state('superAdminHost', {
			    url: '/superAdminHost',
			    templateUrl: 'Views/superAdmin/projectManage/superAdmin_host.html',
			    controller: 'ngSuperAdminHostCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/projectManage/superAdmin_host.js");
			        }]
			    }
			})
			//分管参与项目
			.state('superAdminParticipate', {
			    url: '/superAdminParticipate',
			    templateUrl: 'Views/superAdmin/projectManage/superAdmin_participate.html',
			    controller: 'ngDivisionPaticipateCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/projectManage/superAdmin_participate.js");
			        }]
			    }
			})
			//待审核任务书
			.state('superAdmin_auditAssignBook', {
			    url: '/superAdmin_auditAssignBook',
			    templateUrl: 'Views/superAdmin/projectManage/audit_assignBook.html',
			    controller: 'ngSuperAuditAssignBookCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/projectManage/audit_assignBook.js");
			        }]
			    }
			})
			//待审核文档
			.state('superAdmin_auditDocument', {
			    url: '/superAdmin_auditDocument',
			    templateUrl: 'Views/superAdmin/projectManage/audit_document.html',
			    controller: 'ngAuditDocumentCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/projectManage/audit_document.js");
			        }]
			    }
			})
            //待审中期报告
			.state('superAdmin_auditMidTermReport', {
			    url: '/superAdmin_auditMidTermReport',
			    templateUrl: 'Views/superAdmin/projectManage/audit_midtermReport.html',
			    controller: 'ngAuditMidtermReportCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/projectManage/audit_midtermReport.js");
			        }]
			    }
			})
            //待审核年度总结
			.state('superAdmin_auditAnnualSummary', {
			    url: '/superAdmin_auditAnnualSummary',
			    templateUrl: 'Views/superAdmin/projectManage/audit_annualSummary.html',
			    controller: 'ngSuperAuditAnnualSummaryCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/projectManage/audit_annualSummary.js");
			        }]
			    }
			})
			//待审核结题报告
			.state('superAdmin_auditConcludeReport', {
			    url: '/superAdmin_auditConcludeReport',
			    templateUrl: 'Views/superAdmin/projectManage/audit_concludeReport.html',
			    controller: 'ngSuperAuditConcludeReportCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/projectManage/audit_concludeReport.js");
			        }]
			    }
			})
			//用户列表
			.state('userList', {
			    url: '/userList',
			    templateUrl: 'Views/superAdmin/staffManagement/userList.html',
			    controller: 'ngUserListDepartCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/staffManagement/userList.js");
			        }]
			    }
			})
			//单位管理员
			.state('divisionAdminManagement', {
			    url: '/divisionAdminManagement',
			    templateUrl: 'Views/superAdmin/staffManagement/divisionAdminManagement.html',
			    controller: 'ngDivisionAdminManagementCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/staffManagement/divisionAdminManagement.js");
			        }]
			    }
			})
			//院管理员
			.state('superAdminManagement', {
			    url: '/superAdminManagement',
			    templateUrl: 'Views/superAdmin/staffManagement/superAdminManagement.html',
			    controller: 'ngSuperAdminManagementCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/staffManagement/superAdminManagement.js");
			        }]
			    }
			})
			//文档模版
			.state('documentsTemplate', {
			    url: '/documentsTemplate',
			    templateUrl: 'Views/superAdmin/documentsManagement/documentsTemplate.html',
			    controller: 'ngDocumentsTemplateCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/documentsManagement/documentsTemplate.js");
			        }]
			    }
			})
			//提交说明
			.state('submitDiscription', {
			    url: '/submitDiscription',
			    templateUrl: 'Views/superAdmin/documentsManagement/submitDiscription.html',
			    controller: 'ngSubmitDiscriptionCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/documentsManagement/submitDiscription.js");
			        }]
			    }
			})
			//用户短信
			.state('userMessage', {
			    url: '/userMessage',
			    templateUrl: 'Views/superAdmin/messageCenter/userMessage.html',
			    controller: 'ngUserMessageCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/messageCenter/userMessage.js");
			        }]
			    }
			})
			//单位管理员短信
			.state('divisionAdminMessage', {
			    url: '/divisionAdminMessage',
			    templateUrl: 'Views/superAdmin/messageCenter/divisionAdminMessage.html',
			    controller: 'ngDivisionAdminMessageCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/messageCenter/divisionAdminMessage.js");
			        }]
			    }
			})
			//院管理员短信
			.state('superAdminMessage', {
			    url: '/superAdminMessage',
			    templateUrl: 'Views/superAdmin/messageCenter/superAdminMessage.html',
			    controller: 'ngSuperAdminMessageCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/messageCenter/superAdminMessage.js");
			        }]
			    }
			})
			//用户短信
			.state('userEmail', {
			    url: '/userEmail',
			    templateUrl: 'Views/superAdmin/emailCenter/userEmail.html',
			    controller: 'ngUserEmailCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/emailCenter/userEmail.js");
			        }]
			    }
			})
			//单位管理员短信
			.state('divisionAdminEmail', {
			    url: '/divisionAdminEmail',
			    templateUrl: 'Views/superAdmin/emailCenter/divisionAdminEmail.html',
			    controller: 'ngDivisionAdminEmailCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/emailCenter/divisionAdminEmail.js");
			        }]
			    }
			})
			//院管理员邮件
			.state('superAdminEmail', {
			    url: '/superAdminEmail',
			    templateUrl: 'Views/superAdmin/emailCenter/superAdminEmail.html',
			    controller: 'ngSuperAdminEmailCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/emailCenter/superAdminEmail.js");
			        }]
			    }
			})
			//通知公告
			.state('noticeList', {
			    url: '/noticeList',
			    templateUrl: 'Views/superAdmin/notice/noticeList.html',
			    controller: 'ngNoticeListCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/superAdmin/notice/noticeList.js");
			        }]
			    }
			})
			/****************************************系统管理员******************************************************/
            //服务器设置
			.state('serverSetting', {
			    url: '/serverSetting',
			    templateUrl: 'Views/systemAdmin/setting/serverSetting.html',
			    controller: 'ngServerSettingCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/systemAdmin/setting/serverSetting.js");
			        }]
			    }
			})
            //运行设置
			.state('runningSetting', {
			    url: '/runningSetting',
			    templateUrl: 'Views/systemAdmin/setting/runningSetting.html',
			    controller: 'ngRunningSettingCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/systemAdmin/setting/runningSetting.js");
			        }]
			    }
			})
            //院管理员设置
			.state('roleSetting', {
			    url: '/roleSetting',
			    templateUrl: 'Views/systemAdmin/setting/roleSetting.html',
			    controller: 'ngRoleSettingCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/systemAdmin/setting/roleSetting.js");
			        }]
			    }
			})
            //单位管理
			.state('divisionManagement', {
			    url: '/divisionManagement',
			    templateUrl: 'Views/systemAdmin/setting/divisionManagement.html',
			    controller: 'ngDivisionManagementCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/systemAdmin/setting/divisionManagement.js");
			        }]
			    }
			})
            //申报设置
			.state('applicationSetting', {
			    url: '/applicationSetting',
			    templateUrl: 'Views/systemAdmin/setting/applicationSetting.html',
			    controller: 'ngApplicationSettingCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/systemAdmin/setting/applicationSetting.js");
			        }]
			    }
			})
            //单位信息
			.state('divisionSetting', {
			    url: '/divisionSetting',
			    templateUrl: 'Views/systemAdmin/setting/divisionSetting.html',
			    controller: 'ngDivisionSettingCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/systemAdmin/setting/divisionSetting.js");
			        }]
			    }
			})
            //操作日志
			.state('operationLog', {
			    url: '/operationLog',
			    templateUrl: 'Views/systemAdmin/log/operationLog.html',
			    controller: 'ngOperationLogCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/systemAdmin/log/operationLog.js");
			        }]
			    }
			})
            //短信记录
			.state('messageLog', {
			    url: '/messageLog',
			    templateUrl: 'Views/systemAdmin/log/messageLog.html',
			    controller: 'ngMessageLogCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/systemAdmin/log/messageLog.js");
			        }]
			    }
			})
            //邮件记录
			.state('mailLog', {
			    url: '/mailLog',
			    templateUrl: 'Views/systemAdmin/log/mailLog.html',
			    controller: 'ngMailLogCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/systemAdmin/log/mailLog.js");
			        }]
			    }
			})
            //类别设置
			.state('typeSetting', {
			    url: '/typeSetting',
			    templateUrl: 'Views/systemAdmin/applicationSetting/typeSetting.html',
			    controller: 'ngTypeSettingCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/systemAdmin/applicationSetting/typeSetting.js");
			        }]
			    }
			})
            //类别设置
			.state('scoreDescription', {
			    url: '/scoreDescription',
			    templateUrl: 'Views/systemAdmin/applicationSetting/scoreDescriptionSetting.html',
			    controller: 'ngScoreDescriptionSettingCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/systemAdmin/applicationSetting/scoreDescriptionSetting.js");
			        }]
			    }
			})
            //申请书模板
			.state('applicationModal', {
			    url: '/applicationModal',
			    templateUrl: 'Views/systemAdmin/applicationSetting/applicationModel.html',
			    controller: 'ngApplicationModelCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/systemAdmin/applicationSetting/applicationModel.js");
			        }]
			    }
			})
            //申请书驳回理由配置
			.state('applicationRefusedReason', {
			    url: '/applicationRefusedReason',
			    templateUrl: 'Views/systemAdmin/noticeModel/applicationRejectReason.html',
			    controller: 'ngApplicationRejectReasonCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/systemAdmin/noticeModel/applicationRejectReason.js");
			        }]
			    }
			})
            //短信模板配置
			.state('messageModel', {
			    url: '/messageModel',
			    templateUrl: 'Views/systemAdmin/noticeModel/messageModel.html',
			    controller: 'ngMessageModelCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/systemAdmin/noticeModel/messageModel.js");
			        }]
			    }
			})
            //邮件模板配置
			.state('mailModel', {
			    url: '/mailModel',
			    templateUrl: 'Views/systemAdmin/noticeModel/mailModel.html',
			    controller: 'ngMailModelCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/systemAdmin/noticeModel/mailModel.js");
			        }]
			    }
			})
			/****************************************通知与设置******************************************************/
			//通知
			.state('notice-yuan', {
			    url: '/notice-yuan',
			    templateUrl: 'Views/notice/notice_yuan.html',
			    controller: 'ngNoticeYuanCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/notice/notice_yuan.js");
			        }]
			    }
			})
			//个人基本信息
			.state('personal_setting', {
			    url: '/personal_setting',
			    templateUrl: 'Views/setting/personal_setting.html',
			    controller: 'ngPersonalSettingCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/setting/personal_setting.js");
			        }]
			    }
			})
			//系统设置
			.state('system_setting', {
			    url: '/system_setting',
			    templateUrl: 'Views/setting/system_setting.html',
			    controller: 'ngSystemSettingCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/setting/system_setting.js");
			        }]
			    }
			})
			//修改密码
			.state('password_change', {
			    url: '/password_change',
			    templateUrl: 'Views/setting/password_change.html',
			    controller: 'ngPasswordChangeCtrl',
			    resolve: {
			        deps: ["$ocLazyLoad", function ($ocLazyLoad) {
			            return $ocLazyLoad.load("js/Views/setting/password_change.js");
			        }]
			    }
			})
	    /****************************************其它******************************************************/

	}
]);

indexApp.controller('ngIndexCtrl', function ($scope, $rootScope, $http, $cookieStore, $state, $setDebugger) {

    //url
    var getNoticeNumberUrl = "/api/number";
    var LogoutUrl = "/api/Logout";
    var getNoticeUrl = "/api/Notice/"
    var postReadNoticeUrl = "/api/Notice/"

    //设置是否显示调试信息
    $setDebugger.setDebuggerFn(false);

    $state.go('home');

    //判断某一元素是否在数组中
    Array.prototype.contains = function (obj) {
        var i = this.length;
        while (i--) {
            if (this[i] === obj) {
                return true;
            }
        }
        return false;
    }

    //确定角色侧边栏显示
    $scope.selectRoles = function () {
        if ($cookieStore.get('role') != null) {
            $scope.roles = $cookieStore.get('role');
            $scope.role.proposer = $scope.roles.contains('用户');
            $scope.role.expert = $scope.roles.contains('专家');
            $scope.role.divisionAdmin = $scope.roles.contains('单位管理员');
            $scope.role.superAdmin = $scope.roles.contains('院管理员');
            $scope.role.systemAdmin = $scope.roles.contains('系统管理员');
            $scope.roleSelects = angular.copy($scope.role);
        }
    }

    //注销
    $scope.Logout = function () {
        $http.put(LogoutUrl)
        .success(function (response) {
            window.self.location = "login.html";
        })
        .error(function (response) {
            window.self.location = "login.html";
        })
    }


    //获取侧边栏通知条数
    $scope.getNoticeNumber = function () {
        $http.get(getNoticeNumberUrl)
            .success(function (response) {
            $scope.noticeNumber = response.response;
        })
        .error(function (response) {

        })
    }

    // 获取标题栏通知
    $scope.getNotice = function () {
        $http.get(getNoticeUrl + 1 + "/" + 0 + "/" + 0)
            .success(function (response) {
                $scope.noticeList = response.response;
            })
            .error(function (response) {
            })
    }

    // 通知页面跳转
    $scope.skipPage = function (skipUrl) {
        $state.go(skipUrl);
    }

    // 标记通知为已读
    $scope.postReadNotice = function (noticeID) {
        $http.post(postReadNoticeUrl + noticeID)
            .success(function (response) {
                $scope.getNotice();
            })
            .error(function (response) {

            })
    }

    // 点击通知触发
    $scope.clickNotice = function (notice) {
        $scope.skipPage(notice.URL);
        $scope.postReadNotice(notice.NoticeID);
    }
    //router-state变化时触发
    $rootScope.$on('$stateChangeStart', function (event, toState, toParams, formState, fromeParams) {
        $scope.getNoticeNumber();
        $scope.getNotice(); 
    })

    //初始化
    $scope.hideProject = true;
    $scope.role = {
        "proposer": false,
        "expert": false,
        "divisionAdmin": false,
        "superAdmin": false,
        "systemAdmin": false
    }
    $scope.UserName = $cookieStore.get("UserName")
    $scope.selectRoles();
    $scope.getNoticeNumber();
    $scope.getNotice()
});
