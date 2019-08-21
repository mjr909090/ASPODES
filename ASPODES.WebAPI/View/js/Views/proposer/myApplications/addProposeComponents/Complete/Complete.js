addProposeApp.component('complete', {
    templateUrl: '/View/Views/proposer/myApplications/addProposeComponents/Complete/complete.html',
    controller: completeCtrl,
    bindings: {
        ApplicationId: '<',
    }
});


function completeCtrl($scope, $http) {

    $scope.SaveLoading = false;
    $scope.pdfCreat = false;
    $scope.BasicInfoCorret = false;

    $scope.saveApplication = function () {
        $scope.SaveLoading = true;
        $http.put("/api/application/save/" + ApplicationId)
        .success(function (response) {
            notif({
                msg: "<b>恭喜:</b> 保存成功！请关闭此页面并在【申请书草稿】页面点击列表上方的刷新按钮。",
                type: "success",
                multiline: true,
                autohide: false
            });
            $scope.SaveLoading = false;
            $scope.pdfCreat = true;
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
            $scope.SaveLoading = false;
        });
    }


    $scope.closeWindow = function () {
        window.close();
    }


        


}