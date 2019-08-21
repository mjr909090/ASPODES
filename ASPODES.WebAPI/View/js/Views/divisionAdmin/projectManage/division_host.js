var divisionHostApp = angular.module('ngDivisionHostApp', ['publicFunction']);

divisionHostApp.directive('projectFile', function () {
    return {
        restrict: 'E',
        templateUrl: '/View/Views/divisionAdmin/projectManage/projectInfo/projectFile.html',
        scope:true,
        controller: function () {
            this.showAllDocument = false;
        }
    }
});

divisionHostApp.controller('ngDivisionHostCtrl', ['$scope', '$http', '$downloadFile', function ($scope, $http, $downloadFile) {
    var projectUrl = "/api/project/inst"; //Get 在研项目列表的Url
    //var leaderInfoUrl = "/api/person/";//Get负责人信息的Url
    var projectTypeUrl = "/api/ProjectType";    //获取下拉框项目类型列表

    $scope.exportInstProjectListUrl = "/api/InstProject/export"; // 导出列表的url

    // 定义
    $scope.exportInstProjectListFields = {};
    $scope.exportInstStudyingListName = "分管在研项目";
    $scope.exportInstFinishedListName = "分管结题项目";

    //********** 初始化数据 *************
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

    $scope.projectType_Research = 0;
    $scope.year_Research = 0;

    $scope.projectType_Finished = 0;
    $scope.year_Finished = 0;

    $scope.judgeIsNull = function () {
        if ($scope.projectType_Research == null) {
            $scope.projectType_Research = 0;
        }
        if ($scope.inst_Research == null) {
            $scope.inst_Research = 0;
        }

        if ($scope.projectType_Finished == null) {
            $scope.projectType_Finished = 0;
        }
        if ($scope.inst_Finished == null) {
            $scope.inst_Finished = 0;
        }

    }


    //中断dropdown的隐藏操作
    $('.dropdown-menu li:first-child').click(function (e) {
        e.stopPropagation();
    });
    //时间选择datepicker
    $('.date').datepicker({
        format: 'yyyy',
        language: 'zh-CN',
        autoclose: true,
        endDate: new Date(),
        startView: 2,
        maxViewMode: 2,
        minViewMode: 2
    });

    //获取-项目类型-下拉框 列表
    $scope.getProjectTypes = function () {
        $http.get(projectTypeUrl)
        .success(function (response) {
            $scope.projectTypes = response.response
        })

        .error(function () {
            notif({
                msg: "<b>错误:</b> 列表获取失败，请检查网络是否通畅",
                type: "error"
            });
        });
    }

    $scope.loadPage = function (page, name) {
        if (name == "researchProject") {
            $scope.listAlready = false;
            $http({
                method: 'GET',
                url: projectUrl,
                params: {
                    projectType: $scope.projectType_Research,
                    inst: $scope.inst_Research,
                    year: $scope.year_Research,
                    Status: 0,
                    page: page
                }
            })
            .success(function (response) {
                if (response.response != null) {
                    $scope.listAlready = true;
                    $scope.researchPage.pages = [];
                    $scope.researchProjectList = response.response.ItemDTOs;
                    $scope.researchPage.pageItem = response.response.TotalPageNum;//总共多少页
                    $scope.researchPage.count = response.response.TotalNum; //页面总共多少条
                    $scope.researchPage.p_current = response.response.NowPage; //当前第几页
                    $scope.researchPage.p_count = response.response.NowNum; //当前页面多少条
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
                    msg: "<b>错误:</b> 在研项目列表加载失败，请检查网络是否通畅",
                    type: "error"
                });
            });
        }
        if (name == "finishedProject") {
            $scope.finishedListAlready = false;
            $http({
                method: 'GET',
                url: projectUrl,
                params: {
                    projectType: $scope.projectType_Finished,
                    inst: $scope.inst_Finished,
                    year: $scope.year_Finished,
                    Status: 1,
                    page: page
                }
            })
            .success(function (response) {
                if (response.response != null) {
                    $scope.finishedListAlready = true;
                    $scope.finishedPage.pages = [];
                    $scope.finishedProjectList = response.response.ItemDTOs;
                    $scope.finishedPage.pageItem = response.response.TotalPageNum;//总共多少页
                    $scope.finishedPage.count = response.response.TotalNum; //页面总共多少条
                    $scope.finishedPage.p_current = response.response.NowPage; //当前第几页
                    $scope.finishedPage.p_count = response.response.NowNum; //当前页面多少条
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
                    msg: "<b>错误:</b> 结题项目列表加载失败，请检查网络是否通畅",
                    type: "error"
                });
            });
        }
    }
    //列表初始化
    $scope.getProjectTypes();//get projectType
    $scope.loadPage(1, "researchProject");
    $scope.loadPage(1, "finishedProject");

    //	 ***************下载文件*****************
    //download task PDF
    $scope.downloadPDF = function (task) { 
        var taskPDFUrl = "/api/annualTaskDoc?annualTaskId=" + task.AnnualTaskId + "&&docType=3 ";
        $downloadFile.downloadFile(taskPDFUrl, "", task.Name);
    }
    //download annual-summary PDF
    $scope.downloadSummary = function (task) {
        var taskSummaryUrl = "/api/annualTaskDoc?annualTaskId=" + task.AnnualTaskId + "&&docType=0 ";
        $downloadFile.downloadFile(taskSummaryUrl, "", task.Name + "-年度报告");
    }
    //download docs(include conclude-report and attachment)
    $scope.downloadFile = function (projectDocId, docName) {
        var documentFileUrl = "/api/projectdoc/download/" + projectDocId;
        $downloadFile.downloadFileType(documentFileUrl, "", docName);
    }

    // 导出列表
    // 导出在研项目列表
    $scope.exportInstStudyingList = function (projectType, year) {
        if (projectType == undefined) {
            $scope.projectType = 0;
        } else {
            $scope.projectType = projectType;
        }
        if (year == undefined) {
            $scope.year = 0;
        } else {
            $scope.year = year;
        }
        $scope.exportInstProjectListFields = {
            "Order": "申请书编号",
            "Name": "申请书名称",
            "Deleage": "委托",
            "Category": "类型",
            "Institute": "单位",
            "Leader": "负责人",
            "period": "执行期限",
            "StartAndStopTime": "起止时间",
            "InstituteId": 0,
            "CategoryId": $scope.projectType,
            "Year": $scope.year,
            "Status": 0
        };
        $downloadFile.exportApplication($scope.exportInstProjectListUrl, $scope.exportInstProjectListFields, $scope.exportInstStudyingListName);
    };

    // 导出结题项目列表
    $scope.exportInstFinishedList = function (projectType, year) {
        if (projectType == undefined) {
            $scope.projectType = 0;
        } else {
            $scope.projectType = projectType;
        }
        if (year == undefined) {
            $scope.year = 0;
        } else {
            $scope.year = year;
        }
        $scope.exportInstProjectListFields = {
            "Order": "申请书编号",
            "Name": "申请书名称",
            "Deleage": "委托",
            "Category": "类型",
            "Institute": "单位",
            "Leader": "负责人",
            "period": "执行期限",
            "StartAndStopTime": "起止时间",
            "InstituteId": 0,
            "CategoryId": $scope.projectType,
            "Year": $scope.year,
            "Status": 1
        };
        $downloadFile.exportApplication($scope.exportInstProjectListUrl, $scope.exportInstProjectListFields, $scope.exportInstFinishedListName);
    };

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
    $('#projectDetail').on('hidden.bs.modal', function () {
        $scope.$broadcast("informTab", { index: 0 });
    })

    $scope.showTaskModal = function (projectInfo) {
        $("#taskModal").modal('show');
        $scope.projectData = projectInfo;
    }

}]);

divisionHostApp.directive('tabs', function () {
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
divisionHostApp.directive('tab', function () {
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
