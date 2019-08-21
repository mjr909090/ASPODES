var projectHostApp = angular.module('ngProjectHostApp', ['publicFunction']);

projectHostApp.controller('ngProjectHostCtrl', function ($scope, $http, Upload, $timeout, $downloadFile) {

    //url
    var getProjectInfoUrl = '/api/project/leader';
    var getAssignmentBookListUrl = '/api/annualTask/';
    var getFileListUrl = '/api/projectdoc';
    var getMemberListUrl = '/api/projectmember';
    var getBudgetListUrl = '/api/annualTaskBudgetItem/';
    var getInstBudgetListUrl = "/api/annualTaskInstBudget/";

    var uploadYearReportUrl = '/api/annualTaskDoc/upload';
    var uploadFinalReportUrl = '/api/projectdoc';
    var uploadOtherReportUrl = '/api/projectdoc';
    var downloadPDFUrl = '/api/annualTaskDoc';
    var downloadProjectDocUrl = '/api/projectdoc/download/';
    var downloadEndReportUrl = '/api/projectdoc/download/endreport/';

    var isSubmitProjectFinishUrl = '/api/project/leader/IsSubmitProjectFinish/';
    var submitProjectFinishUrl = '/api/project/leader/SubmitProjectFinish/';




    $scope.selectProjectBody = 'assignmentBook';
    $scope.memberList = {
        'Leader': null,
        'HostDepartMember': null,
        'OtherDepartMember': null
    }
    $scope.uploadFile = {
        'type': null,
        'id': null
    }

    $scope.switchProjectBody = function (bodyName) {
        $scope.selectProjectBody = bodyName;
    }



    //modal manage
    $scope.showMemberModal = function (Leader, HostDepartMember, OtherDepartMember) {
        $scope.memberList.Leader = Leader;
        $scope.memberList.HostDepartMember = HostDepartMember;
        $scope.memberList.OtherDepartMember = OtherDepartMember;
        $("#memberDetailModal").modal('show');
    }

    $scope.showBudgetModal = function (taskId) {
        $scope.getBudgetList(taskId);
        $scope.getInstBudgetList(taskId);
        $("#budgetDetailModal").modal('show');
    }

    $scope.showFileUploadModal = function () {
        $("#fileUploadModal").modal('show');
    }

    $scope.jumpToDetail = function (projectId) {
        window.open("/View/Views/proposer/myProject/projectDetail.html?projectId=" + projectId);
    }

    $scope.refresh = function () {
        $scope.getHostProjectInfo();
    }



    //获取在研项目基本信息
    $scope.getHostProjectInfo = function () {
        $http({
            method: 'GET',
            url: getProjectInfoUrl,
            params: {
                "Status": 0
            }
        })
        .success(function (response) {
            if (response.status == 0) {
                $scope.hostProjectList = response.response;
            }
            else {
                notif({
                    type: "error",
                    msg: "错误：" + response.errorMsg,
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "错误：" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }

    //获取在研项目任务书列表
    $scope.getAssignmentBookList = function (projectId) {
        $http({
            method: 'GET',
            url: getAssignmentBookListUrl + projectId + '/tasks'
        })
        .success(function (response) {
            if (response.status == 0) {
                $scope.assignmentBookList = response.response;
            }
            else {
                notif({
                    type: "error",
                    msg: "错误：" + response.errorMsg,
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "错误：" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }

    //获取在研项目文档列表
    $scope.getFileList = function (projectId) {
        $http({
            method: 'GET',
            url: getFileListUrl,
            params: {
                'ProjectId': projectId
            }
        })
        .success(function (response) {
            if (response.status == 0) {
                $scope.fileList = response.response;
            }
            else {
                notif({
                    type: "error",
                    msg: "错误：" + response.errorMsg,
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "错误：" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }

    //获取人员详细信息列表
    $scope.getMemberList = function (projectId) {
        $http({
            method: 'GET',
            url: getMemberListUrl,
            params: {
                'ProjectId': projectId
            }
        })
        .success(function (response) {
            if (response.status == 0) {
                $scope.memberList = response.response;
            }
            else {
                notif({
                    type: "error",
                    msg: "错误：" + response.errorMsg,
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "错误：" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }

    //获取经费科目信息列表
    $scope.getBudgetList = function (taskId) {
        $http({
            method: 'GET',
            url: getBudgetListUrl + taskId
        })
        .success(function (response) {
            if (response.status == 0) {
                $scope.budgetList = response.response;

                $scope.yearTotalBudget = 0;

                angular.forEach($scope.budgetList, function (data) {
                    $scope.yearTotalBudget = $scope.yearTotalBudget + data.Amount;
                })

            }
            else {
                notif({
                    type: "error",
                    msg: "错误：" + response.errorMsg,
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "错误：" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }

    //获取单位经费信息列表
    $scope.getInstBudgetList = function (taskId) {
        $http({
            method: 'GET',
            url: getInstBudgetListUrl + taskId
        })
        .success(function (response) {
            if (response.status == 0) {
                $scope.instBudgetList = response.response;

                $scope.instTotalBudget = 0;

                angular.forEach($scope.instBudgetList, function (data) {
                    $scope.instTotalBudget = $scope.instTotalBudget + data.Amount;
                })

            }
            else {
                notif({
                    type: "error",
                    msg: "错误：" + response.errorMsg,
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "错误：" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }


    //获取已结题项目基本信息
    $scope.getFinishedProjectInfo = function () {
        $http({
            method: 'GET',
            url: getProjectInfoUrl,
            params: {
                "Status": 1
            }
        })
        .success(function (response) {
            if (response.status == 0) {
                $scope.finishedProjectList = response.response;
            }
            else {
                notif({
                    type: "error",
                    msg: "错误：" + response.errorMsg,
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "错误：" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }

    //上传文档
    $scope.uploadFiles = function (files, errFiles) {

        //if (files.length == 0) {
        //    notif({
        //        type: "error",
        //        msg: "错误：上传失败！请检查您的网络并确定文件格式正确！",
        //        position: "center",
        //        width: 500,
        //        height: 60,
        //        autohide: false
        //    });
        //}

        if ($scope.uploadFile.type == 'yearReport') {
            var uploadUrl = uploadYearReportUrl;
            var uploadData = {
                "annualTaskId": $scope.uploadFile.id,
                "docType": 0
            }

            $scope.files = files;
            $scope.errFiles = errFiles;
            angular.forEach(files, function (file) {
                file.upload = Upload.upload({
                    url: uploadUrl,
                    file: file,
                    params: uploadData
                });

                file.upload.then(
                    function (response) {
                        $timeout(function () {
                            file.result = response.response;
                        });
                    },
                    function (response) {
                        if (response.status > 0) {
                            $scope.showErrorMsg = response.status + ':' + response.errorMsg;
                        }
                    },
                    function (evt) {
                        file.progress = Math.min(100, parseInt(100.0 * evt.loaded / evt.total));
                    }
                )
                .then(function () {
                    $scope.clearFiles();
                    $scope.getHostProjectInfo();
                })
            })


        }
        else {
            if ($scope.uploadFile.type == 'finalReport') {
                var uploadUrl = uploadFinalReportUrl;
                var uploadData = {
                    "ProjectId": $scope.uploadFile.id,
                    "Type": 0
                }
            }
            else if ($scope.uploadFile.type == 'other') {
                var uploadUrl = uploadOtherReportUrl;
                var uploadData = {
                    "ProjectId": $scope.uploadFile.id,
                    "Type": 1
                }
            }


            $scope.files = files;
            $scope.errFiles = errFiles;
            angular.forEach(files, function (file) {
                file.upload = Upload.upload({
                    url: uploadUrl,
                    data: uploadData,
                    file: file
                });

                file.upload.then(
                    function (response) {
                        $timeout(function () {
                            file.result = response.response;
                        });
                    },
                    function (response) {
                        if (response.status > 0) {
                            $scope.showErrorMsg = response.status + ':' + response.errorMsg;
                        }
                    },
                    function (evt) {
                        file.progress = Math.min(100, parseInt(100.0 * evt.loaded / evt.total));
                    }
                )
                .then(function () {
                    $scope.clearFiles();
                    $scope.getHostProjectInfo();
                })
            })

        }

    }

    $scope.clearFiles = function () {
        $scope.files = {};
        $scope.errFiles = {};
        $scope.showErrorMsg = null;
    }


    //申请结题
    $scope.finishProjcet = function (projectId) {
        $http.get(isSubmitProjectFinishUrl + projectId)
        .success(function (response) {
            $scope.submitProjectFinishInfo = response.response;
            if ($scope.submitProjectFinishInfo.IsSubmit == true) {
                $("#submitProjectFinishModal").modal('show');
            }
            else {
                $("#checkSubmitProjectFinishModal").modal('show');
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "错误：" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }


    $scope.submitFinishProject = function () {
        $http.get(submitProjectFinishUrl + $scope.submitProjectFinishInfo.ProjectId)
        .success(function (response) {
            if (response.response == '提交成功') {
                notif({
                    msg: "<b>恭喜:</b> 申请结题成功！",
                    type: "success"
                });
                $scope.getHostProjectInfo();
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "错误：" + response.errorMsg,
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


    


    //下载PDF
    $scope.downloadPDF = function (annualTaskId, fileName) {
        $downloadFile.downloadFileType(downloadPDFUrl + '?annualTaskId=' + annualTaskId + '&docType=3', '.pdf', fileName);
    }

    //下载文档
    $scope.downloadDoc = function (projectDocId, fileName) {
        $downloadFile.downloadFileWithoutFileType(downloadProjectDocUrl + projectDocId, fileName);
    }

    //下载结题报告
    $scope.downloadEndReport = function (projectDocId, fileName) {
        $downloadFile.downloadFileType(downloadProjectDocUrl + projectDocId, '.pdf', fileName);
    }

    //下载年度任务报告
    $scope.downloadYearReport = function (annualTaskId) {
        $downloadFile.downloadFileType(downloadPDFUrl + '?annualTaskId=' + annualTaskId + '&docType=0', '.pdf', '年度报告');
    }


    //跳转到填报申请书界面
    $scope.jumpToEditPage = function (taskId, projectId) {
        $scope.editAssignmentBookUrl = "Views/proposer/myProject/addAssignmentBook.html?TaskId=" + taskId + "&ProjectId=" + projectId;
        window.open($scope.editAssignmentBookUrl);
    }




    //初始化
    $scope.getHostProjectInfo();
    $scope.getFinishedProjectInfo();
    

});


projectHostApp.directive('projectFile', function () {
    return {
        restrict: 'E',
        templateUrl: '/View/Views/superAdmin/projectManage/projectInfo/projectFile.html',
        scope: true,
        controller: function () {
            this.showAllDocument = false;
        }
    }
});

projectHostApp.directive('tabs', function () {
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

projectHostApp.directive('tab', function () {
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


projectHostApp.filter('changeAnnualTaskStatus', function () {
    return function (status) {
        var changedStatus = '';

        switch (status) {
            case 0: changedStatus = '新建'; break;
            case 1: changedStatus = '待单位审核'; break;
            case 2: changedStatus = '单位驳回'; break;
            case 3: changedStatus = '待院审核'; break;
            case 4: changedStatus = '院驳回'; break;
            case 5: changedStatus = '待提交年度报告'; break;
            case 6: changedStatus = '待单位审核年度报告'; break;
            case 7: changedStatus = '单位驳回年度报告'; break;
            case 8: changedStatus = '待院审核年度报告'; break;
            case 9: changedStatus = '院驳回年度报告'; break;
            case 10: changedStatus = '已完成'; break;
        }


        return changedStatus;
    }
});

projectHostApp.filter('changeTaskStatus', function () {
    return function (status) {
        var changedStatus = '';

        switch (status) {
            case 0: changedStatus = '执行中'; break;
            case 1: changedStatus = '项目结题'; break;
            case 2: changedStatus = '单位结题审核'; break;
            case 3: changedStatus = '单位驳回审核'; break;
            case 4: changedStatus = '院结题审核'; break;
            case 5: changedStatus = '院驳回审核'; break;
        }


        return changedStatus;
    }
});

projectHostApp.filter('changeFileType', function () {
    return function (status) {
        var changedStatus = '';

        switch (status) {
            case 0: changedStatus = '结题报告'; break;
            case 1: changedStatus = '其它'; break;
        }


        return changedStatus;
    }
})

