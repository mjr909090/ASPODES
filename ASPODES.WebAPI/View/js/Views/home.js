var homeApp = angular.module('ngHomeApp', ["ngCookies"]);

homeApp.controller('ngHomeCtrl', function ($scope, $http, $state, $cookies) {

    //��ʼ��״̬
    $scope.divisionAnnounceIsNull = true;
    $scope.divisionAnnounceIsReady = false;
    $scope.DepartAnnounceIsNull = true;
    $scope.DepartAnnounceIsReady = false;
    $scope.mainOptions = {
        resize: false,
        anchors: ['noticeAndAnnounce', 'divisionAdminCharts', 'superAdminCharts'],
        scrollingSpeed: '700',
        navigation: true,
        navigationPosition: 'right',
        navigationColor: '000',
        autoScrolling: true,
        scrollOverflow: true
    }

    //URL
    var getNoticeUrl = "/api/Notice/";
    var getDivisionAnnounceUrl = "/api/Announcement/user/inst/";
    var getDepartAnnounceUrl = "/api/Announcement/user/depart/";
    var postReadNoticeUrl = "/api/Notice/";


    $scope.getNoticeList = function (state, page) {
        $scope.listAlready = false;
        $scope.noticeListState = state;     // ֪ͨҳ״̬
        $http.get(getNoticeUrl + page + "/" + $scope.noticeListState + "/" + 0)
            .success(function (response) {
                if (response.response != null) {
                    $scope.NoticeList = response.response.ItemDTOs;
                    $scope.listAlready = true;
                    if ($scope.NoticeList.length == 0) {
                        $scope.divisionAppliesIsNull = true;
                    } else {
                        $scope.divisionAppliesIsNull = false;
                    }
                }
            })
            .error(function () {
                $scope.divisionAppliesIsNull = true;
                $scope.listAlready = true;
                notif({
                    msg: "<b>Error:</b> �б�δ�ܼ��أ�������",
                    type: "error"
                });
            });
    }

    $scope.getDivisionAnnounce = function () {
        $scope.divisionAnnounceIsReady = false;
        $http.get(getDivisionAnnounceUrl + 1)
        .success(function (response) {
            if (response.response.NowNum == 0) {
                $scope.divisionAnnounceIsNull = true;
            }
            else {
                $scope.divisionAnnounces = response.response.ItemDTOs;
                $scope.divisionAnnounceIsNull = false;
            }
            $scope.divisionAnnounceIsReady = true;
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "����" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
            $scope.divisionAnnounceIsNull = false;
        })
    }



    $scope.getDepartAnnounce = function () {
        $scope.DepartAnnounceIsReady = false;
        $http.get(getDepartAnnounceUrl + 1)
        .success(function (response) {
            if (response.response.NowNum == 0) {
                $scope.departAnnounceIsNull = true;
            }
            else {
                $scope.departAnnounces = response.response.ItemDTOs;
                $scope.departAnnounceIsNull = false;
            }
            $scope.DepartAnnounceIsReady = true;
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "����" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
            $scope.departAnnounceIsNull = true;
            $scope.DepartAnnounceIsReady = true;
        })
    }

    $scope.openAnnounceDetail = function (announceId) {
        var token = $cookies.get("token");
        window.open("/View/Views/announce/announceDetail.html?announceId=" + announceId + "&token=" + token);
    }


    // ֪ͨҳ����ת
    $scope.skipPage = function (skipUrl) {
        $state.go(skipUrl);
    }

    // ���֪ͨΪ�Ѷ�
    //$scope.postReadNotice = function (noticeID) {
    //    $http.post(postReadNoticeUrl + noticeID)
    //        .success(function (response) {
    //            $scope.getNoticeList(0, 1);
    //        })
    //        .error(function (response) {

    //        })
    //}

    // ���֪ͨ������ת�ͱ��
    $scope.clickNotice = function (notice) {
        if (notice.Read == false) {
            $scope.skipPage(notice.URL);
            //$scope.postReadNotice(notice.NoticeID);
        }
    }

    // ����Ѷ���ť����ǵ�����ת
    //$scope.clickNoticeBtn = function (notice) {
    //    $scope.postReadNotice(notice.NoticeID);
    //}



    //��ʼ��
    $scope.getNoticeList(0, 1);
    $scope.getDivisionAnnounce();
    $scope.getDepartAnnounce();

    $(function () {
        $('.carousel').carousel({
            interval: 5000
        });
    });

})

homeApp.config(function () {
    
})
