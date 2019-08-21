var auditMidtermReportApp = angular.module('ngAuditMidtermReportApp', []);

auditMidtermReportApp.controller('ngAuditMidtermReportCtrl', ['$scope', '$http', function ($scope, $http) {
    $scope.showRejectConfirmModal = function () {
        $("#rejectApplication").modal();
    }

    $scope.showPassedConfirmModal = function () {
        $("#submitApplication").modal();
    }

}])