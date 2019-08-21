//¥˝…Û∫ÀΩ·Ã‚…Í«Î
var auditedConclusionApplication = angular.module('ngAuditedConclusionApplicationApp',[]);

auditedConclusionApplication.controller('ngAuditedConclusionApplicationCtrl', function($scope) {
    //…Û∫ÀΩ·Ã‚…Í«Î
    $scope.showRejectConclusionModal = function(){
        $("#rejectConclusionModal").modal("show");
    };

    $scope.showCheckConclusionPassModal = function(){
        $("#checkAuditedDocumentsModal").modal("show");
    };
});
