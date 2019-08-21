var noticeYuanApp = angular.module('ngNoticeYuanApp',[]);

noticeYuanApp.controller('ngNoticeYuanCtrl', function ($scope, $http, $compile, $state) {

    //Url
    var getNoticeUrl = "/api/Notice/"; // api/Notice/{page}/{read}
    var postReadNoticeUrl = "/api/Notice/";

    $scope.divisionAppliesIsNull = false;//通知数目为零时
    $scope.listAlready = false;
    $scope.pageNum = 1;
    $scope.isEnd = false;
    $scope.noticeListState = 2; // 通知筛选

    // 获取通知列表
    $scope.getNoticeList = function (state,page) {
        $scope.listAlready = false;
        $scope.noticeListState = state;     // 通知页状态
        $http.get(getNoticeUrl + page + "/" + $scope.noticeListState + "/" + 0)
            .success(function (response) {
                if (response.response != null) {
                    $scope.NoticeList = response.response.ItemDTOs;
                    $scope.pageItem = response.response.TotalPageNum;//总共多少页
                    $scope.count = response.response.TotalNum; //页面总共多少条
                    $scope.p_current = response.response.NowPage; //当前第几页
                    $scope.p_count = response.response.NowNum; //当前页面多少条
                    $scope.pages = [];
                    for (var i = 0; i < $scope.pageItem; i++) {
                        $scope.pages[i] = i + 1;
                    }
                    $scope.listAlready = true;
                    if ($scope.NoticeList.length == 0) {
                        $scope.divisionAppliesIsNull = true;
                    } else {
                        $scope.divisionAppliesIsNull = false;
                    }
                }
            })
            .error(function () {
                $scope.listAlready = true;
                notif({
                    msg: "<b>Error:</b> 列表未能加载，请重试",
                    type: "error"
                });
            });
    }

    // 加载页面
    $scope.load_page = function (page) {
        $scope.getNoticeList($scope.noticeListState,page);
    }

    $scope.load_page(1);// 启动方法

    // 通知页面跳转
    $scope.skipPage = function (skipUrl) {
        $state.go(skipUrl);
    }

    // 标记通知为已读
    $scope.postReadNotice = function (noticeID) {
        $http.post(postReadNoticeUrl + noticeID)
            .success(function (response) {
                $scope.getNoticeList($scope.noticeListState, 1);
            })
            .error(function (response) {

            })
    }

    // 点击通知触发跳转和标记
    $scope.clickNotice = function (notice) {
        if (notice.Read == false) {
            $scope.skipPage(notice.URL);
            $scope.postReadNotice(notice.NoticeID);
        }
    }

    // 点击已读按钮，标记但不跳转
    $scope.clickNoticeBtn = function (notice) {
        $scope.postReadNotice(notice.NoticeID);
    }
})