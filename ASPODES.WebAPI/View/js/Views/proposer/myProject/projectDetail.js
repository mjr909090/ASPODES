function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

var projectId = getQueryString("projectId");


var projectDetailApp = angular.module('ngProjectDetailApp', []);
projectDetailApp.controller('ngProjectDetailCtrl', function ($scope, $http) {

    $scope.showMemberModal = function () {
        $("#memberDetailModal").modal('show');
    }

    $scope.showBudgetModal = function () {
        $("#budgetDetailModal").modal('show');
    }

    $scope.showFileUploadModal = function () {
        $("#fileUploadModal").modal('show');
    }


    var getProjectDetailUrl = '/api/project/detail/';
    var downloadPDFUrl = '/api/annualTaskDoc';
    var downloadProjectDocUrl = '/api/projectdoc/download/';
    var downloadEndReportUrl = '/api/projectdoc/download/endreport/';

    //获取项目基本信息
    $scope.getProjectDetail = function () {
        $http({
            method: 'GET',
            url: getProjectDetailUrl + projectId
        })
        .success(function (response) {
            if (response.status == 0) {
                $scope.projectDetail = response.response;
            }
            else {
                notif({
                    type: "error",
                    msg: "错误：" + response.errorMsg,
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "错误：" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }


    //下载PDF
    $scope.downloadPDF = function (annualTaskId) {
        $downloadFile.downloadFileType(downloadPDFUrl + '?annualTaskId=' + annualTaskId + '&docType=3', '.pdf', '任务书');
    }

    //下载文档
    $scope.downloadDoc = function (projectDocId, fileName) {
        $downloadFile.downloadFileWithoutFileType(downloadProjectDocUrl + projectDocId, fileName);
    }

    //下载结题报告
    $scope.downloadEndReport = function (projectDocId, fileName) {
        $downloadFile.downloadFileType(downloadProjectDocUrl + projectDocId, '.pdf', fileName);
    }

    //下载年度任务报告
    $scope.downloadYearReport = function (annualTaskId) {
        $downloadFile.downloadFileType(downloadPDFUrl + '?annualTaskId=' + annualTaskId + '&docType=0', '.pdf', '年度报告');
    }



    //初始化
    $scope.getProjectDetail();

});



projectDetailApp.filter('changeAnnualTaskStatus', function () {
    return function (status) {
        var changedStatus = '';

        switch (status) {
            case 0: changedStatus = '新建'; break;
            case 1: changedStatus = '待单位审核'; break;
            case 2: changedStatus = '单位驳回'; break;
            case 3: changedStatus = '待院审核'; break;
            case 4: changedStatus = '院驳回'; break;
            case 5: changedStatus = '待提交年度报告'; break;
            case 6: changedStatus = '待单位审核年度报告'; break;
            case 7: changedStatus = '单位驳回年度报告'; break;
            case 8: changedStatus = '待院审核年度报告'; break;
            case 9: changedStatus = '院驳回年度报告'; break;
            case 10: changedStatus = '已完成'; break;
        }


        return changedStatus;
    }
});

projectDetailApp.filter('changeFileType', function () {
    return function (status) {
        var changedStatus = '';

        switch (status) {
            case 0: changedStatus = '结题报告'; break;
            case 1: changedStatus = '其它'; break;
        }


        return changedStatus;
    }
})