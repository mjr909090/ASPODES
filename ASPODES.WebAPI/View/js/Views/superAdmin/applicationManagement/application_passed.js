//既往申请书
var application_passed = angular.module('ngApplication_PassedApp', ['publicFunction']);

application_passed.controller('ngApplication_PassedCtrl', function ($scope, $http, $downloadFile) {
    var applicationInfo = "/api/application/";
    var personURL = "/api/person/"
    var getPassedApplicationListUrl = "/api/departapplication/previous/"
    var projectTypeUrl = "/api/ProjectType/Deaprt"//var projectTypeUrl = "/api/projecttype"//获取下拉框项目类型列表
    var instTypeUrl = "/api/inst"       //获取下拉框单位列表

    $scope.downloadPDFUrl = "/api/applicationdoc/download/pdf/";//下载PDF的Url

    $scope.exportDeptPassedListUrl = "/api/deptapplication/previous"; // 单位管理员待受理申请书导出列表的url

    // 定义
    $scope.exportDeptPassedListFields = {};
    $scope.exportDeptPassedListName = "既往申请书";

    $scope.selectedInst = {};
    $scope.selectedProject = {};
    $scope.selectedInst.InstituteId = 0;
    $scope.selectedProject.ProjectTypeId = 0;

    // ********************初始化json数据********************
    $scope.divisionAppliesIsNull = false;

    //获取下拉框项目类型列表
    $scope.getProjectTypes = function () {
        $http.get(projectTypeUrl)
        .success(function (response) {
            $scope.projectTypes = response.response;
        })
        .error(function () {
           // alert("error!");
        });
    }
    $scope.getProjectTypes();//启动方法

    //获取下拉框单位列表
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
	

   //刷新
    $scope.refresh = function(){
        $scope.getPassedApplicationList();
    }
    $scope.saveApplicationId = function (ApplicationId) {
        $scope.postId = ApplicationId;
    }
    //点击申请书名称得到ApplicationId
    $scope.passedApplicationId = function (ApplicationId) {
      
        $scope.ApplicationId = ApplicationId;
    }
    //详细信息的模态框（点击项目名称）
    $scope.viewApplicationModal = function () {
       
        $http.get(applicationInfo + $scope.ApplicationId)
        .success(function (response) {
            $scope.applicationInfo = response.response;

            $("#viewApplication").modal("show");
        })
        .error(function () {
            //alert("error!!");
        });
    };
    //点击负责人得到LeaderId
    $scope.passedLeaderId = function (LeaderId) {
        $scope.LeaderId = LeaderId;
    }

    //负责人信息的模态框（点击项目名称）
    $scope.responserLeaderModal = function () {
        $http.get(personURL + $scope.LeaderId)
        .success(function (response) {
            $scope.personInfo = response.response;
            $("#responserLeader").modal("show");
        })
        .error(function () {
            //alert("error!!");
        });

    };
    //显示PDF页面
    $scope.showPDFpage = function (PDFUrl) {
        var PDFPageUrl = "/View/plugins/pdfjs/web/viewer.html?file=" + PDFUrl;
        window.open(PDFPageUrl);
    };
    //点击下载PDF
    $scope.downPDF = function (applicationId) {
        window.open("/api/applicationdoc/download/pdf/" + applicationId);
    }

    /*************分页************************/
  
    //获取既往申请书列表  api/departapplication/previous/{instituteId}/{projectTypeId}/{pageId}
    $scope.getInstIdForPage = function (InstituteId) {//单位
       
        if (InstituteId == undefined) {
            $scope.InstituteId = '0'
        } else {
            $scope.InstituteId = InstituteId
        }
        //$scope.getPassedApplicationList();
        $scope.searchContent(1);//新加
    }
    $scope.getProjectTypeIdForPage = function (ProjectTypeId) {//类型
     
        if (ProjectTypeId == undefined) {
            $scope.ProjectTypeId = '0'
        } else {
            $scope.ProjectTypeId = ProjectTypeId
        }
        //$scope.getPassedApplicationList();
        $scope.searchContent(1);//新加
    }
    
    /*新加 start*/
    $scope.load_page = function (page) {
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
        var passedApplicationList = getPassedApplicationListUrl + thisInstituteId + "/" + thisProjectTypeId + "/" + page;

        $http.get(passedApplicationList)
            .success(function (response) {
                if (response.response != null) {
                    $scope.passedApplications = response.response.ItemDTOs;
                    $scope.pageItem = response.response.TotalPageNum; //总共多少页
                    $scope.count = response.response.TotalNum; //页面总共多少条
                    $scope.p_current = response.response.NowPage; //当前第几页
                    $scope.p_count = response.response.NowNum; //当前页面多少条
                    $scope.pages = [];
                    for (var i = 0; i < $scope.pageItem; i++) {
                        $scope.pages[i] = i + 1;
                    }
                    if ($scope.passedApplications.length == 0) {
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
        if ($scope.ProjectTypeId == undefined) {//类型
            thisProjectTypeId = '0'
        } else {
            thisProjectTypeId = $scope.ProjectTypeId
        }
        var passedApplicationList = getPassedApplicationListUrl + thisInstituteId + "/" + thisProjectTypeId + "/" +  searchPage;

        if ($scope.InstituteId == undefined || $scope.ProjectTypeId == undefined) {
            $scope.load_page(1);
        } else {
            $http({
                method: 'GET',
                url: passedApplicationList,
            })
            .success(function (response) {
                if (response.status == 0) {
                    $scope.passedApplications = response.response.ItemDTOs;
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

    //	 ***************下载PDF-模态框*****************
    $scope.downloadPDF = function (applicationId, projectName) {
        $downloadFile.downloadFile($scope.downloadPDFUrl, applicationId, projectName);
    }

    // 导出列表
    $scope.exportDeptPassedList = function (ProjectTypeId, InstituteId) {
        if (ProjectTypeId == undefined) {
            $scope.ProjectTypeId = 0;
        } else {
            $scope.ProjectTypeId = ProjectTypeId;
        }
        if (InstituteId == undefined) {
            $scope.InstituteId = 0;
        } else {
            $scope.InstituteId = InstituteId;
        }
        $scope.exportDeptPassedListFields = {
            "Order": "序号",
            "Name": "申请书名称",
            "Deleage": "委托",
            "Category": "类型",
            "Leader": "牵头负责人",
            "InstituteId": $scope.InstituteId,
            "CategoryId": $scope.ProjectTypeId,
            "Year": 0
        };
        $downloadFile.exportApplication($scope.exportDeptPassedListUrl, $scope.exportDeptPassedListFields, $scope.exportDeptPassedListName);
    };

    /*
   新加 end
   */
    //上面有新加，下面全都注释掉

    /*
     //获取既往申请书列表
    $scope.getPassedApplicationList = function () {
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
        var passedApplicationList = getPassedApplicationListUrl + thisInstituteId + "/" + thisProjectTypeId + "/" + "1";
        $http.get(passedApplicationList)
               .success(function (response) {
                   $scope.passedApplications = response.response.ItemDTOs;
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
                       msg: "<b>Error:</b> 既往申请书列表未能加载，请重试",
                       type: "error"
                   });
               });
    }
    $scope.getPassedApplicationList();//启动方法
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
        var passedApplicationList = getPassedApplicationListUrl + thisInstituteId + "/" + thisProjectTypeId + "/" + "1";
      
        $http.get(passedApplicationList)
              .success(function (response) {
                  $scope.passedApplications = response.response.ItemDTOs;
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
        
        $scope.p_current = $scope.p_current - 1;
        var passedApplicationList = getPassedApplicationListUrl + thisInstituteId + "/" + thisProjectTypeId + "/" + $scope.p_current;
        //var passedApplicationList = getPassedApplicationListUrl + "0/" + "0/" + $scope.p_current;
        $http.get(passedApplicationList)
              .success(function (response) {
                  $scope.passedApplications = response.response.ItemDTOs;
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
            thisInstituteId = '0'
        } else {
            thisInstituteId = $scope.InstituteId
        }
        if ($scope.ProjectTypeId == undefined) {//类型
            thisProjectTypeId = '0'
        } else {
            thisProjectTypeId = $scope.ProjectTypeId
        }

        var passedApplicationList = getPassedApplicationListUrl + thisInstituteId + "/" + thisProjectTypeId + "/" + page;

       
        $http.get(passedApplicationList)
              .success(function (response) {
                  $scope.passedApplications = response.response.ItemDTOs;
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
            thisInstituteId = '0'
        } else {
            thisInstituteId = $scope.InstituteId
        }
        if ($scope.ProjectTypeId == undefined) {//类型
            thisProjectTypeId = '0'
        } else {
            thisProjectTypeId = $scope.ProjectTypeId
        }

       
        $scope.p_current = $scope.p_current +  + 1;
        var passedApplicationList = getPassedApplicationListUrl + thisInstituteId + "/" + thisProjectTypeId + "/" + $scope.p_current;
      
        $http.get(passedApplicationList)
              .success(function (response) {
                  $scope.passedApplications = response.response.ItemDTOs;
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
            thisInstituteId = '0'
        } else {
            thisInstituteId = $scope.InstituteId
        }
        if ($scope.ProjectTypeId == undefined) {//类型
            thisProjectTypeId = '0'
        } else {
            thisProjectTypeId = $scope.ProjectTypeId
        }

        var passedApplicationList = getPassedApplicationListUrl + thisInstituteId + "/" + thisProjectTypeId + "/" + $scope.pageItem;
  
        $http.get(passedApplicationList)
              .success(function (response) {
                  $scope.passedApplications = response.response.ItemDTOs;
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

    //刷新
    $scope.refresh = function () {
        $scope.load_page($scope.p_current); //刷新的是当前页
    }
    */
   
});

