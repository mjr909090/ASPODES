var noticeYuanApp = angular.module('ngNoticeYuanApp',[]);

noticeYuanApp.controller('ngNoticeYuanCtrl', function ($scope, $http, $compile, $state) {

    //Url
    var getNoticeUrl = "/api/Notice/"; // api/Notice/{page}/{read}
    var postReadNoticeUrl = "/api/Notice/";

    $scope.divisionAppliesIsNull = false;//֪ͨ��ĿΪ��ʱ
    $scope.listAlready = false;
    $scope.pageNum = 1;
    $scope.isEnd = false;
    $scope.noticeListState = 2; // ֪ͨɸѡ

    // ��ȡ֪ͨ�б�
    $scope.getNoticeList = function (state,page) {
        $scope.listAlready = false;
        $scope.noticeListState = state;     // ֪ͨҳ״̬
        $http.get(getNoticeUrl + page + "/" + $scope.noticeListState + "/" + 0)
            .success(function (response) {
                if (response.response != null) {
                    $scope.NoticeList = response.response.ItemDTOs;
                    $scope.pageItem = response.response.TotalPageNum;//�ܹ�����ҳ
                    $scope.count = response.response.TotalNum; //ҳ���ܹ�������
                    $scope.p_current = response.response.NowPage; //��ǰ�ڼ�ҳ
                    $scope.p_count = response.response.NowNum; //��ǰҳ�������
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
                    msg: "<b>Error:</b> �б�δ�ܼ��أ�������",
                    type: "error"
                });
            });
    }

    // ����ҳ��
    $scope.load_page = function (page) {
        $scope.getNoticeList($scope.noticeListState,page);
    }

    $scope.load_page(1);// ��������

    // ֪ͨҳ����ת
    $scope.skipPage = function (skipUrl) {
        $state.go(skipUrl);
    }

    // ���֪ͨΪ�Ѷ�
    $scope.postReadNotice = function (noticeID) {
        $http.post(postReadNoticeUrl + noticeID)
            .success(function (response) {
                $scope.getNoticeList($scope.noticeListState, 1);
            })
            .error(function (response) {

            })
    }

    // ���֪ͨ������ת�ͱ��
    $scope.clickNotice = function (notice) {
        if (notice.Read == false) {
            $scope.skipPage(notice.URL);
            $scope.postReadNotice(notice.NoticeID);
        }
    }

    // ����Ѷ���ť����ǵ�����ת
    $scope.clickNoticeBtn = function (notice) {
        $scope.postReadNotice(notice.NoticeID);
    }
})