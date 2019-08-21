var reviewApp = angular.module('ngReviewApp', ['publicFunction', 'ngMessages']);

reviewApp.controller('ngReviewCtrl', ['$scope', '$http','$timeout','$downloadFile', function ($scope, $http, $timeout, $downloadFile) {
    var reviewAppliesListUrl = "/api/reviewassignment/expert";//Get待评审申请书列表URL
    var pdfPassword = "/api/expert/review/password";//Get获取pdf密码
    var addCommentUrl = '/api/ReviewComment'; //  post添加评审结果
    var addAllCommentUrl = '/api/reviewcomment/multy';//post添加批量评审结果
    var downloadScoreDescriptionUrl = '/api/reviewDoc/download'; //下载评分说明的Url
    var getscoreDescriptionFileListUrl = "/api/ReviewDocList";
    $scope.downloadPDFUrl = "/api/applicationdoc/download/passwordpdf/";//Get下载PDF的Url

    $scope.packageDownloadUrl = "/api/applicationdoc/expert/downloadPackage"; // 打包下载

    //  ************************初始化************************
    $scope.formData = {};
    $scope.reviewApplySelected = [];

    $scope.listAlready = false;
    $scope.AppliesIsNull = true;
    $scope.waitBlurIsHide = true;

    $scope.preReviewPackageName = "待评审申请书";

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

    // 评分说明
    $scope.typeArr = [];
    $scope.recessive = true;
    $scope.init = function () {
        $("#ruleDescriptionModal").modal("show");

    }


    // *******************获取列表&分页**************************
    //get
    //获取待评审申请书列表
    $scope.getReviewAppliesList = function () {
        $http.get(reviewAppliesListUrl)
            .success(function (response) {
                if (response.response != null) {
                    $scope.reviewApplies = response.response;
                    if ($scope.reviewApplies.length == 0) {
                        $scope.AppliesIsNull = true;
                        $scope.listAlready = true;
                    }
                    if ($scope.reviewApplies.length != 0) {
                        $scope.AppliesIsNull = false;
                        $scope.listAlready = true;
                    }
                } else {
                    $scope.AppliesIsNull = true;
                    $scope.listAlready = true;
                }
            })
            .error(function () {
                notif({
                    msg: "<b>错误:</b> 用户列表获取失败，请检查网络是否通畅",
                    type: "error"
                });
            });
    }
    $scope.getReviewAppliesList();


    //获取文件列表
    $scope.getscoreDescriptionFileList = function () {
        $http.get(getscoreDescriptionFileListUrl)
        .success(function (response) {
            $scope.scoreDescriptionfileList = response.response;
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

    $scope.getscoreDescriptionFileList();

    //	 ***************下载PDF*****************
    $scope.downloadPDF = function (applicationId, projectName) {
        $downloadFile.downloadFile($scope.downloadPDFUrl, applicationId, projectName);
    }
    //	 ***************下载评分说明*****************
    $scope.downloadScoreDescription = function (fileType, fileName) {
        $downloadFile.downloadFileWithoutFileType(downloadScoreDescriptionUrl + '?Type=' + fileType, fileName);
    }

    // 批量下载
    $scope.downloadFileType = function (downloadPDFUrl, fileType, downloadName) {
        var pdfUrl = downloadPDFUrl;
        //$http({
        //    method: 'GET',
        //    url: pdfUrl,
        //    responseType: 'arraybuffer'
        //}).then(function successCallback(response) {
        //    if (response == undefined) {
        //        $scope.waitBlurIsHide = true;
        //    }
        //    var blob = new Blob([data], { type: "application/pdf" });
        //    var objectUrl = URL.createObjectURL(blob);
        //    var aForPDF = $("<a download='" + downloadName + fileType + "'><span class='aForPDF' ></span></a>").attr("href", objectUrl);
        //    $("body").append(aForPDF);
        //    $(".aForPDF").click();
        //    aForPDF.remove();
        //    $scope.waitBlurIsHide = true;
        //},function errorCallback(response) {

        //    });


        $http.get(pdfUrl, { responseType: 'arraybuffer' }).success(function (data) {
            var blob = new Blob([data], { type: "application/pdf" });
            var objectUrl = URL.createObjectURL(blob);
            var aForPDF = $("<a download='" + downloadName + fileType + "'><span class='aForPDF' ></span></a>").attr("href", objectUrl);
            $("body").append(aForPDF);
            $(".aForPDF").click();
            aForPDF.remove();
            $scope.waitBlurIsHide = true;
        });
    }

    // 批量下载
    $scope.packageDownload = function (projectName) {
        $scope.waitBlurIsHide = false;
        $scope.downloadFileType($scope.packageDownloadUrl, ".zip", $scope.preReviewPackageName);
        $timeout(function () {
            $scope.waitBlurIsHide = true;
        }, 10000);
    };

    //在线评议
    $scope.showReviewModal = function (confirmModalData) {
        $("#onlineReviewModal").modal("show");
        $scope.onlineModalData = angular.copy(confirmModalData);
    }
    $scope.initLevel = function () {
        $scope.formData.Level = "A";
        $scope.formData.Imburse = "A";
        $scope.formData.Amount = 0;
        $scope.formData.ReviewAssignmentId = $scope.onlineModalData.ReviewAssignmentId;
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
    //单个提交
    $scope.onlineReviewSubmit = function () {
        $scope.isOnlineFormSubmit = true;
        if ($scope.OnlineForm.$valid) {
            $http({
                method: 'POST',
                data: $scope.formData,
                url: addCommentUrl
            })
            .success(function (response) {
                if (response.status == 0) {
                    notif({
                        msg: "<b>恭喜:</b> 提交评议成功",
                        type: "success"
                    });
                    $("#onlineReviewModal").modal("hide");
                    $scope.getReviewAppliesList();
                } else {
                    notif({
                        msg: "<b>错误:</b>" + response.errorMsg,
                        type: "error"
                    });
                }
            })
            .error(function () {
                notif({
                    msg: "<b>错误:</b> 评议提交失败，请检查网络是否通畅",
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


    //去掉$$hashkey
    $scope.removeHashkey = function (item) {
        return angular.copy(item);
    }

    //切换选中
    $scope.toggle = function (item, list) {

        var idx = $scope.exists(item, list);
        if (idx > -1) {
            list.splice(idx, 1);
        }
        else {
            list.push(item);
        }

    }

    //判断是否选中
    $scope.exists = function (item, list) {

        var newList = $scope.removeHashkey(list);
        var newItem = $scope.removeHashkey(item);

        var position = -1;

        for (var i = 0; i < newList.length; i++) {
            if (_.isEqual(newList[i], newItem)) {
                position = i;
            }
        }

        return position;

    };

    //切换全部选中
    //$scope.alltoggle = function (list1, list2) {

    //    var idx = $scope.exists(list1, list2);
    //    if (idx != []) {
    //        list2.splice(idx[0], list2.length)
    //    }
    //    else {
    //        list2.push(list1);
    //    }
    //}

    //判断是否全部选中
    //$scope.allexists = function (list1, list2) {

    //    var newList1 = $scope.removeHashkey(list1);
    //    var newList2 = $scope.removeHashkey(list2);

    //    var position = new Array();
    //        for (var i = 0; i < newList2.length; i++) {
    //            if (_.isEqual(newList2[i], newList1[i])) {
    //                position[i] = i;
    //            }
    //        }

    //        return position;

    //};


    //批量评审
    $scope.showBatchReviewModal = function (confirmModalData) {
        $("#OnlineBatchReviewModal").modal("show");
        $scope.onlineBatchModalData = angular.copy(confirmModalData);
    }

    //批量提交

    $scope.onlineBatchReviewSubmit = function () {
        $scope.isBatchOnlineFormSubmit = true;


        if ($scope.BatchOnlineForm.$valid) {
            console.log($scope.reviewApplySelected);
            $http({
                method: 'POST',
                data: $scope.reviewApplySelected,
                url: addAllCommentUrl
            })
            .success(function (response) {
                if (response.status == 0) {
                    notif({
                        msg: "<b>恭喜:</b> 提交评议成功",
                        type: "success"
                    });
                    $("#OnlineBatchReviewModal").modal("hide");
                    $scope.getReviewAppliesList();
                    $scope.reviewApplySelected = [];
                } else {
                    notif({
                        msg: "<b>错误:</b>" + response.errorMsg,
                        type: "error"
                    });
                }

            })
            .error(function () {
                notif({
                    msg: "<b>错误:</b> 评议提交失败，请检查网络是否通畅",
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



    //在线评议模态框隐藏时触发置空函数
    $('#onlineReviewModal').on('hidden.bs.modal', function (e) {
        $scope.reloadOnlineReviewModal();
    })
    $scope.reloadOnlineReviewModal = function () {
        $scope.OnlineForm.$setPristine();
        $scope.OnlineForm.$setUntouched();
        $scope.isOnlineFormSubmit = false;

        $scope.formData = {};
    }

    ////批量评审模态框隐藏时触发置空函数
    //$('#OnlineBatchReviewModal').on('hidden.bs.modal', function (e) {
    //    $scope.reloadOnlineBatchReviewModal();
    //})
    //$scope.reloadOnlineBatchReviewModal = function () {
    //    $scope.BatchOnlineForm.$setpristine();
    //    $scope.BatchOnlineForm.$setuntouched();
    //    $scope.isBatchOnlineFormSubmit = false;

    //    $scope.reviewApplySelected = [];
    //}

    $scope.onlineBatchReviewCancle = function () {
        $scope.isBatchOnlineFormSubmit = false;
        //$scope.reviewApplySelected = [];
        //$scope.BatchOnlineForm.$setuntouched();
    }

}]);
reviewApp.directive('type', function () {
    return {
        restrict: 'A',
        controller: function () {
            this.typeArr = [];
            this.addType = function addType(type) {
                this.typeArr.push(type);
            }
        },
        link: function (scope, element, attrs) {

        }
    }
});
reviewApp.directive('detail', function () {
    return {
        restrict: 'A',
        scope: {
            label: '@'
        },
        require: '^type',
        link: function (scope, element, attrs) {

        }
    }
});

reviewApp.filter('changeScoreToLevel', function () {
    return function (score) {
        console.log("a");
        console.log(typeof (score));

        var level;

        if (score >= 90) {
            level = "优";
        }
        else if (score >= 80 && score < 90) {
            level = "良";
        }
        else if (score >= 70 && score < 80) {
            level = "中";
        }
        else if (score >= 0 && score < 70) {
            level = "差";
        }
        else {
            level = null;
        }

        return level;
    }
});

