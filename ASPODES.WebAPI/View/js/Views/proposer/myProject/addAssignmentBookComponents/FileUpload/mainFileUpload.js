addTaskApp.component('mainfileupload', {
    templateUrl: '/View/Views/proposer/myProject/addAssignmentBookComponents/FileUpload/fileUpload.html',
    controller: mainFileUploadCtrl,
    bindings: {
        ApplicationId: '<',
    }
});


function mainFileUploadCtrl($scope, $http, Upload, $timeout, $rootScope, annualTaskFileType) {

    //URL
    var getFileListUrl = "/api/annualTaskDoc/";
    var fileUploadUrl = "/api/annualTaskDoc/upload";
    var fileDeleteUrl = "/api/annualTaskDoc";


    $scope.uploadType = "mainBody";
    $scope.filterDocType = 1;
    $scope.updataModalName = "mainUpdataModal";
    $scope.deleteModalName = "mainDeleteModal";


    //获取文件列表
    $scope.getFileList = function () {
        $http.get(getFileListUrl + TaskId + "/docs")
        .success(function (response) {
            $scope.fileList = response.response;
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


    //打开上传文件模态框
    $scope.openFileUploadModal = function () {
        $('#mainUpdataModal').modal({ backdrop: 'static', keyboard: false });
    }

    $scope.openFielDeleteModal = function () {
        $('#mainDeleteModal').modal("show");
    }

    //文件上传
    $scope.uploadFiles = function (files, errFiles) {
        $scope.files = files;
        $scope.errFiles = errFiles;
        angular.forEach(files, function (file) {
            file.upload = Upload.upload({
                url: fileUploadUrl,
                params: {
                    "annualTaskId": TaskId,
                    "docType": annualTaskFileType.getAnnualTaskFileType($scope.fileUploadType)
                },
                file: file
            });

            file.upload.then(
                function (response) {
                    $timeout(function () {
                        file.result = response.response;
                    });
                },
                function (response) {
                    if (response.data.status > 0) {
                        $scope.showErrorMsg = response.data.status + ':' + response.data.errorMsg;
                    }
                },
                function (evt) {
                    file.progress = Math.min(100, parseInt(100.0 * evt.loaded / evt.total));
                }
            )
            .then(function () {
                $scope.getFileList();
            })
        })
    }

    //文件删除
    $scope.deleteFile = function () {
        if ($scope.deleteFile.AnnualTaskDocId == null) {
            notif({
                type: "error",
                msg: "错误：请至少选择一个文件",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        }
        else {
            $http({
                method: 'DELETE',
                url: fileDeleteUrl + '/' + $scope.deleteFile.AnnualTaskDocId
                //params: {
                //    AnnualTaskDocId: $scope.deleteFile.AnnualTaskDocId
                //}
            })
            .success(function (response) {
                notif({
                    msg: "<b>恭喜:</b> 文件删除成功",
                    type: "success"
                });
                $scope.getFileList();
                $scope.deleteFile.AnnualTaskDocId == null;
            })
            .error(function () {
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
    }

    $scope.clearFiles = function () {
        $scope.files = {};
        $scope.errFiles = {};
        $scope.showErrorMsg = null;
    }

    $scope.toNextPage = function () {
        $scope.mainFileCount = 0;
        angular.forEach($scope.fileList, function (file) {
            if (file.Type == annualTaskFileType.getAnnualTaskFileType($scope.uploadType)) {
                $scope.mainFileCount += 1;
            }
        });
        if ($scope.mainFileCount > 0) {
            $rootScope.toNext();
        }
        else {
            notif({
                type: "error",
                msg: "请至少上传一个正文文件！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        }
    }

    $scope.backPage = function () {
        $rootScope.toPre();
    }

    //获取文件列表
    $scope.getFileList();
    

}