function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

var announceDetailApp = angular.module('ngAnnounceDetailApp', ['ngCookies']);

announceDetailApp.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptor');
})



announceDetailApp.controller('ngAnnounceDetailCtrl', function ($scope, $http, $compile) {
    $scope.announceId = getQueryString('announceId');
    $scope.fileListIsNull = true;

    //url
    var getAnnounceDetailUrl = "/api/Announcement/";

    $scope.getAnnoucneDetail = function () {
        $http.get(getAnnounceDetailUrl + $scope.announceId)
            .success(function (response) {
                if (response.response != null) {
                    $scope.announceElement = response.response;

                    if (response.response.Attachments.length == 0) {
                        $scope.fileListIsNull = true;
                    }
                    else {
                        $scope.fileListIsNull = false;
                    }

                    //compile动态加载dom
                    var compileFn = $compile($scope.announceElement.Content);
                    var dom = compileFn($scope);
                    $(".announceBody").append(dom);
                }
            })
            .error(function (response) {
                alert("错误：" + response.errorMsg);
            })
    }



    //初始化
    $scope.getAnnoucneDetail();

})




announceDetailApp.factory('authInterceptor', ['$q', '$injector', "$rootScope", "$cookies", function ($q, $injector, $rootScope, $cookies) {
    return {
        request: function (config) {
            config.headers = config.headers || {};
            if (getQueryString('token')) {
                var token = getQueryString('token');
                token = token.replace(/\"/g, "");
                config.headers.authorization = 'Basic ' + token;
            }
            return config;
        },
        responseError: function (response) {
            if (response.status == 401 && $rootScope.interceptorTimes == 0) {
                $rootScope.interceptorTimes = $rootScope.interceptorTimes + 1;
                alert("您尚未登录！即将跳转到登录页面，请重新登录！");
                window.self.location = "login.html";
                return $q.reject(response);
            }
            else if (response.status == 400) {
                notif({
                    type: "error",
                    msg: "错误：" + response.data.errorMsg,
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            }
            else if (response.status == 403) {
                if (response.data.status == 400) {
                    notif({
                        type: "error",
                        msg: response.data.errorMsg,
                        position: "center",
                        width: 500,
                        height: 60,
                        autohide: false
                    });
                    $timeout(function () {
                        window.self.location = "login.html";
                    }, 3000);


                }
                else {
                    notif({
                        type: "error",
                        msg: "错误：您无权进行该操作，请联系系统管理员确认有该操作的权限",
                        position: "center",
                        width: 500,
                        height: 60,
                        autohide: false
                    });
                }
            }
            else if (response.status == 410) {
                notif({
                    type: "error",
                    msg: "错误：该数据不存在，请刷新后重试",
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            }
            else if (response.status == 412) {
                notif({
                    type: "error",
                    msg: "错误：" + response.data.errorMsg,
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            }
            else if (response.status == 500) {
                notif({
                    type: "error",
                    msg: "错误：" + response.data.errorMsg,
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            }
        }
    };
}])