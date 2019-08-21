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

        //��֤�������ύ��ʼ����
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

        //��֤�������ύ��ֹ����
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

        //��֤��λ��˽�ֹ����
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

        //��֤ר�����ֽ�ֹ����
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

    // ��Ŀ��ʼ��� Modal
    $scope.showConfirmStartYearModal = function () {
        $("#confirmStartYear").modal("show");
    }

    // ��Ŀ��ʼ���
    $scope.ApplicationStartYearChange = function () {
        //��֤��Ŀ��ʼ���
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
                    msg: "���³ɹ�",
                });
                // ����ɹ�ִ�д���
                }, function errorCallback(response) {
                $("#confirmStartYear").modal("hide");
                // ����ʧ��ִ�д���
            });
        }
    }


    $scope.saveDates = function () {


        //��֤�������ύ��ʼ����
        if ($scope.ApplicationSubmitBeginTime == null) {
            $scope.SubmitBeginTimeIsEmpty = true;
        }
        else {
            $scope.SubmitBeginTimeIsEmpty = false;
        }

        //��֤�������ύ��ֹ����
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


        //��֤��λ��˽�ֹ����
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



        //��֤ר�����ֽ�ֹ����
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


        //��֤��Ŀ��ʼ���
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
                    msg: "δ�ܳɹ���ȡ���ѱ���ĸ��������ޡ������������һ��ʹ�ã�����Դ˾��档",
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
                    msg: "���³ɹ�",
                });
            })
            .error(function (response) {
                notif({
                    type: "error",
                    msg: "����" + response.errorMsg,
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
                msg: "��������ȷ��д������",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        }
    }




    //��ʼ��
    $scope.getSaveDate();


});