var projectInfoApp = angular.module('ngProjectInfoApp', ["ngCookies", "interceptorFactory"]);

projectInfoApp.config(['$locationProvider', '$httpProvider', function ($locationProvider, $httpProvider) {
    $locationProvider.html5Mode(true);
    $httpProvider.interceptors.push('authInterceptor');
}]);

projectInfoApp.controller('ngProjectInfoCtrl', ['$scope', '$http', '$location', function ($scope, $http, $location) {
    if ($location.search().ProjectId) {
        $scope.projectId = $location.search().ProjectId;
    }

    var projectInfoUrl = "/api/project";

    //获取项目信息
    $scope.getProjectInfo = function () {
        $http({
            method: "GET",
            url: projectInfoUrl,
            params:{
                ProjectId: $scope.projectId
            }
        })
        .success(function(response){
            if (response.response != null) {
                $scope.projectInfo = response.response;
            }
        })
        .error(function () {
            notif({
                msg: "<b>错误:</b> 在研项目列表加载失败，请检查网络是否通畅",
                type: "error"
            });
        });
    }
    $scope.getProjectInfo();


    // look up the TASK details
    //$scope.openTask = function (taskId) {
    //    var iTop = (window.screen.availHeight - 30 - 600) / 2;
    //    var iLeft = (window.screen.availWidth - 10 - 1000) / 2;
    //    var url = "Views/divisionAdmin/projectManage/taskInfo.html?TaskId=" + taskId;
    //    window.open(url, "", 'height=600,width=1000,top=' + iTop + ',left=' + iLeft + ',status=no,toolbar=no,menubar=no,location=no,resizable=no,scrollbars=0,titlebar=no');
    //}

}])