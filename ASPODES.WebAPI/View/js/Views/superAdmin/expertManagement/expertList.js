
var expertList= angular.module('ngExpertListApp',[]);

expertList.controller('ngExpertListCtrl', function ($scope, $http) {
    //url
    var expertListUrl = "/api/expert/depart/";//获取专家列表 
    var userListUrl = "/api/departperson"//get用户列表数据的url
    var deleteExpertUrl = "/api/expert/"//删除专家userId
    var addExpertUrl = " /api/expert/"//添加专家PersonId
    var instTypeUrl = "/api/inst "       //获取下拉框单位列表
    var editExpertUrl = "/api/instPerson"//编辑专家
    var getExpertFieldUrl = "/api/expertfield/"//获取研究领域
    var addExpertField = "/api/expertfield/depart" //添加研究领域
    var editExpertField = "/api/expertfield/depart" //编辑研究领域
    var getSubfieldUrl = "/api/subfield/";
    var getFieldUrl = "/api/field";
    var expertInfoUrl = "/api/person/"//查看详细信息
    var getExpertNameUrl = "/api/person/combo/inst/" //添加专家
    // ********************初始化json数据********************
    $scope.divisionAppliesIsNull = false;
    //获取用户列表
    $scope.getExpertNameList = function (InstituteId) {
        $http.get(getExpertNameUrl + InstituteId)
        .success(function (response) {
            $scope.userList = response.response;//只是第一页
        })
        .error(function () {
            //alert("error!");
        });
    }
   
    //添加专家页面的modal show 
    $scope.showAddExpertModal = function () {
        $("#addExpert").modal("show");

    }
    //添加专家的确定
    $scope.addExpertSave = function (PersonId) {
        $http({
            method: 'POST',
            url: addExpertUrl + PersonId
        })
       .then(function successCallback(response) {
          // console.log('111', response)
           //console.log('222', response.data.status)
           if (response == undefined) {
               notif({
                   type: "error",
                   msg: "添加操作失败！用户已经拥有该角色.",
                   position: "center",
                   width: 500,
                   height: 60,
                   autohide: false
               })
           } else {
               $("#addExpert").modal("hide");
               notif({
                   msg: "添加操作成功！",
                   type: "success"
               });
               $scope.getExpertList();
           }
          
         
       })
       };
       
        /*
         if (response.data.status == 0) {
                    $("#addExpert").modal("hide");
                    notif({
                        msg: "添加操作成功！",
                        type: "success"
                    });
                    $scope.getExpertList();
                } else {
                    notif({
                        type: "error",
                        msg: "添加操作失败！" + response.errorMsg,
                        position: "center",
                        width: 500,
                        height: 60,
                        autohide: false
                    })
                }
               
            }, function errorCallback(response) {
                notif({
                    type: "error",
                    msg: "添加操作失败！" + response.errorMsg,
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                })
            });
 

        */
          
      
  
    //每次点击添加专家按钮，强制置空文本框的值
    $scope.reloadExpertModal = function (unhandledApplication) {
        $scope.selectInstExpert = "";
        $scope.selectName = "";
        $(':input', unhandledApplication).each(function () {
            var type = this.type;
            var tag = this.tagName.toLowerCase(); // normalize case
            if (tag == 'select')
                $scope.addExpert.Name = "";
        });
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
    //删除专家
    $scope.showDeleteExpertModal = function (PersonId) {
        $("#deleteExpertModal").modal("show");
        $scope.PersonId = PersonId;
    };
    //删除专家的modal的确定
    $scope.deleteExpert = function () {
        if ($scope.p_count == 1) {//当前只有一条 应该是第一页或者最后一页
            if ($scope.pageItem == $scope.p_current) {//判断是否为最后一页
              $http.delete(deleteExpertUrl + $scope.PersonId)
                .success(function (response) {
                    notif({
                        msg: "<b>恭喜:</b> 专家删除成功！",
                        type: "success"
                    });
                    // $scope.getExpertList();
                    // $scope.pageItem = $scope.pageItem - 1;//总页数
                
                    $scope.load_page($scope.p_current);//加载倒数第二页为最后一页
                   
                })
                .error(function (response) {
                    notif({
                        type: "error",
                        msg: "专家删除失败，请检查您的网络！",
                        position: "center",
                        width: 500,
                        height: 60,
                        autohide: false
                    });
                });
            }
        
        } else {
           
            $http.delete(deleteExpertUrl + $scope.PersonId)
            .success(function (response) {
                notif({
                    msg: "<b>恭喜:</b> 专家删除成功！",
                    type: "success"
                });
                $scope.load_page($scope.p_current);
            })
            .error(function (response) {
                notif({
                    type: "error",
                    msg: "专家删除失败，请检查您的网络！",
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            });

        }
  
    }
    //获取单位下拉框单位列表
    $scope.instTypes = function () {
        $http.get(instTypeUrl)
        .success(function (response) {
            $scope.instTypes = response.response;
        })
        .error(function () {
          //  alert("error!");
        });
    }
    $scope.instTypes();//启动方法
    //获取研究领域
    $http.get(getFieldUrl)
	.success(function (response) {
	    $scope.fields = response.response;
	})
	.error(function () {
	    notif({
	        type: "error",
	        msg: "没有获取到可选的研究领域，请检查您的网络是否畅通！",
	        position: "center",
	        width: 500,
	        height: 60,
	        autohide: false
	    });
	});
    //获取子领域
   
    $scope.getSubField2 = function (field) {
        $http.get(getSubfieldUrl + field)
       .success(function (response) {
           $scope.subFields2 = response.response;
       })
       .error(function () {
           notif({
               type: "error",
               msg: "没有获取到该研究领域下的子领域，请检查您的网络是否畅通！",
               position: "center",
               width: 500,
               height: 60,
               autohide: false
           });
       });
    };

    /*************分页************************/
   
    //获取专家列表列表   api/expertList/depart/{instId}/{subField1}/{subField2}/{page}
    // 新的 api/expertList/depart/{instId}/{page}/?subField1= & subField2=
    $scope.getInstIdForPage = function (InstituteId) {//单位
        if (InstituteId == undefined) {
            $scope.InstituteId = "0"
        } else {
            $scope.InstituteId = InstituteId
        }
       // $scope.getExpertList();
        $scope.searchContent();//新加
    }
    $scope.getSubField = function (selectFieldName) {//获取子研究领域并发送研究领域的Id
        $http.get(getSubfieldUrl + selectFieldName)
       .success(function (response) {
           $scope.subFields = response.response;
       })
       .error(function () {
           notif({
               type: "error",
               msg: "没有获取到该研究领域下的子领域，请检查您的网络是否畅通！",
               position: "center",
               width: 500,
               height: 60,
               autohide: false
           });
       });
        if (selectFieldName == undefined) {
            $scope.selectFieldName = 'all'
        } else {
            $scope.selectFieldName = selectFieldName
        }
        //$scope.getExpertList();
        $scope.searchContent();//新加
    };
    $scope.getSubFieldIdForPage = function (selectSubFieldName) {//子研究领域1
        if (selectSubFieldName == undefined) {
            $scope.selectSubFieldName = 'all'
        } else {
            $scope.selectSubFieldName = selectSubFieldName
        }
        //$scope.getExpertList();
        $scope.searchContent();//新加
    }
    $scope.getSubFieldIdForPage2 = function (selectSubFieldName2) {//子研究领域2
        if (selectSubFieldName2 == undefined) {
            $scope.selectSubFieldName2 = 'all'
        } else {
            $scope.selectSubFieldName2 = selectSubFieldName2
        }
        //$scope.getExpertList();
        $scope.searchContent();//新加
    }


    /*
    新加 start
    */
    $scope.load_page = function (page) {
        $scope.isSearch = false;
        if ($scope.InstituteId == undefined) {//单位
            thisInstituteId = '0'
        } else {
            thisInstituteId = $scope.InstituteId
        }
        if ($scope.selectFieldName == undefined) {//研究领域
            selectFieldName = 'all'
        } else {
            selectFieldName = $scope.selectFieldName
        }
        if ($scope.selectSubFieldName == undefined) {//子研究领域1
            selectSubFieldName = 'all'
        } else {
            selectSubFieldName = $scope.selectSubFieldName
        }
        if ($scope.selectSubFieldName2 == undefined) {//子研究领域2
            selectSubFieldName2 = 'all'
        } else {
            selectSubFieldName2 = $scope.selectSubFieldName2
        }
        var expertList = expertListUrl + thisInstituteId + "/" + page + "?fieldId=" + selectFieldName + "&subFieldId=" + selectSubFieldName;
        $http.get(expertList)
            .success(function (response) {
                if (response.response != null) {
                    $scope.expertList = response.response.ItemDTOs;
                    $scope.pageItem = response.response.TotalPageNum; //总共多少页
                    $scope.count = response.response.TotalNum; //页面总共多少条
                    $scope.p_current = response.response.NowPage; //当前第几页
                    $scope.p_count = response.response.NowNum; //当前页面多少条
                    $scope.pages = [];
                    for (var i = 0; i < $scope.pageItem; i++) {
                        $scope.pages[i] = i + 1;
                    }
                    if ($scope.expertList.length == 0) {
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
        if ($scope.selectFieldName == undefined) {//研究领域
            selectFieldName = 'all'
        } else {
            selectFieldName = $scope.selectFieldName
        }
        if ($scope.selectSubFieldName == undefined) {//子研究领域1
            selectSubFieldName = 'all'
        } else {
            selectSubFieldName = $scope.selectSubFieldName
        }
        if ($scope.selectSubFieldName2 == undefined) {//子研究领域2
            selectSubFieldName2 = 'all'
        } else {
            selectSubFieldName2 = $scope.selectSubFieldName2
        }

        var expertList = expertListUrl + thisInstituteId + "/" + searchPage + "?fieldId=" + selectFieldName + "&subFieldId=" + selectSubFieldName;

        if ($scope.InstituteId == undefined || $scope.selectFieldName == undefined || $scope.selectSubFieldName == undefined || $scope.selectSubFieldName2 == undefined) {
            $scope.load_page(1);
        } else {
            $http({
                method: 'POST',
                url: expertList,
               // data: $scope.searchData
            })
           .success(function (response) {
               if (response.status == 0) {
                   $scope.expertList = response.response.ItemDTOs;
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
    $scope.getExpertList = function () {
       // alert('获取专家列表');
       // alert($scope.InstituteId);
       // alert($scope.selectSubFieldName);
      //  alert($scope.selectSubFieldName2);
        if ($scope.InstituteId == undefined) {//单位
            thisInstituteId = '0'
        } else {
            thisInstituteId = $scope.InstituteId
        }
        if ($scope.selectFieldName == undefined) {//研究领域
            selectFieldName = 'all'
        } else {
            selectFieldName = $scope.selectFieldName
        }
        if ($scope.selectSubFieldName == undefined) {//子研究领域1
            selectSubFieldName = 'all'
        } else {
            selectSubFieldName = $scope.selectSubFieldName
        }
        if ($scope.selectSubFieldName2 == undefined) {//子研究领域2
            selectSubFieldName2 = 'all'
        } else {
            selectSubFieldName2 = $scope.selectSubFieldName2
        }
       // alert(thisInstituteId);
        //var expertList = expertListUrl + thisInstituteId + "/1" + "?subField1=" + selectSubFieldName + "&subField2=" + selectSubFieldName2;
        var expertList = expertListUrl + thisInstituteId + "/1" + "?fieldId=" + selectFieldName + "&subFieldId=" + selectSubFieldName;
       // alert(expertList);
   
        $http.get(expertList)
               .success(function (response) {
                   $scope.expertList = response.response.ItemDTOs;
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
    $scope.getExpertList();//启动方法
    //选择首页
    $scope.p_index = function () {
        if ($scope.InstituteId == undefined) {//单位
            InstituteId = '0'
        } else {
            InstituteId = $scope.InstituteId
        }
        if ($scope.selectFieldName == undefined) {//研究领域
            selectFieldName = 'all'
        } else {
            selectFieldName = $scope.selectFieldName
        }
        if ($scope.selectSubFieldName == undefined) {//子研究领域1
            selectSubFieldName = 'all'
        } else {
            selectSubFieldName = $scope.selectSubFieldName
        }
        if ($scope.selectSubFieldName2 == undefined) {//子研究领域2
            selectSubFieldName2 = 'all'
        } else {
            selectSubFieldName2 = $scope.selectSubFieldName2
        }
        //var expertList = expertListUrl + InstituteId + "/1" + "?subField1=" + selectSubFieldName + "&subField2=" + selectSubFieldName2;
        var expertList = expertListUrl + InstituteId + "/1" + "?fieldId=" + selectFieldName + "&subFieldId=" + selectSubFieldName;
      
        $http.get(expertList)
              .success(function (response) {
                  $scope.expertList = response.response.ItemDTOs;
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
        if ($scope.selectFieldName == undefined) {//研究领域
            selectFieldName = 'all'
        } else {
            selectFieldName = $scope.selectFieldName
        }
        if ($scope.selectSubFieldName == undefined) {//子研究领域1
            selectSubFieldName = 'all'
        } else {
            selectSubFieldName = $scope.selectSubFieldName
        }
        if ($scope.selectSubFieldName2 == undefined) {//子研究领域2
            selectSubFieldName2 = 'all'
        } else {
            selectSubFieldName2 = $scope.selectSubFieldName2
        }

        $scope.p_current = $scope.p_current - 1;
        var expertList = expertListUrl + InstituteId + "/" + $scope.p_current + "?fieldId=" + selectFieldName + "&subFieldId=" + selectSubFieldName;
        //var expertList = expertListUrl + InstituteId + "/" + $scope.p_current + "?subField1=" + selectSubFieldName + "&subField2=" + selectSubFieldName2;
        $http.get(expertList)
              .success(function (response) {
                 
                  $scope.expertList = response.response.ItemDTOs;
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
    $scope.load_page = function (page,flag) {
        if ($scope.InstituteId == undefined) {//单位
            InstituteId = '0'
        } else {
            InstituteId = $scope.InstituteId
        }
        if ($scope.selectFieldName == undefined) {//研究领域
            selectFieldName = 'all'
        } else {
            selectFieldName = $scope.selectFieldName
        }
        if ($scope.selectSubFieldName == undefined) {//子研究领域1
            selectSubFieldName = 'all'
        } else {
            selectSubFieldName = $scope.selectSubFieldName
        }
        if ($scope.selectSubFieldName2 == undefined) {//子研究领域2
            selectSubFieldName2 = 'all'
        } else {
            selectSubFieldName2 = $scope.selectSubFieldName2
        }

        //var expertList = expertListUrl + InstituteId + "/" + page + "?subField1=" + selectSubFieldName + "&subField2=" + selectSubFieldName2;
        var expertList = expertListUrl + InstituteId + "/" + page + "?fieldId=" + selectFieldName + "&subFieldId=" + selectSubFieldName;
      
        $http.get(expertList)
              .success(function (response) {
              
                  $scope.expertList = response.response.ItemDTOs;
                  $scope.pageItem = response.response.TotalPageNum;//总共多少页
                  $scope.count = response.response.TotalNum; //页面总共多少条
                  $scope.p_current = response.response.NowPage; //当前第几页
                  $scope.p_count = response.response.NowNum; //当前页面多少条
                  
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
        if ($scope.InstituteId == undefined) {//单位
            InstituteId = '0'
        } else {
            InstituteId = $scope.InstituteId
        }
        if ($scope.selectFieldName == undefined) {//研究领域
            selectFieldName = 'all'
        } else {
            selectFieldName = $scope.selectFieldName
        }
        if ($scope.selectSubFieldName == undefined) {//子研究领域1
            selectSubFieldName = 'all'
        } else {
            selectSubFieldName = $scope.selectSubFieldName
        }
        if ($scope.selectSubFieldName2 == undefined) {//子研究领域2
            selectSubFieldName2 = 'all'
        } else {
            selectSubFieldName2 = $scope.selectSubFieldName2
        }

        $scope.p_current = $scope.p_current + 1;
        var expertList = expertListUrl + InstituteId + "/" + $scope.p_current + "?fieldId=" + selectFieldName + "&subFieldId=" + selectSubFieldName;
       // var expertList = expertListUrl + InstituteId + "/" + $scope.p_current + "?subField1=" + selectSubFieldName + "&subField2=" + selectSubFieldName2;
        
        $http.get(expertList)
              .success(function (response) {
                
                  $scope.expertList = response.response.ItemDTOs;
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
        if ($scope.selectFieldName == undefined) {//研究领域
            selectFieldName = 'all'
        } else {
            selectFieldName = $scope.selectFieldName
        }
        if ($scope.selectSubFieldName == undefined) {//子研究领域1
            selectSubFieldName = 'all'
        } else {
            selectSubFieldName = $scope.selectSubFieldName
        }
        if ($scope.selectSubFieldName2 == undefined) {//子研究领域2
            selectSubFieldName2 = 'all'
        } else {
            selectSubFieldName2 = $scope.selectSubFieldName2
        }

        //var expertList = expertListUrl + InstituteId + "/" + $scope.pageItem + "?subField1=" + selectSubFieldName + "&subField2=" + selectSubFieldName2;
        var expertList = expertListUrl + InstituteId + "/" + $scope.pageItem + "?fieldId=" + selectFieldName + "&subFieldId=" + selectSubFieldName;
        $http.get(expertList)
              .success(function (response) {
                  
                  $scope.expertList = response.response.ItemDTOs;
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


