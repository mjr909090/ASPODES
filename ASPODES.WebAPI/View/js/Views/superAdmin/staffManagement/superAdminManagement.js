//院管理员

var superAdminManagement = angular.module('ngSuperAdminManagementApp',[]);

superAdminManagement.controller('ngSuperAdminManagementCtrl', function($scope) {
    $scope.exportExcelSuperAdmin = function(){//导出
        $("#exportExcelSuperAdminModal").modal("show");
    };
    $scope.showModifySuperAdminModal = function(){//编辑
        $("#modifySuperAdminModal").modal("show");
    };
    $scope.showDeleteSuperAdminModal = function(){//删除
        $("#deleteSuperAdminModal").modal("show");
    };
});

