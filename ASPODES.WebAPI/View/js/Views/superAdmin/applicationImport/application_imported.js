/**
 * Created by majunjun on 2017/4/7.
 */
var application_exported = angular.module('ngApplication_ImportedApp',[]);

/*application_exported.config([
    '$routeProvider',
    function($routeProvider) {
        $routeProvider
            .when('/Application_Imported', {
                templateUrl: 'Views/superAdmin/applicationImport/application_imported.html',
                controller: 'ngApplication_ImportedCtrl'
            })
    }
]);*/

application_exported.controller('ngApplication_ImportedCtrl', function($scope) {
    $scope.exportApplication = function(){
        $("#exportApplication").modal("show");
    };

});
//模态框居中
$(function() {
    var $m_btn = $('#modalBtn');
    var $modal = $('#btn_viewImportApplication,#applyExport,#grounpBy, #PDFDownLoad,#review,#saveApplication,#expertApplication_yes');
    $m_btn.on('click', function() {
        $modal.modal({
            backdrop: 'static'
        });
    });

    $modal.on('show.bs.modal', function() {
        var $this = $(this);
        var $modal_dialog = $this.find('.modal-dialog');
        // 关键代码，如没将modal设置为 block，则$modala_dialog.height() 为零
        $this.css('display', 'block');
        $modal_dialog.css({
            'margin-top': Math.max(0, ($(window).height() - $modal_dialog.height())/2)
        });
    });
});