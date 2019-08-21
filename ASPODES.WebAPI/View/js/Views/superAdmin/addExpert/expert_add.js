/**
 * Created by majunjun on 2017/4/7.
 */
var expert_add = angular.module('ngExpert_AddApp',[]);

/*expert_add.config([
    '$routeProvider',
    function($routeProvider) {
        $routeProvider
            .when('/Expert_Add', {
                templateUrl: 'Views/superAdmin/addExpert/expert_add.html',
                controller: 'ngExpert_AddCtrl'
            })
    }
]);
*/

expert_add.controller('ngExpert_AddCtrl', function($scope) {
    $scope.setTable = function(){
        $("#thisTable").bootstrapTable({
            toolbar: "#toolbar1",
            striped: true,
            cache: false,
            pagination: true,
            sortable: true,
            sortOrder: "asc",
            sidePagination: "client",
            pageNumber: 1,
            pageSize: 5,
            pageList: [5, 10, 25, 50],
            clickToSelect: true,
            uniqueId: 'ID',
            detailView: false,
            showExport: true,
            exportDataType: "selected",
            /*以下是搜索框*/
            showToggle: true,
            showRefresh: true,
            search: true

        });

       $("#searchCircleTable").bootstrapTable({
             toolbar: "#toolbar",
             striped: true,
             cache: false,
             clickToSelect: true,
             uniqueId: 'ID',
        });
    };

    $scope.setTable();
});

//模态框居中
$(function() {
    var $m_btn = $('#modalBtn');
    var $modal = $('#addExpert, #viewExpertInfo, #modifyExpert,#deleteExpert');
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