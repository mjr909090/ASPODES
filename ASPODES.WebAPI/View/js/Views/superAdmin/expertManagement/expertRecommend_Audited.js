var expertCommend_Audited = angular.module('ngExpertReCommend_AuditedApp', []);

expertCommend_Audited.controller('ngExpertReCommend_AuditedCtrl', function ($scope, $http) {
    //url
    var getExpertRecommendListUrl = "/api/recomendation/depart/"//获取专家推荐列表  api/recomendation/depart
    var auditWinSingleUrl = "/api/recommendation/adopt/"//同意专家推荐  recommendationId}
    var rejectRecommendUrl = "/api/recommendation/refuse/"//拒绝专家推荐  recommendationId}
    var aduitWinSureUrl = "/api/recommendation/adopt/all"//批量审核的确定
    var getExpertFieldUrl = "/api/expertfield/"  //获取研究领域
    var expertInfoUrl = "/api/person/"   //查看详细信息
    var instTypeUrl = "/api/inst "       //获取下拉框单位列表
    var deleteExpertRecommendation = "/api/Recommendation/"  //{id} 删除专家推荐

    $scope.selectedInst = null;

    // ********************初始化json数据********************
    $scope.selectedInst = {};
    $scope.divisionAppliesIsNull = false;
    //刷新
    $scope.refresh = function () {
        $scope.getExpertRecommendList();
    }
    //点击专家名称，查看详细信息的模态框
    $scope.showViewExpertInfoModal = function (PersonId) {
        $http.get(expertInfoUrl + PersonId)
        .success(function (response) {
            $scope.expertInfo = response.response;
            $scope.instTypes.forEach(function (e) {//对单位进行遍历
                if (e.InstituteId == $scope.expertInfo.InstituteId) {
                    $scope.expertInfo.InstituteName = e.Name
                }
            });

            $http.get(getExpertFieldUrl + PersonId)//获取专家研究领域
                .success(function (response) {
                    $scope.expertFieldInfo = response.response;
                    $scope.expertFieldInfo.forEach(function (val, index, arr) {//val为数组中当前的值，index为当前值的下表，arr为原数组
                        $scope.expertFieldInfo1 = arr[0];
                        $scope.expertFieldInfo2 = arr[1];
                        $scope.getSubField($scope.editFieldInfo.FieldId);//初始化时获取子研究领域的数据
                    });

                })
                .error(function () {
                    //alert("error!!");
                });

            $("#viewExpertInfoModal").modal("show");
        })
        .error(function () {
           // alert("error!!");
        });
    };
    //单个审核的模态框
    $scope.showAuditWinSingleModal = function (RecommendationId) {
        $("#auditWinSingle").modal("show");
        $scope.RecommendationId = RecommendationId;
    };
    //单个审核中的确定
    $scope.aduitWinSingleSure = function () {
        $http({
            method: 'PUT',
            url: auditWinSingleUrl + $scope.RecommendationId
        })
        .then(function successCallback(response) {
            if (response == null) {
                notif({
                    type: "error",
                    msg: "审核操作失败！专家研究领域不全.",
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                })
            } else {
                $("#addExpert").modal("hide");
                notif({
                    msg: "审核操作成功！",
                    type: "success"
                });

                if ($scope.selectedInst != null) {
                    $scope.getInstIdForPage($scope.selectedInst.InstituteId);
                }
                else {
                    $scope.getInstIdForPage(0);
                }
                
            }


        })
        /*
        .success(function (response) {
            notif({
                msg: "审核操作成功！",
                type: "success"
            });
           // $scope.getExpertRecommendList();
            $scope.load_page($scope.p_current);
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "单个审核操作失败！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        });
        */
        
    };

    //驳回模态框
    $scope.showRejectExpertModal = function (RecommendationId) {
        $("#rejectExpertModal").modal("show");
        $scope.rejectRecommendationId = RecommendationId;
    };
    //驳回的确定
    $scope.rejectSure = function () {
        $http({
            method: 'PUT',
            url: rejectRecommendUrl + $scope.rejectRecommendationId,
            data: $scope.checked
        })

        .success(function (response) {
            notif({
                msg: "操作成功！",
                type: "success"
            });
            // $scope.getExpertRecommendList();
            $scope.load_page($scope.p_current);
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "操作失败！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        });
    };
    //获取单位下拉框单位列表
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
    //状态
    $scope.expertRecommendStatus = [
        { status: "adopt", Name:"已通过"},
       { status: "refuse", Name: "已拒绝" },
       { status: "handle", Name: "未处理" },
    ]
   
    //删除专家推荐的模态框
    $scope.showDeleteExpertModal = function (RecommendationId) {
        $("#deleteExpertRecommandModal").modal("show");
        $scope.deleteRecommendationId = RecommendationId;
    };
    //删除专家推荐的modal的确定
    $scope.deleteExpertRecommand = function () {
        $http.delete(deleteExpertRecommendation + $scope.deleteRecommendationId)
        .success(function (response) {
            //if (response.status == 0) {
            notif({
                msg: "<b>恭喜:</b> 专家推荐删除成功！",
                type: "success"
            });
            $scope.getExpertRecommendList();
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "专家推荐删除失败，请检查您的网络！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        });
    }
    /*************分页************************/

    //获取专家推荐列表    // api/recomendation/depart/{status}/{instId}/{page}

    $scope.getInstIdForPage = function (InstituteId) {//单位
        if (InstituteId == undefined) {
            $scope.InstituteId = "0"
        } else {
            $scope.InstituteId = InstituteId
        }
        //$scope.getExpertRecommendList();
        $scope.searchContent(1);//新加,搜索时默认从第一页

    }
    $scope.getStatusIdForPage = function (status) {//状态
        if (status == undefined) {
            $scope.status = "all"
        } else {
            $scope.status = status
        }
        // $scope.getExpertRecommendList();
        $scope.searchContent(1);//新加
    }

    /*新加 start*/
    $scope.load_page = function (page) {
        $scope.isSearch = false;
        if ($scope.InstituteId == undefined) {//单位
            thisInstituteId = '0'
        } else {
            thisInstituteId = $scope.InstituteId
        }
        if ($scope.status == undefined) {//状态
            thisStatus = 'all'
        } else {
            thisStatus = $scope.status
        }
        var expertRecommendListUrl = getExpertRecommendListUrl + thisStatus + "/" + thisInstituteId + "/" + page;
       
        $http.get(expertRecommendListUrl)
            .success(function (response) {
                if (response.response != null) {
                    $scope.expertRecommendList = response.response.ItemDTOs;
                    $scope.pageItem = response.response.TotalPageNum; //总共多少页
                    $scope.count = response.response.TotalNum; //页面总共多少条
                    $scope.p_current = response.response.NowPage; //当前第几页
                    $scope.p_count = response.response.NowNum; //当前页面多少条
                    $scope.pages = [];
                    for (var i = 0; i < $scope.pageItem; i++) {
                        $scope.pages[i] = i + 1;
                    }
                    if ($scope.expertRecommendList.length == 0) {
                        $scope.divisionAppliesIsNull = true;
                    } else {
                        $scope.divisionAppliesIsNull = false;
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
    $scope.isSearch = false;//初始化状态
    $scope.nextPage = function (page) {
        if (page <= $scope.pageItem && page >= 1 && page != $scope.p_current) {
            if ($scope.isSearch == false) {
                $scope.load_page(page);
            } else {
                $scope.searchContent(page);
            }
        }
    }
    $scope.judgeAbs = function (current, page) {
        if (Math.abs(current - page) < 3) {
            return true;
        } else
            return false;
    }
    $scope.searchContent = function (searchPage) {
        $scope.isSearch = true;
        if ($scope.InstituteId == undefined) {//单位
            thisInstituteId = '0'
        } else {
            thisInstituteId = $scope.InstituteId
        }
        if ($scope.status == undefined) {//状态
            thisStatus = 'all'
        } else {
            thisStatus = $scope.status
        }
        
        var expertRecommendListUrl = getExpertRecommendListUrl + thisStatus + "/" + thisInstituteId + "/" + searchPage;
       
        if ($scope.InstituteId == undefined || $scope.status == undefined) {
            $scope.load_page(1);
        } else {
            $http({
                method: 'GET',
                url: expertRecommendListUrl,
            })
            .success(function (response) {
                if (response.status == 0) {
                    $scope.expertRecommendList = response.response.ItemDTOs;
                    $scope.pageItem = response.response.TotalPageNum; //总共多少页
                    $scope.count = response.response.TotalNum; //页面总共多少条
                    $scope.p_current = response.response.NowPage; //当前第几页
                    $scope.p_count = response.response.NowNum; //当前页面多少条
                    $scope.pages = [];
                    for (var i = 0; i < $scope.pageItem; i++) {
                        $scope.pages[i] = i + 1;
                    }
                } else {
                    notif({
                        msg: "<b>错误:</b>" + response.errorMsg,
                        type: "error"
                    });
                }
            })
            .error(function () {
                notif({
                    msg: "<b>错误:</b> 页面加载失败，请检查网络是否通畅",
                    type: "error"
                });
            });
        }
       
    }
    /*
   新加 end
   */


    //上面有新加，下面全都注释掉

    /*
     //获取专家推荐列表
    $scope.getExpertRecommendList = function () {
        if ($scope.InstituteId == undefined) {//单位
            thisInstituteId = '0'
        } else {
            thisInstituteId = $scope.InstituteId
        }
        if ($scope.status == undefined) {//状态
            thisStatus = 'all'
        } else {
            thisStatus = $scope.status
        }

        var expertRecommendListUrl = getExpertRecommendListUrl + thisStatus + "/" + thisInstituteId + "/1";
        $http.get(expertRecommendListUrl)
              .success(function (response) {
                  $scope.expertRecommendList = response.response.ItemDTOs;
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
              })
              .error(function () {
                  notif({
                      msg: "<b>Error:</b> 专家列表未能加载，请重试",
                      type: "error"
                  });
              });
    }
    $scope.getExpertRecommendList();//启动方法
    //选择首页
    $scope.p_index = function () {
        if ($scope.InstituteId == undefined) {//单位
            InstituteId = '0'
        } else {
            InstituteId = $scope.InstituteId
        }
        if ($scope.status == undefined) {//状态
            thisStatus = 'all'
        } else {
            thisStatus = $scope.status
        }
        var expertRecommendListUrl = getExpertRecommendListUrl + thisStatus + "/" + thisInstituteId + "/1";

        $http.get(expertRecommendListUrl)
              .success(function (response) {
                  $scope.expertRecommendList = response.response.ItemDTOs;
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
            InstituteId = '0'
        } else {
            InstituteId = $scope.InstituteId
        }
        if ($scope.status == undefined) {//状态
            thisStatus = 'all'
        } else {
            thisStatus = $scope.status
        }
        $scope.p_current = $scope.p_current - 1;
        var expertRecommendListUrl = getExpertRecommendListUrl + thisStatus + "/" + thisInstituteId + "/" + $scope.p_current;
        $http.get(expertRecommendListUrl)
              .success(function (response) {
                  $scope.expertRecommendList = response.response.ItemDTOs;
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
    $scope.load_page = function (page) {
        if ($scope.InstituteId == undefined) {//单位
            InstituteId = '0'
        } else {
            InstituteId = $scope.InstituteId
        }
        if ($scope.status == undefined) {//状态
            thisStatus = 'all'
        } else {
            thisStatus = $scope.status
        }
        var expertRecommendListUrl = getExpertRecommendListUrl + thisStatus + "/" + thisInstituteId + "/" + page;

        $http.get(expertRecommendListUrl)
              .success(function (response) {
                  $scope.expertRecommendList = response.response.ItemDTOs;
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
                      msg: "<b>Error:</b> Page's not loaded",
                      type: "error"
                  });
              });
    }
    //Next 后一页
    $scope.p_next = function () {
        if ($scope.InstituteId == undefined) {//单位
            InstituteId = '0'
        } else {
            InstituteId = $scope.InstituteId
        }
        if ($scope.status == undefined) {//状态
            thisStatus = 'all'
        } else {
            thisStatus = $scope.status
        }
        $scope.p_current = $scope.p_current + 1;
        var expertRecommendListUrl = getExpertRecommendListUrl + thisStatus + "/" + thisInstituteId + "/" + $scope.p_current;

        $http.get(expertRecommendListUrl)
              .success(function (response) {
                  $scope.expertRecommendList = response.response.ItemDTOs;
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
        if ($scope.InstituteId == undefined) {//单位
            InstituteId = '0'
        } else {
            InstituteId = $scope.InstituteId
        }
        if ($scope.status == undefined) {//状态
            thisStatus = 'all'
        } else {
            thisStatus = $scope.status
        }
        var expertRecommendListUrl = getExpertRecommendListUrl + thisStatus + "/" + thisInstituteId + "/" + $scope.pageItem;

        $http.get(expertRecommendListUrl)
              .success(function (response) {
                  $scope.expertRecommendList = response.response.ItemDTOs;
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
    */

   
});

