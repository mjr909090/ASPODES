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
                //���̨��������
            }
        }
        else {
            var r = confirm("ȷ�Ϲر�������������");
            if (r == true) {
                //						alert("�ر�����������ɹ�!");
                $("#male").prop('checked', false);
            }
            else {
                alert("�ر�ʧ��!");
                $("#male").prop('checked', true);
            }
        }
    }

    //datePicker����
    $('.datepicker').datepicker({
        format: 'yyyy-mm-dd',
        language: 'zh-CN'
    });
	

})
//���ƿ���
//$(document).ready(function () {
//    $('#male').click(function () {
//        if ($("#male").prop('checked') == true) {
//            var r = confirm("ȷ�Ͽ���������������");
//            if (r == true) {
//                //						alert("��������������ɹ�!");
//                $("#male").prop('checked', true);
//            }
//            else {
//                alert("����ʧ��!");
//                $("#male").prop('checked', false);
//            }
//        }
//        else {
//            var r = confirm("ȷ�Ϲر�������������");
//            if (r == true) {
//                //						alert("�ر�����������ɹ�!");
//                $("#male").prop('checked', false);
//            }
//            else {
//                alert("�ر�ʧ��!");
//                $("#male").prop('checked', true);
//            }
//        }

//    });
//});
      