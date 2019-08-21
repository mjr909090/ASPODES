//文档模板

var documentsTemplate = angular.module('ngDocumentsTemplateApp',[]);

documentsTemplate.controller('ngDocumentsTemplateCtrl', function($scope) {
    $scope.showModifyDocumentModal = function(){//编辑
        $("#modifyDocumentModal").modal("show");
    };
    $scope.showDeleteDocumentModal = function(){//删除
        $("#deleteDocumentModal").modal("show");
    };
});

