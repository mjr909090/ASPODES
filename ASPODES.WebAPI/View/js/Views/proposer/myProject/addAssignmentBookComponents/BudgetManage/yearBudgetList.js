addTaskApp.component('yearbudgetlist', {
    templateUrl: '/View/Views/proposer/myProject/addAssignmentBookComponents/BudgetManage/yearBudgetList.html',
    controller: yearBudgetListCtrl,
    bindings: {
        ApplicationId: '<',
    }
});


function yearBudgetListCtrl($scope, $http, $rootScope) {

    $scope.yusuanYear = 1;
    $scope.manageAnnualBudget = { BudgetItemId: "" };



    //Url
    var getYearAnnualBudgetItemUrl = "/api/annualTaskBudgetItem/";
    var getAllSubjectUrl = "/api/accountingsubject";
    var postAnnualBudgetItemUrl = "/api/annualTaskBudgetItem/";
    var delBudgetItemUrl = "/api/annualTaskBudgetItem/";



    //获取年度经费条目
    $scope.getYearBudget = function () {
        $http.get(getYearAnnualBudgetItemUrl + TaskId)
        .success(function (response) {
            $scope.yearTotalBudget = 0;

            $scope.yearAnnualBudgetItems = response.response;

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

    //计算已分配金额
    $scope.checkTotalBudget = function () {

        $scope.yearTotalBudget = 0;

        angular.forEach($scope.yearAnnualBudgetItems, function (data) {
            $scope.yearTotalBudget = $scope.yearTotalBudget + data.Amount;
        })
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
            "AnnualTaskId": TaskId,
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

    $scope.enterPress = function (e) {
        var keycode = window.event ? e.keyCode : e.which;
        if (keycode == 13) {
            $scope.addAnnualBudgetItemClick();
        }
    }

    //修改经费条目
    //$scope.putAnnualBudgetItemClick = function (yearAnnualBudgetItem) {

    //    $scope.putYearAnnualBudgetItem = angular.copy(yearAnnualBudgetItem);

    //    $http.put(postAnnualBudgetItemUrl, {
    //        "AnnualTaskBudgetItemId": $scope.putYearAnnualBudgetItem.AnnualTaskBudgetItemId,
    //        "SubjectId": $scope.putYearAnnualBudgetItem.SubjectId,
    //        "Amount": $scope.putYearAnnualBudgetItem.Amount,
    //        "Reason": $scope.putYearAnnualBudgetItem.Reason
    //    })
    //    .success(function (response) {
    //        if (response.status == 0) {
    //            notif({
    //                msg: "<b>恭喜:</b> 经费条目修改成功",
    //                type: "success"
    //            });
    //            $scope.getYearBudget();
    //            $scope.addAnnualBudgetItem = null;
    //        }
    //    })
    //    .error(function () {
    //        notif({
    //            type: "error",
    //            msg: "错误:" + response.errorMsg,
    //            position: "center",
    //            width: 500,
    //            height: 60,
    //            autohide: false
    //        });
    //    })
    //}


    //修改经费条目(全部发送)
    $scope.putAnnualBudgetItemClick = function () {

        $http.put(postAnnualBudgetItemUrl, $scope.yearAnnualBudgetItems)
        .success(function (response) {
            if (response.status == 0) {
                notif({
                    msg: "<b>恭喜:</b> 经费条目修改成功",
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
        })
    }


    //显示删除经费条目模态框
    $scope.showDeleteAnnualBudgetItemModal = function (AnnualBudgetItemId) {
        $scope.manageAnnualBudget.AnnualBudgetItemId = AnnualBudgetItemId;
        $("#deleteAnnualBudgetItem").modal("show");
    }

    //删除经费条目
    $scope.delBudgetItem = function () {
        $http.delete(delBudgetItemUrl + $scope.manageAnnualBudget.AnnualBudgetItemId)
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
    $scope.getAllSubject();
    $scope.getYearBudget();

}