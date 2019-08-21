/**
 * Created by majunjun on 2017/4/7.
 */
var exportedApplication = angular.module('ngExportedApplicationApp',[]);


exportedApplication.controller('ngExportedApplicationCtrl', function($scope) {
   //导出Modal
    $scope.exportApplication = function(){
        $("#exportApplication").modal("show");
    };
});
