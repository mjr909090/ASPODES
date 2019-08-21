
var expertAssignmentResults = angular.module('ngExpertAssignmentResultsApp', []);

expertAssignmentResults.controller('ngExpertAssignmentResultsCtrl', function ($scope, $http) {
    //url
    var expertListUrl = "/api/expert/depart/";//获取专家列表 
    var userListUrl = "/api/departperson"//get用户列表数据的url
    var deleteExpertUrl = " /api/expert/"//删除专家userId

    //获取用户列表
    $scope.getExpertNameList = function (InstituteId) {
        //alert("111");
        // alert(getExpertNameUrl + InstituteId);
        $http.get(getExpertNameUrl + InstituteId)
        .success(function (response) {
            $scope.userList = response.response;//只是第一页
        })
        .error(function () {
            alert("error!");
        });
    }



});


