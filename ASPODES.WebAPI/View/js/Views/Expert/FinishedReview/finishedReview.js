var finishedReviewApp = angular.module('ngFinishedReviewApp', ['publicFunction']);

finishedReviewApp.controller('ngFinishedReviewCtrl', ['$scope', '$http', '$downloadFile', function ($scope, $http, $downloadFile) {
    var finishedReviewUrl = "/api/reviewcomment/expert/"; //Get既往申请书列表URL
    var pdfPassword = "/api/expert/review/password";//Get获取pdf密码
    $scope.downloadPDFUrl = "/api/applicationdoc/download/passwordpdf/";//Get下载PDF的Url

    //时间选择datepicker
    $('.date').datepicker({
        format: 'yyyy',
        language: 'zh-CN',
        autoclose: true,
        startView: 2,
        maxViewMode: 2,
        minViewMode: 2
    });

    // ********************初始化json数据********************
    $scope.listAlready = false;
    $scope.AppliesIsNull = true;

    $scope.yearValue = null;

    $scope.pageItem = 0;
    $scope.count = 0;
    $scope.p_current = 0;
    $scope.p_count = 0;

    //获取pdf密码
    $scope.getPDFPassword = function () {
        $http.get(pdfPassword)
            .success(function (response) {
                $scope.PDFpassword = response;
            })
        .error(function (response) {
            notif({
                msg: "<b>错误:</b> 口令获取失败，请检查网络是否通畅",
                type: "error"
            });
        })
    }
    $scope.getPDFPassword();

    // *******************获取列表&分页**************************
    //get
    //获取既往申请书列表
    //选择某一页时
    $scope.load_page = function (page) {
        var year = null;
        if ($scope.yearValue == null) {
            year = -2;
        }
        if ($scope.yearValue != null) {
            year = $scope.yearValue;
        }
        var finishedApplyListUrl = finishedReviewUrl + year + "/" + page;
        $http.get(finishedApplyListUrl)
            .success(function (response) {
                if (response.response != null) {
                    $scope.finishedApplies = response.response.ItemDTOs;
                    $scope.pageItem = response.response.TotalPageNum;//总共多少页
                    $scope.count = response.response.TotalNum; //页面总共多少条
                    $scope.p_current = response.response.NowPage; //当前第几页
                    $scope.p_count = response.response.NowNum; //当前页面多少条
                    $scope.pages = [];
                    for (var i = 0; i < $scope.pageItem; i++) {
                        $scope.pages[i] = i + 1;
                    }
                    if ($scope.finishedApplies.length == 0) {
                        $scope.AppliesIsNull = true;
                        $scope.listAlready = true;
                    }
                    if ($scope.finishedApplies.length != 0) {
                        $scope.AppliesIsNull = false;
                        $scope.listAlready = true;
                    }
                }
            })
            .error(function () {
                notif({
                    msg: "<b>错误:</b> 页面加载失败，请检查网络是否通畅",
                    type: "error"
                });
            });
    }
    
    //初始化获取列表
    $scope.load_page(1);
    
    //	 ***************下载加密PDF*****************
    $scope.downloadPDF = function (applicationId, projectName) {
        $downloadFile.downloadFile($scope.downloadPDFUrl, applicationId, projectName);
    }

    //查看评议具体意见
    $scope.showResuiltModal = function (apply) {
        $("#resultModal").modal("show");
        $scope.formData = angular.copy(apply);
    }
}]);

finishedReviewApp.filter('changeScoreToLevel', function () {
    return function (score) {
        console.log("a");
        console.log(typeof (score));

        var level;

        if (score >= 90) {
            level = "A.优";
        }
        else if (score >= 80 && score < 90) {
            level = "B.良";
        }
        else if (score >= 70 && score < 80) {
            level = "C.中";
        }
        else if (score >= 0 && score < 70) {
            level = "D.差";
        }
        else {
            level = null;
        }

        return level;
    }
});
