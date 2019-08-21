var userDeletedApp = angular.module('ngUserDeletedApp', []);

userDeletedApp.controller('ngUserDeletedCtrl', function ($scope, $http) {
    
    var userDeletedListUrl = "/api/instperson/delete/";//Get已删除用户列表的Url
    var deletedUserRestartUrl = "/api/instperson/revive/"; //	Get启用被删用户的Url

    //  ********初始化********
    $scope.listAlready = false;
    $scope.userListIsNull = true;
	
    // *******************获取列表&分页**************************
    //选择某一页时
    $scope.load_page = function (page) {
        var personList = userDeletedListUrl + page;
        $http.get(personList)
            .success(function (response) {
                if (response.response != null) {
                    $scope.deletedUserList = response.response.ItemDTOs;
                    //console.log($scope.deletedUserList);
                    $scope.pageItem = response.response.TotalPageNum;//总共多少页
                    $scope.count = response.response.TotalNum; //页面总共多少条
                    $scope.p_current = response.response.NowPage; //当前第几页
                    $scope.p_count = response.response.NowNum; //当前页面多少条
                    $scope.pages = [];
                    for (var i = 0; i < $scope.pageItem; i++) {
                        $scope.pages[i] = i + 1;
                    }
                    if ($scope.deletedUserList.length == 0) {
                        $scope.userListIsNull = true;
                        $scope.listAlready = true;
                    } else {
                        $scope.userListIsNull = false;
                        $scope.listAlready = true;
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

    //初始化获取列表
    $scope.load_page(1);

    //查看-用户具体信息
    $scope.showUserInfoModal = function (deletedUser) {
        $("#userInfoModal").modal("show");
        $scope.infoModalData = deletedUser;
    }


    //*******************启用用户********************
    $scope.showUserRestartModal = function (deletedUser) {
        $("#userRestartModal").modal("show");
        $scope.userRestartModalData = deletedUser;
    }
    $scope.restartUser = function (personId) {
        var userRestartUrl = deletedUserRestartUrl + personId;
        $http({
            method: 'PUT',
            url: userRestartUrl
        })
         .success(function (response) {
             notif({
                 msg: "<b>恭喜:</b> 启用用户成功",
                 type: "success"
             });
             $("#userRestartModal").modal("hide"); //提交成功后模态框消失
             $scope.load_page($scope.p_current);  //刷新
         })
          .error(function () {
              notif({
                  msg: "<b>错误:</b> 启用用户失败",
                  type: "error"
              });
          });
    } 
})