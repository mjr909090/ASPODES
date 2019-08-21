var erApp = angular.module('ngERApp',['ngRoute']);

//for Two level routing
erApp.config([
	'$routeProvider',
	function($routeProvider) {
		$routeProvider
			.when('/er', {
				templateUrl: 'Views/expertReview/preReview.html',
				controller: 'ngERCtrl'
			})
	}
]);

erApp.controller('ngERCtrl', function($scope) {
	$scope.msg = "This is ER page."
})