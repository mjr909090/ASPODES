//定向委托

var directedDelegation = angular.module('ngDirectedDelegationApp',[]);

directedDelegation.controller('ngDirectedDelegationCtrl', function($scope) {
    $scope.showRejectApplicationModal = function(){//不受理的模态框
        $("#rejectApplication").modal("show");
    };
    $scope.showRejectApplicationModal2 = function(){ $("#rejectApplication").modal("show"); };
    $scope.showRejectApplicationModal3 = function(){ $("#rejectApplication").modal("show"); };
    $scope.showRejectApplicationModal4 = function(){ $("#rejectApplication").modal("show"); };
    $scope.showRejectApplicationModal5 = function(){ $("#rejectApplication").modal("show"); };
    $scope.showRejectApplicationModal6 = function(){ $("#rejectApplication").modal("show"); };

    $scope.showHandleModal = function(){//受理的模态框
        $("#handleApplication").modal("show");
    };
    $scope.showHandleModal2 = function(){ $("#handleApplication").modal("show"); };
    $scope.showHandleModal3 = function(){ $("#handleApplication").modal("show"); };
    $scope.showHandleModal4 = function(){ $("#handleApplication").modal("show"); };
    $scope.showHandleModal5 = function(){ $("#handleApplication").modal("show"); };
    $scope.showHandleModal6 = function(){ $("#handleApplication").modal("show"); };

    //第一行记录
    $scope.isShow1 = true;
    $scope.waitHandle1 = true;//待受理
    $scope.handleApplicationSure = function() {//受理模态框中的确定
        console.log('受理的确定');
        //状态
        $scope.waitHandle1 = false;//待受理
        $scope.hasHandle1 = true;//已受理
        $scope.notHnadle1 = false;//不受理
    };
    $scope.handleApplicationNotSure = function() {//不受理模态框中的确定
        console.log('不受理的确定');
        //状态
        $scope.waitHandle1 = false;//待受理
        $scope.notHnadle1 = true;//不受理
        $scope.hasHandle1 = false;//已受理
    };
});
