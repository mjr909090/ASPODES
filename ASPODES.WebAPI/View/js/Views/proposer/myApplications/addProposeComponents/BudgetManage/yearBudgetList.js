addProposeApp.component('yearbudgetlist', {
    templateUrl: '/View/Views/proposer/myApplications/addProposeComponents/BudgetManage/yearBudgetList.html',
    controller: yearBudgetListCtrl,
    bindings: {
        ApplicationId: '<',
    }
});


function yearBudgetListCtrl($scope, $http, $rootScope) {

    $scope.yusuanYear = 1;
    $scope.manageAnnualBudget = { BudgetItemId: "" };



    //Url
    var getYearAnnualBudgetItemUrl = "/api/annualbudget/";
    var getAllSubjectUrl = "/api/accountingsubject";
    var postAnnualBudgetItemUrl = "/api/annualbudgetitem";
    var delBudgetItemUrl = "/api/annualbudgetitem/";



    //设置年份
    $scope.setYear = function (year) {
        $scope.yusuanYear = year;
    }

    //获取年度经费条目
    $scope.getYearBudget = function () {
        $http.get(getYearAnnualBudgetItemUrl + ApplicationId + "/" + $scope.yusuanYear)
        .success(function (response) {
            $scope.yearTotalBudget = 0;

            if (response.response != null) {
                $scope.yearAnnualBudgetItems = response.response.Items;
            }
            else {
                $scope.yearAnnualBudgetItems = null;
            }
            

            angular.forEach($scope.yearAnnualBudgetItems, function (data) {
                $scope.yearTotalBudget = $scope.yearTotalBudget + data.Amount;
            })
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
            $scope.yearAnnualBudgetItems = null;
            $scope.yearTotalBudget = 0;
        });
    }


    //获取所有会计科目
    $scope.getAllSubject = function () {
        $http.get(getAllSubjectUrl)
        .success(function (response) {
            $scope.subjects = response.response;
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


    //添加经费条目
    $scope.addAnnualBudgetItemClick = function () {
        $http.post(postAnnualBudgetItemUrl, {
            "ApplicationId": ApplicationId,
            "Year": $scope.yusuanYear,
            "SubjectId": $scope.addAnnualBudgetItem.SubjectCode,
            "Amount": $scope.addAnnualBudgetItem.Amount,
            "Reason": $scope.addAnnualBudgetItem.Reason
        })
        .success(function (response) {
            if (response.status == 0) {
                notif({
                    msg: "<b>恭喜:</b> 经费条目添加成功",
                    type: "success"
                });
                $scope.getYearBudget();
                $scope.addAnnualBudgetItem = null;
            }
        })
        .error(function () {
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

    //显示删除经费条目模态框
    $scope.showDeleteAnnualBudgetItemModal = function (AnnualBudgetItemId) {
        $scope.manageAnnualBudget.AnnualBudgetItemId = AnnualBudgetItemId;
        $("#deleteAnnualBudgetItem").modal("show");
    }

    //删除经费条目
    $scope.delBudgetItem = function () {
        $http.delete(delBudgetItemUrl + "/" + $scope.manageAnnualBudget.AnnualBudgetItemId)
        .success(function (response) {
            if (response.status == 0) {
                notif({
                    msg: "<b>恭喜:</b> 经费条目删除成功",
                    type: "success"
                });
                $scope.getYearBudget();
            }
        })
        .error(function () {
            notif({
                type: "error",
                msg: "错误:" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        });
    }





    //初始化
    $scope.$on('initBudget', function () {
        $scope.getAllSubject();
        $scope.getYearBudget();
    })




}