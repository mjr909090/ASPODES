var typeSettingApp = angular.module('ngTypeSettingApp',[]);

typeSettingApp.controller('ngTypeSettingCtrl', function($scope, $http){
	
    var getProjectTypeUrl = "/api/ProjectType";
    var getSupportCategoryUrl = "/api/supportcategory/";
    var addProjectTypeUrl = "/api/systemperson/supportcategory";
    var addSupportCategoryUrl = "/api/systemperson/supportcategory";

    //获取项目类型
    $scope.getProjectType = function () {
        $http.get(getProjectTypeUrl)
        .success(function (response) {
            if (response.response != null) {
                $scope.projectTypes = response.response;
            }
            else {
                notif({
                    type: "error",
                    msg: "当前没有已添加的项目类型，请点击下方的添加按钮进行添加！",
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
        })
    }

    //获取支持类型
    $scope.getSupportCategory = function (parentProjectTypeId) {
        $http.get(getSupportCategoryUrl + parentProjectTypeId)
        .success(function (response) {
            if (response.response != null) {
                $scope.supportCategorys = response.response;
            }
            else if(response.errorMsg == "未找到数据")
            {
                $scope.supportCategorys = null;
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
                msg: "错误：" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
            $scope.supportCategorys = null;
        })
    }

    //添加项目类型
    $scope.addProjectType = function () {
        $http.post(addProjectTypeUrl, {
            "SupportCategoryId": 0,
            "Name": $scope.addProjectTypeName
        })
        .success(function (response) {
            if (response.status == 0) {
                notif({
                    msg: "<b>恭喜:</b> 已成功添加项目类型！",
                    type: "success"
                });
                $scope.getProjectType();
                $scope.addProjectTypeName = null;
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "未能成功添加项目类型，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }

    //添加支持类型
    $scope.addSupportCategory = function () {
        $http.post(addSupportCategoryUrl, {
            "SupportCategoryId": $scope.projectTypes[$scope.isSelected].ProjectTypeId,
            "Name": $scope.addSupportCategoryName
        })
        .success(function (response) {
            if (response.status == 0) {
                notif({
                    msg: "<b>恭喜:</b> 已成功添加支持类型！",
                    type: "success"
                });
                $scope.getSupportCategory($scope.projectTypes[$scope.isSelected].ProjectTypeId);
                $scope.addSupportCategoryName = null;
            }
            else {
                notif({
                    type: "error",
                    msg: "错误："+response.errorMsg,
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
        })
        
    }

    //初始化
    $scope.getProjectType();

});