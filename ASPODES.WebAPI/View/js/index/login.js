var loginApp = angular.module('ngLoginApp', ['ngCookies', 'interceptorFactory']);

loginApp.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptor');
})

loginApp.controller('ngLoginCtrl', function ($scope, $http, $cookieStore) {

    //Url
    var loginUrl = "/api/login";
    var resetPasswordUrl = "/api/resetPassword";
    var forgetPasswordUrl = "/api/forgetPassword";

    //初始化
    $scope.resetPassword = {};

    $scope.login = function () {

        $http({
            method: "POST",
            url: loginUrl,
            data: $scope.input
        })
        .success(
            function (response) {
                if (response.status == 0) {
                    $cookieStore.put("token", response.response.Token);
                    $cookieStore.put("role", response.response.Roles);
                    $cookieStore.put("PersonId", response.response.PersonId);
                    $cookieStore.put("UserName", response.response.UserName);
                    window.self.location = "index.html"
                }
            })
            .error(function (response) {
                if (response.status == 400) {
                    notif({
                        type: "error",
                        msg: "用户名或密码错误！",
                        position: "center",
                        width: 500,
                        height: 60,
                        autohide: false
                    });
                }
                if (response.status == 999) {
                    notif({
                        type: "error",
                        msg: "未知错误！",
                        position: "center",
                        width: 500,
                        height: 60,
                        autohide: false
                    });
                }
            });
        };

    $scope.enterPress = function (e) {
        var keycode = window.event ? e.keyCode : e.which;
        if (keycode == 13) {
            $scope.login();
        }
    }

    //忘记密码
    $scope.showResetPasswordModal = function () {
        $("#ResetPasswordModal").modal("show");
    }

    //ResetPassword
    $scope.resetPasswordFun = function () {
        $http({
            method: "POST",
            url: resetPasswordUrl,
            data: $scope.resetPassword
        })
        .success(function () {

            notif({
                msg: "<b>恭喜:</b> 登录密码重置成功！",
                type: "success"
            });
            $scope.hasSubmitted = false;
            $('#ResetPasswordModal').modal('hide');
        })
        .error(function () {
            notif({
                type: "error",
                msg: "未能成功重置登录密码，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }

    //ForgetPassword
    $scope.forgetPassword = function () {
        $http({
            method: "POST",
            url: forgetPasswordUrl,
            data:
                {
                    "IDCard" : $scope.resetPassword.IDCard,
                    "Name": $scope.resetPassword.Name
                }
        })
        .success(function () {

            notif({
                msg: "验证码发送成功，请登录邮箱查看！",
                type: "success"
            });
        })
        .error(function () {
            notif({
                type: "error",
                msg: "未能成功发送验证码，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }
    //在线评议模态框隐藏时触发置空函数
    $('#ResetPasswordModal').on('hidden.bs.modal', function (e) {
        $scope.PasswordFormCancle();
    })
    $scope.PasswordFormCancle = function () {
        $scope.resetPasswordForm = false;
        $scope.resetPassword = {};
    }

});