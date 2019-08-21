//初审结果
var firstReviewResult = angular.module('ngFirstReviewResultApp',[]);

firstReviewResult.controller('ngFirstReviewResultCtrl', function ($scope, $http, $rootScope) {
    //url
    var reviewCommentListUrl = "/api/reviewcomment/depart/";//获取初审结果列表 
    var expertReviewUrl = "/api/reviewcomment/"//点击专家名字 获取评审
   // var projectTypeUrl = "/api/projecttype";//获取下拉框项目类型列表
   var projectTypeUrl = "/api/ProjectType/Deaprt"
    var instTypeUrl = "/api/inst";       //获取下拉框单位列表
    var reviewCommnetOneUrl = "/api/ReviewComment/";       //获取一个专家对该申请书的评审意见
    var exportUrl = "/api/reviewcomment/export/"//{instId}/{projectTypeId}/{count}"
    // ********************初始化json数据********************
    $scope.divisionAppliesIsNull = false;
    $scope.selectedInst = {
        InstituteId: 0
    };
    $scope.selectedProject = {
        ProjectTypeId: 0
    };
    $scope.selectedYear = {
        year: 0
    };
    $scope.InstituteId = 0;
    $scope.ProjectTypeId = 0;

    //导出
    $scope.exportData = function () {
        $("#exportDataModal").modal("show");
    }
    //导出的确定
    $scope.exportDataSure = function () {
        //alert("导出");
        $http.get(exportUrl + $scope.InstituteId + '/' + $scope.ProjectTypeId + '/' + $scope.selectedYear.year, { responseType: 'arraybuffer' })
            .success(function (data) {
            var blob = new Blob([data], { type: "application/vnd.ms-excel" });
            var objectUrl = URL.createObjectURL(blob);
            var aForDocument = $("<a download='初审结果.xls'><span class='aForDocument' ></span></a>").attr("href", objectUrl);
            $("body").append(aForDocument);
            $(".aForDocument").click();
            aForDocument.remove();
            })
        .error();
       
    }
    //获取下拉框 项目类型列表
    $scope.getProjectTypes = function () {
        $http.get(projectTypeUrl)
        .success(function (response) {
            $scope.projectTypes = response.response
        })
        .error(function () {
            //alert("error!");
        });
    }
    $scope.getProjectTypes();//启动方法
    //获取下拉框 单位列表
    $scope.instTypes = function () {
        $http.get(instTypeUrl)
        .success(function (response) {
            $scope.instTypes = response.response;
        })
        .error(function () {
           // alert("error!");
        });
    }
    $scope.instTypes();//启动方法


    //时间选择datepicker
    $('.date').datepicker({
        format: 'yyyy',
        language: 'zh-CN',
        autoclose: true,
        startView: 2,
        maxViewMode: 2,
        minViewMode: 2
    });
    //点击专家名称 获取评审结果expertReviewUrl {id}
    $scope.showExpertReviewModal = function (reviewComment, expertIndex) {
        $scope.ReviewCommentId = reviewComment.ReviewComments[expertIndex].ReviewCommentId;
        $http.get(reviewCommnetOneUrl + $scope.ReviewCommentId)
           .success(function (response) {
               $scope.reviewCommentInfo = response.response;
               $scope.reviewCommentInfo.ProjectName = reviewComment.ProjectName;
               $scope.reviewCommentInfo.ProjectTypeName = reviewComment.ProjectTypeName;
               $scope.reviewCommentInfo.LeaderName = reviewComment.LeaderName;

               $("#reviewCommentInfoModal").modal("show");
           })
           .error(function () {
               //alert("error!");
           });
    }
    /*************分页************************/

    $scope.getInstIdForPage = function (InstituteId) {//单位
        if (typeof(InstituteId) == 'undefined'|| InstituteId == null) {//单位
            $scope.InstituteId = "0"
        } else {
            $scope.InstituteId = InstituteId
        }
        $scope.getFirstReviewResultList();
    }
    $scope.getProjectTypeIdForPage = function (ProjectTypeId) {//类型
        if (typeof (ProjectTypeId) == undefined || ProjectTypeId == null) {
            $scope.ProjectTypeId = '0'
        } else {
            $scope.ProjectTypeId = ProjectTypeId
        }
        $scope.getFirstReviewResultList();
    }
    $scope.getYearForPage = function (yearId) {//年度
        
        if (yearId == undefined) {
            $scope.yearId = '0'
        } else {
            $scope.yearId = yearId
        }
       
        $scope.getFirstReviewResultList();
    }

    $scope.getFirstReviewResultList = function () {

        $scope.finishedListAlready = false;

        if ($scope.InstituteId == undefined) {//单位
            thisInstituteId = '0'
        } else {
            thisInstituteId = $scope.InstituteId
        }
        if ($scope.ProjectTypeId == undefined) {//类型
            thisProjectTypeId = '0'
        } else {
            thisProjectTypeId = $scope.ProjectTypeId
        }
        if ($scope.yearId == undefined) {//年度
            thisYearId = '0'
        } else {
            thisYearId = $scope.yearId
        }
        var reviewCommentList = reviewCommentListUrl + thisInstituteId + "/" + thisProjectTypeId + "/" + thisYearId + "/14" + "/" + "1";
       
        $http.get(reviewCommentList)
               .success(function (response) {
                   $scope.reviewCommentList = response.response.ItemDTOs;
                   $scope.pageItem = response.response.TotalPageNum;//总共多少页
                   $scope.count = response.response.TotalNum; //页面总共多少条
                   $scope.p_current = response.response.NowPage; //当前第几页
                   $scope.p_count = response.response.NowNum; //当前页面多少条
                   $scope.pages = [];
                   for (var i = 0; i < $scope.pageItem; i++) {
                       $scope.pages[i] = i + 1;
                   }
                   if ($scope.pageItem == 0) {
                       $scope.divisionAppliesIsNull = true;
                   }
                   if ($scope.pageItem != 0) {
                       $scope.divisionAppliesIsNull = false;
                   }

                   $scope.finishedListAlready = true;
               })
               .error(function () {
                   notif({
                       msg: "<b>Error:</b> 专家列表未能加载，请重试",
                       type: "error"
                   });
               });
    }
    $scope.getFirstReviewResultList();//启动方法
    //选择首页
    $scope.p_index = function () {
        if ($scope.InstituteId == undefined) {//单位
            thisInstituteId = '0'
        } else {
            thisInstituteId = $scope.InstituteId
        }
        if ($scope.ProjectTypeId == undefined) {//类型
            thisProjectTypeId = '0'
        } else {
            thisProjectTypeId = $scope.ProjectTypeId
        }
        if ($scope.yearId == undefined) {//年度
            thisYearId = '0'
        } else {
            thisYearId = $scope.yearId
        }
        var reviewCommentList = reviewCommentListUrl + thisInstituteId + "/" + thisProjectTypeId + "/" + thisYearId + "/" + "1";
       
        $http.get(reviewCommentList)
              .success(function (response) {
                  $scope.reviewCommentList = response.response.ItemDTOs;
                  $scope.pageItem = response.response.TotalPageNum;//总共多少页
                  $scope.count = response.response.TotalNum; //页面总共多少条
                  $scope.p_current = response.response.NowPage; //当前第几页
                  $scope.p_count = response.response.NowNum; //当前页面多少条
                  if ($scope.pageItem == 0) {
                      $scope.divisionAppliesIsNull = true;
                  }
                  if ($scope.pageItem != 0) {
                      $scope.divisionAppliesIsNull = false;
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
        if ($scope.InstituteId == undefined) {//单位
            thisInstituteId = '0'
        } else {
            thisInstituteId = $scope.InstituteId
        }
        if ($scope.ProjectTypeId == undefined) {//类型
            thisProjectTypeId = '0'
        } else {
            thisProjectTypeId = $scope.ProjectTypeId
        }
        if ($scope.yearId == undefined) {//年度
            thisYearId = '0'
        } else {
            thisYearId = $scope.yearId
        }

        $scope.p_current = $scope.p_current - 1;
        var reviewCommentList = reviewCommentListUrl + thisInstituteId + "/" + thisProjectTypeId + "/" + thisYearId + "/" + $scope.p_current;
        $http.get(reviewCommentList)
              .success(function (response) {
                  $scope.expertList = response.response.ItemDTOs;
                  $scope.reviewCommentList = response.response.ItemDTOs;
                  $scope.pageItem = response.response.TotalPageNum;//总共多少页
                  $scope.count = response.response.TotalNum; //页面总共多少条
                  $scope.p_current = response.response.NowPage; //当前第几页
                  $scope.p_count = response.response.NowNum; //当前页面多少条
                  if ($scope.pageItem == 0) {
                      $scope.divisionAppliesIsNull = true;
                  }
                  if ($scope.pageItem != 0) {
                      $scope.divisionAppliesIsNull = false;
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
        if ($scope.InstituteId == undefined) {//单位
            thisInstituteId = '0'
        } else {
            thisInstituteId = $scope.InstituteId
        }
        if ($scope.ProjectTypeId == undefined) {//类型
            thisProjectTypeId = '0'
        } else {
            thisProjectTypeId = $scope.ProjectTypeId
        }
        if ($scope.yearId == undefined) {//年度
            thisYearId = '0'
        } else {
            thisYearId = $scope.yearId
        }

        var reviewCommentList = reviewCommentListUrl + thisInstituteId + "/" + thisProjectTypeId + "/" + thisYearId + "/" + page;

        $http.get(reviewCommentList)
              .success(function (response) {
                  $scope.reviewCommentList = response.response.ItemDTOs;
                  $scope.pageItem = response.response.TotalPageNum;//总共多少页
                  $scope.count = response.response.TotalNum; //页面总共多少条
                  $scope.p_current = response.response.NowPage; //当前第几页
                  $scope.p_count = response.response.NowNum; //当前页面多少条
                  //alert($scope.pageItem)
                  $scope.pages = [];
                  for (var i = 0; i < $scope.pageItem; i++) {//如果删除时 最后一条独占一页 则调用他的话没事，因为pageItem已经变成减去1的
                      $scope.pages[i] = i + 1;
                  }
                  if ($scope.pageItem == 0) {
                      $scope.divisionAppliesIsNull = true;
                  }
                  if ($scope.pageItem != 0) {
                      $scope.divisionAppliesIsNull = false;
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
        // alert('选择 后一页');
        if ($scope.InstituteId == undefined) {//单位
            thisInstituteId = '0'
        } else {
            thisInstituteId = $scope.InstituteId
        }
        if ($scope.ProjectTypeId == undefined) {//类型
            thisProjectTypeId = '0'
        } else {
            thisProjectTypeId = $scope.ProjectTypeId
        }
        if ($scope.yearId == undefined) {//年度
            thisYearId = '0'
        } else {
            thisYearId = $scope.yearId
        }

        $scope.p_current = $scope.p_current + 1;
        var reviewCommentList = reviewCommentListUrl + thisInstituteId + "/" + thisProjectTypeId + "/" + thisYearId + "/" + $scope.p_current;

        $http.get(reviewCommentList)
              .success(function (response) {
                  $scope.reviewCommentList = response.response.ItemDTOs;
                  $scope.pageItem = response.response.TotalPageNum;//总共多少页
                  $scope.count = response.response.TotalNum; //页面总共多少条
                  $scope.p_current = response.response.NowPage; //当前第几页
                  $scope.p_count = response.response.NowNum; //当前页面多少条
                  if ($scope.pageItem == 0) {
                      $scope.divisionAppliesIsNull = true;
                  }
                  if ($scope.pageItem != 0) {
                      $scope.divisionAppliesIsNull = false;
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
        //  alert('选择最后一页');
        if ($scope.InstituteId == undefined) {//单位
            thisInstituteId = '0'
        } else {
            thisInstituteId = $scope.InstituteId
        }
        if ($scope.ProjectTypeId == undefined) {//类型
            thisProjectTypeId = '0'
        } else {
            thisProjectTypeId = $scope.ProjectTypeId
        }
        if ($scope.yearId == undefined) {//年度
            thisYearId = '0'
        } else {
            thisYearId = $scope.yearId
        }

        var reviewCommentList = reviewCommentListUrl + thisInstituteId + "/" + thisProjectTypeId + "/" + thisYearId + "/" + $scope.pageItem;
        $http.get(reviewCommentList)
              .success(function (response) {
                  $scope.reviewCommentList = response.response.ItemDTOs;
                  $scope.pageItem = response.response.TotalPageNum;//总共多少页
                  $scope.count = response.response.TotalNum; //页面总共多少条
                  $scope.p_current = response.response.NowPage; //当前第几页
                  $scope.p_count = response.response.NowNum; //当前页面多少条
                  if ($scope.pageItem == 0) {
                      $scope.divisionAppliesIsNull = true;
                  }
                  if ($scope.pageItem != 0) {
                      $scope.divisionAppliesIsNull = false;
                  }
              })
              .error(function () {
                  notif({
                      msg: "<b>Error:</b> LastPage's not loaded",
                      type: "error"
                  });
              });
    }

    //计算总分
    $scope.calScore = function () {
        $http.get('/api/reviewComment/calScore')
        .success(function (response) {
                notif({
                    msg: "计算成功",
                    type: "success"
                });
        })
        .error(function(response){
            notif({
                msg: response.errorMsg,
                type: "error"
            });
        })
    }


    //刷新
    $scope.refresh = function () {
        $scope.load_page($scope.p_current); //刷新的是当前页
    }
});
