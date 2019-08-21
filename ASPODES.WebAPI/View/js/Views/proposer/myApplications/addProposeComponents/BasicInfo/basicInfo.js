addProposeApp.component('basicinfo', {
    templateUrl: '/View/Views/proposer/myApplications/addProposeComponents/BasicInfo/basicInfo.html',
    controller: basicInfoCtrl,
    bindings: {
    }
});


function basicInfoCtrl($scope, $http,  $rootScope) {

    var that = this;

    //Url
    var getProposeBasicInfoUrl = "/api/application/step/one/left";
    var postProposeBasicInfoUrl = "/api/application";
    var postFieldUrl = "/api/applicationfield";


    //获取申请书基本信息
    $scope.getBasicInfo = function () {
        $http.get(getProposeBasicInfoUrl+ "/" + ApplicationId)
        .success(function (response) {
            that.basicInfo = response.response;
        })
    }


    //发送领域信息
    $scope.postFieldInfo = function () {
        $http({
            method: "POST",
            url: postFieldUrl,
            data: [{
                "ApplicationId": ApplicationId,
                "ApplicationFieldId": that.basicInfo.ApplicationFields[0].ApplicationFieldId,
                "SubFieldId": that.basicInfo.ApplicationFields[0].SubFieldId,
                "KeyWordsCN": that.basicInfo.ApplicationFields[0].KeyWordsCN,
                "KeyWordsEN": that.basicInfo.ApplicationFields[0].KeyWordsEN
            },
            {
                "ApplicationId": ApplicationId,
                "ApplicationFieldId": that.basicInfo.ApplicationFields[1].ApplicationFieldId,
                "SubFieldId": that.basicInfo.ApplicationFields[1].SubFieldId,
                "KeyWordsCN": that.basicInfo.ApplicationFields[1].KeyWordsCN,
                "KeyWordsEN": that.basicInfo.ApplicationFields[1].KeyWordsEN
            }]
        })
        .success(function (response) {
            if (response.status == 0) {
                notif({
                    msg: "<b>恭喜:</b> 成功保存当前页-申请书基本信息",
                    type: "success"
                });
                $rootScope.Period = that.basicInfo.Period;
                $rootScope.TotalBudget = that.basicInfo.TotalBudget;
                $rootScope.FirstYearBudget = that.basicInfo.FirstYearBudget;
                $rootScope.$broadcast('initMember', true);
                $rootScope.toNext();
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
         })
    }


    //发送申请书基本信息
    $scope.postBasicInfo = function () {
        if ($scope.hasSupportCategoryChange == false && $scope.isEdit == 'true') {
            $scope.propose.SupportCategoryId = $scope.linshiSupportCategoryId;
        }
        $http({
            method: "POST",
            url: postProposeBasicInfoUrl,
            data: {
                "ApplicationId": that.basicInfo.ApplicationId,
                "ProjectName": that.basicInfo.ProjectName,
                "DeleageType": that.basicInfo.DeleageType,
                "Period": that.basicInfo.Period,
                "ProjectTypeId": that.basicInfo.ProjectTypeId,
                "SupportCategoryId": that.basicInfo.SupportCategoryId,
                "TotalBudget": that.basicInfo.TotalBudget,
                "FirstYearBudget": that.basicInfo.FirstYearBudget,
                "AbstractContent": that.basicInfo.AbstractContent,
            }
        })
        .success(function (response) {
            if (response.status == 0) {
                $rootScope.Period = that.basicInfo.Period;
                //POST领域信息
                $scope.postFieldInfo();
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


    


    //初始化
    $scope.getBasicInfo();
}