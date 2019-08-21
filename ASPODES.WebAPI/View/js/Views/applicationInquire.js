var applicationInquireApp = angular.module('ngApplicationInquireApp', ['publicFunction']);
applicationInquireApp.controller('ngApplicationInquireCtrl', function ($scope, $http, $downloadFile) {
    // URL
    var getProjectTypeUrl = "/api/projectType";
    var getInstitutionUrl = "/api/inst";
    var queryApplicationsUrl = "/api/departapplication/ApplicationInquiries/";
    $scope.downloadPDFUrl = "/api/applicationdoc/download/pdf/";//下载PDF的Url
    
    // 定义
    $scope.queryConditions = {};
    $scope.queryedApplicationsIsNull = true;

    // 获取项目类型
    $scope.getProjectType = function () {
        $http({
            method: 'GET',
            url: getProjectTypeUrl
        }).then(function successCallback(response) {
            $scope.projectTypes = response.data.response;
        }, function errorCallback(response) {
                
        });
    }

    // 获取单位信息
    $scope.getInstitution = function () {
        $http({
            method: 'GET',
            url: getInstitutionUrl
        }).then(function successCallback(response) {
            $scope.institutions = response.data.response;
        }, function errorCallback(response) {

        });
        
    }

    //启动方法
    $scope.getInstitution();
    $scope.getProjectType();

    $scope.load_page = function (page) {
        var data = {};
        data = angular.copy($scope.queryConditions);
        if (data.projectPeriod != undefined) {
            data.projectPeriod = [$scope.queryConditions.projectPeriod];
        }
        if (data.ProjectType != undefined) {
            data.ProjectType = [$scope.queryConditions.ProjectType];
        }
        if (data.Institute != undefined) {
            data.Institute = [$scope.queryConditions.Institute];
        }
        $http({
            method: 'POST',
            url: queryApplicationsUrl + page,
            data: data
        }).then(function successCallback(response) {
            if (response.data.response != null) {
                $scope.queryedApplicationsIsNull = false;
                $scope.queryedApplications = response.data.response.ItemDTOs;
                $scope.pageItem = response.data.response.TotalPageNum;//总共多少页     pageItem  count  p_current  p_count  pages  这五个变量是必须要写的
                $scope.count = response.data.response.TotalNum; //页面总共多少条
                $scope.p_current = response.data.response.NowPage; //当前第几页
                $scope.p_count = response.data.response.NowNum; //当前页面多少条
                $scope.pages = [];
                for (var i = 0; i < $scope.pageItem; i++) {
                    $scope.pages[i] = i + 1;
                }
            }
        }, function errorCallback(response) {
            $scope.queryedApplicationsIsNull = true;
            notif({
                msg: "<b>错误:</b> 页面加载失败，请检查网络是否通畅",
                type: "error"
            });
        });
    }

    $scope.downloadPDF = function (applicationId, projectName) {
        $downloadFile.downloadFile($scope.downloadPDFUrl, applicationId, projectName);
    }

    //点击项目名称显示PDF页面
    //$scope.showPDFpage = function (PDFUrl) {
    //    var PDFPageUrl = "/View/plugins/pdfjs/web/viewer.html?file=" + PDFUrl;
    //    window.open(PDFPageUrl);
    //}

});