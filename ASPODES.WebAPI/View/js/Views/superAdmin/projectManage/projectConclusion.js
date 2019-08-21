/**
 * Created by majunjun on 2017/5/12.
 */
//已结题项目
var projectConclusion = angular.module('ngProjectConclusionApp',[]);

projectConclusion.controller('ngProjectConclusionCtrl', function($scope) {
    $scope.showProjectInfo = function (projectId) {
        var iTop = (window.screen.availHeight - 30 - 600) / 2;
        var iLeft = (window.screen.availWidth - 10 - 1000) / 2;
        var url = "Views/divisionAdmin/projectManage/projectInfo.html?ProjectId=" + projectId;
        window.open(url, "", 'height=600,width=1000,top=' + iTop + ',left=' + iLeft + ',status=no,toolbar=no,menubar=no,location=no,resizable=no,scrollbars=0,titlebar=no');
    }
    $scope.showTaskModal = function () {
        $("#taskModal").modal('show');
    }
    $scope.showFinishedTaskModal = function () {
        $("#finishedTaskModal").modal('show');
    }
});