addProposeApp.component('leaderinfo', {
    templateUrl: '/View/Views/proposer/myApplications/addProposeComponents/MemberManage/leaderInfo.html',
    controller: leaderInfoCtrl,
    bindings: {
        ApplicationId: '<',
    }
});


function leaderInfoCtrl($scope, $http, $cookieStore, $rootScope) {

    //url
    var getLeaderInfoUrl = "/api/Person/";
    var getLeaderTaskUrl = "/api/member/";
    var postAddPersonUrl = "/api/member";

    //获取负责人个人信息
    $scope.getLeaderInfo = function () {
        $http.get(getLeaderInfoUrl + $cookieStore.get("PersonId"))
        .success(function (response) {
            $scope.leader = response.response;
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


    //获取负责人分工
    $scope.getLeaderTask = function () {
        $http.get(getLeaderTaskUrl + ApplicationId + "/" + $cookieStore.get("PersonId"))
        .success(function (response) {
                $scope.leaderTask = response.response.Task;
        })
        .error(function (response) {
            //if ($rootScope.isEdit == true) {
                notif({
                    type: "error",
                    msg: "错误：" + response.errorMsg,
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            //}
        })
    }

    //发送负责人分工并下一步
    $scope.postLeaderTask = function () {
        $http.put(postAddPersonUrl, {
            "ApplicationId": ApplicationId,
            "PersonId": $scope.leader.PersonId,
            "Task": $scope.leaderTask
        })
        .success(function (response) {
            if (response.status == 0) {
                notif({
                    msg: "<b>恭喜:</b> 成功保存",
                    type: "success"
                });
                $rootScope.toNext();
            }
            else {
                notif({
                    type: "error",
                    msg: "错误：" + response.errorMsg,
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            }
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



    //监听memberManage中的广播事件
    $scope.$on("postLeaderTask", function (e, saveStatus) {
        $scope.postLeaderTask();
    })



    //初始化
    $scope.$on('initMember', function () {
        $scope.getLeaderInfo();
        $scope.getLeaderTask()
    })
    
}