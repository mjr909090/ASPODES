var applyUnitApp = angular.module('ngApplyUnitApp',['ngRoute']);

//for Two level routing
applyUnitApp.config([
	'$routeProvider',
	function($routeProvider) {
		$routeProvider
			.when('/ApplyUnit', {
				templateUrl: 'Views/expertReview/preReview.html',
				controller: 'ngApplyUnitCtrl'
			})
	}
]);

applyUnitApp.controller('ngApplyUnitCtrl', function($scope) {
	$scope.msg = "This is ApplyUnit page."
})