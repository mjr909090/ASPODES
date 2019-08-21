angular.module('ngProjectHostApp', [])
    .component('projectFile', {
        templateUrl: '/View/Views/superAdmin/projectManage/projectInfo/projectFile.html',
        controller: ProjectFileController,
        bindings: {
        }
    })

function ProjectFileController($scope) {

    $scope.showAllDocument = false;

}
