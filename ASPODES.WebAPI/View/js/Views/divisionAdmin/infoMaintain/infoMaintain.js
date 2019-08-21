var infoMaintainApp = angular.module('ngInfoMaintainApp', []);

infoMaintainApp.controller('ngInfoMaintainCtrl', function ($scope, $http) {

    //	 *****Get当前单位信息的Url*****
    var divisionInfoUrl = "/api/inst/self";

    //	 *****Get本单位用户列表的Url*****
    var userListUrl = "/api/person/combo/inst/0";

    //	 *****Put单位维护信息-提交-的Url*****
    var maintainInfoUrl = "/api/Inst";

    //  ************************初始化json数据************************
    $scope.listAlready = false;

    // ********************显示当前单位信息内容*********************************
    $scope.geDivisionInfoList = function () {
        $http.get(divisionInfoUrl)
               .success(function (response) {
                   if (response.response != null) {
                       $scope.divisionInfoList = response.response;
                       $scope.infoModalData = $scope.divisionInfoList;
                       $scope.listAlready = true;
                   }
                   
               })
               .error(function () {
                   notif({
                       msg: "<b>错误:</b> 页面获取失败，请检查网络是否通畅",
                       type: "error"
                   });
               });
    }
    $scope.geDivisionInfoList(); 

    // ********************联系人下拉列表框内容*********************************
    //get
    //获取本单位用户列表
    $scope.geUserList = function () {
        $http.get(userListUrl)
               .success(function (response) {
                   $scope.userList = response.response;
                   //console.log($scope.userList);
               })
               .error(function () {
                   notif({
                       msg: "<b>错误:</b> 下拉列内容获取失败，请检查网络是否通畅",
                       type: "error"
                   });
               });
    }
    $scope.geUserList(); 
   
    //提交
    $scope.editInstituteInfo = function () {
        $scope.infoModalData.ContactId = $scope.selectItem.Email;
        //console.log($scope.infoModalData);
        $http({
            method: 'PUT',
            url: maintainInfoUrl,
            data: {
                "InstituteId": $scope.infoModalData.InstituteId,
                "Code":$scope.infoModalData.Code,
                "Name":$scope.infoModalData.Name,
                "ShortName": $scope.infoModalData.ShortName,
                "ParentId": $scope.infoModalData.ParentId,
                "Type": $scope.infoModalData.Type,
                "Address": $scope.infoModalData.Address,
                "Zip": $scope.infoModalData.Zip,
                "WebSite": $scope.infoModalData.WebSite,
                "Comment": $scope.infoModalData.Comment,
                "Status": $scope.infoModalData.Status,
                "ContactId":$scope.infoModalData.ContactId,
                "ContactName": null,
                "ContactEmail": null,
                "ContactPhone":null
            }
        })
       .success(function (response) {
           if (response.status == 0) {
               notif({
                   msg: "<b>恭喜:</b> 编辑成功",
                   type: "success"
               });
           }
           //console.log(response.response);
          // $scope.geDivisionInfoList(); //编辑成功后重新获取列表
       })
        .error(function (response) {
            notif({
                msg: "<b>错误:</b> 编辑失败，请检查网络是否通畅",
                type: "error"
            });
           // console.log(response.response);
        });
    }
})