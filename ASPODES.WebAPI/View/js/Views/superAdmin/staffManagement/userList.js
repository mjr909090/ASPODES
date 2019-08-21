//用户管理
var userList = angular.module('ngUserListDepartApp', ["ngFileUpload"]);

userList.controller('ngUserListDepartCtrl', ['$scope', '$http', 'Upload', '$timeout', function ($scope, $http, Upload, $timeout) {
    //url
    var getUserListUrl = "/api/departperson/"//get用户列表数据的url
    var instTypeUrl = "/api/inst"       //获取下拉框单位列表
    var addUserUrl = "/api/departperson"       //添加用户
    var editUserUrl = "/api/departperson"       //编辑用户
    var userInfoUrl = "/api/person/"       //查看用户{userId}
    var deleteUserUrl = "/api/departperson?id="       //删除用户{userId}
    var uploadUrl = "/api/departperson/upload/"//批量导入的url
    var setDivisionAdminUrl = "/api/departperson/appoint/instadmin/"            // /api/departperson/appoint/instadmin?userId="   //设置单位管理员
    var deleteDivisionAdminUrl = "/api/departperson/dismiss/instadmin/"            // /api/departperson/dismiss/instadmin?userId="  //删除单位管理员

    var deleteUserListUrl = "/api/departperson/delete"  //禁用用户列表
    var reviveUserUrl = "/api/departperson/revive/"  //+{personId}启用被删用户
    
  
    //获取用户列表
    /*
     $scope.getUserList = function () {
        //alert('getUserList');
        $http.get(getUserListUrl)
        .success(function (response) {
            $scope.userList = response.response.Persons;
        })
        .error(function () {
            alert("1error!");
        });
    }
    $scope.getUserList();//启动方法
    */
   
    //添加用户模态框中的确定
    $scope.addUserSure = function () {
        //alert('addUserSure')
        $http({
            method: 'POST',
            url: addUserUrl,
            data: $scope.addUserInfo
        })
        .success(function (response) {
            notif({
                msg: "添加操作成功！",
                type: "success"
            });
            $scope.getUserList();
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

    //查看详细信息的模态框
    $scope.showUserInfoModal = function (UserId) {
        //alert("showUserInfoModal");
        $http.get(userInfoUrl + UserId)
        .success(function (response) {
            $scope.userInfo = response.response;
           // alert($scope.userInfo.InstituteId);
            $scope.instTypes.forEach(function (e)  {//对单位进行遍历
                //alert(e.InstituteId)
                if (e.InstituteId == $scope.userInfo.InstituteId) {
                    $scope.userInfo.InstituteName = e.Name
                }
            });
            $("#viewUserInfo").modal("show");
        })
        .error(function () {
            alert("showUserInfoModal error!!");
        });   
    };
    //编辑模态框
    $scope.showModifyUserModal = function (num) {
        $scope.EditPerson = $scope.userList[num];
        $("#modifyUserModal").modal("show");
    };
    //编辑用户模态框中的确定
    $scope.modifyUserSure = function () {
        $http({
            method: 'PUT',
            url: editUserUrl,
            data: $scope.EditPerson
        })
        .success(function (response) {
            notif({
                msg: "编辑操作成功！",
                type: "success"
            });
            $scope.getUserList();
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
    $scope.showDeleteUseModal = function (PersonId) {//删除模态框
        $("#deleteUseModal").modal("show");
        $scope.deletePersonId = PersonId;
    };
    //删除用户的modal的确定
    $scope.deleteUser = function () {
        //alert("delete");
        $http.delete(deleteUserUrl + $scope.deletePersonId)
        .success(function (response) {
            if (response.status == 0) {
                notif({
                    msg: "<b>恭喜:</b> 用户删除成功！",
                    type: "success"
                });
                $scope.getUserList();
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "用户删除失败，请检查您的网络！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        });
    }
    $scope.showSetDivisionAdminModal = function (PersonId) {//设置为单位管理员
        $("#setDivisionAdminModal").modal("show");
        $scope.PersonId_set = PersonId;
    };
    //设置为单位管理员的modal的确定
    $scope.setDivisionAdminSure = function () {
       // alert(setDivisionAdminUrl + $scope.PersonId_set);
        $http({
            method: 'PUT',
            url: setDivisionAdminUrl + $scope.PersonId_set,
        })       
        .success(function (response) {
            if (response.status == 0) {
                notif({
                    msg: "<b>恭喜:</b> 设置为单位管理员成功！",
                    type: "success"
                });
                $scope.getUserList();
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "设置为单位管理员失败，请检查您的网络！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        });
    }
    $scope.showDeleteDivisionAdminModal = function (PersonId) {//删除单位管理员
        $("#deleteDivisionAdminModal").modal("show");
        $scope.PersonId_delete = PersonId;
    };
    //删除单位管理员的modal的确定
    $scope.deleteDivisionAdminSure = function () {
        //alert(deleteDivisionAdminUrl + $scope.PersonId_delete);
        $http({
            method: 'PUT',
            url: deleteDivisionAdminUrl + $scope.PersonId_delete
        })  
        .success(function (response) {
            if (response.status == 0) {
                notif({
                    msg: "<b>恭喜:</b> 删除单位管理员成功！",
                    type: "success"
                });
                $scope.getUserList();
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "删除单位管理员失败，请检查您的网络！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        });
    }
   
    /***************全选&反选********************/
    $scope.m = [];
    $scope.checked = [];
    $scope.selectAll = function () {
        if ($scope.select_all) {
            $scope.checked = [];
            angular.forEach($scope.userList, function (user) {
                user.checked = true;
                $scope.checked.push(user.PersonId);
            })
        } else {
            angular.forEach($scope.userList, function (user) {
                user.checked = false;
                $scope.checked = [];
            })
        }
    };
    /***************单选********************/
    $scope.selectSingle = function () {
        angular.forEach($scope.userList, function (user) {
            var index = $scope.checked.indexOf(user.PersonId);
            if (user.checked && index === -1) {
                $scope.checked.push(user.PersonId);
            } else if (!user.checked && index !== -1) {
                $scope.checked.splice(index, 1);
            };
        })

        if ($scope.userList.length === $scope.checked.length) {
            $scope.select_all = true;
        } else {
            $scope.select_all = false;
        }
    }
    //批量导入的模态框
    $scope.showBatchImportModal = function () {
        $("#batchImportModal").modal("show");
    };
    //文件上传    
    $scope.submitFile = function () {//该方法暂时只有刷新
        //alert('submit')
        //alert($scope.batchImportInstituteId)
        $scope.getUserList();
       
    }
    $scope.upload = function (file) {
       // alert('upload')
        Upload.upload({
            //服务端接收
            url: uploadUrl + $scope.batchImportInstituteId,
            //上传的同时带的参数
           // data: { 'username': $scope.username },
            //上传的文件
            file: file
        })
    }
    
    //另一种格式
    $scope.uploadFiles = function (file, errFiles) {
        // alert('uploadUrl')
        // alert($scope.batchImportInstituteId)
        $scope.file = file;
         $scope.errFiles = errFiles;
          file.upload = Upload.upload({
              url: uploadUrl + $scope.batchImportInstituteId,
              file: file
          });
          //$("#addApplication").modal("hide");
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

    //禁用用户列表
    $scope.getDeleteUserList = function () {
        //alert('getUserList');
        $http.get(deleteUserListUrl)
        .success(function (response) {
            $scope.deleteUserList = response.response;
        })
        .error(function () {
            alert("1error!");
        });
    }
    $scope.getDeleteUserList();//启动方法
    //启用被删用户
    $scope.showReviveUserModal = function (PersonId) {
       // alert('22211')
        $http({
            method: 'PUT',
            url: reviveUserUrl + PersonId,
        })
           .success(function (response) {
               if (response.status == 0) {
                   notif({
                       msg: "<b>恭喜:</b> 启用成功！",
                       type: "success"
                   });
                   $scope.getDeleteUserList();
                   $scope.getUserList();
               }
           })
           .error(function (response) {
               notif({
                   type: "error",
                   msg: "启用失败，请检查您的网络！",
                   position: "center",
                   width: 500,
                   height: 60,
                   autohide: false
               });
           });
    }
    $scope.exportExcelUserList = function () {//导出
        $("#exportExcelUserListModal").modal("show");
    };



    // *******************获取列表&分页**************************
    //get  api/departperson/{instituteId}/{pageId}
    $scope.getInstIdForPage = function (InstituteId) {
        //alert(InstituteId);
        if (InstituteId == undefined) {
            $scope.InstituteId = '0'
        } else {
            $scope.InstituteId = InstituteId
        }
        $scope.getUserList();
    }
    //获取用户列表
    $scope.getUserList = function () {
       // alert('获取用户列表');
        //alert($scope.InstituteId);
        if ($scope.InstituteId == undefined) {
            thisInstituteId = '0'
        } else {
            thisInstituteId = $scope.InstituteId
        }
        var personListUrl = getUserListUrl + thisInstituteId + '/' + "1";
        $http.get(personListUrl)
               .success(function (response) {
                   $scope.userList = response.response.Persons;
                   $scope.pageItem = response.response.TotalPageNum;//总共多少页
                   $scope.count = response.response.TotalNum; //页面总共多少条
                   $scope.p_current = response.response.NowPage; //当前第几页
                   $scope.p_count = response.response.NowNum; //当前页面多少条
                   $scope.pages = [];
                   for (var i = 0; i < $scope.pageItem; i++) {
                       $scope.pages[i] = i + 1;
                   }
               })
               .error(function () {
                   notif({
                       msg: "<b>Error:</b> UserList is not loaded,pls try again",
                       type: "error"
                   });
               });
    }
    $scope.getUserList();
    //选择首页
    $scope.p_index = function () {
        //alert('选择首页');
        //alert($scope.InstituteId);
        if ($scope.InstituteId == undefined ) {
            thisInstituteId = '0'
        } else {
            thisInstituteId = $scope.InstituteId
        }
        var personListUrl = getUserListUrl + thisInstituteId + '/' + "1";
        $http.get(personListUrl)
              .success(function (response) {
                  $scope.userList = response.response.Persons;
              })
              .error(function () {
                  notif({
                      msg: "<b>Error:</b> FirstPage's not loaded",
                      type: "error"
                  });
              });
    }
    //Previous 前一页
    $scope.p_prev = function () {
        //alert('选择前一页');
        //alert($scope.InstituteId);
        if ($scope.InstituteId == undefined) {
            thisInstituteId = '0'
        } else {
            thisInstituteId = $scope.InstituteId
        }

        $scope.p_current = $scope.p_current - 1;
        var personListUrl = getUserListUrl + thisInstituteId + '/' + $scope.p_current;
        $http.get(personListUrl)
              .success(function (response) {
                  $scope.userList = response.response.Persons;
                  $scope.p_count = response.response.NowNum; //当前页面多少条
              })
              .error(function () {
                  notif({
                      msg: "<b>Error:</b> PreviousPage's not loaded",
                      type: "error"
                  });
              });
    }
    //选择某一页时
    $scope.load_page = function (page) {
        //alert('选择某一页');
       // alert($scope.InstituteId);
        if ($scope.InstituteId == undefined) {
            thisInstituteId = '0'
        } else {
            thisInstituteId = $scope.InstituteId
        }
        var personListUrl = getUserListUrl + thisInstituteId + '/' + page;
        //alert(personList);
        
        $http.get(personListUrl)
              .success(function (response) {
                  $scope.userList = response.response.Persons;
                  $scope.p_current = response.response.NowPage; //当前页号
                  $scope.p_count = response.response.NowNum; //当前页面多少条
                  console.log($scope.userList);
              })
              .error(function () {
                  notif({
                      msg: "<b>Error:</b> Page's not loaded",
                      type: "error"
                  });
              });
    }
    //Next 后一页
    $scope.p_next = function () {
        //alert('选择后一页');
       // alert($scope.InstituteId);
        if ($scope.InstituteId == undefined) {
            thisInstituteId = '0'
        } else {
            thisInstituteId = $scope.InstituteId
        }
        $scope.p_current = $scope.p_current + thisInstituteId + '/' + 1;
        var personListUrl = getUserListUrl + $scope.p_current;
        $http.get(personListUrl)
              .success(function (response) {
                  $scope.userList = response.response.Persons;
                  $scope.p_count = response.response.NowNum; //当前页面多少条
              })
              .error(function () {
                  notif({
                      msg: "<b>Error:</b> NextPage's not loaded",
                      type: "error"
                  });
              });
    }
    //最后一页
    $scope.p_last = function () {
        //alert('选择最后一页');
        //alert($scope.InstituteId);
        if ($scope.InstituteId == undefined) {
            thisInstituteId = '0'
        } else {
            thisInstituteId = $scope.InstituteId
        }
        var personListUrl = getUserListUrl + thisInstituteId + '/' + $scope.pageItem;
        $http.get(personListUrl)
              .success(function (response) {
                  $scope.userList = response.response.Persons;
                  $scope.p_current = response.response.NowPage; //当前页号
                  $scope.p_count = response.response.NowNum;//当前页面多少条
                  //console.log($scope.userList);
              })
              .error(function () {
                  notif({
                      msg: "<b>Error:</b> LastPage's not loaded",
                      type: "error"
                  });
              });
    }

    //刷新
    $scope.refresh = function () {
        $scope.load_page($scope.p_current); //刷新的是当前页
    }

}]);

