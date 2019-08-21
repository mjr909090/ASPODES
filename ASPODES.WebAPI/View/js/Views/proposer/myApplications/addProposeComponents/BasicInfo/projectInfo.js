

addProposeApp.component('projectinfo', {
    templateUrl: '/View/Views/proposer/myApplications/addProposeComponents/BasicInfo/projectInfo.html',
    controller: projectInfoCtrl,
    controllerAs:'pInfoCtrl',
    bindings: {
        ApplicationId: '<',
        projectSource:'='
    }
});

function projectInfoCtrl($scope, $http, $rootScope, ngVerify) {

    var that = this;

    $scope.propose = {
        "ApplicationId": ApplicationId
    }
    

    //URL
    var getProposeBasicInfoUrl = "/api/application/";
    var getFieldInfoUrl = "/api/applicationfield/";
    var getProjectTypeUrl = "/api/projectType";

    var getSupportCategoryUrl = "/api/supportcategory/";





    //获取支持类别
    $scope.getSupportCategory = function (ProjectTypeId) {
        $http.get(getSupportCategoryUrl + ProjectTypeId)
	    .success(function (response) {
	        that.projectSource.SupportCategorys = response.response;
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
    
    
    



    


    //调用$getFieldList服务获得研究领域列表
    //var pro = $getFieldList.get();
    //pro.then(function (response) {
    //    that.fields = response.data.response;
    //})



}

