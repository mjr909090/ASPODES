addTaskApp.component('leaderinfo', {
    templateUrl: '/View/Views/proposer/myProject/addAssignmentBookComponents/MemberManage/leaderInfo.html',
    controller: leaderInfoCtrl,
    bindings: {
        ApplicationId: '<',
        leaderInfoData: '='
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



    //监听memberManage中的广播事件
    $scope.$on("postLeaderTask", function (e, saveStatus) {
        $rootScope.toNext();
    })



    //初始化
    $scope.$on('initMember', function () {
        $scope.getLeaderInfo();
        $scope.getLeaderTask()
    })
    
}