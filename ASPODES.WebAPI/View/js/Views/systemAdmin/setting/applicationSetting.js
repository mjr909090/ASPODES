var applicationSettingApp = angular.module('ngApplicationSettingApp',[]);

applicationSettingApp.controller('ngApplicationSettingCtrl', function($scope, $http){
	
    var todayDate = new Date();
    var twoLaterYear = todayDate.getFullYear() + 2;
    var twoLaterYearDate = new Date();
    twoLaterYearDate.setFullYear(twoLaterYear);

    //URL
    var getSaveDateUrl = "/api/Setting/ApplicationSetting";
    var putSaveDateUrl = "/api/Setting/ApplicationSetting";
    var putApplicationStartYearUrl = "/api/setting/startApplication/";


    function compareDate(date1, date2) {
        var oDate1 = new Date(date1);
        var oDate2 = new Date(date2);

        return oDate1.valueOf() > oDate2.valueOf()
    }


    $(".ApplicationSubmitBeginTime").datepicker({
        language: 'zh-CN',
        autoclose: true,
        maxViewMode: 2,
        todayHighlight: true,
        format: "yyyy-mm-dd",
        startDate: new Date(),
        todayBtn: "linked"
    })


    $(".ApplicationSubmitDeadline").datepicker({
        language: 'zh-CN',
        autoclose: true,
        maxViewMode: 2,
        todayHighlight: true,
        format: "yyyy-mm-dd",
        startDate: new Date()
    })

    $(".ApplicationVerifyDeadline").datepicker({
        language: 'zh-CN',
        autoclose: true,
        maxViewMode: 2,
        todayHighlight: true,
        format: "yyyy-mm-dd",
        startDate: new Date()
    })

    $(".ApplicationExpertDeadline").datepicker({
        language: 'zh-CN',
        autoclose: true,
        maxViewMode: 2,
        todayHighlight: true,
        format: "yyyy-mm-dd",
        startDate: new Date()
    })

    $(".ApplicationStartYear").datepicker({
        language: 'zh-CN',
        autoclose: true,
        minViewMode: 2,
        maxViewMode: 2,
        todayHighlight: true,
        format: "yyyy",
        startDate: new Date(),
        endDate: twoLaterYearDate
    })
    
    $scope.ApplicationSubmitBeginTimeChange = function () {
        $(".ApplicationSubmitDeadline").datepicker("setStartDate", $scope.ApplicationSubmitBeginTime);

        //验证申请书提交开始日期
        if ($scope.ApplicationSubmitBeginTime == null) {
            $scope.SubmitBeginTimeIsEmpty = true;
        }
        else {
            $scope.SubmitBeginTimeIsEmpty = false;
        }

        if (compareDate($scope.ApplicationSubmitBeginTime, $scope.ApplicationSubmitDeadline)) {
            $scope.SubmitDeadlineIsBefore = true;
        }
        else {
            $scope.SubmitDeadlineIsBefore = false;
        }

    }

    $scope.ApplicationSubmitDeadlineChange = function () {
        $(".ApplicationVerifyDeadline").datepicker("setStartDate", $scope.ApplicationSubmitDeadline);

        //验证申请书提交截止日期
        if ($scope.ApplicationSubmitDeadline == null) {
            $scope.SubmitDeadlineIsEmpty = true;
        }
        else {
            $scope.SubmitDeadlineIsEmpty = false;
        }

        if (compareDate($scope.ApplicationSubmitBeginTime, $scope.ApplicationSubmitDeadline)) {
            $scope.SubmitDeadlineIsBefore = true;
        }
        else {
            $scope.SubmitDeadlineIsBefore = false;
        }

        if (compareDate($scope.ApplicationSubmitDeadline, $scope.ApplicationVerifyDeadline)) {
            $scope.VerifyDeadlineIsBefore = true;
        }
        else {
            $scope.VerifyDeadlineIsBefore = false;
        }

    }

    $scope.ApplicationVerifyDeadlineChange = function () {
        $(".ApplicationExpertDeadline").datepicker("setStartDate", $scope.ApplicationVerifyDeadline);

        //验证单位审核截止日期
        if ($scope.ApplicationVerifyDeadline == null) {
            $scope.VerifyDeadlineIsEmpty = true;
        }
        else {
            $scope.VerifyDeadlineIsEmpty = false;
        }

        if (compareDate($scope.ApplicationSubmitDeadline, $scope.ApplicationVerifyDeadline)) {
            $scope.VerifyDeadlineIsBefore = true;
        }
        else {
            $scope.VerifyDeadlineIsBefore = false;
        }

        if (compareDate($scope.ApplicationVerifyDeadline, $scope.ApplicationExpertDeadline)) {
            $scope.ExpertDeadlineIsBefore = true;
        }
        else {
            $scope.ExpertDeadlineIsBefore = false;
        }

    }

    $scope.ApplicationExpertDeadlineChange = function () {

        //验证专家评分截止日期
        if ($scope.ApplicationExpertDeadline == null) {
            $scope.ExpertDeadlineIsEmpty = true;
        }
        else {
            $scope.ExpertDeadlineIsEmpty = false;
        }

        if (compareDate($scope.ApplicationVerifyDeadline, $scope.ApplicationExpertDeadline)) {
            $scope.ExpertDeadlineIsBefore = true;
        }
        else {
            $scope.ExpertDeadlineIsBefore = false;
        }   
    }

    // 项目开始年份 Modal
    $scope.showConfirmStartYearModal = function () {
        $("#confirmStartYear").modal("show");
    }

    // 项目开始年份
    $scope.ApplicationStartYearChange = function () {
        //验证项目开始年份
        if ($scope.ApplicationStartYear == null) {
            $scope.StartYearIsEmpty = true;
        }
        else {
            $scope.StartYearIsEmpty = false;
            var putStartYearUrl = putApplicationStartYearUrl + $scope.ApplicationStartYear;
            $http({
                method: 'PUT',
                url: putStartYearUrl,
                data: $scope.ApplicationStartYear
            }).then(function successCallback(response) {
                $("#confirmStartYear").modal("hide");
                notif({
                    type: "success",
                    msg: "更新成功",
                });
                // 请求成功执行代码
                }, function errorCallback(response) {
                $("#confirmStartYear").modal("hide");
                // 请求失败执行代码
            });
        }
    }


    $scope.saveDates = function () {


        //验证申请书提交开始日期
        if ($scope.ApplicationSubmitBeginTime == null) {
            $scope.SubmitBeginTimeIsEmpty = true;
        }
        else {
            $scope.SubmitBeginTimeIsEmpty = false;
        }

        //验证申请书提交截止日期
        if ($scope.ApplicationSubmitDeadline == null) {
            $scope.SubmitDeadlineIsEmpty = true;
        }
        else {
            $scope.SubmitDeadlineIsEmpty = false;
        }


        if (compareDate($scope.ApplicationSubmitBeginTime, $scope.ApplicationSubmitDeadline)) {
            $scope.SubmitDeadlineIsBefore = true;
        }
        else {
            $scope.SubmitDeadlineIsBefore = false;
        }


        //验证单位审核截止日期
        if ($scope.ApplicationVerifyDeadline == null) {
            $scope.VerifyDeadlineIsEmpty = true;
        }
        else {
            $scope.VerifyDeadlineIsEmpty = false;
        }

        if (compareDate($scope.ApplicationSubmitDeadline, $scope.ApplicationVerifyDeadline)) {
            $scope.VerifyDeadlineIsBefore = true;
        }
        else {
            $scope.VerifyDeadlineIsBefore = false;
        }



        //验证专家评分截止日期
        if ($scope.ApplicationExpertDeadline == null) {
            $scope.ExpertDeadlineIsEmpty = true;
        }
        else {
            $scope.ExpertDeadlineIsEmpty = false;
        }


        if (compareDate($scope.ApplicationVerifyDeadline, $scope.ApplicationExpertDeadline)) {
            $scope.ExpertDeadlineIsBefore = true;
        }
        else {
            $scope.ExpertDeadlineIsBefore = false;
        }


        //验证项目开始年份
        if ($scope.ApplicationStartYear == null) {
            $scope.StartYearIsEmpty = true;
        }
        else {
            $scope.StartYearIsEmpty = false;
        }


    }

    
    $scope.getSaveDate = function () {
        $http.get(getSaveDateUrl)
        .success(function (response) {
            if (response.response == null) {
                notif({
                    type: "error",
                    msg: "未能成功获取到已保存的各申请期限。如果这是您第一次使用，请忽略此警告。",
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            }
            else {
                $scope.ApplicationSubmitBeginTime = response.response.ApplicationSubmitBeginTime;
                $scope.ApplicationSubmitDeadline = response.response.ApplicationSubmitDeadline;
                $scope.ApplicationVerifyDeadline = response.response.ApplicationVerifyDeadline;
                $scope.ApplicationExpertDeadline = response.response.ApplicationExpertDeadline;
                $scope.ApplicationStartYear = response.response.ApplicationStartYear;
            }
        })
    }


    $scope.putSaveDate = function () {
        if(($scope.SubmitBeginTimeIsEmpty
            || $scope.SubmitDeadlineIsEmpty || $scope.SubmitDeadlineIsBefore
            || $scope.VerifyDeadlineIsEmpty || $scope.VerifyDeadlineIsBefore
            || $scope.ExpertDeadlineIsEmpty || $scope.ExpertDeadlineIsBefore
            || $scope.StartYearIsEmpty) != true)
        {
            $http.put(putSaveDateUrl, {
                "ApplicationSubmitBeginTime": $scope.ApplicationSubmitBeginTime,
                "ApplicationSubmitDeadline": $scope.ApplicationSubmitDeadline,
                "ApplicationVerifyDeadline": $scope.ApplicationVerifyDeadline,
                "ApplicationExpertDeadline": $scope.ApplicationExpertDeadline,
                "ApplicationStartYear": $scope.ApplicationStartYear
            })
            .success(function (response) {
                notif({
                    type: "success",
                    msg: "更新成功",
                });
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
        else {
            notif({
                type: "error",
                msg: "错误：请正确填写后重试",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        }
    }




    //初始化
    $scope.getSaveDate();


});