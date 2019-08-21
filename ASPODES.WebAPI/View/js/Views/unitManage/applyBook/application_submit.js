var applySubmitApp = angular.module('ngApplySubmitApp',['ngRoute']);

//for Two level routing
applySubmitApp.config([
	'$routeProvider',
	function($routeProvider) {
		$routeProvider
			.when('/ApplySubmit', {
				templateUrl: 'Views/expertReview/preReview.html',
				controller: 'ngApplySubmitCtrl'
			})
	}
]);

applySubmitApp.controller('ngApplySubmitCtrl', function($scope) {
	$scope.msg = "This is ApplySubmit page."
})