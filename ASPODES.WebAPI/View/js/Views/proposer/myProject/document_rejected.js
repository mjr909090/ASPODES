var documentRejectedApp = angular.module('ngDocumentRejectedApp',[]);

documentRejectedApp.controller('ngDocumentRejectedCtrl',function($scope){
	
	$scope.showSuggestionModal = function(){
		$("#showSuggestionModal").modal("show");
	}
	
});