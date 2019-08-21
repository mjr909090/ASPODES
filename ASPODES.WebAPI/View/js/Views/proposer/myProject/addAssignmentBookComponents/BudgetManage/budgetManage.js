addTaskApp.component('budgetmanage', {
    templateUrl: '/View/Views/proposer/myProject/addAssignmentBookComponents/BudgetManage/budgetManage.html',
    controller: budgetManageCtrl,
    bindings: {
        ApplicationId: '<',
    }
});


function budgetManageCtrl($scope, $http, $rootScope) {

    $scope.goToFilePage = function () {
        $rootScope.toNext();
    }

    $scope.backPage = function () {
        $rootScope.toPre();
    }

}