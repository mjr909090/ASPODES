var scoreDescriptionSettingApp = angular.module('ngScoreDescriptionApp', []);

scoreDescriptionSettingApp.controller('ngScoreDescriptionSettingCtrl', function ($scope, $http, Upload, $timeout) {

    //URL
    var scoreDescriptionFileUploadUrl = "/api/ReviewDoc";
    var getscoreDescriptionFileListUrl = "/api/ReviewDocList";

    //$scope.fileNameList = {
    //    Total: null,
    //    AllianceFocusWorkfileclass: null,
    //    EmergencyResearchWorkfileclass: null,
    //    BasicScienceAndTechnologyWorkfileclass: null,
    //    BasicResearchGuidancePlanfileclass: null,
    //    MajorResultsDevelopmentProgramfileclass: null,
    //    MajorPlatformPromotionPlanfileclass: null,
    //    MajorProjectReservePlanfileclass: null,
    //    AgriculturalThinkTankConstructionPlanfileclass: null
    //}

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

    //文件上传
    $scope.uploadscoreDescriptionFiles = function (files, errFiles) {
        $scope.uploadComplete = false;
        $scope.files = files;
        $scope.errFiles = errFiles;
        angular.forEach(files, function (file) {
            file.upload = Upload.upload({
                url: scoreDescriptionFileUploadUrl,
                file: file,
                data: {
                    Type: $scope.fileType
                }
            });

            file.upload
                .success(function (response) {
                    $scope.getscoreDescriptionFileList();
                    })
                    //switch ($scope.fileType) {
                    //    case "Total": $scope.fileNameList.Total = response.response; break;
                    //    case "AllianceFocusWorkfileclass": $scope.fileNameList.AllianceFocusWorkfileclass = response.response; break;
                    //    case "EmergencyResearchWorkfileclass": $scope.fileNameList.EmergencyResearchWorkfileclass = response.response; break;
                    //    case "BasicScienceAndTechnologyWorkfileclass": $scope.fileNameList.BasicScienceAndTechnologyWorkfileclass = response.response; break;
                    //    case "BasicResearchGuidancePlanfileclass": $scope.fileNameList.BasicResearchGuidancePlanfileclass = response.response; break;
                    //    case "MajorResultsDevelopmentProgramfileclass": $scope.fileNameList.MajorResultsDevelopmentProgramfileclass = response.response; break;
                    //    case "MajorPlatformPromotionPlanfileclass": $scope.fileNameList.MajorPlatformPromotionPlanfileclass = response.response; break;
                    //    case "MajorProjectReservePlanfileclass": $scope.fileNameList.MajorProjectReservePlanfileclass = response.response; break;
                    //    case "AgriculturalThinkTankConstructionPlanfileclass": $scope.fileNameList.AgriculturalThinkTankConstructionPlanfileclass = response.response; break;
                    //}
                //})
                .then(
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
        })
    }

    $scope.getscoreDescriptionFileList();

})