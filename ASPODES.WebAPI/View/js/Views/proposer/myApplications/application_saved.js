var applicationSavedApp = angular.module('ngApplicationSavedApp', ['setDebuggerService', 'publicFunction']);

applicationSavedApp.controller('ngApplicationSavedCtrl', function ($scope, $http, $state, $setDebugger, $downloadFile) {

    $scope.listAlready = false;

    //URL
    var savedProposeListUrl = "/api/application/draft";
    var submitApplicationUrl = "/api/application/submit/"
    var deleteApplicationUrl = "/api/application/";
    var getReasonUrl = "/api/applicationlog/";

    $scope.downloadPDFUrl = "/api/applicationdoc/download/pdf/";//下载PDF的Url



    //get
    //获取申请书列表
    $scope.getSavedList = function () {
        $scope.listAlready = false;
        $http.get(savedProposeListUrl)
        .success(function (response) {
            if (response.response != null) {
                $scope.savedProposes = response.response;
                $scope.listAlready = true;
            }
            else {
                $scope.savedProposes = response.response;
                $scope.listAlready = true;
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "错误：" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
            $scope.listAlready = true;
        });
    }
    //刷新
    $scope.refresh = function () {
        $scope.listAlready = false;
        $scope.getSavedList();
    }
    

    $scope.saveApplicationId = function (ApplicationId) {
        $scope.postId = ApplicationId;
    }

    //修改申请书
    $scope.jumpToEditPage = function (ApplicationId) {
        $scope.editApplicationUrl = "Views/proposer/myApplications/addPropose.html?ApplicationId=" + ApplicationId + "&isEdit=true";
        window.open($scope.editApplicationUrl);
    }

    //提交申请书
    $scope.submitApplication = function () {
        $http.put(submitApplicationUrl + $scope.postId)
        .success(function (response) {
            if (response.status == 0) {
                notif({
                    msg: "<b>恭喜:</b> 申请书提交成功！",
                    type: "success"
                });
                $scope.getSavedList();
            }
            else if (response.status == 4) {
                notif({
                    type: "error",
                    msg: "错误：" + response.errorMsg,
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
                $scope.getSavedList();
            }
            else {
                notif({
                    type: "error",
                    msg: "错误：" + response.errorMsg,
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
                $scope.getSavedList();
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "错误：" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        });
    }
    //删除申请书
    $scope.deleteApplication = function () {
        $http.delete(deleteApplicationUrl + $scope.postId)
        .success(function (response) {
            if (response.status == 0) {
                notif({
                    msg: "<b>恭喜:</b> 申请书删除成功！",
                    type: "success"
                });
                $scope.getSavedList();
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "错误：" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        });
    }

    //显示PDF页面
    $scope.showPDFpage = function (PDFUrl) {
        var PDFPageUrl = "/View/plugins/pdfjs/web/viewer.html?file=" + PDFUrl;
        window.open(PDFPageUrl);
    }

    //下载PDF
    $scope.downloadFile = function (applicationId, downloadName) {
        $downloadFile.downloadFile($scope.downloadPDFUrl, applicationId, downloadName);
    }

    //显示驳回理由
    $scope.showRejectReason = function (applicationId) {
        $http.get(getReasonUrl + applicationId + '/8')
        .success(function (response) {
            $scope.rejectReason = response.response[0].Comment;
            $('#rejectReasonModal').modal('show');
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "错误：" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }

    //显示不受理理由
    $scope.showNotPassReason = function (applicationId) {
        $http.get(getReasonUrl + applicationId + '/9')
        .success(function (response) {
            $scope.noPassReason = response.response[0].Comment;
            $('#noPassReasonModal').modal('show');
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "错误：" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }

	
    $scope.addApplication = function () {

        $http.get("/api/application/applicationId")
        .success(function (response) {
            $scope.addApplicationUrl = "Views/proposer/myApplications/addPropose.html?ApplicationId=" + response.response.ApplicationId + "&isEdit=false";
            window.open($scope.addApplicationUrl);
        })

        
	}
	
	$scope.showDeleteModal = function(){
		$("#deleteApplication").modal("show");
	}
	
	$scope.showSubmitModal = function(){
		$("#submitApplication").modal("show");
	}

    //初始化
	$scope.getSavedList();

	$scope.debugger = $setDebugger.getDebuggerFn();
	
})