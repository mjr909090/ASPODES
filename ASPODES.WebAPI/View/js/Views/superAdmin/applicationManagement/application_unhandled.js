/**
 * Created by majunjun on 2017/4/7.
 */
var application_accepted = angular.module('ngApplication_UnhandledApp', ['publicFunction']);

application_accepted.controller('ngApplication_UnhandledCtrl', function ($scope, $http, $downloadFile) {
    $scope.postId = null;
    //url
    var unhandledApplicationUrl = "/api/departapplication/accept/";
    var refusedApplicationUrl = "/api/departapplication/refuse";
    var applicationInfo = "/api/application/";
    var personURL = "/api/person/";
    var projectTypeUrl = "/api/ProjectType/Deaprt"//"/api/projecttype";//获取下拉框项目类型列表
    var instTypeUrl = "/api/inst";       //获取下拉框单位列表
    var assignExpert = "/api/reviewassignmeng/application/"             //自动给申请书指派专家

    $scope.downloadPDFUrl = "/api/applicationdoc/download/pdf/";//下载PDF的Url

    $scope.exportDeptUnhandledListUrl = "/api/deptapplication/export"; // 导出列表的url
    $scope.packageDownloadUrl = "/api/applicationdoc/inst/downloadPackage/"; // 批量下载

    // 定义
    $scope.exportDeptUnhandledListFields = {};
    $scope.exportDeptUnhandledListName = "待受理申请书列表";
    $scope.deptApplicationPackageName = "待受理申请书文件";

    // ********************初始化json数据********************
    $scope.divisionAppliesIsNull = false;
    $scope.selectedInst = {};
    $scope.selectedProject = {};
    $scope.selectedInst.InstituteId = 0;
    $scope.selectedProject.ProjectTypeId = 0;
    
    //get
    //获取下拉框项目类型列表
    $scope.getProjectTypes = function () {
        $http.get(projectTypeUrl)
        .success(function (response) {
            $scope.projectTypes = response.response
        })
        
        .error(function () {
            //alert("error!");
        });
        console.log('1getprojectTypes', $scope.projectTypes)
    }
    $scope.getProjectTypes();//启动方法
    //获取下拉框单位列表
    $scope.instTypes= function () {
        $http.get(instTypeUrl)
        .success(function (response) {
            $scope.instTypes = response.response;
        })
        .error(function () {
            //alert("error!");
        });
    }
    $scope.instTypes();//启动方法

    /********************不受理 start*************************/
    //不受理的模态框
    $scope.showRejectApplicationModal = function (ApplicationId) {
        $("#rejectApplication").modal("show");
        $scope.rejectApplicationId = ApplicationId;
    };

    $scope.array = ['222', '21', '3', '33'];
    $scope.unhandledReasonChange = function () {
      //  document.getElementById("unhandledReasonId").value = $scope.selectOne;
        $scope.unhandledReason = $scope.selectOne;
    };
    //不受理模态框中的确定
    $scope.handleApplicationNotSure = function () {
        $http({
            method: 'PUT',
            url: refusedApplicationUrl,
            data: {
                "ApplicationId": $scope.rejectApplicationId,
                "Comment": $scope.unhandledReason
            }
        })
        //$http.put(refusedApplicationUrl)
        .success(function (response) {
            $("#rejectApplication").modal("hide");
            // if (response.status == 0) {
            notif({
                msg: "申请书不受理操作成功！",
                type: "success"
            });
            $scope.load_page($scope.p_current);
            // }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "申请书不受理操作失败！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        });
    };
    //每次点击驳回按钮，强制置空文本框的值
    $scope.reloadRejectModal = function (unhandledApplication) {
        $scope.selectOne = "";
        $scope.unhandledReason = "";
        $(':input', unhandledApplication).each(function () {
            var type = this.type;
            var tag = this.tagName.toLowerCase(); // normalize case
            if (tag == 'textarea')
                $scope.unhandledReason = "";
        });
    }
    /********************不受理 end *************************/

    /********************受理 start *************************/
    //受理的模态框
    $scope.showHandleModal = function (ApplicationId) {
        $("#handleApplication").modal("show");
        $scope.handleApplicationId = ApplicationId;
    };
    //第一行记录
    $scope.isShow1 = true;
    $scope.handleApplicationSure = function () {//受理模态框中的确定
        $http.put(unhandledApplicationUrl + $scope.handleApplicationId)
        .success(function (response) {
           
            //$http.post(assignExpert + $scope.handleApplicationId)//院管理员受理申请书之后自动给申请书指派专家

                notif({
                    msg: "申请书受理操作成功！",
                    type: "success"
                });
                $scope.load_page($scope.p_current);
         
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "申请书受理操作失败！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        });
            
    };
    /********************受理 end *************************/

    //点击申请书名称得到ApplicationId
    $scope.unhandledApplicationId = function (ApplicationId) {
        $scope.ApplicationId = ApplicationId;
    }
    //详细信息的模态框（点击项目名称）
    $scope.viewApplicationModal = function () {
        $http.get(applicationInfo + $scope.ApplicationId )
        .success(function (response) {            
            $scope.applicationInfo = response.response;
          
            $("#viewApplication").modal("show");
        })
        .error(function () {
            //alert("error!!");
        });
    };
    //点击负责人得到LeaderId
    $scope.unhandledLeaderId = function (LeaderId) {
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
           // alert("error!!");
        });
        
    };
    //点击项目名称显示PDF页面
    $scope.showPDFpage = function (PDFUrl) {
        var PDFPageUrl = "/View/plugins/pdfjs/web/viewer.html?file=" + PDFUrl;
        window.open(PDFPageUrl);
    }
    //点击下载
    $scope.downPDF = function (applicationId) {
        window.open("/api/applicationdoc/download/pdf/" + applicationId);
        //$http.get("/api/applicationdoc/download/pdf/135a1f69-127c-4bc4-9cc9-88ba5b89c263")
       // .success(function (response) {
        //    alert("successRequest!");
       //     $scope.downPdfResult = response.response;
       // })
       // .error(function () {
       //     alert("downPDF error!");
       // });
       
        //alert($scope.downPdfResult);
        //window.open(PDFPageUrl);
    }

    /*************分页************************/
    //获取未受理申请书列表   api/departapplication/accept/{instituteId}/{projectTypeId}/{pageId}
    $scope.getInstIdForPage = function (InstituteId) {//单位
        if (InstituteId == undefined) {
            $scope.InstituteId = '0'
        } else {
            $scope.InstituteId = InstituteId
        }
        // $scope.getUnhandledApplicationList();
        $scope.searchContent(1);//新加
    }
    $scope.getProjectTypeIdForPage = function (ProjectTypeId) {//类型
        if (ProjectTypeId == undefined) {
            $scope.ProjectTypeId = '0'
        } else {
            $scope.ProjectTypeId = ProjectTypeId
        }
        //$scope.getUnhandledApplicationList();
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
        var unhandledApplicationList = unhandledApplicationUrl + thisInstituteId + "/" + thisProjectTypeId + "/" +  page;

        $http.get(unhandledApplicationList)
            .success(function (response) {
                if (response.response != null) {
                    $scope.unhandledApplications = response.response.ItemDTOs;
                    $scope.pageItem = response.response.TotalPageNum; //总共多少页
                    $scope.count = response.response.TotalNum; //页面总共多少条
                    $scope.p_current = response.response.NowPage; //当前第几页
                    $scope.p_count = response.response.NowNum; //当前页面多少条
                    $scope.pages = [];
                    for (var i = 0; i < $scope.pageItem; i++) {
                        $scope.pages[i] = i + 1;
                    }
                    if ($scope.unhandledApplications.length == 0) {
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
        var unhandledApplicationList = unhandledApplicationUrl + thisInstituteId + "/" + thisProjectTypeId + "/" +  searchPage;

        if ($scope.InstituteId == undefined || $scope.ProjectTypeId == undefined) {
            $scope.load_page(1);
        } else {
            $http({
                method: 'GET',
                url: unhandledApplicationList,
            })
            .success(function (response) {
                if (response.status == 0) {
                    $scope.unhandledApplications = response.response.ItemDTOs;
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

    $scope.exportDeptUnhandledList = function (ProjectTypeId, InstituteId) {
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
        $scope.exportDeptUnhandledListFields = {
            "Order": "序号",
            "Name": "申请书名称",
            "Deleage": "委托",
            "Category": "类型",
            "Leader": "牵头负责人",
            "InstituteId": $scope.InstituteId,
            "CategoryId": $scope.ProjectTypeId
        };
        $downloadFile.exportApplication($scope.exportDeptUnhandledListUrl, $scope.exportDeptUnhandledListFields, $scope.exportDeptUnhandledListName);
    };
    
    //	 ***************下载PDF-模态框*****************
    $scope.downloadPDF = function (applicationId, projectName) {
        $downloadFile.downloadFile($scope.downloadPDFUrl, applicationId, projectName);
    }

    //批量下载
    $scope.packageDownloadApplications = function (ProjectTypeId, InstituteId) {
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
        $downloadFile.downloadFileType($scope.packageDownloadUrl + $scope.ProjectTypeId + "/" + $scope.InstituteId, ".zip", $scope.deptApplicationPackageName);
    }


    /*
   新加 end
   */
    //上面有新加，下面全都注释掉

    /*
     $scope.getUnhandledApplicationList = function () {
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
        var unhandledApplicationList = unhandledApplicationUrl + thisInstituteId + "/" + thisProjectTypeId + "/" + "1";
        //var unhandledApplicationList = unhandledApplicationUrl + "0/" + "0/" + "1";
        $http.get(unhandledApplicationList)
               .success(function (response) {
                   $scope.unhandledApplications = response.response.ItemDTOs;
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
                       msg: "<b>Error:</b> 未受理申请书列表未能加载，请重试！",
                       type: "error"
                   });
               });
    }
    $scope.getUnhandledApplicationList();//启动方法
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
        var unhandledApplicationList = unhandledApplicationUrl + thisInstituteId + "/" + thisProjectTypeId + "/" + "1";
       // var unhandledApplicationList = unhandledApplicationUrl + "0/" + "0/" + "1";
        $http.get(unhandledApplicationList)
              .success(function (response) {
                  $scope.unhandledApplications = response.response.ItemDTOs;
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
        var unhandledApplicationList = unhandledApplicationUrl + thisInstituteId + "/" + thisProjectTypeId + "/" + $scope.p_current;
       
        $http.get(unhandledApplicationList)
              .success(function (response) {
                  $scope.unhandledApplications = response.response.ItemDTOs;
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
        
        var unhandledApplicationList = unhandledApplicationUrl + thisInstituteId + "/" + thisProjectTypeId + "/" + page;
        //var unhandledApplicationList = unhandledApplicationUrl + "0/" + "0/" + page;
       
        $http.get(unhandledApplicationList)
              .success(function (response) {
                  $scope.unhandledApplications = response.response.ItemDTOs;
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
        $scope.p_current = $scope.p_current + 1;
        var unhandledApplicationList = unhandledApplicationUrl + thisInstituteId + "/" + thisProjectTypeId + "/" + $scope.p_current;
       
        $http.get(unhandledApplicationList)
              .success(function (response) {
                  $scope.unhandledApplications = response.response.ItemDTOs;
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

        var unhandledApplicationList = unhandledApplicationUrl + thisInstituteId + "/" + thisProjectTypeId + "/" + $scope.pageItem;
        //var unhandledApplicationList = unhandledApplicationUrl + "0/" + "0/" + $scope.pageItem;
        $http.get(unhandledApplicationList)
              .success(function (response) {
                  $scope.unhandledApplications = response.response.ItemDTOs;
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
//模态框居中
$(function() {
    var $m_btn = $('#modalBtn');
    var $modal = $('#PDFDownLoad, #viewApplication, #handleApplication,#rejectApplication, #responserLeader, #modify_Application , #pageNextWaring');
    $m_btn.on('click', function() {
        $modal.modal({
            backdrop: 'static'
        });
    });

    $modal.on('show.bs.modal', function() {
        var $this = $(this);
        var $modal_dialog = $this.find('.modal-dialog');
        // 关键代码，如没将modal设置为 block，则$modala_dialog.height() 为零
        $this.css('display', 'block');
        $modal_dialog.css({
            'margin-top': Math.max(0, ($(window).height() - $modal_dialog.height())/2)
        });
    });
});