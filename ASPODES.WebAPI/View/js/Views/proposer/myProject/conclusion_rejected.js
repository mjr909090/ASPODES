var conclusionRejectedApp = angular.module('ngConclusionRejectedApp', []);

conclusionRejectedApp.controller('ngConclusionRejectedCtrl', function($scope) {
	
	$scope.showSuggestionModal = function(){
		$("#showSuggestionModal").modal("show");
	}

})