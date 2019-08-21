addTaskApp.component('membermanage', {
    templateUrl: '/View/Views/proposer/myProject/addAssignmentBookComponents/MemberManage/memberManage.html',
    controller: memberManageCtrl,
    bindings: {
        ApplicationId: '<',
    }
});


function memberManageCtrl($scope, $http, $rootScope, $cookieStore) {

    var self = this;

    //url
    var getLeaderUrl = "/api/person/";
    var getProjectMemberUrl = "/api/projectmember/"

    //获取项目负责人基本信息
    $scope.getLeaderInfo = function () {
        $http.get(getLeaderUrl + $cookieStore.get("PersonId"))
	    .success(function (response) {
	        if (response.status == 0) {
	            self.leader = response.response;
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
	            self.leader = null;
	            $scope.BasicInfoCorret = true;
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
	        $scope.BasicInfoCorret = true;
	    });
    }


    //获取项目成员信息
    $scope.getProjectMember = function () {
        $http.get(getProjectMemberUrl + ProjectId)
        .success(function (response) {
            if (response.status == 0) {
                self.memberInfo = response.response;
            }
        })
    }


    //init
    $scope.getLeaderInfo();
    $scope.getProjectMember();


    $scope.backPage = function(){
        $rootScope.toPre();
    }

}