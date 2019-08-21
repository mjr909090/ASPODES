addProposeApp.component('membermanage', {
    templateUrl: '/View/Views/proposer/myApplications/addProposeComponents/MemberManage/memberManage.html',
    controller: memberManageCtrl,
    bindings: {
        ApplicationId: '<',
    }
});


function memberManageCtrl($scope, $http, $rootScope) {

    $scope.postLeaderTask = function () {
        $scope.$broadcast("postLeaderTask", true);
        $rootScope.$broadcast("initBudget",true);
    }

    $scope.backPage = function(){
        $rootScope.toPre();
    }

}