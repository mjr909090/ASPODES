var applyUnitApp = angular.module('ngApplyUnitApp', ['publicFunction', 'selectAllFunction']);

applyUnitApp.directive('principalDetail', function () {
    return {
        restrict: 'E',
        templateUrl: 'Views\\divisionAdmin\\applicationManagement\\principalDetail.html'
    }
});

applyUnitApp.controller('ngApplyUnitCtrl', ['$scope', '$http', '$downloadFile', 'selectService', function ($scope, $http, $downloadFile, selectService) {
    var divisionApplyListUrl = "/api/instapplication/check/"; //Get待审核申请书的Url
    var leaderInfoUrl = "/api/person/";//Get负责人信息的Url
    var projectTypeUrl = "/api/ProjectType";//Get获取分类的类别
    var rejectReasonUrl = "/api/Setting/TemplateReason/Inst";//Get 驳回原因  api/Setting/TemplateReason/Inst

    var submitApplicationUrl = "/api/instapplication/handin/";//Put申请书-提交-的Url   
    var batchSubmitApplicationUrl = "/api/instapplication/handins";//Put申请书-批量-提交-的Url
    var rejectApplicationUrl = "/api/instapplication/reject";//Put申请书-驳回-的Url

    //初始化数据
    $scope.downloadPDFUrl = "/api/applicationdoc/download/pdf/";//下载PDF的Url
    $scope.packageDownloadUrl = "/api/applicationdoc/inst/downloadPackage/"

    $scope.exportInstUnhandledListUrl = "/api/instapplication/export"; // 单位管理员待受理申请书导出列表的url

    // 定义
    $scope.exportInstUnhandledListFields = {};
    $scope.exportInstUnhandledListName = "待审核申请书列表";
    $scope.instApplicationPackageName = "待审核申请书文件";

    // ********************初始化json数据********************
    $scope.listAlready = false;
    $scope.divisionAppliesIsNull = true;

    $scope.selectValue = null;
    $scope.rejectReson = {
        "ApplicationId": null,
        "Comment": null
    }

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
        var divisionApplyUrl = divisionApplyListUrl + $scope.selectValue + "/" + page;
        $http.get(divisionApplyUrl)
            .success(function (response) {
                if (response.response != null) {
                    $scope.divisionApplies = response.response.ItemDTOs;
                    //console.log($scope.divisionApplies);
                    $scope.pageItem = response.response.TotalPageNum;//总共多少页
                    $scope.count = response.response.TotalNum; //页面总共多少条
                    $scope.p_current = response.response.NowPage; //当前第几页
                    $scope.p_count = response.response.NowNum; //当前页面多少条
                    $scope.pages = [];
                    for (var i = 0; i < $scope.pageItem; i++) {
                        $scope.pages[i] = i + 1;
                    }
                    if ($scope.divisionApplies.length == 0) {
                        $scope.divisionAppliesIsNull = true;
                        $scope.listAlready = true;
                    }
                    if ($scope.divisionApplies.length != 0) {
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

    //全选、反选
    $scope.m = [];
    $scope.checked = [];
    $scope.selectAll = function () {
        selectService.selectAll($scope);
    }
    //单选
    $scope.selectSingle = function () {
        selectService.selectSingle($scope);
    }

    //	 ***************负责人具体信息-模态框*****************
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
        //var promise = $leaderInfo.getLeaderInfo(leaderInfoUrl, leaderId);
        //promise.then(function (data) {
        //    $scope.modalData = data;
        //}, function () {
        //    alert("error");
        //})
    }

    //	 ***************下载PDF-模态框*****************
    $scope.downloadPDF = function (applicationId, projectName) {
        $downloadFile.downloadFile($scope.downloadPDFUrl, applicationId, projectName);
    }

    //批量下载
    $scope.packageDownloadApplications = function (projectType) {
        if (projectType == undefined) {
            $scope.projectType = 0;
        } else {
            $scope.projectType = projectType;
        }
        $downloadFile.downloadFileType($scope.packageDownloadUrl + $scope.projectType, ".zip", $scope.instApplicationPackageName);
    }

    //	 ***************驳回-操作*****************
    $scope.showRejectApplicationModal = function (divisionApply) {
        //$scope.getRejectReason();//get rejectResonList

        $('#rejectApplication').modal("show");
        $scope.modalData = angular.copy(divisionApply);
    }
    //下拉列表框-选择原因
    $scope.rejectValue = function () {
        $scope.rejectReason = $scope.selectRejectValue;
    }

    $scope.reject = function (applicationId) {
        $scope.rejectReson.ApplicationId = applicationId;
        $scope.rejectReson.Comment = $scope.rejectReason;

        $http({
            method: 'PUT',
            url: rejectApplicationUrl,
            data: $scope.rejectReson
        })
        .success(function (response) {
            if (response.status == 0) {
                $('#rejectApplication').modal("hide");
                notif({
                    msg: "<b>恭喜:</b> 驳回成功",
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
                msg: "<b>错误:</b> 驳回失败，请检查网络是否通畅",
                type: "error"
            });
        });
    }

    //rejectApplication 隐藏 触发
    $('#rejectApplication').on('hidden.bs.modal', function (e) {
        $scope.reloadRejectModal();
    })

    //每次点击驳回按钮，强制置空文本框的值
    $scope.reloadRejectModal = function () {
        $scope.rejectReason = "";
        $scope.selectRejectValue = null;
    }

    //	 ***************提交操作*****************
    $scope.showSubmitApplicationModal = function (divisionApply) {
        $('#submitApplication').modal("show");
        $scope.modalData = angular.copy(divisionApply);
    }
    $scope.submit = function (applicationId) {
        var submitUrl = submitApplicationUrl + applicationId;
        $http.put(submitUrl)
            .success(function (response) {
                if (response.status == 0) {
                    $('#submitApplication').modal("hide");
                    notif({
                        msg: "<b>恭喜:</b> 申请书提交成功",
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
                    msg: "<b>错误:</b> 提交失败，请检查网络是否通畅",
                    type: "error"
                });
            });
    };

    $scope.exportDivisionUnhandledList = function (projectType) {
        if (projectType == undefined) {
            $scope.projectType = 0;
        } else {
            $scope.projectType = projectType;
        }
        $scope.exportInstUnhandledListFields = {
            "Order": "序号",
            "Name": "申请书名称",
            "Deleage": "委托",
            "Category": "类型",
            "Leader": "牵头负责人",
            "CategoryId": $scope.projectType
        };
        $downloadFile.exportApplication($scope.exportInstUnhandledListUrl, $scope.exportInstUnhandledListFields, $scope.exportInstUnhandledListName);
    };
}]);
