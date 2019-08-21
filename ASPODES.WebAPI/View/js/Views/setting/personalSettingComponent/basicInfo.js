addProposeApp.component('basicinfo', {
    templateUrl: '/View/Views/setting/personalSettingComponent/basicInfo.html',
    controller: basicInfoCtrl,
    bindings: {
        
    }
});


function basicInfoCtrl($scope, $http) {

    //BasicInfo
    var getBasicInfoUrl = "/api/profile";
    var putBasicInfoUrl = "/api/profile";

    $scope.getBasicInfo = function () {
        $http.get(getBasicInfoUrl)
        .success(function (response) {
            if (response.response != null) {
                $scope.BasicInfo = response.response;
                $scope.studyHistorys = response.response.EducationList;
                $scope.workingHistorys = response.response.WorkingList;
                $scope.Achievements = response.response.AchievementList;
                $scope.postInfo = {
                    "Email": response.response.Email,
                    "Phone": response.response.Phone,
                    "Address": response.response.Address
                }
                $scope.newField.PersonId = $scope.BasicInfo.PersonId;
                $scope.updataField.PersonId = $scope.BasicInfo.PersonId;
                $scope.getUserField();
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "没有获取到个人信息，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        });
    }

    $scope.putBasicInfo = function () {
        $http.put(putBasicInfoUrl, $scope.postInfo)
        .success(function (response) {
            notif({
                msg: "<b>恭喜:</b> 成功保存个人基本信息",
                type: "success"
            });
            $scope.getBasicInfo();
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "未能成功保存个人基本信息，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        });
    }

}