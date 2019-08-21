//单位管理员

var divisionAdminManagement = angular.module('ngDivisionAdminManagementApp',[]);

divisionAdminManagement.controller('ngDivisionAdminManagementCtrl', function ($scope, $http) {
    //url
    var divisionAdminListUrl = "/api/departperson/instadmins";
    var addDivisionAdminUrl = "/api/departperson/appoint/instadmin/";//添加为单位管理员
    var cancelSetDivisionAdminUrl = "/api/departperson/dismiss/instadmin/";//取消设置为单位管理员
    var editDivisionAdminUrl = "/api/departperson";//修改单位管理员信息
    var instTypeUrl = "/api/inst"       //获取下拉框单位列表

    //获取专家列表
    $scope.getDivisionAdminManagementList = function () {
        //alert('getDivisionAdminManagementList');
        $http.get(divisionAdminListUrl)
        .success(function (response) {
            $scope.divisionAdminList = response.response;
        })
        .error(function () {
            alert("error!");
        });
    }

    $scope.getDivisionAdminManagementList();//启动方法
    //获取下拉框单位列表
    $scope.instTypes = function () {
        //alert("1getinstTypes");
        $http.get(instTypeUrl)
        .success(function (response) {
            // alert("2success!");
            $scope.instTypes = response.response;
        })
        .error(function () {
            alert("insterror!");
        });
    }
    $scope.instTypes();//启动方法
    //添加时获取某单位的人员
    $scope.getPersonListForAdd = function (InstituteId) {
        //alert('1111');
       // alert(InstituteId);
        $http.get('/api/person/inst/' + InstituteId)
        .success(function (response) {
           $scope.PersonListForAdd = response.response;
       })
       .error(function () {
           notif({
               type: "error",
               msg: "没有获取到该单位下的人员，请检查您的网络是否畅通！",
               position: "center",
               width: 500,
               height: 60,
               autohide: false
           });
       });
    };
    $scope.getPersonInfoForAdd = function (Name) {//选择PersonId后，根据它得到详细信息
       // alert('getPersonInfoForAdd');
       // alert(Name);
        $scope.PersonListForAdd.forEach(function (e) {
            //alert('e');
            if (e.Name == Name) {
                $scope.addDivisionAdminInfo = e;
            }
        })
    }
    //添加单位管理员模态框中的确定
    $scope.addDivisionAdminSure = function () {
        //alert('addDivisionAdminSure')
        //alert(addDivisionAdminUrl + $scope.addDivisionAdminInfo.PersonId)
        $http({
            method: 'PUT',
            url: addDivisionAdminUrl + $scope.addDivisionAdminInfo.PersonId,
           // data: $scope.addDivisionAdminInfo.PersonId
        })
        .success(function (response) {
            notif({
                msg: "添加操作成功！",
                type: "success"
            });
            $scope.getDivisionAdminManagementList();
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "添加操作失败！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        });
    };
    //编辑Modal
    $scope.showModifyDivisionAdminModal = function (num) {
        $scope.EditDivisionAdmin = $scope.divisionAdminList[num];
        $("#modifyDivisionAdminModal").modal("show");
        
    };
    //编辑用户模态框中的确定
    $scope.editDivisionAdminSure = function () {
       // alert('editDivisionAdminSure')
        $http({
            method: 'PUT',
            url: editDivisionAdminUrl,
            data: $scope.EditDivisionAdmin
        })
        .success(function (response) {
            notif({
                msg: "编辑操作成功！",
                type: "success"
            });
            $scope.getDivisionAdminManagementList();
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "编辑操作失败！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        });
    };
    $scope.showDeleteDivisionAdminModal = function (PersonId) {//删除
        //alert('showDeleteDivisionAdminModal')
        $("#deleteDivisionAdminModal").modal("show");
        $scope.deletePersonId = PersonId;
    };
    //删除模态框中的确定
    $scope.deleteDivisionAdminSure = function () {
        //alert('deleteDivisionAdminSure')
        //alert(cancelSetDivisionAdminUrl + $scope.deletePersonId)
        $http({
            method: 'PUT',
            url: cancelSetDivisionAdminUrl + $scope.deletePersonId
        })
        .success(function (response) {
            notif({
                msg: "删除操作成功！",
                type: "success"
            });
            $scope.getDivisionAdminManagementList();
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "删除操作失败！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        });
    };
    $scope.exportExcelDivisionAdmin = function () {//导出
        $("#exportExcelDivisionAdminModal").modal("show");
    };
});

