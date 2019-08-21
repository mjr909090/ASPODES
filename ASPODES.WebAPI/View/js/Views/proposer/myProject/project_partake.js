var projectPartakeApp = angular.module('ngProjectPartakeApp', []);

projectPartakeApp.controller('ngProjectPartakeCtrl', function ($scope, $http) {

    var getParticipantProjectUrl = "/api/project/participant/person";


    $scope.jumpToDetail = function (projectId) {
        window.open("/View/Views/proposer/myProject/projectDetail.html?projectId=" + projectId);
    }


    $scope.getParticipantProjectNow = function () {
        $http({
            methon: 'GET',
            url: getParticipantProjectUrl,
            params: {
                Status: 0
            }
        })
        .success(function (response) {
            if (response.status == 0) {
                $scope.nowProject = response.response;
            }
            else {
                notif({
                    type: "error",
                    msg: "´íÎó£º" + response.errorMsg,
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            }
        })
        .error(function () {
            notif({
                type: "error",
                msg: "´íÎó£º" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }


    $scope.getParticipantProjectPast = function () {
        $http({
            methon: 'GET',
            url: getParticipantProjectUrl,
            params: {
                Status: 1
            }
        })
        .success(function (response) {
            if (response.status == 0) {
                $scope.pastProject = response.response;
            }
            else {
                notif({
                    type: "error",
                    msg: "´íÎó£º" + response.errorMsg,
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            }
        })
        .error(function () {
            notif({
                type: "error",
                msg: "´íÎó£º" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }


    //show projectInfo details
    $scope.showProjectInfo = function (projectInfo) {
        
        $scope.projectData = projectInfo;
        $scope.docData = $scope.projectData.Docs;

        if ($scope.docData.length == 0) {
            $scope.docIsNull = true;
        } else {
            $scope.docIsNull = false;
        }

        $("#projectDetail").modal('show');
    }



    //Init
    $scope.getParticipantProjectNow();
    $scope.getParticipantProjectPast();

});


projectPartakeApp.directive('projectFile', function () {
    return {
        restrict: 'E',
        templateUrl: '/View/Views/superAdmin/projectManage/projectInfo/projectFile.html',
        scope: true,
        controller: function () {
            this.showAllDocument = false;
        }
    }
});

projectPartakeApp.directive('tabs', function () {
    return {
        restrict: 'E',
        scope: {},
        transclude: true,
        controller: function ($scope) {
            var that = this;
            this.tabs = [];
            this.addTab = function addTab(tab) {
                this.tabs.push(tab);
            };
            this.selectTab = function selectTab(index) {
                for (var i = 0; i < this.tabs.length; i++) {
                    this.tabs[i].selected = false;
                }
                this.tabs[index].selected = true;
            };
            $scope.$on("informTab", function (event, data) {
                that.selectTab(0);
            });
        },
        controllerAs: 'tabs',
        link: function ($scope, $element, $attrs, $ctrl) {
            $ctrl.selectTab($attrs.active || 0);
        },
        template:
            '<div>' +
                '<ul class="nav nav-tabs">' +
                    '<li ng-repeat="tab in tabs.tabs" ng-class="{true:\'active\'}[tab.selected]">' +
                        '<a ng-click="tabs.selectTab($index);" ng-bind="tab.label"></a>' +
                    '</li>' +
                '</ul>' +
                '<div style="margin-top:30px" ng-transclude></div>' +
            '</div>'
    };
});

projectPartakeApp.directive('tab', function () {
    return {
        restrict: 'E',
        scope: {
            label: '@'
        },
        require: '^tabs',
        transclude: true,
        template:
            '<div ng-if="tab.selected">' +
                '<div ng-transclude></div>' +
            '</div>'
        ,
        link: function ($scope, $element, $attrs, $ctrl) {
            $scope.tab = {
                label: $scope.label,
                selected: false
            };
            $ctrl.addTab($scope.tab);
        }
    }
});

