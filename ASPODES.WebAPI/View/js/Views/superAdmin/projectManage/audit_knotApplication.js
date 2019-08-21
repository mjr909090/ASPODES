var auditKnotApplicationApp = angular.module('ngAuditKnotApplicationApp', []);

auditKnotApplicationApp.controller('ngAuditKnotApplicationCtrl', function($scope) {
	//全文
	$scope.downloadPDF = function(){
		$("#pdfDownloadModal").modal("show");
	}
	//年度任务书
	$scope.showSubmitTaskBookModal = function(){
		$("#submitTaskBookModal").modal("show");
	}
	//结题报告
	$scope.showSubmitknotReportModal = function(){
		$("#submitKnotReportModal").modal("show");
	}

})