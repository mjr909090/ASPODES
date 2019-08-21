var pmApp = angular.module('ngPMApp',['ngRoute']);

//for Two level routing
pmApp.config([
	'$routeProvider',
	function($routeProvider) {
		$routeProvider
			.when('/pm', {
				templateUrl: 'Views/expertReview/preReview.html',
				controller: 'ngPMCtrl'
			})
	}
]);

pmApp.controller('ngPMCtrl', function($scope) {
	$scope.msg = "This is pm page."
})