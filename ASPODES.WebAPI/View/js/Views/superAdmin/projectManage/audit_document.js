var auditDocumentApp = angular.module('ngAuditDocumentApp', []);

auditDocumentApp.controller('ngAuditDocumentCtrl', function($scope) {
	//全文
	$scope.downloadPDF = function(){
		$("#pdfDownloadModal").modal("show");
	}
	
	

})