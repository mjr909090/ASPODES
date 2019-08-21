var finishedApplicationApp = angular.module('ngFinishedApplicationApp', ['publicFunction']);

finishedApplicationApp.directive('principalDetail', function () {
    return {
        restrict: 'E',
        templateUrl: 'Views\\divisionAdmin\\applicationManagement\\principalDetail.html'
    }
});

finishedApplicationApp.controller('ngFinishedApplicationCtrl', ['$scope', '$http', '$downloadFile', function ($scope, $http, $downloadFile) {
    var finishedApplyListUrl = "/api/instapplication/previous/";  //Get既往申请书-按类型-按分页-列表的Url
    var leaderInfoUrl = "/api/person/";//Get负责人信息的Url
    var projectTypeUrl = "/api/ProjectType";//Get获取分类的类别

    $scope.downloadPDFUrl = "/api/applicationdoc/download/pdf/"; //下载PDF的Url

    $scope.exportInstFinishedListUrl = "/api/instapplication/Previous"; // 单位管理员待受理申请书导出列表的url

    // 定义
    $scope.exportInstFinishedListFields = {};
    $scope.exportInstFinishedListName = "既往申请书";

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
        var ApplyListUrl = finishedApplyListUrl + $scope.selectValue + "/" + page;
        $http.get(ApplyListUrl)
            .success(function (response) {
                if (response.response != null) {
                    $scope.finishedApplies = response.response.ItemDTOs;
                    $scope.pageItem = response.response.TotalPageNum;//总共多少页
                    $scope.count = response.response.TotalNum; //页面总共多少条
                    $scope.p_current = response.response.NowPage; //当前第几页
                    $scope.p_count = response.response.NowNum; //当前页面多少条
                    $scope.pages = [];
                    for (var i = 0; i < $scope.pageItem; i++) {
                        $scope.pages[i] = i + 1;
                    }
                    if ($scope.finishedApplies.length == 0) {
                        $scope.divisionAppliesIsNull = true;
                        $scope.listAlready = true;
                    }else {
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
    $scope.principalDetail = function (finishedApply) {
        var personId = finishedApply.LeaderId;
        var detailUrl = leaderInfoUrl + personId;
        $http.get(detailUrl)
		        .success(function (response) {
		            $scope.modalData = response.response;
		        })
		        .error(function () {
		            notif({
		                msg: "<b>错误:</b> 信息获取失败，请检查网络是否通畅",
		                type: "error"
		            });
		        });
    }

    //	 ***************下载PDF-模态框*****************
    $scope.downloadPDF = function (applicationId, projectName) {
        $downloadFile.downloadFile($scope.downloadPDFUrl, applicationId, projectName);
    }

    $scope.exportDivisionfinishedList = function (projectType) {
        if (projectType == undefined) {
            $scope.projectType = 0;
        } else {
            $scope.projectType = projectType;
        }
        $scope.exportInstFinishedListFields = {
            "Order": "序号",
            "Name": "申请书名称",
            "Deleage": "委托",
            "Category": "类型",
            "Leader": "牵头负责人",
            "Status" : "状态",
            "CategoryId": $scope.projectType
        };
        $downloadFile.exportApplication($scope.exportInstFinishedListUrl, $scope.exportInstFinishedListFields, $scope.exportInstFinishedListName);
    };
}]);