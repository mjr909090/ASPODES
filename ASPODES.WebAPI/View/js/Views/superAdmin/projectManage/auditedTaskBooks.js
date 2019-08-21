//待审核任务书

var auditedTaskBooks = angular.module('ngAuditedTaskBooksApp',[]);

auditedTaskBooks.controller('ngAuditedTaskBooksCtrl', function ($scope) {

    $scope.downloadPDF = function () {
        $("#pdfDownloadModal").modal("show");
    }

    $scope.auditPassed = function () {
        $("#auditPassedModal").modal("show");
    }

    $scope.showRejectConfirmModal = function () {
        $("#rejectModal").modal("show");
    }
    $scope.showPassedConfirmModal = function () {
        $("#passedModal").modal("show");
    }

    $scope.showProjectInfo = function (projectId) {
        var iTop = (window.screen.availHeight - 30 - 600) / 2;
        var iLeft = (window.screen.availWidth - 10 - 1000) / 2;
        var url = "Views/divisionAdmin/projectManage/projectInfo.html?ProjectId=" + projectId;
        window.open(url, "", 'height=600,width=1000,top=' + iTop + ',left=' + iLeft + ',status=no,toolbar=no,menubar=no,location=no,resizable=no,scrollbars=0,titlebar=no');
    }



    $scope.showCheckPassTaskBooksModal = function(){//审核通过
        $("#checkPassAuditedTaskBooksModal").modal("show");
    };

    $scope.showRejectTaskBooksModal = function(){//驳回
        $("#rejectAuditedTaskBooksModal").modal("show");
    };
});
