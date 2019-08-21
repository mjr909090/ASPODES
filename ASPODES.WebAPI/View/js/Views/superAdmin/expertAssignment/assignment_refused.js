/**
 * Created by majunjun on 2017/4/7.
 */
var expert_accepted = angular.module('ngAssignment_RefusedApp',[]);

/*expert_accepted.config([
    '$routeProvider',
    function($routeProvider) {
        $routeProvider
            .when('/Assignment_Refused', {
                templateUrl: 'Views/superAdmin/expertAssignment/assignment_refused.html',
                controller: 'ngAssignment_RefusedCtrl'
            })
    }
]);*/

expert_accepted.controller('ngAssignment_RefusedCtrl', function($scope) {
    $scope.BatchAllocationExperts = function(){//指派专家
        $("#BatchAllocationExperts").modal("show");
    };
    $scope.BatchSendMessages = function(){//发送短信
        $("#BatchSendMessages").modal("show");
    };
    /*$scope.setTable = function(){

        $("#refusedExpertTable").bootstrapTable({
            //url: '../../data/declaration.json',
            // method: 'get',
            toolbar: "#toolbar",
            //striped: true,
            // cache: false,
            pagination: true,
            sortable: true,
            sortOrder: "asc",
            //sidePagination: "client",
            pageNumber: 1,
            pageSize: 2,
            pageList: [2, 4, 6],
            // showRefresh: true,
            //clickToSelect: true,
            // uniqueId: 'ID',
            //showToggle: true,
            detailView: false,
            // showExport: true,
            // exportDataType: "selected",
            //search: true

        });

    };
    $scope.setTable();*/
});
//模态框居中
$(function() {
    var $m_btn = $('#modalBtn');
    var $modal = $('#NewExpertNameModal, #RejectExpertNameModal,#expertInfo_ViewModal');
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