var applyRejectApp = angular.module('ngApplyRejectApp', ['publicFunction']);

applyRejectApp.directive('principalDetail', function () {
    return {
        restrict: 'E',
        templateUrl: 'Views\\divisionAdmin\\applicationManagement\\principalDetail.html'
    }
});

applyRejectApp.controller('ngApplyRejectCtrl', ['$scope', '$http', '$downloadFile', function ($scope, $http, $downloadFile) {
    var rejectApplyListUrl = "/api/instapplication/reject/";//Get已驳回申请书的Url
    var projectTypeUrl = "/api/ProjectType";//Get获取分类的类别
    var leaderInfoUrl = "/api/person/";//Get负责人信息的Url
    var rejectReasonUrl = "/api/applicationlog/";//Get已驳回申请书-驳回理由的Url  
    
    $scope.downloadPDFUrl = "/api/applicationdoc/download/pdf/";//下载PDF的Url

    // ********************初始化数据********************
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
        var ApplyListUrl = rejectApplyListUrl + $scope.selectValue + "/" + page;
        $http.get(ApplyListUrl)
            .success(function (response) {
                if (response.response != null) {
                    $scope.rejectApplies = response.response.ItemDTOs;
                    $scope.pageItem = response.response.TotalPageNum;//总共多少页
                    $scope.count = response.response.TotalNum; //页面总共多少条
                    $scope.p_current = response.response.NowPage; //当前第几页
                    $scope.p_count = response.response.NowNum; //当前页面多少条
                    $scope.pages = [];
                    for (var i = 0; i < $scope.pageItem; i++) {
                        $scope.pages[i] = i + 1;
                    }
                    if ($scope.rejectApplies.length == 0) {
                        $scope.divisionAppliesIsNull = true;
                        $scope.listAlready = true;
                    } else {
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
    $scope.principalDetail = function (rejectApply) {
        var personId = rejectApply.LeaderId;
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

    //	 ***************驳回原因查看-模态框*****************
    $scope.showRejectReasonModal = function (rejectApply) {
        var reasonUrl = rejectReasonUrl + rejectApply.ApplicationId + "/8";
        //get
        //获取申请书驳回理由
        $http.get(reasonUrl)
            .success(function (response) {
                $scope.rejectReson = response.response[0];
            })
            .error(function () {
                notif({
                    msg: "<b>错误:</b> 信息获取失败，请检查网络是否通畅",
                    type: "error"
                });
            });    
        $('#bhInfo').modal("show");
    }
}]);