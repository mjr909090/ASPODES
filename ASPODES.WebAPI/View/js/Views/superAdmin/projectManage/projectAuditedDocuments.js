//待审核文档
var projectAuditedDocuments = angular.module('ngProjectAuditedDocumentsApp',[]);

projectAuditedDocuments.controller('ngProjectAuditedDocumentsCtrl', function($scope) {
    //待审核文档申请
    $scope.showCheckAuditedDocumentsModal = function(){//审核通过
        $("#checkAuditedDocumentsModal").modal("show");
    };
    $scope.showRejectAuditedDocumentsModal = function(){//驳回
        $("#rejectAuditedDocumentsModal").modal("show");
    };

});