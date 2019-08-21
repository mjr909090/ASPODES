/**
 * Created by majunjun on 2017/4/12.
 */
var stageManagement = angular.module('ngStageManagementApp',[]);

/*stageManagement.config([
    '$routeProvider',
    function($routeProvider) {
        $routeProvider
            .when('/stageManagement', {
                templateUrl: 'Views/superAdmin/stageManagement/stageManagement.html',
                controller: 'ngStageManagementCtrl'
            })
    }
]);*/

stageManagement.controller('ngStageManagementCtrl', function($scope) {
    $scope.popup1 = {
        opened: false
    };
    $scope.open1 = function () {
        $scope.popup1.opened = true;
    };

});


$(function() {
    var $m_btn = $('#modalBtn');
    var $modal = $('#save_StageManagement');
    $m_btn.on('click', function() {
        $modal.modal({
            backdrop: 'static'
        });
    });

    $modal.on('show.bs.modal', function() {
        var $this = $(this);
        var $modal_dialog = $this.find('.modal-dialog');

        $this.css('display', 'block');
        $modal_dialog.css({
            'margin-top': Math.max(0, ($(window).height() - $modal_dialog.height())/2)
        });
    });
});;