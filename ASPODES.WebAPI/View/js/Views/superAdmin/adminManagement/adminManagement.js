/**
 * Created by majunjun on 2017/4/12.
 */
var expert_add = angular.module('ngAdminManagementApp',[]);

/*expert_add.config([
    '$routeProvider',
    function($routeProvider) {
        $routeProvider
            .when('/adminManagement', {
                templateUrl: 'Views/superAdmin/adminManagement/adminManagement.html',
                controller: 'ngAdminManagementCtrl'
            })
    }
]);*/

expert_add.controller('ngAdminManagementCtrl', function($scope) {
    $scope.addAdmin_Save = function(){
        window.open("Views/proposer/myApplications/addPropose.html");
    };
    $scope.showModifyAdminModal = function(){
        $("#ModifyAdminModal").modal("show");
    };

    $scope.showDeleteAdminModal = function(){
        $("#deleteAdminModal").modal("show");
    };

});

//ģ̬�����
$(function() {
    var $m_btn = $('#modalBtn');
    var $modal = $('#addAdmin,#viewAdminInfo,#deleteAdmin');
    $m_btn.on('click', function() {
        $modal.modal({
            backdrop: 'static'
        });
    });

    $modal.on('show.bs.modal', function() {
        var $this = $(this);
        var $modal_dialog = $this.find('.modal-dialog');
        // �ؼ����룬��û��modal����Ϊ block����$modala_dialog.height() Ϊ��
        $this.css('display', 'block');
        $modal_dialog.css({
            'margin-top': Math.max(0, ($(window).height() - $modal_dialog.height()) / 2)
        });
    });
});

//��ӹ���Ա��ȡ����ť
function addAdmin_Close(){
    window.open("Views/proposer/myApplications/addPropose.html");
}

