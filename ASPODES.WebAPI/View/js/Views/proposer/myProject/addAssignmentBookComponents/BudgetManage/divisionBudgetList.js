addTaskApp.component('divisionbudgetlist', {
    templateUrl: '/View/Views/proposer/myProject/addAssignmentBookComponents/BudgetManage/divisionBudgetList.html',
    controller: divisionBudgetListCtrl,
    bindings: {
        ApplicationId: '<',
    }
});


function divisionBudgetListCtrl($scope, $http, $cookieStore, $rootScope) {

    //URL
    var getWholeBudgetAssistInstUrl = "/api/project/";
    var getInstBudgetUrl = "/api/annualTaskInstBudget/";
    var addInstBudgetUrl = "/api/annualTaskInstBudget";
    var editInstBudgetUrl = "/api/instbudget";
    var delInstBudgetUrl = "/api/instbudget/";


    //获取参与单位列表
    $scope.getWholeBudgetAssistInst = function () {
        $http.get(getWholeBudgetAssistInstUrl + ProjectId + "/jionInst")
        .success(function (response) {
            $scope.wholeBudgetAssistInsts = response.response;
        })
        .error(function () {
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

    //获取各单位经费列表
    $scope.getInstBudget = function () {
        $http.get(getInstBudgetUrl + TaskId)
        .success(function (response) {
            $scope.instBudgets = response.response;
            $scope.instTotalBudget = 0;

            angular.forEach($scope.instBudgets, function (data) {
                $scope.instTotalBudget = $scope.instTotalBudget + data.Amount;
            })
        })
        .error(function () {
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

    //计算已分配金额
    $scope.checkTotalBudget = function () {

        $scope.instTotalBudget = 0;

        angular.forEach($scope.instBudgets, function (data) {
            $scope.instTotalBudget = $scope.instTotalBudget + data.Amount;
        })

    }


    //添加各单位经费列表
    $scope.addDivisionBudgetItemClick = function (instBudget) {

        $http({
            method: "PUT",
            url: addInstBudgetUrl,
            data: {
                "AnnualTaskId": TaskId,
                "InstituteId": instBudget.InstituteId,
                "Amount": instBudget.Amount,
            }
        })
        .success(function (response) {
            if (response.status == 0) {
                notif({
                    msg: "<b>恭喜:</b> 单位经费条目修改成功",
                    type: "success"
                });
                $scope.getInstBudget();
                $scope.getWholeBudgetAssistInst();
                $scope.addInstBudgetItem = {
                    "Amount": 0
                };
            }
        })
        .error(function () {
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



    //修改各单位经费列表
    $scope.putDivisionBudgetItemClick = function () {

        $http({
            method: "PUT",
            url: addInstBudgetUrl,
            data: $scope.instBudgets
        })
        .success(function (response) {
            if (response.status == 0) {
                notif({
                    msg: "<b>恭喜:</b> 单位经费条目修改成功",
                    type: "success"
                });
                $scope.getInstBudget();
            }
        })
        .error(function () {
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

    //回车键响应
    $scope.enterPress = function (e) {
        var keycode = window.event ? e.keyCode : e.which;
        if (keycode == 13) {
            $scope.addDivisionBudgetItemClick();
        }
    }

    //删除各单位经费列表
    $scope.delInstBudgetItem = function () {
        $http.delete(delInstBudgetUrl + $scope.manageInstBudget.InstBudgetId)
        .success(function (response) {
            if (response.status == 0) {
                notif({
                    msg: "<b>恭喜:</b> 删除成功",
                    type: "success"
                });
                $scope.radioSelected = false;
                $scope.getInstBudget();
                $scope.getWholeBudgetAssistInst();
            }
        })
        .error(function () {
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


    //获取参与单位列表
    $scope.getWholeBudgetAssistInst();
    $scope.getInstBudget();




}