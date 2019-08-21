
var expertsListApp = angular.module('ngExpertsListApp', []);

expertsListApp.controller('ngExpertsListCtrl', function ($scope, $http) {
    var divisionExpertListUrl = "/api/expert/inst/";//Get本单位专家列表的Url
    var expertInfoUrl = "/api/person/";//Get专家信息的Url
    //var expertFieldUrl = "/api/expertfield/"; //Get专家领域的Url
    var getFieldUrl = "/api/field"; //领域下拉列
    var getSubfieldUrl = "/api/subfield/"; //子领域下拉列

    //  ************************初始化json数据************************
    $scope.listAlready = false;
    $scope.expertListIsNull = true;

    $scope.fieldInfo = {
        "FieldId": null,
        "SubFieldId": null
    };

    //筛选条件
    //获取研究领域
    $scope.getField = function () {
        $http.get(getFieldUrl)
	    .success(function (response) {
	        $scope.fields = response.response;
	        //console.log($scope.fields);
	    })
	    .error(function () {
	        notif({
	            msg: "<b>错误:</b> 研究领域获取失败",
	            type: "error"
	        });
	    });
    }
    //获取子领域
    $scope.getSubField = function (field) {
        $http.get(getSubfieldUrl + field)
       .success(function (response) {
           $scope.subFields = response.response;
           //console.log($scope.subFields);
       })
       .error(function () {
           notif({
               msg: "<b>错误:</b> 研究子领域1获取失败",
               type: "error"
           });
       });
    }

    //	 ***************专家具体信息-模态框*****************
    $scope.showExpertInfoModal = function (divisionExpert) {
        var personId = divisionExpert.PersonId;
        //alert(personId);
        var detailUrl = expertInfoUrl + personId;
        $http.get(detailUrl)
		        .success(function (response) {
		            $scope.modalData = response.response;
		            //console.log($scope.modalData);
		            //$('#fzrInfo').modal("show");
		        })
		        .error(function () {
		            notif({
		                msg: "<b>错误:</b> 负责人信息获取失败，请检查网络是否通畅",
		                type: "error"
		            });
		        });
        $('#fzrInfo').modal("show");
    }

    //选择某一页时
    $scope.load_page = function (page) {
        if ($scope.fieldInfo.FieldId == null) {
            $scope.fieldInfo.FieldId = "all";
        }
        if ($scope.fieldInfo.SubFieldId == null) {
            $scope.fieldInfo.SubFieldId = "all"
        }
            $http({
                    method: "GET",
                    params: {
                        "fieldId": $scope.fieldInfo.FieldId,
                        "subFieldId": $scope.fieldInfo.SubFieldId
                    },
                    url: divisionExpertListUrl + page
                })
                .success(function (response) {
                    if (response.response != null) {
                        $scope.divisionExpertList = response.response.ItemDTOs;
                        $scope.pageItem = response.response.TotalPageNum;//总共多少页
                        $scope.count = response.response.TotalNum; //页面总共多少条
                        $scope.p_current = response.response.NowPage; //当前第几页
                        $scope.p_count = response.response.NowNum; //当前页面多少条
                        $scope.pages = [];
                        for (var i = 0; i < $scope.pageItem; i++) {
                            $scope.pages[i] = i + 1;
                        }
                        if ($scope.divisionExpertList.length == 0) {
                            $scope.expertListIsNull = true;
                            $scope.listAlready = true;
                        }
                        if ($scope.divisionExpertList.length != 0) {
                            $scope.expertListIsNull = false;
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
    $scope.getField();//获取领域

})