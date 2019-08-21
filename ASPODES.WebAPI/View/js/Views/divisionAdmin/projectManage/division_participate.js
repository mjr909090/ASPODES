var divisionPaticipateApp = angular.module('ngDivisionPaticipateApp', ['publicFunction']);

divisionPaticipateApp.directive('projectFile', function () {
    return {
        restrict: 'E',
        templateUrl: '/View/Views/divisionAdmin/projectManage/projectInfo/projectFile.html',
        scope: true,
        controller: function () {
            this.showAllDocument = false; 
        },
    }
});

divisionPaticipateApp.controller('ngDivisionPaticipateCtrl', ['$scope', '$http', '$downloadFile', function ($scope, $http, $downloadFile) {
    var projectUrl = "/api/project/participant/institute"; //Get ������Ŀ�б��Url
    //var leaderInfoUrl = "/api/person/";//Get��������Ϣ��Url

    //********** ��ʼ������ *************
    $scope.researchPage = {
        pageItem: 0,
        count: 0,
        p_current: 0,
        p_count: 0,
        pages: []
    }
    $scope.finishedPage = {
        pageItem: 0,
        count: 0,
        p_current: 0,
        p_count: 0,
        pages: []
    }

    $scope.listAlready = false;
    $scope.researchProjectListIsNull = true;
    $scope.finishedProjectListIsNull = true;

    $scope.projectData = {
        leaderInfo: []
    }

    $scope.loadPage = function (page, name) {
        if (name == "researchProject") {
            $http({
                method: 'GET',
                url: projectUrl,
                params: {
                    Status: 0,
                    page: page
                }
            })
            .success(function (response) {
                if (response.response != null) {
                    $scope.listAlready = true;
                    $scope.researchProjectList = response.response.ItemDTOs;
                    //console.log($scope.divisionApplies);
                    $scope.researchPage.pageItem = response.response.TotalPageNum;//�ܹ�����ҳ
                    $scope.researchPage.count = response.response.TotalNum; //ҳ���ܹ�������
                    $scope.researchPage.p_current = response.response.NowPage; //��ǰ�ڼ�ҳ
                    $scope.researchPage.p_count = response.response.NowNum; //��ǰҳ�������
                    for (var i = 0; i < $scope.researchPage.pageItem; i++) {
                        $scope.researchPage.pages[i] = i + 1;
                    }
                    if ($scope.researchProjectList.length == 0) {
                        $scope.researchProjectListIsNull = true;
                    }
                    if ($scope.researchProjectList.length != 0) {
                        $scope.researchProjectListIsNull = false;
                    }
                }
            })
            .error(function () {
                notif({
                    msg: "<b>����:</b> ������Ŀ�б����ʧ�ܣ����������Ƿ�ͨ��",
                    type: "error"
                });
            });
        }
        if (name == "finishedProject") {
            $http({
                method: 'GET',
                url: projectUrl,
                params: {
                    Status: 1,
                    page: page
                }
            })
            .success(function (response) {
                if (response.response != null) {
                    $scope.finishedListAlready = true;
                    $scope.finishedProjectList = response.response.ItemDTOs;
                    $scope.finishedPage.pageItem = response.response.TotalPageNum;//�ܹ�����ҳ
                    $scope.finishedPage.count = response.response.TotalNum; //ҳ���ܹ�������
                    $scope.finishedPage.p_current = response.response.NowPage; //��ǰ�ڼ�ҳ
                    $scope.finishedPage.p_count = response.response.NowNum; //��ǰҳ�������
                    for (var i = 0; i < $scope.finishedPage.pageItem; i++) {
                        $scope.finishedPage.pages[i] = i + 1;
                    }
                    if ($scope.finishedProjectList.length == 0) {
                        $scope.finishedProjectListIsNull = true;
                    }
                    if ($scope.finishedProjectList.length != 0) {
                        $scope.finishedProjectListIsNull = false;
                    }
                }
            })
            .error(function () {
                notif({
                    msg: "<b>����:</b> ������Ŀ�б����ʧ�ܣ����������Ƿ�ͨ��",
                    type: "error"
                });
            });
        }
    }
    //�б��ʼ��
    $scope.loadPage(1, "researchProject");
    $scope.loadPage(1, "finishedProject");

    $scope.showProjectInfo = function (projectInfo) {
        $("#projectDetail").modal('show');
        $scope.projectData = projectInfo;
        $scope.docData = $scope.projectData.Docs;

        if ($scope.docData.length == 0) {
            $scope.docIsNull = true;
        } else {
            $scope.docIsNull = false;
        }
    }
    //close the modal, init the tab
    $('#projectDetail').on('hidden.bs.modal', function () {
        $scope.$broadcast("informTab", { index: 0 });
    })

    //	 ***************�����ļ�*****************
    //download task PDF
    $scope.downloadPDF = function (task) {
        var taskPDFUrl = "/api/annualTaskDoc?annualTaskId=" + task.AnnualTaskId + "&&docType=3 ";
        $downloadFile.downloadFile(taskPDFUrl, "", task.Name);
    }
    //download annual-summary PDF
    $scope.downloadSummary = function (task) {
        var taskSummaryUrl = "/api/annualTaskDoc?annualTaskId=" + task.AnnualTaskId + "&&docType=0 ";
        $downloadFile.downloadFile(taskSummaryUrl, "", task.Name + "-��ȱ���");
    }
    //download docs(include conclude-report and attachment)
    $scope.downloadFile = function (projectDocId, docName) {
        var documentFileUrl = "/api/projectdoc/download/" + projectDocId;
        $downloadFile.downloadFileType(documentFileUrl, "", docName);
    }

    $scope.showTaskModal = function (projectInfo) {
        $("#taskModal").modal('show');
        $scope.projectData = projectInfo;
    }
}]);

divisionPaticipateApp.directive('tabs', function () {
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
divisionPaticipateApp.directive('tab', function () {
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
