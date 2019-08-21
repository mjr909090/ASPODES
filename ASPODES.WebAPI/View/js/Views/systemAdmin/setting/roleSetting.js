var roleSettingApp = angular.module('ngRoleSettingApp',[]);

roleSettingApp.controller('ngRoleSettingCtrl', function ($scope, $http) {

    $('.datepicker').datepicker({
        format: 'yyyy-mm-dd',
        language: 'zh-CN'
    });

    //初值
    $scope.noSuperAdmin = true;
    $scope.superAdminListReady = false;
    $scope.selectPerson = null;
    $scope.addForm = {
        "submitted": false
    }
    $scope.addSuperAdmin = {
        "InstituteId": 999
    }

    //url
    var getSuperAdminUrl = "/api/systemperson/departadmins/";
    var postSuperAdminUrl = "/api/systemperson";
    var putDismissSuperAdminUrl = "/api/systemperson/dismiss/";
    var putAppointSuperAdminUrl = "/api/systemperson/appoint/";
    var getHeadQuatePersonUrl = "/api/systemperson/combo/departadmin/candidate";
    var getSuperAdminDutyUrl = "/api/applicationassignments";
    var putSuperAdminDutyUrl = "/api/applicationassignments";

    //获取
	
    //获取院管理员列表
    $scope.getSuperAdminList = function (pageNum) {
        $http.get(getSuperAdminUrl + pageNum)
        .success(function (response) {
            if (response.response.ItemDTOs != null) {
                $scope.NowPage = response.response.NowPage;
                $scope.TotalPageNum = response.response.TotalPageNum;
                $scope.superAdminList = response.response.ItemDTOs;
                $scope.noSuperAdmin = false;
                $scope.superAdminListReady = true;
            }
            else if (response.response.ItemDTOs == null) {
                $scope.superAdminList = null;
                $scope.noSuperAdmin = true;
                $scope.superAdminListReady = true;
            }
            else {
                notif({
                    type: "error",
                    msg: "未知错误！",
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
                $scope.superAdminListReady = true;
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "未获取到院管理员列表！请检查您的网络链接后重试！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
            $scope.superAdminList = null;
            $scope.noSuperAdmin = true;
            $scope.superAdminListReady = true;
        })
    }


    //获取院机关人员列表
    $scope.getHeadQuatePerson = function () {
        $http.get(getHeadQuatePersonUrl)
        .success(function (response) {
            if (response.response != null) {
                $scope.headQuatePersonList = response.response;
            }
            if (response.response == null) {
                $scope.headQuatePersonList = null;
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "未获取到院机关人员列表！请检查您的网络链接后重试！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }

    //添加院管理员
    $scope.addSuperAdminFun = function () {
        $scope.addForm.submitted = true;
        if ($scope.selectAddType == 0) {
            $http.put(putAppointSuperAdminUrl + $scope.selectSuperAdminPerson)
            .success(function (response) {
                notif({
                    msg: "<b>恭喜:</b> 添加院管理员成功成功！",
                    type: "success"
                });
                $scope.getSuperAdminList($scope.NowPage);
                $scope.getHeadQuatePerson();
                $scope.selectAddType = null;
                $scope.selectSuperAdminPerson = null;
                $scope.addSuperAdmin = null;
                $scope.addSuperAdmin = {
                    "InstituteId": 999,
                    "Name": null
                };
            })
            .error(function (response) {
                notif({
                    type: "error",
                    msg: "未能成功添加院管理员！请检查您的网络链接后重试！",
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            })
        }
        else if ($scope.selectAddType == 1) {
            $http.post(postSuperAdminUrl, $scope.addSuperAdmin)
            .success(function (response) {
                notif({
                    msg: "<b>恭喜:</b> 添加院管理员成功！",
                    type: "success"
                });
                $scope.getSuperAdminList($scope.NowPage);
                $scope.getHeadQuatePerson();
                $scope.selectAddType = null;
                $scope.selectSuperAdminPerson = null;
                $scope.addSuperAdmin = null;
                $scope.addSuperAdmin = {
                    "InstituteId": 999
                };
                $scope.addForm.submitted = false;
            })
            .error(function (response) {
                notif({
                    type: "error",
                    msg: "未能成功添加院管理员！请检查您的网络链接后重试！",
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            })
        }

        $("#AddSuperAdmin").modal("hide");


    }

    //删除院管理员
    $scope.dismissSuperAdmin = function (personId) {
        $http.put(putDismissSuperAdminUrl + personId)
        .success(function (response) {
            notif({
                msg: "<b>恭喜:</b> 取消院管理员成功！",
                type: "success"
            });
            $scope.getSuperAdminList($scope.NowPage);
            $scope.getHeadQuatePerson();
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "未能成功取消院管理员！请检查您的网络链接后重试！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }

    //获取某一院管理员的责任分派
    $scope.getSuperAdminDuty = function (superAdminId) {
        $http({
            method: "GET",
            url: getSuperAdminDutyUrl + "/" + superAdminId
        } 
        )
        .success(function (response) {
            if (response.status == 0) {
                $scope.superAdminDutys = response.response;
            }
            else {
                notif({
                    type: "error",
                    msg: "未知错误：" + response.errorMsg,
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
                msg: "未能成功获取该管理员的负责情况！请检查您的网络链接后重试！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }

    //更新某一院管理员的责任分派
    $scope.putSuperAdminDutys = function () {
        $http({
            method: "PUT",
            url: putSuperAdminDutyUrl + "/" + $scope.manageSuperAdmin,
            data: $scope.superAdminDutys
        })
        .success(function (response) {
            if (response.status == 0) {
                notif({
                    type: "success",
                    msg:"<b>恭喜:</b> 院管理员分管项目类型更新成功！"
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
        $("#setDuty").modal("hide");
    }



    //上一页
    $scope.getPrePageList = function(pageNum){
        if (pageNum > 1) {
            $scope.getSuperAdminList(pageNum);
        }
    }

    //下一页
    $scope.getNextPageList = function (pageNum) {
        if (pageNum < $scope.TotalPageNum) {
            $scope.getSuperAdminList(pageNum);
        }
    }

    //初始化
    $scope.getSuperAdminList(1);
    $scope.getHeadQuatePerson();


    //modal控制
    $scope.addSuperAdminModal = function () {
        $("#AddSuperAdmin").modal("show");
    }
    $scope.showSetDutyModal = function (adminId) {
        $scope.manageSuperAdmin = adminId;
        $scope.getSuperAdminDuty(adminId);
        $("#setDuty").modal("show");
    }
});



