var userListApp = angular.module('ngUserListApp', ['ngFileUpload', 'ngMessages']);

userListApp.controller('ngUserListCtrl', ['$scope', '$http', 'Upload', '$timeout', '$location', function ($scope, $http, Upload, $timeout, $location) {
    var userListUrl = "/api/instperson/normal/"; //	Get本单位用户列表的Url
    var instituteInfoUrl = "/api/inst/self"; //Get单位管理员所属单位的信息的Url
    //	 *****Get专家领域的Url*****
    var expertFieldUrl = "/api/expertfield/";
    var getFieldUrl = "/api/field"; //领域下拉列
    var getSubfieldUrl = "/api/subfield/"; //子领域下拉列

    var searchUrl = "/api/instperson/search/"; //子领域下拉列
    var editExpertFieldUrl = "/api/expertfield/inst"; //Post修改专家领域的Url
    var userAddUrl = "/api/instperson"; //Post用户-添加的Url
    var editInfoUrl = "/api/instperson"; //Put用户编辑的Url
    var resetUserUrl = "/api/instperson/resetpwd/"; //Put用户重置密码的Url
    var deleteUrl = "/api/instperson/"; //Delete用户Url
    

    //  ************************初始化json数据************************
    $scope.listAlready = false;
    $scope.userListIsNull = true;

    $scope.isSearch = false;
    $scope.isEdit = false;
    $scope.isExpertField = false;

    $scope.pageItem = 0;
    $scope.count = 0;
    $scope.p_current = 0;
    $scope.p_count = 0;

    $scope.searchTypeList = [
        { "Type": "all", "Name": "全部" },
        { "Type": "name", "Name": "按名字" },
        { "Type": "email", "Name": "按电子邮箱" },
        { "Type": "phone", "Name": "按手机号码" },
        { "Type": "idcard", "Name": "按身份证号" },
    ]
    $scope.searchData = {
        "types": "all",
        "keywords": "all"
    }

    $scope.newUserPersonId = 0;
    $scope.formData = {
        "InstituteId": null
    }
    $scope.fieldInfo = {
        "FieldId": null,
        "SubFieldId": null,
        "KeyWordsCN": null,
        "KeyWordsEN": null,
    }
    $scope.fieldInfo2 = {
        "FieldId": null,
        "SubFieldId": null,
        "KeyWordsCN": null,
        "KeyWordsEN": null,
    }

    // *******************获取列表&分页**************************
    //选择某一页时
    $scope.load_page = function (page) {
        $scope.listAlready = false;
        $scope.userListIsNull = true;
            $http({
                method: 'POST',
                url: searchUrl + page,
                data: $scope.searchData
            })
           .success(function (response) {
               if (response.status == 0) {
                   $scope.userList = response.response.ItemDTOs;
                   $scope.pageItem = response.response.TotalPageNum; //总共多少页
                   $scope.count = response.response.TotalNum; //页面总共多少条
                   $scope.p_current = response.response.NowPage; //当前第几页
                   $scope.p_count = response.response.NowNum; //当前页面多少条
                   $scope.pages = [];
                   for (var i = 0; i < $scope.pageItem; i++) {
                       $scope.pages[i] = i + 1;
                   }
                   $scope.listAlready = true;
                   $scope.userListIsNull = false;
               } else {
                   $scope.listAlready = true;
                   $scope.userListIsNull = true;
                   notif({
                       msg: "<b>错误:</b>" + response.errorMsg,
                       type: "error"
                   });
               }
           })
           .error(function () {
               $scope.listAlready = true;
               $scope.userListIsNull = true;
               notif({
                   msg: "<b>错误:</b> 页面加载失败，请检查网络是否通畅",
                   type: "error"
               });
           });

    }

    //初始化获取列表
    $scope.load_page(1);

    // *******************获取-search-列表&分页**************************

    //$scope.judgeType = function () {
    //    if ($scope.searchData.types == null) {
    //        $scope.searchData.keywords = null;
    //    }
    //}
    $scope.searchContent = function (searchPage) {
        $scope.isSearch = true;
        if ($scope.searchData.types == null || $scope.searchData.keywords == null) {
            $scope.isSearch = false;
            $scope.load_page(1);
        } else {
            $scope.load_page(searchPage);
        }
    }
    //回车检测
    $scope.enterEvent = function (e) {
        var keycode = window.event ? e.keyCode : e.which;
        if (keycode == 13) {
            $scope.searchContent(1);
        }
    }

    //   ***************Toolbar************************
    //添加人员
    $scope.showAddUserModal = function () {
        $scope.isEdit = false;
        $("#addApplication").modal("show");
    }
    //表单-初始化
    //添加用户所属单位时跟登录的单位管理员所属单位相同
    $scope.divisionInstitute = function () {
        $http.get(instituteInfoUrl)
            .success(function (response) {
                $scope.divisionInstituteInfo = response.response;
                $scope.formData.InstituteId = $scope.divisionInstituteInfo.InstituteId;
                $scope.formData.InstituteName = $scope.divisionInstituteInfo.Name;
                $scope.formData.Male = "男";
            })
            .error(function () {
                notif({
                    msg: "<b>错误:</b> 所属单位获取失败，请检查网络是否通畅",
                    type: "error"
                });
            });
    }
    
    //根据身份证号确定用户性别和出生日期
    $scope.fixMaleBithday = function () {
        if ($scope.formData.IDCard != null) {
            if ($scope.formData.IDCard.length == 18) {
                var year = $scope.formData.IDCard.substring(6, 10);
                var month = $scope.formData.IDCard.substring(10, 12);
                var day = $scope.formData.IDCard.substring(12, 14);
                $scope.formData.Birthday = year + "-" + month + "-" + day;
                var male = $scope.formData.IDCard.substring(16, 17);
                var judegeMale = male % 2;
                if (judegeMale == 0) {
                    $scope.formData.Male = "女";
                } else {
                    $scope.formData.Male = "男";
                }
            }
        }
    }
    //提交-添加表单
    $scope.submitAddUserForm = function () {
        $scope.isAddFormSubmit = true;
        if ($scope.AddUserForm.$valid) {
            $http({
                method: 'POST',
                url: userAddUrl,
                data: $scope.formData
            })
            .success(function (response) {
                if (response.status == 0) {
                    $scope.isExpertField = true;
                    notif({
                        msg: "<b>恭喜:</b> 用户信息添加成功",
                        type: "success"
                    });
                    $scope.newUserPersonId = response.response.PersonId; //添加用户后返回的PersonId
                    $scope.load_page($scope.p_current); //refresh
                } else {
                    notif({
                        msg: "<b>错误:</b>" + response.errorMsg,
                        type: "error"
                    });
                }
            })
            .error(function (response) {
                notif({
                    msg: "<b>错误:</b> " + response.errorMsg,
                    type: "error"
                });
            });
        } else {
            notif({
                type: "error",
                msg: "填写的信息存在不规范或者错误，请根据提示进行修改！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        }
    }

    //提交-用户的研究领域
    $scope.submitExpertField = function () {
        $scope.isFieldSubmit = true;
        $scope.fieldInfo.PersonId = $scope.newUserPersonId;
        $scope.fieldInfo2.PersonId = $scope.newUserPersonId;
        if ($scope.BasicInfo.$valid) {
            $http({
                method: 'POST',
                url: editExpertFieldUrl,
                data: [$scope.fieldInfo, $scope.fieldInfo2]
            })
            .success(function (response) {
                if (response.status == 0) {
                    notif({
                        msg: "<b>恭喜:</b> 该用户研究领域添加成功！",
                        type: "success"
                    });
                    $("#addApplication").modal("hide");
                    $scope.load_page($scope.p_current); //refresh
                } else {
                    notif({
                        msg: "<b>错误:</b>" + response.errorMsg,
                        type: "error"
                    });
                }
            })
            .error(function () {
                notif({
                    msg: "<b>错误:</b> 添加失败，请检查网络是否通畅",
                    type: "error"
                });
            });
        } else {
            notif({
                type: "error",
                msg: "填写的信息存在不规范或者错误，请根据提示进行修改！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        }
    }

    //添加用户模态框隐藏时触发置空函数
    $('#addApplication').on('hidden.bs.modal', function (e) {
        $scope.reloadAddUserModal();
    })
    //每次添加完用户，强制置空模态框
    $scope.reloadAddUserModal = function () {
        $scope.AddUserForm.$setPristine();
        $scope.AddUserForm.$setUntouched();
        $scope.isAddFormSubmit = false;

        $scope.isExpertField = false;
        $scope.newUserPersonId = 0;
        $scope.formData = {
            "InstituteId": null
        }
        $scope.BasicInfo.$setPristine();
        $scope.BasicInfo.$setUntouched();
        $scope.isFieldSubmit = false;

        $scope.fieldInfo.FieldId = "";
        $scope.fieldInfo.SubFieldId = "";

        $scope.fieldInfo.KeyWordsCN = "";
        $scope.fieldInfo.KeyWordsEN = "";

        $scope.fieldInfo2.FieldId = "";
        $scope.fieldInfo2.SubFieldId = "";

        $scope.fieldInfo2.KeyWordsCN = "";
        $scope.fieldInfo2.KeyWordsEN = "";
    }


    /********************************上传Excel文件********************************************/
    $scope.showImportUserFileModal = function () {
        $("#importUserFileModal").modal("show");
    }

    //URL
    var fileUploadUrl = "/api/instperson/upload";

    //文件上传
    $scope.uploadFiles = function (file, errFile) {
        $scope.clearFile();
        $scope.file = file;
        $scope.errFile = errFile;

        if ($scope.file != null) {
            $scope.file.upload = Upload.upload({
                url: fileUploadUrl,
                file: $scope.file
            });
        }

        file.upload.then(
            function (response) {
                $timeout(function () {
                    file.result = response.response;
                    notif({
                        msg: "<b>恭喜:</b> 文件上传成功",
                        type: "success"
                    });
                    $scope.load_page($scope.p_current); //刷新
                });
            },
            function (response) {
                if (response.status > 0) {
                    notif({
                        msg: "<b>错误:</b> 文件上传失败，请检查网络是否通畅",
                        type: "error"
                    });
                    $scope.showErrorMsg = response.status + ':' + response.errorMsg;
                }
            },
            function (evt) {
                file.progress = Math.min(100, parseInt(100.0 * evt.loaded / evt.total));
            }
        )
    }
    //Modal隐藏时调用置空
    $('#importUserFileModal').on('hidden.bs.modal', function (e) {
        $scope.clearFile();
    })
    //清空文件队列
    $scope.clearFile = function () {
        $scope.file = {};
        $scope.errFile = {};
    }

    //  *******************查看-用户具体信息********************
    //用户具体信息
    $scope.showUserInfoModal = function (user) {
        $scope.infoModalData = angular.copy(user);
        $("#userInfoModal").modal("show");
        $scope.lookExpertField($scope.infoModalData.PersonId);
    }
    //专家领域
    $scope.lookExpertField = function (lookPersonId) {
        $scope.lookFieldInfo = null;
        $scope.lookFieldInfo2 = null;

        var lookExpertFieldInfoUrl = expertFieldUrl + lookPersonId;
        $http.get(lookExpertFieldInfoUrl)
            .success(function (response) {
                if (response.response.length != 0) {
                    $scope.lookFieldInfo = response.response[0];
                    $scope.lookFieldInfo2 = response.response[1];
                }
            })
            .error(function () {
                notif({
                    msg: "<b>错误:</b> 专家领域获取失败",
                    type: "error"
                });
            });
    }
    $('#userInfoModal').on('hidden.bs.modal', function (e) {
        $timeout(function () {
            $location.url("/divisionUserList");
        }, 1)
        // 选取第一个标签页
        $('#userListTab li:eq(0) a').tab('show');
    })

    //编辑
    $scope.showEditUserInfoModal = function (user) {
        $scope.isEdit = true;
        $("#addApplication").modal("show");
        $scope.formData = angular.copy(user);
        $scope.newUserPersonId = $scope.formData.PersonId;
    }
    //编辑-下一步
    $scope.submitEditUserForm = function () {
        $scope.isAddFormSubmit = true;
        if ($scope.AddUserForm.$valid) {
            $http({
                method: 'PUT',
                url: editInfoUrl,
                data: $scope.formData
            })
                .success(function (response) {
                    if (response.status == 0) {
                        notif({
                            msg: "<b>恭喜:</b> 用户信息编辑成功",
                            type: "success"
                        });
                        $scope.expertField($scope.formData);
                        $scope.isExpertField = true;
                    } else {
                        notif({
                            msg: "<b>错误:</b> " + response.errorMsg,
                            type: "error"
                        });
                    }
                })
                .error(function (response) {
                    notif({
                        msg: "<b>错误:</b> 编辑失败，请检查网络是否通畅",
                        type: "error"
                    });
                });
        } else {
            notif({
                type: "error",
                msg: "填写的信息存在不规范或者错误，请根据提示进行修改！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        }
    }
    //获取研究领域
    $scope.getField = function () {
        $http.get(getFieldUrl)
            .success(function (response) {
                $scope.fields = response.response;
            })
            .error(function () {
                notif({
                    msg: "<b>错误:</b> 研究领域获取失败",
                    type: "error"
                });
            });
    }
    //获取子领域1
    $scope.getSubField = function (field) {
        $http.get(getSubfieldUrl + field)
            .success(function (response) {
                $scope.subFields = response.response;
            })
            .error(function () {
                notif({
                    msg: "<b>错误:</b> 研究子领域获取失败",
                    type: "error"
                });
            });
    }
    //获取子领域2
    $scope.getSubField2 = function (field2) {
        $http.get(getSubfieldUrl + field2)
            .success(function (response) {
                $scope.subFields2 = response.response;
            })
            .error(function () {
                notif({
                    msg: "<b>错误:</b> 研究子领域2获取失败",
                    type: "error"
                });
            });
    }
    //获取要编辑的-专家领域
    $scope.expertField = function (editModalData) {
        var personId = editModalData.PersonId;
        var expertFieldInfoUrl = expertFieldUrl + personId;

        $http.get(expertFieldInfoUrl)
            .success(function (response) {
                if (response.response.length != 0) {
                    $scope.fieldInfo = response.response[0];
                    $scope.fieldInfo2 = response.response[1];
                    $scope.getSubField($scope.fieldInfo.FieldId);
                    $scope.getSubField2($scope.fieldInfo2.FieldId);
                }
            })
            .error(function () {
                notif({
                    msg: "<b>错误:</b> 专家研究领域获取失败",
                    type: "error"
                });
            });
    }

    //删除
    $scope.showDeleteUserModal = function (user) {
        $("#deleteApplication").modal("show");
        $scope.deleteModalData = angular.copy(user);
    }
    //删除-提交
    $scope.deleteUser = function (personId) {
        var deleteUserUrl = deleteUrl + personId;
        $http({
            method: 'DELETE',
            url: deleteUserUrl
        })
            .success(function (response) {
                if (response.status == 0) {
                    notif({
                        msg: "<b>恭喜:</b> 成功删除该用户！",
                        type: "success"
                    });
                    $("#deleteApplication").modal("hide"); //提交成功后模态框消失
                    $scope.load_page($scope.p_current); //刷新当前页面
                } else {
                    notif({
                        msg: "<b>错误:</b>" + response.errorMsg,
                        type: "error"
                    });
                }
            })
            .error(function () {
                notif({
                    msg: "<b>错误:</b> 删除用户失败，请检查网络是否通畅",
                    type: "error"
                });
            });
    }

    //重置密码
    $scope.showResetPasswordModal = function (user) {
        $("#resetPwdModal").modal("show");
        $scope.resetPwdFormData = angular.copy(user);
    }
    $scope.resetPwd = function (personId) {
        var resetURL = resetUserUrl + personId;
        $http({
            method: 'PUT',
            url: resetURL
        })
            .success(function (response) {
                if (response.status == 0) {
                    notif({
                        msg: "<b>恭喜:</b> 密码重置成功",
                        type: "success"
                    });
                    $("#resetPwdModal").modal("hide"); //提交成功后模态框消失
                    $scope.load_page($scope.p_current); //刷新
                } else {
                    notif({
                        msg: "<b>错误:</b>" + response.errorMsg,
                        type: "error"
                    });
                }
            })
            .error(function (response) {
                notif({
                    msg: "<b>错误:</b> 密码重置失败，请检查网络是否通畅",
                    type: "error"
                });
            });
    }

    // ********************************推荐成为专家**************************************
    //modal show
    $scope.showRecommendedModal = function (user) {
        $("#beExpertApplication").modal("show");
        $scope.beExpertMomalData = user;
    }
    //submit modal
    $scope.submitBeExpert = function (personId) {
        $http({
            method: 'POST',
            url: batchBeExpertUrl + personId
        })
            .success(function (response) {
                if (response.status == 0) {
                    notif({
                        msg: "<b>恭喜:</b> 推荐专家成功",
                        type: "success"
                    });
                    $("#beExpertApplication").modal("hide"); //提交成功后模态框消失
                } else {
                    notif({
                        msg: "<b>错误:</b> " + response.errorMsg,
                        type: "error"
                    });
                }
                $scope.load_page($scope.p_current); //刷新
            })
            .error(function (response) {
                notif({
                    msg: "<b>错误:</b> 用户推荐失败，请检查网络是否通畅",
                    type: "error"
                });

            });
    }

    // ********************************初始化**************************************
    $scope.getField(); //获取研究领域

}]);