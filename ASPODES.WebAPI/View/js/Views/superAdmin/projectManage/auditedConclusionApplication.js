//����˽�������
var auditedConclusionApplication = angular.module('ngAuditedConclusionApplicationApp',[]);

auditedConclusionApplication.controller('ngAuditedConclusionApplicationCtrl', function($scope) {
    //��˽�������
    $scope.showRejectConclusionModal = function(){
        $("#rejectConclusionModal").modal("show");
    };

    $scope.showCheckConclusionPassModal = function(){
        $("#checkAuditedDocumentsModal").modal("show");
    };
});
