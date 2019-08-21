var announcementApp = angular.module('ngAnnouncementApp', ['ngFileUpload', 'ngMessages', "ngCookies"]);

announcementApp.controller('ngAnnouncementCtrl', ['$scope', '$http', 'Upload', '$timeout','$cookies', function ($scope, $http, Upload, $timeout, $cookies) {


    //url
    var getNoticeUrl = "/api/Announcement/inst/";//院管理员获取公告 api/Announcement/depart/{page}

    var addNoticeUrl = "/api/Announcement/inst/"//院管理员添加公告 post
    var editNoticeUrl = "/api/Announcement/inst";//院管理员编辑公告信息 put
    var deleteNoticeUrl = "/api/Announcement/inst/";       //院管理员删除公告
    var uploadUrl = "/api/AnnouncementAttachment/";
    //URL
    var getFileListUrl = "/api/AnnouncementAttachment/";//{AnnouncementId}
    var fileUploadUrl = "/api/AnnouncementAttachment/";//上传附件
    var fileDeleteUrl = "/api/AnnouncementAttachment/"//删除附件{AnnouncementAttachmentId}",delete,

    // ********************初始化json数据********************
    $scope.departAppliesIsNull = false;//表格为空时
    $scope.fileTableList = {};//添加
    $scope.editfileTableList = {}; //编辑

    $scope.filesArry = [];
    $scope.fileIdArry = []; //文件列表Id集合
    $scope.filesTable = true;//添加时 的文件列表
    $scope.editFilesTableIsNull = true;//编辑时 的文件列表


    //去掉$$hashkey
    $scope.removeHashkey = function (item) {
        return angular.copy(item);
    }

    //判断是否选中
    $scope.exists = function (item, list) {

        var newList = $scope.removeHashkey(list);
        var newItem = $scope.removeHashkey(item);


        var position = -1;

        for (var i = 0; i < newList.length; i++) {
            if (_.isEqual(newList[i], newItem)) {
                position = i;
            }
        }

        return position;

    };

    //选中和取消选中操作
    $scope.toggle = function (item, list) {

        var idx = $scope.exists(item, list);
        console.log(idx);
        if (idx > -1) {
            list.splice(idx, 1);
        }

    }



    //添加公告模态框
    $scope.showAddNoticeModal = function () {
        $scope.filesArry = [];
        $scope.fileIdArry = [];
        $scope.fileTableList = {};
        $("#addNoticeModal").modal("show");
    }
    //添加公告模态框中的确定
    $scope.saveNotice = function () {
        if ($scope.fileIdArry != []) {
            $scope.formData.Attachments = $scope.fileIdArry;
        }
        $scope.formData.Content = CKEDITOR.instances.editorNotice.getData(); //html文本
        //console.log($scope.formData);
        //$scope.formData.Content = CKEDITOR.instances.editorNotice.document.getBody().getText(); //取得纯文本

        $http({
            method: 'POST',
            url: addNoticeUrl,
            data: $scope.formData
        })
        .success(function (response) {
            notif({
                msg: "添加操作成功！",
                type: "success"
            });
            $scope.getNoticeList();
            $scope.getAnnouncementId = response.response.AnnouncementId;
            $("#addNoticeModal").modal("hide");
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
    //添加-获取文件列表
    $scope.filesTableList = function () {
        $scope.fileIdArry = [];
        $http.get(getFileListUrl + $scope.getAnnouncementId)
        .success(function (response) {
            $scope.fileTableList = response.response;
            if ($scope.fileTableList != null) {
                $scope.filesTable = false;
                angular.forEach($scope.fileTableList, function (file) {
                    $scope.fileIdArry.push(file.AnnouncementAttachmentId);
                })
            }
            else {
                $scope.filesTable = true;
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "未能获取到文件列表，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }

    //添加-文件上传
    $scope.uploadFiles = function (files, errFiles) {
        $scope.files = files;
        $scope.errFiles = errFiles;
        angular.forEach(files, function (file) {
            file.upload = Upload.upload({
                url: fileUploadUrl,
                file: file
            });
            file.upload.then(
                function (response) {
                    $scope.fileIdArry = [];
                    file.result = response.data.response;
                    $scope.filesArry.push(file.result);
                    $scope.fileTableList = $scope.filesArry;
                    $scope.filesTable = false;
                    angular.forEach($scope.fileTableList, function (file) {
                        $scope.fileIdArry.push(file.AnnouncementAttachmentId);
                    })
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
        })
    }
    //添加-文件删除
    $scope.deleteFile = function (file) {
        $http.delete(fileDeleteUrl + file.AnnouncementAttachmentId)
        .success(function (response) {
            notif({
                msg: "<b>恭喜:</b> 文件删除成功",
                type: "success"
            });

            //将删除的条目从list中去除
            $scope.toggle(file, $scope.fileTableList);

            $scope.fileIdArry = [];
            angular.forEach($scope.fileTableList, function (file) {
                $scope.fileIdArry.push(file.AnnouncementAttachmentId);
            })


        })
        .error(function () {
            notif({
                type: "error",
                msg: "未能成功删除文件，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        });
    }
    //AddNoticeModal 隐藏 触发
    $('#addNoticeModal').on('hidden.bs.modal', function (e) {
        $scope.resetText();
        $scope.clearFiles();//添加用户模态框隐藏时触发置空函数
    })
    //置空 
    $scope.resetText = function () {
        CKEDITOR.instances.editorNotice.setData('');
        $scope.formData = {};
        $scope.fileTableList = {};//添加
        $scope.filesTable = true;
    }
    //添加页面 刷新
    $scope.clearFiles = function () {
        $scope.files = {};
        $scope.errFiles = {};
        $scope.filesTableList = {};
        $scope.filesTable = true;
    }

    //删除公告模态框
    $scope.showDeleteNoticeModal = function (AnnouncementId) {
        $scope.AnnouncementId = AnnouncementId;
        $("#deleteNoticeModal").modal("show");
    }
    //删除的modal的确定
    $scope.deleteNotice = function () {
        $http.delete(deleteNoticeUrl + $scope.AnnouncementId)
        .success(function (response) {
            notif({
                msg: "<b>恭喜:</b> 公告删除成功！",
                type: "success"
            });
            $scope.getNoticeList();

        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "公告删除失败，请检查您的网络！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        });
    }

    //查看公告
    $scope.showViewNoticeModal = function (notice) {
        var token = $cookies.get("token");
        window.open("/View/Views/announce/announceDetail.html?announceId=" + notice.AnnouncementId + "&token=" + token);
    }


    //编辑公告模态框
    $scope.showEditNoticeModal = function (notice) {
        // console.log(notice)
        $scope.editAnnouncementId = notice.AnnouncementId;
        $http.get('/api/Announcement/' + notice.AnnouncementId)
            .success(function (response) {
                $scope.editNoticeFormData = response.response;
                //console.log(response.response);
                CKEDITOR.instances.editor2.setData($scope.editNoticeFormData.Content);
                $scope.editfileTableList = $scope.editNoticeFormData.Attachments;

                if ($scope.editfileTableList.length != 0) {
                    $scope.editFilesTableIsNull = false;
                } else {
                    $scope.editFilesTableIsNull = true;
                }

                $("#editNoticeModal").modal("show");
            })
            .error(function () {
                notif({
                    msg: "<b>错误：</b> 公告获取失败，请检查网络是否通畅",
                    type: "error"
                });
            })

    }
    //编辑公告的确定
    $scope.editNotice = function () {
        if ($scope.fileIdArry != []) {
            $scope.editNoticeFormData.Attachments = $scope.fileIdArry;
        }
        $scope.editNoticeFormData.Content = CKEDITOR.instances.editor2.getData(); //html文本
        $http({
            method: 'PUT',
            url: editNoticeUrl,
            data: $scope.editNoticeFormData
        })
        .success(function (response) {
            notif({
                msg: "编辑操作成功！",
                type: "success"
            });
            $("#editNoticeModal").modal('hide');
            $scope.getNoticeList();
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
    //编辑 获取文件列表
    $scope.editFilesTableList = function () {
        $scope.fileIdArry = [];
        $http.get(getFileListUrl + $scope.editAnnouncementId)
        .success(function (response) {
            $scope.editfileTableList = response.response;
            if ($scope.editfileTableList != null) {
                $scope.editFilesTableIsNull = false;
                angular.forEach($scope.editfileTableList, function (file) {
                    $scope.fileIdArry.push(file.AnnouncementAttachmentId);
                })
            }
            else {
                $scope.editFilesTableIsNull = true;
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "未能获取到文件列表，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }
    //编辑 文件上传
    $scope.uploadFiles2 = function (files, errFiles) {

        $scope.files = files;
        $scope.errFiles = errFiles;
        angular.forEach(files, function (file) {
            file.upload = Upload.upload({
                url: fileUploadUrl + $scope.editAnnouncementId,
                file: file
            });
            file.upload.then(
                function (response) {
                    $timeout(function () {
                        file.result = response.response;
                        $scope.editFilesTableIsNull = false;
                        $scope.editFilesTableList();
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
        })
    }
    //编辑 文件删除
    $scope.deleteFile2 = function (AnnouncementAttachmentId) {

        $http.delete(fileDeleteUrl + AnnouncementAttachmentId)
        .success(function (response) {

            notif({
                msg: "<b>恭喜:</b> 文件删除成功",
                type: "success"
            });

            $scope.editFilesTableList();

        })
        .error(function () {
            notif({
                type: "error",
                msg: "未能成功删除文件，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        });
    }
    //editNoticeModal 隐藏 触发
    $('#editNoticeModal').on('hidden.bs.modal', function (e) {
        $scope.resetText2();
        $scope.editClearFiles();//添加用户模态框隐藏时触发置空函数
    })
    //置空 
    $scope.resetText2 = function () {
        CKEDITOR.instances.editor2.setData('');
        $scope.editNoticeFormData = {};
        $scope.editfileTableList = {};
        $scope.editFilesTableIsNull = true;
    }
    //刷新
    $scope.editClearFiles = function () {
        $scope.files = {};
        $scope.errFiles = {};
        $scope.editfileTableList = {};
        $scope.editFilesTableIsNull = true;
    }
    //获取公告
    $scope.getNoticeList = function () {
        var getNoticeList = getNoticeUrl + "1";
        $http.get(getNoticeList)
               .success(function (response) {
                   $scope.noticeList = response.response.ItemDTOs;
                   $scope.pageItem = response.response.TotalPageNum;//总共多少页
                   $scope.count = response.response.TotalNum; //页面总共多少条
                   $scope.p_current = response.response.NowPage; //当前第几页
                   $scope.p_count = response.response.NowNum; //当前页面多少条
                   $scope.pages = [];
                   for (var i = 0; i < $scope.pageItem; i++) {
                       $scope.pages[i] = i + 1;
                   }
                   if ($scope.pageItem == 0) {
                       $scope.departAppliesIsNull = true;
                   } else {
                       $scope.departAppliesIsNull = false;
                   }
               })
               .error(function () {
                   notif({
                       msg: "<b>Error:</b> 列表未能加载，请重试",
                       type: "error"
                   });
               });
    }
    $scope.getNoticeList();//启动方法
    //选择首页
    $scope.p_index = function () {
        var getNoticeList = getNoticeUrl + "1";
        $http.get(getNoticeList)
              .success(function (response) {
                  $scope.noticeList = response.response.ItemDTOs;
                  $scope.pageItem = response.response.TotalPageNum;//总共多少页
                  $scope.count = response.response.TotalNum; //页面总共多少条
                  $scope.p_current = response.response.NowPage; //当前第几页
                  $scope.p_count = response.response.NowNum; //当前页面多少条
                  if ($scope.pageItem == 0) {
                      $scope.departAppliesIsNull = true;
                  }
                  if ($scope.pageItem != 0) {
                      $scope.departAppliesIsNull = false;
                  }
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
        $scope.p_current = $scope.p_current - 1;
        var getNoticeList = getNoticeUrl + $scope.p_current;
        $http.get(getNoticeList)
              .success(function (response) {
                  $scope.noticeList = response.response.ItemDTOs;
                  $scope.pageItem = response.response.TotalPageNum;//总共多少页
                  $scope.count = response.response.TotalNum; //页面总共多少条
                  $scope.p_current = response.response.NowPage; //当前第几页
                  $scope.p_count = response.response.NowNum; //当前页面多少条
                  if ($scope.pageItem == 0) {
                      $scope.departAppliesIsNull = true;
                  }
                  if ($scope.pageItem != 0) {
                      $scope.departAppliesIsNull = false;
                  }
              })
              .error(function () {
                  notif({
                      msg: "<b>Error:</b> PreviousPage's not loaded",
                      type: "error"
                  });
              });
    }
    //选择某一页时
    $scope.load_page = function (page, flag) {
        var getNoticeList = getNoticeUrl + page;
        $http.get(getNoticeList)
              .success(function (response) {
                  $scope.noticeList = response.response.ItemDTOs;
                  $scope.pageItem = response.response.TotalPageNum;//总共多少页
                  $scope.count = response.response.TotalNum; //页面总共多少条
                  $scope.p_current = response.response.NowPage; //当前第几页
                  $scope.p_count = response.response.NowNum; //当前页面多少条

                  $scope.pages = [];
                  for (var i = 0; i < $scope.pageItem; i++) {//如果删除时 最后一条独占一页 则调用他的话没事，因为pageItem已经变成减去1的
                      $scope.pages[i] = i + 1;
                  }
                  if ($scope.pageItem == 0) {
                      $scope.departAppliesIsNull = true;
                  }
                  if ($scope.pageItem != 0) {
                      $scope.departAppliesIsNull = false;
                  }
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
        $scope.p_current = $scope.p_current + 1;
        var getNoticeList = getNoticeUrl + $scope.p_current;
        $http.get(getNoticeList)
              .success(function (response) {
                  $scope.noticeList = response.response.ItemDTOs;
                  $scope.pageItem = response.response.TotalPageNum;//总共多少页
                  $scope.count = response.response.TotalNum; //页面总共多少条
                  $scope.p_current = response.response.NowPage; //当前第几页
                  $scope.p_count = response.response.NowNum; //当前页面多少条
                  if ($scope.pageItem == 0) {
                      $scope.departAppliesIsNull = true;
                  }
                  if ($scope.pageItem != 0) {
                      $scope.departAppliesIsNull = false;
                  }
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
        var getNoticeList = getNoticeUrl + $scope.pageItem;

        $http.get(getNoticeList)
              .success(function (response) {
                  $scope.noticeList = response.response.ItemDTOs;
                  $scope.pageItem = response.response.TotalPageNum;//总共多少页
                  $scope.count = response.response.TotalNum; //页面总共多少条
                  $scope.p_current = response.response.NowPage; //当前第几页
                  $scope.p_count = response.response.NowNum; //当前页面多少条
                  if ($scope.pageItem == 0) {
                      $scope.departAppliesIsNull = true;
                  }
                  if ($scope.pageItem != 0) {
                      $scope.departAppliesIsNull = false;
                  }
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