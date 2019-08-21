//单位管理

var divisionManagement = angular.module('ngDivisionManagementApp', ["ngFileUpload"]);

divisionManagement.controller('ngDivisionManagementCtrl', ['$scope','$http','Upload','$timeout', function ($scope, $http, Upload, $timeout) {

    $scope.EditDivision = {
        "Name": null
    }


    //url
    var divisionListUrl = "/api/inst/list";                //单位管理的列表
    var deleteDivisionUrl = "/api/inst/";                  //删除单位
    var addInstUrl = "/api/inst";                          //添加单位
    var editDivisionUrl = "/api/inst/depart/update";       //编辑单位
    var instInfoUrl = "/api/inst/";                       //查看详细信息
    var instTypeUrl = "/api/inst";                         //获取下拉框单位列表
    var uploadDivisionUrl = "/api/inst/upload";            //上传单位信息
    var contactInfoListUrl = "/api/person/inst/";
    var getInstPersonUrl = "/api/person/combo/inst/"
    var getInstAdminUrl = "/api/systemperson/instadmins/";
    var addDivisionAdminUrl = "/api/systemperson/appoint/instadmin/";       //添加单位管理员
    var dismissDivisionAdminUrl = "/api/systemperson/dismiss/instadmin/";


    //获取单位列表
    $scope.getDivisionList = function () {
        $http.get(divisionListUrl)
        .success(function (response) {
            $scope.divisionList = response.response;
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
        });
    }
    
 
    //刷新
    $scope.refresh = function () {
        $scope.getDivisionList();
    }

    
 
    //添加单位
    $scope.showAddInstModal = function () {
        $("#addInstModal").modal("show");
    };


    //添加页面获取单位下拉框单位列表
    $scope.instTypes = function () {
        $http.get(instTypeUrl)
        .success(function (response) {
            $scope.instTypes = response.response;
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
        });
    }

    

    $scope.selectInstTypeList = [
       { instTypeId: "0", instType: "外单位" },
       { instTypeId: "1", instType: "研究所" },
       { instTypeId: "2", instType: "院机关" }
    ];


    //添加模态框中的确定
    $scope.addDivision = function () {     
        $http({
            method: 'POST',
            url: addInstUrl,
            data: $scope.addDivisionInfo
        })  
        .success(function (response) {
            notif({
                msg: "添加操作成功！",
                type: "success"
            });
            $scope.getDivisionList();
            $scope.instTypes();
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
   


    //查看单位详细信息的模态框
    $scope.showInstInfoModal = function (instId) {
        $http.get(instInfoUrl + instId)
        .success(function (response) {
            $scope.instInfo = response.response;
            $("#viewDivisionInfo").modal("show");
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
        });
    };


    //查看联系人详细信息的模态框
    $scope.showContactPersonInfoModal = function (instId) {
        $http.get(instInfoUrl + instId)
        .success(function (response) {
            $scope.contactPersonInfo = response.response;
            $("#viewAdminInfo").modal("show");
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
        });
    };


    //修改的Modal 
    $scope.showModifyDivisionModal = function (num) {
        $scope.EditDivision = angular.copy($scope.divisionList[num]);
            $http.get(contactInfoListUrl + $scope.EditDivision.InstituteId)
            .success(function (response) {
                $scope.contactInfoList = response.response;
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
            });
        
        $("#modifyDivisionModal").modal("show");
    };
  

    //修改模态框中的确定
    $scope.eidtDivisionSure = function () {
        $http({
            method: 'PUT',
            url: editDivisionUrl,
            data: $scope.EditDivision
        })
        .success(function (response) {
            notif({
                msg: "编辑操作成功！",
                type: "success"
            });
            $scope.getDivisionList();
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


    //删除的Modal
    $scope.showDeleteDivisionModal = function (division) {
        $("#deleteDivisionModal").modal("show");
        $scope.instId = division.InstituteId;
    };


    //删除单位的modal的确定
    $scope.deleteDivision = function () {
        $http.delete(deleteDivisionUrl + $scope.instId)
        .success(function (response) {
            if (response.status == 0) {
                notif({
                    msg: "<b>恭喜:</b> 单位删除成功！",
                    type: "success"
                });
                $scope.getDivisionList();
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "单位管理删除失败，请检查您的网络！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        });
    }

    //上传单位信息的导入文件的模态框
    $scope.showImportDocumentModal = function () {
       $("#importDocumentModal").modal("show");
    };
     
    $scope.uploadFiles = function (file, errFiles) {     
         $scope.file = file;
         $scope.errFiles = errFiles;
        
        file.upload = Upload.upload({
            url: uploadDivisionUrl ,
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
    }

    //获取某一单位的人员
    $scope.getInstPerson = function (instId) {
        $http.get(getInstPersonUrl + instId)
        .success(function (response, Status) {
            if (response.status == 0) {
                $scope.instPersonList = response.response;
            }
            else if (response.response == null) {
                notif({
                    type: "error",
                    msg: "当前单位还没有人员，请联系单位联系人进行添加",
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
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

    //获取单位管理员列表
    $scope.getInstAdminList = function (instId) {
        $http.get(getInstAdminUrl + instId + "/1")
        .success(function (response, Status) {
            if (response.status == 0) {
                $scope.instAdminList = response.response.ItemDTOs;
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

    //添加单位管理员
    $scope.AddDivisionAdmin = function (selectPerson) {
        $http.put(addDivisionAdminUrl + selectPerson.description.PersonId)
        .success(function (response, Status) {
            if (response.status == 0) {
                notif({
                    msg: "<b>恭喜:</b> 单位管理员添加成功！",
                    type: "success"
                });
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
            $scope.getInstAdminList(selectPerson.description.InstituteId);
            $scope.$broadcast('angucomplete-alt:clearInput');
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

    //取消单位管理员
    $scope.dismissDivisionAdmin = function (instAdmin) {
        $http.put(dismissDivisionAdminUrl + instAdmin.PersonId)
        .success(function (response, Status) {
            if (response.status == 0) {
                notif({
                    msg: "<b>恭喜:</b> 删除单位管理员成功！",
                    type: "success"
                });
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
            $scope.getInstAdminList(instAdmin.InstituteId);
            $scope.$broadcast('angucomplete-alt:clearInput');
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


    //添加单位管理员模态框
    $scope.showAddDivisionAdminModal = function (instId) {
        $scope.getInstPerson(instId);
        $scope.getInstAdminList(instId);
        $("#AddDivisionAdminModal").modal("show");
    }


    //初始化
    $scope.getDivisionList();
    $scope.instTypes();


    //Modal操作
    $('#importDocumentModal').on('hidden.bs.modal', function () {
        $scope.getDivisionList();
    })

}]);