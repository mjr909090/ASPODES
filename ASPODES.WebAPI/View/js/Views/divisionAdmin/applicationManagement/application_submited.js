var applySubmitApp = angular.module('ngApplySubmitApp', ['publicFunction']);

applySubmitApp.directive('principalDetail', function () {
    return {
        restrict: 'E',
        templateUrl: 'Views\\divisionAdmin\\applicationManagement\\principalDetail.html'
    }
});

applySubmitApp.controller('ngApplySubmitCtrl', ['$scope', '$http', '$downloadFile', function ($scope, $http, $downloadFile) {
    var acceptApplyListUrl = "/api/instapplication/submit/";//Get已通过申请书的Url
    var projectTypeUrl = "/api/ProjectType";//Get获取分类的类别
    var leaderInfoUrl = "/api/person/";//Get负责人信息的Url
    var rejectReasonUrl = "/api/applicationlog/";//Get负责人信息的Url
    var reviewsUrl = "/api/reviewComment/user/";//Get 评审意见信息 的Url
    var backApplicationUrl = "/api/instapplication/cancel/";//Put-撤回的Url 

    $scope.downloadPDFUrl = "/api/applicationdoc/download/pdf/";//下载PDF的Url

    $scope.exportInstSubmitedListUrl = "/api/instapplication/exportAccept"; // 导出列表的url

    // 定义
    $scope.exportInstSubmitedListFields = {};
    $scope.exportInstSubmitedListName = "已通过申请书";

    // ********************初始化json数据********************
    $scope.listAlready = false;
    $scope.divisionAppliesIsNull = true;

    $scope.selectValue = null;

    //获取分类
    $scope.getProjectType = function () {
        $http.get(projectTypeUrl)
            .success(function (response) {
                $scope.projectTypeList = response.response;
            })
        .error(function (response) {
            notif({
                msg: "<b>错误:</b> 列表获取失败，请检查网络是否通畅",
                type: "error"
            });
        })
    }
    $scope.getProjectType();

    // *******************获取列表&分页**************************
    //选择某一页时
    $scope.load_page = function (page) {
        if ($scope.selectValue == null) {
            $scope.selectValue = "0";
        }
        var ApplyListUrl = acceptApplyListUrl + $scope.selectValue + "/" + page;
        $http.get(ApplyListUrl)
            .success(function (response) {
                if (response.response != null) {
                    $scope.acceptApplies = response.response.ItemDTOs;
                    $scope.pageItem = response.response.TotalPageNum;//总共多少页
                    $scope.count = response.response.TotalNum; //页面总共多少条
                    $scope.p_current = response.response.NowPage; //当前第几页
                    $scope.p_count = response.response.NowNum; //当前页面多少条
                    $scope.pages = [];
                    for (var i = 0; i < $scope.pageItem; i++) {
                        $scope.pages[i] = i + 1;
                    }
                    if ($scope.acceptApplies.length == 0) {
                        $scope.divisionAppliesIsNull = true;
                        $scope.listAlready = true;
                    }
                    if ($scope.acceptApplies.length != 0) {
                        $scope.divisionAppliesIsNull = false;
                        $scope.listAlready = true;
                    }
                }
            })
            .error(function () {
                notif({
                    msg: "<b>错误:</b> 页面加载失败，请检查网络是否通畅",
                    type: "error"
                });
            });
    }

    //初始化获取列表
    $scope.load_page(1);

    //	 ***************负责人具体信息-模态框*****************
    $scope.principalDetail = function (acceptApply) {
        var personId = acceptApply.LeaderId;
        var detailUrl = leaderInfoUrl + personId;
        $http.get(detailUrl)
		        .success(function (response) {
		            $scope.modalData = response.response;
		        })
		        .error(function () {
		            notif({
		                msg: "<b>错误:</b> 负责人信息获取失败，请检查网络是否通畅",
		                type: "error"
		            });
		        });        
    }

    //	 ***************下载PDF-模态框*****************
    $scope.downloadPDF = function (applicationId, projectName) {
        $downloadFile.downloadFile($scope.downloadPDFUrl, applicationId, projectName);
    }

    //查看院不受理原因
    $scope.lookRejectReason = function (acceptApply) {
        var reasonUrl = rejectReasonUrl + acceptApply.ApplicationId + "/9";
        //get
        //获取申请书驳回理由
        $http.get(reasonUrl)
            .success(function (response) {
                $scope.rejectReson = response.response[0];
            })
            .error(function () {
                notif({
                    msg: "<b>错误:</b> 驳回信息获取失败，请检查网络是否通畅",
                    type: "error"
                });
            });
        $('#bhInfo').modal("show");
    }

    //	 ***************查看评审结果*****************
    $scope.lookExpertReviews = function (acceptApply) {
        var lookReviewsUrl = reviewsUrl + acceptApply.ApplicationId;
        //get
        //获取申请书驳回理由
        $http.get(lookReviewsUrl)
            .success(function (response) {
                $scope.reviewComments = response.response.ReviewComments;

            })
            .error(function () {
                notif({
                    msg: "<b>错误:</b> 评审意见获取失败，请检查网络是否通畅",
                    type: "error"
                });
            });
        $('#resultModal').modal("show");
    }

    //	 ***************撤回-操作*****************
    $scope.showBackApplicationModal = function (acceptApply) {
        $scope.modalData = acceptApply;
        $('#backApplication').modal("show");
    }
    $scope.backApply = function (applicationId) {
        var backApplyUrl = backApplicationUrl + applicationId;

        $http.put(backApplyUrl)
            .success(function (response) {
                if (response.status == 0) {
                    $('#backApplication').modal("hide");
                    notif({
                        msg: "<b>恭喜:</b> 撤回成功，申请书已退回到申请人处",
                        type: "success"
                    });
                    $scope.load_page($scope.p_current);
                } else {
                    notif({
                        msg: "<b>错误:</b>" + response.errorMsg,
                        type: "error"
                    });
                }
            })
            .error(function () {
                notif({
                    msg: "<b>错误:</b> 撤回失败，请检查网络是否通畅",
                    type: "error"
                });
            });
    }

    $scope.exportDivisionSubmitedList = function (projectType) {
        if (projectType == undefined) {
            $scope.projectType = 0;
        } else {
            $scope.projectType = projectType;
        }
        $scope.exportInstSubmitedListFields = {
            "Order": "序号",
            "Name": "申请书名称",
            "Deleage": "委托",
            "Category": "类型",
            "Leader": "牵头负责人",
            "Status" : "状态",
            "CategoryId": $scope.projectType
        };
        $downloadFile.exportApplication($scope.exportInstSubmitedListUrl, $scope.exportInstSubmitedListFields, $scope.exportInstSubmitedListName);
    };
}]);