addProposeApp.component('budgetmanage', {
    templateUrl: '/View/Views/proposer/myApplications/addProposeComponents/BudgetManage/budgetManage.html',
    controller: budgetManageCtrl,
    bindings: {
        ApplicationId: '<',
    }
});


function budgetManageCtrl($scope, $http, $rootScope) {

    $scope.goToFilePage = function () {
        $rootScope.$broadcast('initFiles', true);
        $rootScope.toNext();
    }

    $scope.backPage = function () {
        $rootScope.toPre();
    }

}