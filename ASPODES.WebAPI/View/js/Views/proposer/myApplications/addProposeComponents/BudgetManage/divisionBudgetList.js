addProposeApp.component('divisionbudgetlist', {
    templateUrl: '/View/Views/proposer/myApplications/addProposeComponents/BudgetManage/divisionBudgetList.html',
    controller: divisionBudgetListCtrl,
    bindings: {
        ApplicationId: '<',
    }
});


function divisionBudgetListCtrl($scope, $http, $cookieStore, $rootScope) {

    //URL
    var getWholeBudgetAssistInstUrl = "/api/application/participateinst/";
    var getInstBudgetUrl = "/api/instbudget/";
    var addInstBudgetUrl = "/api/instbudget";
    var editInstBudgetUrl = "/api/instbudget";
    var delInstBudgetUrl = "/api/instbudget/";


    $scope.addInstBudgetItem = {
        'yearAmount': [0, 0, 0, 0, 0]
    };


    $scope.showEditInstBudgetItemModal = function () {
        $scope.manageInstBudgetCopy = angular.copy($scope.manageInstBudget);
        $scope.manageInstBudgetCopy.AnnualBudgets = [
            { Amount: 0 },
            { Amount: 0 },
            { Amount: 0 },
            { Amount: 0 },
            { Amount: 0 }
        ]

        angular.forEach($scope.manageInstBudget.AnnualBudgets, function (data, index) {
            if (data != undefined) {
                $scope.manageInstBudgetCopy.AnnualBudgets[index] = $scope.manageInstBudget.AnnualBudgets[index];
            }
        })

        $("#editInstBudgetItemModal").modal("show");
    }


    //获取参与单位列表
    $scope.getWholeBudgetAssistInst = function () {
        $http.get(getWholeBudgetAssistInstUrl + ApplicationId)
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
        $http.get(getInstBudgetUrl + ApplicationId)
        .success(function (response) {
            $scope.instBudgets = response.response;
            $scope.instTotalBudget = 0;
            $scope.eachYearTotalBudget = new Array();

            $scope.eachYearTotalBudget = [0, 0, 0, 0, 0];
            angular.forEach($scope.instBudgets, function (data) {
                $scope.instTotalBudget = $scope.instTotalBudget + data.Amount;
                angular.forEach(data.AnnualBudgets, function (eachYear, index) {
                    $scope.eachYearTotalBudget[index] = $scope.eachYearTotalBudget[index] + eachYear.Amount;
                })
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

    //添加各单位经费列表
    $scope.addInstBudgetItemFun = function () {
        if ($scope.addInstBudgetItem.SecondAmount == null) {
            $scope.addInstBudgetItem.SecondAmount = 0;
        }
        if ($scope.addInstBudgetItem.ThirdAmount == null) {
            $scope.addInstBudgetItem.ThirdAmount = 0;
        }

        $http({
            method: "POST",
            url: addInstBudgetUrl,
            data: {
                "ApplicationId": ApplicationId,
                "InstituteId": $scope.addInstBudgetItem.InstituteId,
                "Amount": $scope.addInstBudgetItem.Amount,
                "AnnualBudgets": [
                    $scope.addInstBudgetItem.yearAmount[0],
                    $scope.addInstBudgetItem.yearAmount[1],
                    $scope.addInstBudgetItem.yearAmount[2],
                    $scope.addInstBudgetItem.yearAmount[3],
                    $scope.addInstBudgetItem.yearAmount[4]
                ]
            }
        })
        .success(function (response) {
            if (response.status == 0) {
                notif({
                    msg: "<b>恭喜:</b> 单位经费条目添加成功",
                    type: "success"
                });
                $scope.getInstBudget();
                $scope.getWholeBudgetAssistInst();
                $scope.addInstBudgetItem = {
                    "Amount": 0,
                    'yearAmount': [0, 0, 0, 0, 0]
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
    $scope.editInstBudgetItem = function () {
        if ($scope.manageInstBudget.secondAmount == null) {
            $scope.manageInstBudget.secondAmount = 0;
        }
        if ($scope.manageInstBudget.thirdAmount == null) {
            $scope.manageInstBudget.thirdAmount = 0;
        }
        $http({
            method: "POST",
            url: editInstBudgetUrl,
            data: {
                "ApplicationId": ApplicationId,
                "InstBudgetId": $scope.manageInstBudgetCopy.InstBudgetId,
                "InstituteId": $scope.manageInstBudgetCopy.InstituteId,
                "Amount": $scope.manageInstBudgetCopy.Amount,
                "AnnualBudgets": [
                    $scope.manageInstBudgetCopy.AnnualBudgets[0].Amount,
                    $scope.manageInstBudgetCopy.AnnualBudgets[1].Amount,
                    $scope.manageInstBudgetCopy.AnnualBudgets[2].Amount,
                    $scope.manageInstBudgetCopy.AnnualBudgets[3].Amount,
                    $scope.manageInstBudgetCopy.AnnualBudgets[4].Amount
                ]
            }
        })
        .success(function (response) {
            if (response.status == 0) {
                notif({
                    msg: "<b>恭喜:</b> 单位经费条目修改成功",
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
        })
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

    $scope.$on('initBudget', function () {
        $scope.getWholeBudgetAssistInst();

        $scope.getInstBudget();

    })



}