addProposeApp.component('leaderdivisionlist', {
    templateUrl: '/View/Views/proposer/myApplications/addProposeComponents/MemberManage/leaderDivisionList.html',
    controller: leaderDivisionCtrl,
    bindings: {
        ApplicationId: '<',
    }
});


function leaderDivisionCtrl($scope, $http, $rootScope) {
    $scope.postPersonId = null;


    //清除modal所选人员
    $scope.cleanPerson = function () {
        $scope.selectedInstPerson = null;
        $scope.leaderPersonTask = null;
        $scope.$broadcast('angucomplete-alt:clearInput');
    }


    //Url
    var getPersonListUrl = "/api/member/";
    var getSelfInstPersonUrl = "/api/person/combo/inst/0";
    var postAddPersonUrl = "/api/member";
    var delPersonUrl = "/api/member/";

    //获取本单位所有人员
    $scope.getSelfInstitutePerson = function () {
        $http.get(getSelfInstPersonUrl)
        .success(function (response) {
            $scope.selfInstitutePerson = response.response;
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
    };


    //获取主持单位参与人员列表
    $scope.getLeaderPersonList = function () {
        $http.get(getPersonListUrl + "leaderinst/" + ApplicationId)
        .success(function (response) {
            $scope.leaderPersons = response.response;
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


    //发送添加的主持单位人员
    $scope.addLeaderPerson = function (leaderPersonId) {
        $http.post(postAddPersonUrl, {
            "ApplicationId": ApplicationId,
            "PersonId": leaderPersonId,
            "Task": $scope.leaderPersonTask
        })

        .success(function (response) {
            if (response.status == 0) {
                notif({
                    msg: "<b>恭喜:</b> 成功添加主持单位人员条目",
                    type: "success"
                });
                $scope.getLeaderPersonList();
            }
        })
        .error(function (response) {
            if (response.status == 4) {
                notif({
                    type: "error",
                    msg: "错误：" + response.errorMsg,
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            }
        });
        $scope.cleanPerson();
    }


    //删除单位成员
    $scope.delPerson = function () {
        if ($scope.postPersonId == null) {
            notif({
                type: "error",
                msg: "错误：请至少选择一个人员",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        }
        else {
            $http.delete(delPersonUrl + ApplicationId + "/" + $scope.postPersonId)
            .success(function (response) {
                if (response.status == 0) {
                    notif({
                        msg: "<b>恭喜:</b> 删除人员条目成功",
                        type: "success"
                    });
                    $scope.getLeaderPersonList();
                    $scope.postPersonId = null;
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
    }



    //初始化
    $scope.$on('initMember', function () {
        $scope.getSelfInstitutePerson();
            $scope.getLeaderPersonList();
    })



}
