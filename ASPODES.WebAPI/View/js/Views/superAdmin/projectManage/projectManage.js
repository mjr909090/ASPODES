var projectManagementApp = angular.module('ngProjectManagementApp',[]);

projectManagementApp.controller('ngProjectManagementCtrl', function($scope) {
	$scope.importApplication = function(){
        $("#importApplication").modal("show");
    };

    //��˽�������
    $scope.showRejectApplicationModal = function(){
        $("#rejectApplication").modal("show");
    };

    $scope.showHandleModal = function(){
        $("#handleApplication").modal("show");
    };

    //������ĵ�����
    $scope.showCheckAuditedDocumentsModal = function(){//���ͨ��
        $("#checkAuditedDocumentsModal").modal("show");
    };
});