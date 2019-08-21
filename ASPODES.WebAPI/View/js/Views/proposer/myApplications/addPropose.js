
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

var ApplicationId = getQueryString("ApplicationId");


var addProposeApp = angular.module('ngAddProposeApp', ["angucomplete-alt", "ngFileUpload", "ngCookies", "ngVerify", "ngMessages", "interceptorFactory"]);

addProposeApp.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptor');
})

addProposeApp.controller('ngAddProposeCtrl', ['$scope', '$http', 'Upload', '$timeout', '$cookieStore', '$rootScope', function ($scope, $http, Upload, $timeout, $cookieStore, $rootScope) {

    $scope.ApplicationId = ApplicationId;
    $rootScope.isEdit = getQueryString("isEdit");
    
    $scope.propose = {};
    $scope.Field = {};
    $scope.yusuanYear = "firstYear";
    $scope.assistPersonId = { postParentId: "" };
    $scope.addInstBudgetItem = {
        "Amount": 0
    };

    $scope.manageAnnualBudget = {
        "AnnualBudgetItemId": null
    };
    $scope.addAnnualBudgetItemYear = null;
    $scope.fileUploadType = 0;
    $scope.BasicInfo = { submitted: false };
	

	
	
/********************************其它function********************************************/
	
    //跳转到下一页
	$rootScope.toNext = function () {
	    var $active = $('.wizard .nav-tabs li.active');
	    $active.next().removeClass('disabled');
	    nextTab($active);
	}

	$rootScope.toPre = function () {
	    var $active = $('.wizard .nav-tabs li.active');
	    prevTab($active);
	}

    //清除modal所选人员
	$scope.cleanPerson = function () {
	    $scope.selectedInstPerson = null;
	    $scope.selectedAssistPerson = null;
	    $scope.leaderPersonTask = null;
	    $scope.assistPersonTask = null;
	    $scope.$broadcast('angucomplete-alt:clearInput');
	}
	


    /********************************Complete********************************************/
	
	
    
}]);



addProposeApp.constant('fileUploadTypeConstant', {
    mainBody: 0,
    other: 1,
    PDF: 2
})



