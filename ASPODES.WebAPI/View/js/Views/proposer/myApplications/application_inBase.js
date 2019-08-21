var applicationInBaseApp = angular.module('ngApplicationInBaseApp', []);

applicationInBaseApp.controller('ngApplicationInBaseCtrl', function ($scope, $http) {

    //$scope.inBaseListReady = false;
    //$scope.inBaseListIsNull = true;
    $scope.previousListReady = false;
    $scope.previousListIsNull = true;

    //在库申请书
    //Url
    //var getProposeInBaseUrl = "/api/applciation/status/16";
    var getProposePreviousUrl = "/api/application/previous";

    //获取在库申请书列表
    //$scope.getProposeInBase = function () {
    //    $scope.inBaseListReady = false;
    //    $scope.inBaseListIsNull = true;
    //    $http.get(getProposeInBaseUrl)
    //    .success(function (response) {
    //        if (response.status == 0) {
    //            proposeInBaseLists = response.response;
    //            $scope.inBaseListReady = true;
    //            $scope.inBaseListIsNull = false;
    //        }
    //        else {
    //            notif({
    //                type: "error",
    //                msg: "错误：" + response.errorMsg,
    //                position: "center",
    //                width: 500,
    //                height: 60,
    //                autohide: false
    //            });
    //            $scope.inBaseListReady = true;
    //            $scope.inBaseListIsNull = true;
    //        }
    //    })
    //    .error(function (response) {
    //        notif({
    //            type: "error",
    //            msg: "未能获取到在库申请书列表，请检查您的网络是否畅通！",
    //            position: "center",
    //            width: 500,
    //            height: 60,
    //            autohide: false
    //        });
    //        $scope.inBaseListReady = true;
    //        $scope.inBaseListIsNull = true;
    //    })
    //}

    //获取既往申请书
    $scope.getProposePrevious = function () {
        $scope.previousListReady = false;
        $http.get(getProposePreviousUrl)
        .success(function (response) {
            if (response.response.length != 0) {
                $scope.previousProposes = response.response;
                $scope.previousListReady = true;
                $scope.previousListIsNull = false;
            }
            else if(response.response.length == 0) {
                $scope.previousListIsNull = true;
                $scope.previousListReady = true;
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
                $scope.previousListIsNull = true;
                $scope.previousListReady = true;
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
            $scope.previousListReady = true;
            $scope.previousListIsNull = true;
        })
    }

    //初始化
    //$scope.getProposeInBase();
    $scope.getProposePrevious();

    //跳转到编辑页面
    $scope.jumpEditProposePage = function (ApplicationId) {
        window.open("Views/proposer/myApplications/addPropose.html&" + ApplicationId);
    }

    $scope.showDetailModal = function () {
        $("#showDetailModal").modal("show");
    }

})