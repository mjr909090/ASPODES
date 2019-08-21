var interceptorFactoryApp = angular.module('interceptorFactory', []);

interceptorFactoryApp.factory('authInterceptor', ['$q', '$injector', "$rootScope", "$cookies","$timeout", function ($q, $injector, $rootScope, $cookies, $timeout) {
    return {
        request: function (config) {
            config.headers = config.headers || {};

            $cookies.put("test", "testContent");
            var test = $cookies.get("test");
            console.log(test);

            var role = $cookies.get();
            console.log(role);

            if ($cookies.get('token')) {
                var token = $cookies.get('token');
                token = token.replace(/\"/g, "");
                config.headers.authorization = 'Basic ' + token;
            }
            return config;
        },
        responseError: function (response) {
            $rootScope.interceptorTimes = 0;
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