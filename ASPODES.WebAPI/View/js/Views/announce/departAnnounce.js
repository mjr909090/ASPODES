var instAnnounceApp = angular.module('ngDepartAnnounceApp', ["ngCookies"]);
instAnnounceApp.controller('ngDepartAnnounceCtrl', function ($scope, $http, $cookies) {

    //Url
    var getDepartAnnounceUrl = "/api/Announcement/user/depart/"


    //获取院公告列表
    $scope.getDepartAnnounceList = function (startTime,endTime,page) {
        $http({
            method: 'GET',
            url: getDepartAnnounceUrl + page,
            params: {
                "StartTime": startTime,
                "EndTime": endTime
            }
        })
        .success(function (response) {
            if (response.status == 0) {
                $scope.announceList = response.response.ItemDTOs;
                $scope.announceListNowPage = response.response.NowPage;
                $scope.announceListTotalPageNum = response.response.TotalPageNum;
                $scope.announceListPageArry = new Array($scope.announceListTotalPageNum);
                for (var i = 0; i < $scope.announceListTotalPageNum; i++) {
                    $scope.announceListPageArry[i] = i + 1;
                }
            }
            else {

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
        })
    }

    //  查看公告
    $scope.openAnnounceDetail = function (announceId) {
        var token = $cookies.get("token");
        window.open("/View/Views/announce/announceDetail.html?announceId=" + announceId + "&token=" + token);
    }


    //判断是否超过3页
    $scope.pageAbs = function (NowPage, page) {
        if (Math.abs(NowPage - page) < 3) {
            return true;
        }
        else {
            return false;
        }
    }

    //跳页
    $scope.announceJumpToPage = function (page) {
        $scope.getDepartAnnounceList(0, 0, page);
    }


    //初始化
    $scope.getDepartAnnounceList(0, 0, 1);
})