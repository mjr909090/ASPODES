var applicationRejectReasonApp = angular.module('ngApplicationRejectReasonApp',[]);

applicationRejectReasonApp.controller('ngApplicationRejectReasonCtrl', function ($scope, $http) {

    //参数初始化
    $scope.DivisionAdminReasonListIsNull = true;
    $scope.DivisionAdminReasonListReady = false;
    $scope.SuperAdminReasonListIsNull = true;
    $scope.SuperAdminReasonListReady = false;


    //URL
    var divisionAdminUrl = "/api/setting/TemplateReason/Inst";
    var superAdminUrl = "/api/setting/TemplateReason/Depart";

	/***************************单位驳回原因*********************************/
    //获取单位驳回原因列表
    $scope.getDivisionRejectReasonList = function () {
        $scope.DivisionAdminReasonListReady = false;
        $http.get(divisionAdminUrl)
        .success(function (response) {
            if (response.response == null) {
                $scope.DivisionAdminReasonListIsNull = true;
            }
            else {
                $scope.divisionReasonList = response.response;
                $scope.DivisionAdminReasonListIsNull = false;
            }
            $scope.DivisionAdminReasonListReady = true;
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "未能成功获取到单位管理员驳回原因列表！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
            $scope.DivisionAdminReasonListIsNull = true;
            $scope.DivisionAdminReasonListReady = true;
        })
    }

    //添加驳回原因
    $scope.addDivisionReason = function () {
        $http.post(divisionAdminUrl, {
            "ReasonId": 9999, "ReasonContent": $scope.addDivisionReasonContent
        })
        .success(function (response) {
            notif({
                type: "success",
                msg: "添加成功！",
            });
            $scope.addDivisionReasonContent = null;
            $scope.getDivisionRejectReasonList();
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

    //修改驳回原因
    $scope.editDivisionReason = function () {
        $http.put(divisionAdminUrl, $scope.selectDivisionReason)
        .success(function (response) {
            notif({
                type: "success",
                msg: "修改成功！",
            });
            $scope.selectDivisionReason = null;
            $scope.getDivisionRejectReasonList();
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

    //删除驳回原因
    $scope.deleteDivisionReason = function () {
        $http({
            method: "DELETE",
            url: divisionAdminUrl,
            dataType: "json",
            headers: { 'Content-Type': 'application/json' },
            data: {
                "ReasonId": $scope.selectDivisionReason.ReasonId,
                "ReasonContent": "驳回未受理"
            }
        })
        .success(function (response) {
            notif({
                type: "success",
                msg: "删除成功！",
            });
            $scope.selectDivisionReason = null;
            $scope.getDivisionRejectReasonList();
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "错误:" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }



    //modal操作
    $scope.showAddDivisionReasonModal = function () {
        $("#addDivisionReason").modal("show");
    }

    $scope.showEditDivisionReasonModal = function (reason) {
        $scope.selectDivisionReason = angular.copy(reason);
        $("#editDivisionReason").modal("show");
    }

    $scope.showDeleteDivisionReasonModal = function (reason) {
        $scope.selectDivisionReason = angular.copy(reason);
        $("#deleteDivisionReason").modal("show");
    }

    //初始化
    $scope.getDivisionRejectReasonList();


    /*******************************院不受理原因*************************************/
    //获取院不受理原因列表
    $scope.getSuperAdminRejectReasonList = function () {
        $scope.SuperAdminAdminReasonListReady = false;
        $http.get(superAdminUrl)
        .success(function (response) {
            if (response.response == null) {
                $scope.SuperAdminReasonListIsNull = true;
            }
            else {
                $scope.superAdminReasonList = response.response;
                $scope.SuperAdminReasonListIsNull = false;
            }
            $scope.SuperAdminReasonListReady = true;
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "未能成功获取到单位管理员驳回原因列表！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
            $scope.SuperAdminReasonListIsNull = true;
            $scope.SuperAdminReasonListReady = true;
        })
    }

    //添加院不受理原因
    $scope.addSuperAdminReason = function () {
        $http.post(superAdminUrl, {
            "ReasonId": 9999, "ReasonContent": $scope.addSuperAdminReasonContent
        })
        .success(function (response) {
            notif({
                type: "success",
                msg: "添加成功！",
            });
            $scope.addSuperAdminReasonContent = null;
            $scope.getSuperAdminRejectReasonList();
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

    //修改院不受理原因
    $scope.editSuperAdminReason = function () {
        $http.put(superAdminUrl, $scope.selectSuperAdminReason)
        .success(function (response) {
            notif({
                type: "success",
                msg: "修改成功！",
            });
            $scope.selectSuperAdminReason = null;
            $scope.getSuperAdminRejectReasonList();
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

    //删除院不受理原因
    $scope.deleteSuperAdminReason = function (reasonId) {
        $http({
            method: "DELETE",
            url: superAdminUrl,
            dataType: "json",
            headers: { 'Content-Type': 'application/json' },
            data: {
                "ReasonId": $scope.selectSuperAdminReason.ReasonId,
                "ReasonContent": "驳回\\未受理"
            }
        })
        .success(function (response) {
            notif({
                type: "success",
                msg: "删除成功！",
            });
            $scope.selectSuperAdminReason = null;
            $scope.getSuperAdminRejectReasonList();
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "错误:" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }



    //modal操作
    $scope.showAddSuperAdminReasonModal = function (reason) {
        $("#addSuperAdminReason").modal("show");
    }

    $scope.showEditSuperAdminReasonModal = function (reason) {
        $scope.selectSuperAdminReason = angular.copy(reason);
        $("#editSuperAdminReason").modal("show");
    }

    $scope.showDeleteSuperAdminReasonModal = function (reason) {
        $scope.selectSuperAdminReason = angular.copy(reason);
        $("#deleteSuperAdminReason").modal("show");
    }


    //初始化
    $scope.getSuperAdminRejectReasonList();

});