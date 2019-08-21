var auditAssignBookApp = angular.module('ngAuditAssignBookApp', ['publicFunction']);

auditAssignBookApp.directive('principalDetail', function () {
    return {
        restrict: 'E',
        templateUrl: 'Views/divisionAdmin/applicationManagement/principalDetail.html'
    }
});

auditAssignBookApp.controller('ngSuperAuditAssignBookCtrl', ['$scope', '$http', '$downloadFile', function ($scope, $http, $downloadFile) {
    var assignBookUrl = "/api/annualTask/departAdmin/review";//Get获取列表
    var leaderInfoUrl = "/api/person/";//Get负责人信息的Url

    var downloadPDFUrl = '/api/annualTaskDoc';
    var downloadProjectDocUrl = '/api/projectdoc/download/';
    var downloadEndReportUrl = '/api/projectdoc/download/endreport/';

    var reviewAssignBookUrl = "/api/annualTask/departReview/"; //put pass=true提交任务书;false驳回

    //init vary
    $scope.listAlready = false;
    $scope.listIsNull = true;

    //get assignBookList
    $scope.load_page = function (page) {
        $scope.listAlready = false;

        $http({
            method: 'GET',
            url: assignBookUrl,
            params: {
                status: 3,
                page: page
            }
        })
        .success(function (response) {
            if (response.response != null) {
                $scope.listAlready = true;
                $scope.assignBookList = response.response.ItemDTOs;
                $scope.pageItem = response.response.TotalPageNum;//总共多少页
                $scope.count = response.response.TotalNum; //页面总共多少条
                $scope.p_current = response.response.NowPage; //当前第几页
                $scope.p_count = response.response.NowNum; //当前页面多少条
                $scope.pages = [];
                for (var i = 0; i < $scope.pageItem; i++) {
                    $scope.pages[i] = i + 1;
                }
                if ($scope.assignBookList.length == 0) {
                    $scope.listIsNull = true;
                } else {
                    $scope.listIsNull = false;
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

    //page loading send the request
    $scope.load_page(1);

    //leaderInfo
    $scope.principalDetail = function (leaderId) {
        var detailUrl = leaderInfoUrl + leaderId;
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
    $scope.downloadPDF = function (AnnualTaskId, taskName) {
        var taskPDFUrl = "/api/annualTaskDoc?AnnualTaskId=" + AnnualTaskId + "&docType=3";
        $downloadFile.downloadFileType(taskPDFUrl, '.pdf', taskName);
    }

    $scope.showPassedConfirmModal = function (taskId) {
        $("#passedModal").modal("show");
        $scope.passedId = taskId;
    }
    //passed the assginBook
    $scope.passedAssignBook = function (id) {
        $http({
            method: 'PUT',
            url: reviewAssignBookUrl + id,
            params: {
                pass:true
            }
        })
        .success(function (response) {
            if (response.status == 0) {
                notif({
                    msg: "<b>恭喜:</b> 任务书审核通过",
                    type: "success"
                });
                $scope.load_page($scope.p_current); //refresh
            }
        })
        .error(function () {
            notif({
                msg: "<b>错误:</b> 任务书提交失败，请检查网络是否通畅",
                type: "error"
            });
        });
    }

    $scope.showRejectConfirmModal = function (taskId) {
        $("#rejectModal").modal("show");
        $scope.rejectId = taskId;
    }
    //reject the AssignBook
    $scope.rejectAssignBook = function (id) {
        $http({
            method: 'PUT',
            url: reviewAssignBookUrl + id,
            params: {
                pass: false
            },
            data: $scope.rejectReason
        })
         .success(function (response) {
             if (response.status == 0) {
                 notif({
                     msg: "<b>提示:</b> 该任务书已驳回至项目负责人处",
                     type: "success"
                 });
                 $scope.load_page($scope.p_current); //refresh
             }
         })
        .error(function () {
            notif({
                msg: "<b>错误:</b> 任务书驳回失败，请检查网络是否通畅",
                type: "error"
            });
        });
    }
}]);