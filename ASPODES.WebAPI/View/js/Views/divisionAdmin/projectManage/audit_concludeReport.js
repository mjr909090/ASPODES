var auditConcludeReportApp = angular.module('ngAuditConcludeReportApp', ['publicFunction']);

auditConcludeReportApp.directive('projectFile', function () {
    return {
        restrict: 'E',
        templateUrl: '/View/Views/divisionAdmin/projectManage/projectInfo/projectFile.html',
        scope: true,
        controller: function () {
            this.showAllDocument = false;
        },
    }
});

auditConcludeReportApp.controller('ngAuditConcludeReportCtrl', ['$scope', '$http', '$downloadFile', function ($scope, $http, $downloadFile) {
    var listUrl = "/api/project/institute";  //  Get 结题申请页面

    var reviewUrl = "/api/project/institute/Auditing/"; //Put pass=true提交; pass=false驳回
    var downloadUrl = "/api/projectdoc/download/endreport/";


    $scope.listAlready = false;
    $scope.listIsNull = true;

    // *******************获取列表&分页**************************
    //选择某一页时
    $scope.load_page = function (page) {
        $http({
            method: 'GET',
            url: listUrl,
            params: {
                Status: 2,
                Page: page
            }
        })
        .success(function (response) {
            if (response.response != null) {
                $scope.listAlready = true;
                $scope.projectList = response.response.ItemDTOs;
                $scope.pageItem = response.response.TotalPageNum;//总共多少页
                $scope.count = response.response.TotalNum; //页面总共多少条
                $scope.p_current = response.response.NowPage; //当前第几页
                $scope.p_count = response.response.NowNum; //当前页面多少条
                $scope.pages = [];
                for (var i = 0; i < $scope.pageItem; i++) {
                    $scope.pages[i] = i + 1;
                }
                if ($scope.projectList.length == 0) {
                    $scope.listIsNull = true;
                }
                else {
                    $scope.listIsNull = false;
                }
            }
        })
        .error(function () {
            notif({
                msg: "<b>错误:</b> 页面加载失败，请检查网络是否通畅",
                type: "error"
            });
        });
    }

    //init list
    $scope.load_page(1);

    //leaderInfo
    $scope.principalDetail = function (leaderId) {
        var detailUrl = leaderInfoUrl + leaderId;
        $http.get(detailUrl)
		    .success(function (response) {
		        $scope.modalData = response.response;
		    })
		    .error(function () {
		        notif({
		            msg: "<b>错误:</b> 负责人信息获取失败，请检查网络是否通畅",
		            type: "error"
		        });
		    });
    }

    //show projectInfo details
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

    //download conclude report File
    $scope.downloadConcludeFile = function (project) {
        var concludeReportUrl = downloadUrl + project.ProjectId;
        $downloadFile.downloadFile(concludeReportUrl, "", project.Name+"结题报告");
    }

    //open passed modal
    $scope.showPassedConfirmModal = function (project) {
        $("#submitApplication").modal("show");
        $scope.passedId = project.ProjectId;
    }
    //passed the assginBook
    $scope.passedReport = function (id) {
        $http({
            method: 'POST',
            url: reviewUrl + id,
            params: {
                Pass: true
            },
            data:""
        })
        .success(function (response) {
            if (response.status == 0) {
                notif({
                    msg: "<b>恭喜:</b> 结题报告审核通过",
                    type: "success"
                });
                $scope.load_page($scope.p_current); //refresh
            }
        })
        .error(function () {
            notif({
                msg: "<b>错误:</b> 结题报告提交失败，请检查网络是否通畅",
                type: "error"
            });
        });
    }

    //open reject Modal
    $scope.showRejectConfirmModal = function (project) {
        $("#rejectApplication").modal("show");
        $scope.rejectId = project.ProjectId;
    }
    //reject the AssignBook
    $scope.rejectReport = function (id) {
        $http({
            method: 'POST',
            url: reviewUrl + id,
            params: {
                Pass: false
            },
            data: $scope.rejectReason
        })
         .success(function (response) {
             if (response.status == 0) {
                 notif({
                     msg: "<b>提示:</b> 该项目结题报告已驳回至项目负责人处",
                     type: "success"
                 });
                 $scope.load_page($scope.p_current); //refresh
             }
         })
        .error(function () {
            notif({
                msg: "<b>错误:</b> 结题报告驳回失败，请检查网络是否通畅",
                type: "error"
            });
        });
    }
}]);
auditConcludeReportApp.directive('tabs', function () {
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
auditConcludeReportApp.directive('tab', function () {
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