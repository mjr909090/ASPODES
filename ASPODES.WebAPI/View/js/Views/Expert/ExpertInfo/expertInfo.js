var expertInfoApp = angular.module('ngExpertInfoApp', []);

expertInfoApp.controller('ngExpertInfoCtrl', function($scope) {
	//基本信息-编辑-模态框，出现
	$scope.editPrimaryInfo = function(){
		$("#editPrimaryModal").modal("show");
	}
	//专业信息-编辑-模态框，出现
	$scope.editExpertInfo = function(){
		$("#editExpertModal").modal("show");
	}

})