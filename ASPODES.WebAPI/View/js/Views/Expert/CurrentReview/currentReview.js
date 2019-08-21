var currentReviewApp = angular.module('ngCurrentReviewApp', ['publicFunction', 'ngMessages']);

currentReviewApp.controller('ngCurrentReviewCtrl', ['$scope', '$http', '$downloadFile', function ($scope, $http, $downloadFile) {
    var currentReviewUrl = "/api/reviewcomment/expert/"; //Get本年度申请书列表URL
    var pdfPassword = "/api/expert/review/password";//Get获取pdf密码
    $scope.downloadPDFUrl = "/api/applicationdoc/download/passwordpdf/";//Get下载PDF的Url
    var updateReviewUrl = "/api/ReviewComment"; //POST更改评审URL

    //  ************************初始化************************
    $scope.listAlready = false;
    $scope.AppliesIsNull = true;

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
    //获取待评审申请书列表

    //选择某一页时
    $scope.load_page = function (page) {
        var currentApplyListUrl = currentReviewUrl + "-1/" + page;
        $http.get(currentApplyListUrl)
            .success(function (response) {
                if (response.response != null) {
                    $scope.currentApplies = response.response.ItemDTOs;
                    $scope.pageItem = response.response.TotalPageNum;//总共多少页
                    $scope.count = response.response.TotalNum; //页面总共多少条
                    $scope.p_current = response.response.NowPage; //当前第几页
                    $scope.p_count = response.response.NowNum; //当前页面多少条
                    $scope.pages = [];
                    for (var i = 0; i < $scope.pageItem; i++) {
                        $scope.pages[i] = i + 1;
                    }
                    if ($scope.currentApplies.length == 0) {
                        $scope.AppliesIsNull = true;
                        $scope.listAlready = true;
                    }
                    if ($scope.currentApplies.length != 0) {
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

    //	 ***************下载PDF*****************
    $scope.downloadPDF = function (applicationId, projectName) {
        $downloadFile.downloadFile($scope.downloadPDFUrl, applicationId, projectName);
    }

    //查看评议具体意见
    $scope.showResuiltModal = function (apply) {
        $("#resultModal").modal("show");
        $scope.lookFormData = angular.copy(apply);
    }

    //修改在线评议
    $scope.showReviewModal = function (currentApply) {
        $("#editReviewModal").modal("show");
        $scope.formData = angular.copy(currentApply);
        $scope.judgeScore();
    }
    //according the score to judge the level
    $scope.judgeScore = function () {
        if ($scope.formData.Score >= 90) {
            $scope.formData.Level = "A";
        } else if ($scope.formData.Score >= 80) {
            $scope.formData.Level = "B";
        } else if ($scope.formData.Score >= 70) {
            $scope.formData.Level = "C";
        } else {
            $scope.formData.Level = "D";
        }
    }

    $scope.notSubsidize = function () {
        if ($scope.formData.Imburse == "C") {
            $scope.formData.Amount = 0;
        }
    }

    //更改评审结果
    $scope.updateReview = function () {
        $scope.isOnlineFormSubmit = true;
        if ($scope.OnlineForm.$valid) {
            $http({
                method: 'POST',
                data: $scope.formData,
                url: updateReviewUrl
            })
            .success(function (response) {
                if (response.status == 0) {
                    $("#editReviewModal").modal("hide");
                    notif({
                        msg: "<b>恭喜:</b> 修改评议成功",
                        type: "success"
                    });
                    $scope.load_page($scope.p_current);
                } else {
                    notif({
                        msg: "<b>错误:</b>" + response.errorMsg,
                        type: "error"
                    });
                }
            })
            .error(function () {
                notif({
                    msg: "<b>错误:</b> 评议修改失败，请检查网络是否通畅",
                    type: "error"
                });
            })
        } else {
            notif({
                type: "error",
                msg: "填写的信息存在不规范或者错误，请根据提示进行修改！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        }
    }
    //模态框隐藏时触发置空函数
    $('#editReviewModal').on('hidden.bs.modal', function (e) {
        $scope.reloadEditReviewModal();
    })
    $scope.reloadEditReviewModal = function () {
        $scope.OnlineForm.$setPristine();
        $scope.OnlineForm.$setUntouched();
        $scope.isOnlineFormSubmit = false;

        $scope.formData = {
            "Level": "A",
            "Imburse": "A"
        };
    }
}]);

currentReviewApp.filter('changeScoreToLevel', function () {
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
