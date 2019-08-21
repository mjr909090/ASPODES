angular.module('ngDivisionHostApp',[])
    .component('projectFile', {
        templateUrl: '/View/Views/divisionAdmin/projectManage/projectInfo/projectFile.html',
        controller: ProjectFileController,
        bindings: {
        }
    })

function ProjectFileController($scope) {
    $scope.showAllDocument = false;
}


