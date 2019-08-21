var expert_accepted = angular.module('ngAssignment_RecommendationApp', []);



expert_accepted.controller('ngAssignment_RecommendationCtrl', function ($scope, $http, $rootScope) {

    //参数初始化
    $scope.artificialAssignSelected = [];
    $scope.expertSelect = [];
    $rootScope.selectChangeExpert = null;
    $rootScope.NeedChangeExpert = {
        "ExpertId": null
    }

    //url
    var getFieldUrl = "/api/Field";
    var getInstListUrl = "/api/Inst";
    var changeExpertList = "/api/reviewassignment/changeexpert/";
    var expertInfoUrl = "/api/expert/";
    var expertFieldInfoUrl = "/api/expertfieldById/"
    var expertReviewCommentUrl = "/api/eviewcommentlist/"


    //中断dropdown的隐藏操作
    $('.dropdown-menu li').click(function (e) {
        e.stopPropagation();
    });



    //获取项目类型
    $scope.getField = function () {
        $http.get(getFieldUrl)
        .success(function (response) {
            if (response.response != null) {
                $rootScope.Fields = response.response;
            }
            else {
                notif({
                    type: "error",
                    msg: "当前没有已添加的研究领域，请点击下方的添加按钮进行添加！",
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "未能成功获取到研究领域列表，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }

    //获取单位列表
    $scope.getInstList = function () {
        $http.get(getInstListUrl)
        .success(function (response) {
            $scope.instList = response.response;
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "未能成功获取到现有单位列表，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }

    //替换专家模态框
    $scope.showChangeExpertModal = function (application, expertIndex) {
        $scope.selectApplication = application;
        $rootScope.NeedChangeExpert = application.ReviewAssignments[expertIndex];
        $scope.expertField = {
            "Field": "all",
            "subField": "all",
            "Institute": 0
        }
        $("#ChangeProfessorModal").modal("show");
        $scope.getExpertList(0, 1);
    }

    //选中专家
    $scope.selectExpert = function (expert) {
        $rootScope.selectChangeExpert = expert;
    }

    //替换专家
    $scope.ChangeExpert = function () {
        if ($rootScope.NeedChangeExpert == null) {
            $rootScope.NeedChangeExpert = {
                "ExpertId":"empty"
            }
        }
        $http({
            method: "PUT",
            url: changeExpertList + $scope.selectApplication.ApplicationId,
            params: {
                "oldExpertId": $rootScope.NeedChangeExpert.ExpertId,
                "newExpertId": $scope.selectChangeExpert.UserId
            }
        })
        .success(function (response) {
            if (response.status == 0) {
                notif({
                    type: "success",
                    msg: "已成功替换专家",
                });
                $("#ChangeProfessorModal").modal("hide");
                $scope.selectApplication = null;
                $rootScope.NeedChangeExpert = {
                    "ExpertId": null
                }
                $rootScope.selectChangeExpert = null;
                $scope.getNeedList($scope.needConfirmField.Institute, 1);
                $scope.getArtificialAssignList($scope.artificialAssignField.institute, 1);
            }
            else {
                notif({
                    type: "error",
                    msg: "错误：" + response.errorMsg,
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "错误：" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        });
    }

    //去掉$$hashkey
    $scope.removeHashkey = function(item){
        return angular.copy(item);
    }

    //选中和取消选中操作
    $scope.toggle = function (item, list) {

        var idx = $scope.exists(item, list);
        if (idx > -1) {
            list.splice(idx, 1);
        }
        else {
            list.push(item);
        }

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

    //判断ng-repeat的页码是否与当前页超过3页
    $scope.pageAbs = function (NowPage, page) {
        if (Math.abs(NowPage - page) < 3) {
            return true;
        }
        else {
            return false;
        }
    }


    //专家列表分栏
    $scope.setLine = function (index, lineNum) {
        return (index % lineNum);
    }


    //初始化
    $scope.getField();
    $scope.getInstList();

    //模态框关闭数据清零
    $('#assignProfessorModal').on('hidden.bs.modal', function () {
        $scope.expertSelect = [];
    })

    $('#ChangeProfessorModal').on('hidden.bs.modal', function () {
        $scope.selectApplication = null;
        $rootScope.selectChangeExpert = [];
    })


    //--------------need list-------------//

    //$scope初始化
    $scope.needListIsNull = true;
    $scope.needConfirmField = {
        "Field": "all",
        "subField": "all",
        "Institute": 0
    }

    //url
    var getSubFieldUrl = "/api/subfield/";
    var getNeedListUrl = "/api/reviewassignment/sendassignment/";
    var confirmAllUrl = "/api/reviewassignment/sendassignment";



    //获取子领域
    $scope.getNeedSubField = function (FieldId) {
        $http.get(getSubFieldUrl + FieldId)
        .success(function (response) {
            if (response.response != null) {
                $scope.needSubFields = response.response;
            }
            else {
                notif({
                    type: "error",
                    msg: "当前项目类型下还没有已添加的支持类型，请点击下方的添加按钮进行添加！",
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "未能成功获取到该项目类型的支持类型列表，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }



    //获取待确认列表
    $scope.getNeedList = function (instId, page) {
        if ($scope.needConfirmField.Field == null) {
            $scope.needConfirmField.Field = "all";
        }
        if ($scope.needConfirmField.subField == null) {
            $scope.needConfirmField.subField = "all";
        }
        if (instId == null) {
            instId = 0;
        }
        $http({
            method: "GET",
            url: getNeedListUrl + instId + "/" + page,
            params: {
                "subFieldId": $scope.needConfirmField.subField,
                "fieldId": $scope.needConfirmField.Field
            }
        })
        .success(function (response) {
            if (response.response.TotalNum != 0) {
                $scope.needConfirmApplications = response.response.ItemDTOs;
                $scope.needConfirmNowPage = response.response.NowPage;
                $scope.needConfirmTotalPageNum = response.response.TotalPageNum;
                $scope.needConfirmPageArry = new Array($scope.needConfirmTotalPageNum);
                for (var i = 0; i < $scope.needConfirmTotalPageNum; i++) {
                    $scope.needConfirmPageArry[i] = i + 1;
                }
                $scope.needListIsNull = false;
            }
            else if (response.response.TotalNum == 0) {
                $scope.needConfirmApplications = null;
                $scope.needConfirmNowPage = 1;
                $scope.needConfirmTotalPageNum = 1;
                $scope.needConfirmPageArry = [1];
                $scope.needListIsNull = true;
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "未能成功获取到待确认专家分配列表，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }

    //全部确认
    $scope.ConfirmAll = function () {
        $http.put(confirmAllUrl)
        .success(function (response) {
            if (response.status == 0) {
                notif({
                    type: "success",
                    msg: "已成功确认全部申请书指派",
                });
            }
            else {
                notif({
                    type: "error",
                    msg: "错误：" + response.errorMsg,
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            }
            $scope.needConfirmField = {
                "Field": "all",
                "subField": "all",
                "Institute": 0
            }
            $scope.getNeedList(0, 1);
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "错误：" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }


    //待确认列表跳页
    $scope.needConfirmJumpToPage = function (page) {
        if ((page == $scope.needConfirmNowPage + 1) && ($scope.needConfirmNowPage == $scope.needConfirmTotalPageNum)) {
            notif({
                msg: "已经是最后一页",
                type: "warning"
            });
        }
        else if ((page == $scope.needConfirmNowPage - 1) && ($scope.needConfirmNowPage == 1)) {
            notif({
                msg: "已经是第一页",
                type: "warning"
            });
        }
        else {
            $scope.getNeedList($scope.needConfirmField.Institute, page);
        }
    }


    //初始化
    $scope.getNeedList(0, 1);



    //--------------artificial assign list-------------//

    //$scope初始化
    $scope.expertSelect = [];
    $scope.artificialAssignListIsNull = false;
    $scope.artificialAssignField = {
        "Field": "all",
        "subField": "all",
        "Institute": 0
    }
    $scope.expertField = {
        "Field": "all",
        "subField": "all",
        "Institute": 0
    }
    $scope.backExpertListIsReady = false;

    //url
    var getArtificialAssignListUrl = "/api/reviewassignment/manualassignment/";
    var getExpertListUrl = "/api/expert/depart/";
    var groupAssignUrl = "/api/reviewassignment/manual"



    //获取子领域
    $scope.getArtificialSubField = function (FieldId) {
        $http.get(getSubFieldUrl + FieldId)
        .success(function (response) {
            if (response.response != null) {
                $scope.artificialSubFields = response.response;
            }
            else {
                notif({
                    type: "error",
                    msg: "当前项目类型下还没有已添加的支持类型，请点击下方的添加按钮进行添加！",
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "未能成功获取到该项目类型的支持类型列表，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }

    //获取专家子领域
    $scope.getExpertSubField = function (FieldId) {
        $http.get(getSubFieldUrl + FieldId)
        .success(function (response) {
            if (response.response != null) {
                $scope.expertSubFields = response.response;
            }
            else {
                notif({
                    type: "error",
                    msg: "当前项目类型下还没有已添加的支持类型，请点击下方的添加按钮进行添加！",
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "未能成功获取到该项目类型的支持类型列表，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }

    //获取手动指派列表
    $scope.getArtificialAssignList = function (instId, page) {
        if ($scope.artificialAssignField.Field == null) {
            $scope.artificialAssignField.Field = "all";
        }
        if ($scope.artificialAssignField.subField == null) {
            $scope.artificialAssignField.subField = "all";
        }
        if (instId == null) {
            instId = 0;
        }
        $http({
            method: "GET",
            url: getArtificialAssignListUrl + instId + "/" + page,
            params: {
                "subFielId": $scope.artificialAssignField.subField,
                "fieldId": $scope.artificialAssignField.Field
            }
        })
        .success(function (response) {
            if (response.response.TotalNum != 0) {
                $scope.artificialAssignApplications = response.response.ItemDTOs;
                $scope.artificialAssignNowPage = response.response.NowPage;
                $scope.artificialAssignTotalPageNum = response.response.TotalPageNum;
                $scope.artificialAssignPageArry = new Array($scope.artificialAssignTotalPageNum);
                for (var i = 0; i < $scope.artificialAssignTotalPageNum; i++) {
                    $scope.artificialAssignPageArry[i] = i + 1;
                }
                $scope.artificialAssignListIsNull = false;
            }
            else if (response.response.TotalNum == 0) {
                $scope.artificialAssignApplications = null;
                $scope.artificialAssignNowPage = 1;
                $scope.artificialAssignTotalPageNum = 1;
                $scope.artificialAssignPageArry = [1];
                $scope.artificialAssignListIsNull = true;
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "未能成功获取到待确认专家分配列表，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }

    //获取专家列表
    $scope.getExpertList = function (instId, page) {
        $scope.backExpertListIsReady = false;
        if ($scope.expertField.Field == null) {
            $scope.expertField.Field = "all";
        }
        if ($scope.expertField.subField == null) {
            $scope.expertField.subField = "all";
        }
        if (instId == null) {
            instId = 0;
        }
        $http({
            method: "GET",
            url: getExpertListUrl + instId + "/" + page,
            params: {
                "subFieldId": $scope.expertField.subField,
                "fieldId": $scope.expertField.Field
            }
        })
        .success(function (response) {
            if (response.response.TotalNum != 0) {
                $scope.expertList = response.response.ItemDTOs;
                $scope.expertNowPage = response.response.NowPage;
                $scope.expertTotalPageNum = response.response.TotalPageNum;
                $scope.expertPageArry = new Array($scope.expertTotalPageNum);
                for (var i = 0; i < $scope.expertTotalPageNum; i++) {
                    $scope.expertPageArry[i] = i + 1;
                }
            }
            else if (response.response.TotalNum == 0) {
                $scope.expertList = null;
                $scope.expertNowPage = 1;
                $scope.expertTotalPageNum = 1;
                $scope.expertPageArry = [1];
            }
            $scope.backExpertListIsReady = true;
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "未能成功获取到专家列表，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
            $scope.backExpertListIsReady = true;
        })
    }


    //批量指派
    $scope.expertGroupAssign = function () {
        var applicationArry = new Array();
        var expertArry = new Array();

        angular.forEach($scope.artificialAssignSelected, function (application) {
            applicationArry.push(application.ApplicationId);
        })
        angular.forEach($scope.expertSelect, function (expert) {
            expertArry.push(expert.UserId);
        })

        $http({
            method: "POST",
            data: {
                "applications": applicationArry,
                "experts": expertArry
            },
            url: groupAssignUrl
        })
        .success(function (response) {
            if (response.status == 0) {
                console.log(expertArry,1111111111111111111111);
                notif({
                    type: "success",
                    msg: "已成功指派专家",
                });
                $("#assignProfessorModal").modal("hide");
                $scope.artificialAssignField = {
                    "Field": "all",
                    "subField": "all",
                    "Institute": 0
                }
                $scope.expertField = {
                    "Field": "all",
                    "subField": "all",
                    "Institute": 0
                }
                $scope.getArtificialAssignList(0, 1);
                $scope.artificialAssignSelected = [];
            }
            else {
                notif({
                    type: "error",
                    msg: "错误:" + response.errorMsg,
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "错误:" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }



    //跳页
    $scope.artificialAssignJumpToPage = function (page) {
        if ((page == $scope.artificialAssignNowPage + 1) && ($scope.artificialAssignNowPage == $scope.artificialAssignTotalPageNum)) {
            notif({
                msg: "已经是最后一页",
                type: "warning"
            });
        }
        else if ((page == $scope.artificialAssignNowPage - 1) && ($scope.artificialAssignNowPage == 1)) {
            notif({
                msg: "已经是第一页",
                type: "warning"
            });
        }
        else {
            $scope.getArtificialAssignList($scope.artificialAssignField.institute, page);
        }
    }

    //候选专家跳页
    $scope.jumpToPage = function (page) {
        if ((page == $scope.expertNowPage + 1) && ($scope.expertNowPage == $scope.expertTotalPageNum)) {
            notif({
                msg: "已经是最后一页",
                type: "warning"
            });
        }
        else if ((page == $scope.expertNowPage - 1) && ($scope.expertNowPage == 1)) {
            notif({
                msg: "已经是第一页",
                type: "warning"
            });
        }
        else {
            $scope.getExpertList($scope.expertField.instituteId, page);
        }
    }


    //初始化
    $scope.getArtificialAssignList(0, 1);




    //--------------already assign list-------------//

    //$scope初始化
    $scope.alreadyAssignListIsNull = false;
    $scope.alreadyAssignField = {
        "Field": "all",
        "subField": "all",
        "Institute": 0
    }

    //url
    var getAlreadyAssignListUrl = "/api/reviewassignment/review/";



    //获取子领域
    $scope.getAlreadySubField = function (FieldId) {
        $http.get(getSubFieldUrl + FieldId)
        .success(function (response) {
            if (response.response != null) {
                $scope.alreadySubFields = response.response;
            }
            else {
                notif({
                    type: "error",
                    msg: "当前项目类型下还没有已添加的支持类型，请点击下方的添加按钮进行添加！",
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "未能成功获取到该项目类型的支持类型列表，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }


    //获取手动指派列表
    $scope.getAlreadyAssignList = function (instId, page) {
        if ($scope.alreadyAssignField.Field == null) {
            $scope.alreadyAssignField.Field = "all";
        }
        if ($scope.alreadyAssignField.subField == null) {
            $scope.alreadyAssignField.subField = "all";
        }
        if (instId == null) {
            instId = 0;
        }
        $http({
            method: "GET",
            url: getAlreadyAssignListUrl + instId + "/" + page,
            params: {
                "subFieldId": $scope.alreadyAssignField.subField,
                "fieldId": $scope.alreadyAssignField.Field
            }
        })
        .success(function (response) {
            if (response.response.TotalNum != 0) {
                $scope.alreadyConfirmApplications = response.response.ItemDTOs;
                $scope.alreadyAssignNowPage = response.response.NowPage;
                $scope.alreadyAssignTotalPageNum = response.response.TotalPageNum;
                $scope.alreadyAssignPageArry = new Array($scope.alreadyAssignTotalPageNum);
                for (var i = 0; i < $scope.alreadyAssignTotalPageNum; i++) {
                    $scope.alreadyAssignPageArry[i] = i + 1;
                }
                $scope.alreadyAssignListIsNull = false;
            }
            else if (response.response.TotalNum == 0) {
                $scope.alreadyConfirmApplications = null;
                $scope.alreadyAssignNowPage = 1;
                $scope.alreadyAssignTotalPageNum = 1;
                $scope.alreadyAssignPageArry = [1];
                $scope.alreadyAssignListIsNull = true;
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "未能成功获取到分配完成分配列表，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }

    //跳页
    $scope.alreadyAssignJumpToPage = function (page) {
        if ((page == $scope.alreadyAssignNowPage + 1) && ($scope.alreadyAssignNowPage == $scope.alreadyAssignTotalPageNum)) {
            notif({
                msg: "已经是最后一页",
                type: "warning"
            });
        }
        else if ((page == $scope.alreadyAssignNowPage - 1) && ($scope.alreadyAssignNowPage == 1)) {
            notif({
                msg: "已经是第一页",
                type: "warning"
            });
        }
        else {
            $scope.getAlreadyAssignList($scope.alreadyAssignField.institute, page);
        }
    }

    //初始化
    $scope.getAlreadyAssignList(0, 1);


    //点击专家名称，查看详细信息的模态框
    $scope.showViewExpertInfoModal = function (expertId) {
        $http.get(expertInfoUrl + expertId)
            .success(function (response) {
                $scope.expertInfo = response.response;

                $http.get(expertFieldInfoUrl + expertId)//获取专家研究领域
                    .success(function (response) {
                        $scope.expertFieldInfo = response.response;
                        $scope.expertFieldInfo.forEach(function (val, index, arr) {//val为数组中当前的值，index为当前值的下表，arr为原数组
                            $scope.expertFieldInfo1 = arr[0];
                            $scope.expertFieldInfo2 = arr[1];
                        });

                    })
                    .error(function () {
                        //alert("error!!");
                    });
                $http.get(expertReviewCommentUrl + expertId + "/1")//获取专家既往评审
                    .success(function (response) {
                        console.log(response);
                        $scope.expertReviewCommentInfo = response.response;
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







});


expert_accepted.service('expertChange', function () {

    var self = this;

    self.selectExpert = function (expert) {
        $rootScope.selectChangeExpert = expert;
    };

    self.removeHashkey = function (item) {
        return angular.copy(item);
    };

    self.toggle = function (item, list) {

        var idx = list.indexOf(item);
        if (idx > -1) {
            list.splice(idx, 1);
        }
        else {
            list.push(item);
        }
    }
})









