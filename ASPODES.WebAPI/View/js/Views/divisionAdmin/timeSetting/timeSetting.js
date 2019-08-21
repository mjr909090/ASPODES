var timeSettingApp = angular.module('ngTimeSettingApp', []);

timeSettingApp.controller('ngTimeSettingCtrl', function($scope) {
    $scope.checkFun = function () {
        if ($("#male").prop('checked') == true) {
            if (($scope.startDate == null) || ($scope.endDate == unll)) {
                alert("The Date must be selected!");
                $("#male").prop('checked', false);
            }
            if (($scope.startDate != null) && ($scope.endDate != unll)) {
                $("#male").prop('checked', true);
                //向后台发送请求
            }
        }
        else {
            var r = confirm("确认关闭申请书申请吗？");
            if (r == true) {
                //						alert("关闭申请书申请成功!");
                $("#male").prop('checked', false);
            }
            else {
                alert("关闭失败!");
                $("#male").prop('checked', true);
            }
        }
    }

    //datePicker设置
    $('.datepicker').datepicker({
        format: 'yyyy-mm-dd',
        language: 'zh-CN'
    });
	

})
//控制开关
//$(document).ready(function () {
//    $('#male').click(function () {
//        if ($("#male").prop('checked') == true) {
//            var r = confirm("确认开放申请书申请吗？");
//            if (r == true) {
//                //						alert("开放申请书申请成功!");
//                $("#male").prop('checked', true);
//            }
//            else {
//                alert("开放失败!");
//                $("#male").prop('checked', false);
//            }
//        }
//        else {
//            var r = confirm("确认关闭申请书申请吗？");
//            if (r == true) {
//                //						alert("关闭申请书申请成功!");
//                $("#male").prop('checked', false);
//            }
//            else {
//                alert("关闭失败!");
//                $("#male").prop('checked', true);
//            }
//        }

//    });
//});
      