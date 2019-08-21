var selectFunctionApp = angular.module('selectAllFunction', []);
selectFunctionApp.service('selectService', function ($http) {

    var self = this;

    this.selectAll = function ($scope,list,loop,id) {
        if ($scope.select_all) {
            $scope.checked = [];
            angular.forEach(list, function (loop) {
                loop.checked = true;
                $scope.checked.push(id);
            })
        } else {
            angular.forEach(list, function (loop) {
                loop.checked = false;
                $scope.checked = [];
            })
        }
        console.log($scope.checked);
    }
    //   ***************单选********************
    this.selectSingle = function ($scope) {
        angular.forEach($scope.divisionApplies, function (divisionApply) {
            var index = $scope.checked.indexOf(divisionApply.ApplicationId);
            if (divisionApply.checked && index === -1) {
                $scope.checked.push(divisionApply.ApplicationId);
            } else if (!divisionApply.checked && index !== -1) {
                $scope.checked.splice(index, 1);
            };
        })

        if ($scope.divisionApplies.length === $scope.checked.length) {
            $scope.select_all = true;
        } else {
            $scope.select_all = false;
        }
        console.log($scope.checked);
    }

})