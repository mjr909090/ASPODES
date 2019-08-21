var passwordChangeApp = angular.module('ngPasswordChangeApp',[]);

passwordChangeApp.controller('ngPasswordChangeCtrl', function($scope, $http){
	
    $scope.hasSubmitted = false;

    //Url
    var postPasswordUrl = "/api/profile/password";

    //ChangePassword
    $scope.updataPassword = function () {
        $http.post(postPasswordUrl, $scope.changePassword)
        .success(function () {

            notif({
                msg: "<b>恭喜:</b> 登录密码更改成功！",
                type: "success"
            });
            //$scope.changePassword = {};
            $scope.hasSubmitted = false;
        })
        .error(function () {
            notif({
                type: "error",
                msg: "未能成功修改登录密码，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }

});