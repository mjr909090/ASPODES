//������ĵ�
var projectAuditedDocuments = angular.module('ngProjectAuditedDocumentsApp',[]);

projectAuditedDocuments.controller('ngProjectAuditedDocumentsCtrl', function($scope) {
    //������ĵ�����
    $scope.showCheckAuditedDocumentsModal = function(){//���ͨ��
        $("#checkAuditedDocumentsModal").modal("show");
    };
    $scope.showRejectAuditedDocumentsModal = function(){//����
        $("#rejectAuditedDocumentsModal").modal("show");
    };

});