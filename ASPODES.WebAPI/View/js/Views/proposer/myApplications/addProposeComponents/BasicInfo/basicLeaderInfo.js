addProposeApp.component('basicleaderinfo', {
    templateUrl: '/View/Views/proposer/myApplications/addProposeComponents/BasicInfo/basicLeaderInfo.html',
    controller: basicLeaderInfoCtrl,
    bindings: {
        ApplicationId: '<',
    }
});


function basicLeaderInfoCtrl($scope, $http, $cookieStore) {


    //Url
    var getLeaderUrl = "/api/person/";
    var getInstUrl = "/api/inst/self";

    
    //获取牵头负责人信息
    $scope.getLeaderInfo = function () {
        $http.get(getLeaderUrl + $cookieStore.get("PersonId"))
	    .success(function (response) {
	        if (response.status == 0) {
	            $scope.leader = response.response;
	            //获取承担单位信息
	            $http.get(getInstUrl)
	            .success(function (response) {
	                if (response.status == 0) {
	                    $scope.institute = response.response;
	                    if (response.response.ContactId == null) {
	                        notif({
	                            type: "error",
	                            msg: "错误：没有获取到联系人信息",
	                            position: "center",
	                            width: 500,
	                            height: 60,
	                            autohide: false
	                        });
	                        $scope.BasicInfoCorret = true;
	                    }
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
	                    $scope.institute = null;
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
	        else {
	            notif({
	                type: "error",
	                msg: "错误：" + response.errorMsg,
	                position: "center",
	                width: 500,
	                height: 60,
	                autohide: false
	            });
	            $scope.leader = null;
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


    //初始化
    $scope.getLeaderInfo();


}