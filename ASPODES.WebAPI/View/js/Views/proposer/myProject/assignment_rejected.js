var assignmentRejectedApp = angular.module('ngAssignmentRejectedApp',[]);

assignmentRejectedApp.controller('ngAssignmentRejectedCtrl',function($scope){
	
	$scope.showSuggestionModal = function(){
		$("#showSuggestionModal").modal("show");
	}
	
});