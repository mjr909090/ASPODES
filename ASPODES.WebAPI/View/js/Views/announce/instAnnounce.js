var instAnnounceApp = angular.module('ngInstAnnounceApp', ["ngCookies"]);
instAnnounceApp.controller('ngInstAnnounceCtrl', function ($scope, $http, $cookies) {

    //Url
    var getInstAnnounceUrl = "/api/Announcement/user/inst/"


    //获取院公告列表
    $scope.getInstAnnounceList = function (startTime, endTime, page) {
        $http({
            method: 'GET',
            url: getInstAnnounceUrl + page,
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
        $scope.getInstAnnounceList(0, 0, page);
    }


    //初始化
    $scope.getInstAnnounceList(0, 0, 1);
})