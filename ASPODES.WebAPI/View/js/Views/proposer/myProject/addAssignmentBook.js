
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

var TaskId = getQueryString("TaskId");
var ProjectId = getQueryString("ProjectId");

var addTaskApp = angular.module('ngAddTaskApp', ["angucomplete-alt", "ngFileUpload", "ngCookies", "ngVerify", "ngMessages", "interceptorFactory", "constParamsModule", "xeditable"]);

addTaskApp.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptor');
})

addTaskApp.controller('ngAddTaskCtrl', ['$scope', '$http', 'Upload', '$timeout', '$cookieStore', '$rootScope', function ($scope, $http, Upload, $timeout, $cookieStore, $rootScope) {
	

	
	
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
	


    /********************************Complete********************************************/
	
	
    
}]);

addTaskApp.run(['editableOptions', function (editableOptions) {
    editableOptions.theme = 'bs3'; // bootstrap3 theme. Can be also 'bs2', 'default'
}]);


addTaskApp.constant('fileUploadType', {
    yearReport: 0,
    mainBody: 1,
    other: 2,
    PDF: 3
})







