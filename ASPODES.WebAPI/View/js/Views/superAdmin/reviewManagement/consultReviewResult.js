//咨询审议结果
var consultReviewResult = angular.module('ngConsultReviewResultApp', ['publicFunction']);

consultReviewResult.controller('ngConsultReviewResultCtrl', function ($scope, $http, Upload, $timeout, $downloadFile) {

    var fileName = '咨询审议模板';
    var applicationId = 0;
    var importYear = new Date().getYear();


    $scope.inputFile = null;
    $scope.hasUploaded = false;

    $scope.importReviewResult = function () {//不受理的模态框
        $("#importReviewResultModal").modal("show");
    };

    $scope.exportExcel = function () {//不受理的模态框
        $("#exportExcelModal").modal("show");
    };

    $scope.checkFileIsNotNull = function () {
        $scope.hasUploaded = true;
    }


    //Url
    var uploadUrl = '/api/consultation/result';
    var downloadFileUrl = '/api/consultation/template';
    var getConsultListUrl = '/api/consultation';

    //获取咨询审议结果
    $scope.getConsultList = function () {

        $scope.finishedListAlready = false;

        $http({
            method: 'GET',
            url: getConsultListUrl,
            params: {
                importYear: 2018
            }
        })
        .success(function (response) {
            $scope.applicationList = response.response.Applications;
            $scope.projectList = response.response.Projects;
            $scope.finishedListAlready = true;
        })
        .error(function (response) {
            $scope.finishedListAlready = true;
            notif({
                type: "error",
                msg: "错误：" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        });
    }

    //上传文档
    $scope.uploadFiles = function (files, errFiles) {

        $scope.files = files;
        $scope.errFiles = errFiles;
        angular.forEach(files, function (file) {
            file.upload = Upload.upload({
                url: uploadUrl,
                file: file
            });

            file.upload.then(
                function (response) {
                    $timeout(function () {
                        file.result = response.response;
                    });
                },
                function (response) {
                    if (response.status > 0) {
                        $scope.showErrorMsg = response.status + ':' + response.errorMsg;
                    }
                },
                function (evt) {
                    file.progress = Math.min(100, parseInt(100.0 * evt.loaded / evt.total));
                }
            )
            .then(function () {
                $scope.finishedListAlready = false;
                $scope.getConsultList();
            })
        })

    }

    //下载模板
    $scope.downloadModalFile = function () {
        $downloadFile.downloadFileType(downloadFileUrl, '.xls', fileName);
    }


    //初始化
    $scope.getConsultList();

});

consultReviewResult.filter('applicationChangeStatus', function () {
    return function (status) {
        var changedStatus = '';

        switch (status) {
            case 0: changedStatus = '入库'; break;
            case 1: changedStatus = '出库'; break;
            case 2: changedStatus = '不资助'; break;
        }


        return changedStatus;
    }
});

consultReviewResult.filter('projectChangeStatus', function () {
    return function (status) {
        var changedStatus = '';

        switch (status) {
            case 0: changedStatus = '持续资助'; break;
            case 1: changedStatus = '中止'; break;
        }


        return changedStatus;
    }
});
