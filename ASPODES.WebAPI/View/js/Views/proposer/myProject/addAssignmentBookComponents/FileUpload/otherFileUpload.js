addTaskApp.component('otherfileupload', {
    templateUrl: '/View/Views/proposer/myProject/addAssignmentBookComponents/FileUpload/fileUpload.html',
    controller: otherFileUploadCtrl,
    bindings: {
        ApplicationId: '<',
    }
});


function otherFileUploadCtrl($scope, $http, Upload, $timeout, $rootScope, annualTaskFileType) {

    $scope.uploadType = "other";
    $scope.filterDocType = 2;
    $scope.updataModalName = "otherUpdataModal";
    $scope.deleteModalName = "otherDeleteModal";

    //URL
    var getFileListUrl = "/api/annualTaskDoc/";
    var fileUploadUrl = "/api/annualTaskDoc/upload";
    var fileDeleteUrl = "/api/annualTaskDoc/"


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
        $('#otherUpdataModal').modal({ backdrop: 'static', keyboard: false });
    }

    $scope.openFielDeleteModal = function () {
        $('#otherDeleteModal').modal("show");
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
                    if (response.status > 0) {
                        $scope.showErrorMsg = response.status + ':' + response.errorMsg;
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

    //uploadFile模态框消失时获取文件列表刷新
    $('#otherUpdataModal').on('hidden.bs.modal', function (e) {
        $scope.getFileList();
    })

    $scope.clearFiles = function () {
        $scope.files = {};
        $scope.errFiles = {};
        $scope.showErrorMsg = null;
    }


    $scope.toNextPage = function () {
        $rootScope.toNext();
    }

    $scope.backPage = function () {
        $rootScope.toPre();
    }

    //获取文件列表
    $scope.getFileList();

}
