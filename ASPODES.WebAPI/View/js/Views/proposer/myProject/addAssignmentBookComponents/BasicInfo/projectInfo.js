

addTaskApp.component('projectinfo', {
    templateUrl: '/View/Views/proposer/myProject/addAssignmentBookComponents/BasicInfo/projectInfo.html',
    controller: projectInfoCtrl,
    bindings: {
        ApplicationId: '<',
    }
});

function projectInfoCtrl($scope, $http, $rootScope, ngVerify) {

    var that = this;

    $scope.propose = {
        "TaskId": TaskId
    }


    //Url
    var getAssignmentBasicInfoUrl = "/api/annualTask";
    var getProposeBasicInfoUrl = "/api/application/step/one/left";


    //获取任务书详细信息
    $scope.getProjectInfo = function () {
        $http.get(getAssignmentBasicInfoUrl + "/" + TaskId)
        .success(function (response) {

        })
    }


    //获取任务书基本信息

    $scope.getProjectBasicInfo = function () {
        $http.get(getAssignmentBasicInfoUrl + "/" + TaskId + "/basic")
        .success(function (response) {
            $scope.basicInfo = response.response;
            $rootScope.yearBudget = $scope.basicInfo.CurrentBudget;
        })
    }



    //初始化
    $scope.getProjectInfo();
    $scope.getProjectBasicInfo();

}

