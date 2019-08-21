var erApp = angular.module('ngERApp',[]);


erApp.controller('ngERCtrl', function ($scope, $http, $timeout, $location) {
    
    var expertRecommendedListUrl = "/api/recommendation/inst/all/";//Get已推荐专家列表的Url{page}
    var expertNotRecommendListUrl = "/api/unRecommendation/inst/"; // 获取可推荐专家列表  {page}
    var expertInfoUrl = "/api/person/";//Get专家信息的Url
    var editExpertInfoUrl = "/api/recommendation/inst";//Put修改专家信息的Url
    var batchBeExpertUrl = "/api/recommendation/"; //Post批量推荐为专家的Url
    var removeUserRecmdUrl = "/api/recommendation/";//Delete:删除专家推荐的Url
    var expertInfoUrl = "/api/person/";
    var expertFieldUrl = "/api/expertfield/";


    // ********************初始化json数据********************
    $scope.listAlready = false;
    $scope.expertListIsNull = true;

    $scope.selectValue = 'canRecommend';
    $scope.expertTypeList = [
        { "Type": "canRecommend", "Name": "可推荐" },
        { "Type": "AlreadyRecommend", "Name": "已推荐" },
    ]
    $scope.pageItem = 0;
    $scope.count = 0;
    $scope.p_current = 0;
    $scope.p_count = 0;

    // *******************获取列表&分页**************************
    //选择某一页时
    $scope.load_page = function (page) {

        $scope.listAlready = false;

        if ($scope.selectValue == 'AlreadyRecommend') {
            $scope.getListUrl = expertRecommendedListUrl;
        }
        else if ($scope.selectValue = 'canRecommend') {
            $scope.getListUrl = expertNotRecommendListUrl;
        }

        $http.get($scope.getListUrl + page)
            .success(function (response) {
                if (response.response != null) {
                    $scope.expertRecommendedList = response.response.ItemDTOs;
                    //console.log($scope.userList);
                    $scope.pageItem = response.response.TotalPageNum;//总共多少页
                    $scope.count = response.response.TotalNum; //页面总共多少条
                    $scope.p_current = response.response.NowPage; //当前第几页
                    $scope.p_count = response.response.NowNum; //当前页面多少条
                    $scope.pages = [];
                    for (var i = 0; i < $scope.pageItem; i++) {
                        $scope.pages[i] = i + 1;
                    }
                    if ($scope.expertRecommendedList.length == 0) {
                        $scope.expertListIsNull = true;
                        $scope.listAlready = true;
                    }
                    if ($scope.expertRecommendedList.length != 0) {
                        $scope.expertListIsNull = false;
                        $scope.listAlready = true;
                    }
                }
                $scope.listAlready = true;
            })
            .error(function () {
                notif({
                    msg: "<b>错误:</b> 页面加载失败，请检查网络是否通畅",
                    type: "error"
                });
                $scope.listAlready = true;
            });
        
    }

    //初始化获取列表
    $scope.load_page(1);

    //   ***************用户基本信息************************
    //用户具体信息
    $scope.showUserInfoModal = function (user) {
        $scope.lookExpertField(user.PersonId);
        $scope.getUserInfo(user.PersonId);
        $("#userInfoModal").modal("show");
    }
    //个人基本信息
    $scope.getUserInfo = function (lookPersonId) {
        $http.get(expertInfoUrl + lookPersonId)
        .success(function (response) {
            $scope.infoModalData = response.response;
        })
        .error(function (response) {
            notif({
                msg: "<b>错误:</b> 个人信息获取失败",
                type: "error"
            });
        })
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
            $location.url("/expertsRecommend");
        }, 1)
        // 选取第一个标签页
        $('#userListTab li:eq(0) a').tab('show');
    })

    //	 ***************专家具体信息-模态框*****************
    //专家具体信息
    $scope.showExpertInfoModal = function (recmdExpert) {
        $scope.infoModalData = recmdExpert;
        var personId = recmdExpert.CandidatePersonId;

        var detailUrl = expertInfoUrl + personId;
        $http.get(detailUrl)
		    .success(function (response) {
		        $scope.modalData = response.response;
		    })
		    .error(function () {
		        notif({
		            msg: "<b>错误:</b> 负责人信息获取失败，请检查网络是否通畅",
		            type: "error"
		        });
		    });
	   $("#expertInfoModal").modal("show");
    }
    // ******************推荐成为专家********************
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



    // ******************撤回专家推荐********************
	//模态框出现
    $scope.rejectRecmd = function (recmdExpert) {
        $("#rejectRecmdModal").modal("show");
        $scope.rejectModalData = recmdExpert;
    }
    //发送撤回请求
    $scope.rejectRecmdUser = function (RecommenderId) {
        var removeRecmdUrl = removeUserRecmdUrl + RecommenderId;
        $http({
            method: 'DELETE',
            url: removeRecmdUrl
        })
      .success(function (response) {
          if (response.status == 0) {
              notif({
                  msg: "<b>恭喜:</b> 撤回成功",
                  type: "success"
              });
              $("#rejectRecmdModal").modal("hide");
              $scope.load_page($scope.p_current);
          } else {
              notif({
                  msg: "<b>错误:</b> " + response.errorMsg,
                  type: "error"
              });
          }          
      })
       .error(function () {
           notif({
               msg: "<b>错误:</b> 撤回失败,请检查网络是否通畅",
               type: "error"
           });
       });
    }
})