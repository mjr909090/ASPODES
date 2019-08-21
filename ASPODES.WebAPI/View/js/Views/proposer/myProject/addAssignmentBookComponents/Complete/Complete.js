addTaskApp.component('complete', {
    templateUrl: '/View/Views/proposer/myProject/addAssignmentBookComponents/Complete/complete.html',
    controller: completeCtrl,
    bindings: {
        ApplicationId: '<',
    }
});


function completeCtrl($scope, $http) {

    $scope.SaveLoading = false;
    $scope.pdfCreat = false;
    $scope.BasicInfoCorret = false;

    $scope.saveAnnualTask = function () {
        $scope.SaveLoading = true;
        $http.put("/api/annualTask/submit/" + TaskId)
        .success(function (response) {
            notif({
                msg: "<b>恭喜:</b> 提交成功！请关闭此页面并刷新【我主持的项目】。",
                type: "success",
                multiline: true,
                autohide: false
            });
            $scope.SaveLoading = false;
            $scope.pdfCreat = true;
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
            $scope.SaveLoading = false;
            $scope.pdfCreat = false;
        })
        .then(function (response) {
            $scope.SaveLoading = false;
        })
    }


    $scope.closeWindow = function () {
        window.close();
    }


        


}