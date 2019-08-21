addTaskApp.component('leaderdivisionlist', {
    templateUrl: '/View/Views/proposer/myProject/addAssignmentBookComponents/MemberManage/leaderDivisionList.html',
    controller: leaderDivisionCtrl,
    bindings: {
        ApplicationId: '<',
        hostDepartMember: '='
    }
});


function leaderDivisionCtrl($scope, $http, $rootScope) {



}
