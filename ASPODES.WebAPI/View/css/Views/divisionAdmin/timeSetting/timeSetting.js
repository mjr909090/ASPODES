var timeSettingApp = angular.module('ngTimeSettingApp',['ngRoute']);

//for Two level routing
timeSettingApp.config([
	'$routeProvider',
	function($routeProvider) {
		$routeProvider
			.when('/timeSetting', {
				templateUrl: 'Views/divisionAdmin/timeSetting/timeSetting.html',
				controller: 'ngTimeSettingCtrl'
			})
	}
]);

timeSettingApp.controller('ngTimeSettingCtrl', function($scope) {
	$scope.msg = 'this is timeSetting page';
})