var pmApp = angular.module('ngPMApp', ['ngRoute']);

//for Two level routing
pmApp.config([
	'$routeProvider',
	function($routeProvider) {
		$routeProvider
			.when('/pm', {
				templateUrl: 'Views/expertReview/preReview.html',
				controller: 'ngPMCtrl'
			})
	}
]);

pmApp.controller('ngPMCtrl', function($scope) {
	$scope.setTable = function() {
		$("#PMTable").bootstrapTable({
			toolbar: '#toolbar',    
			clickToSelect: true,
			striped: true, //是否显示行间隔色
			cache: false, //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
			pagination: true, //是否显示分页（*）
			sidePagination: "client", //分页方式：client客户端分页，server服务端分页（*） 
			pageNumber: 1, //初始化加载第一页，默认第一页 
			pageSize: 5, //每页的记录行数（*） 
			pageList: [5, 10, 25, 50],
			search: true,
			sortable: true, //是否启用排序
			sortOrder: "asc",
			sortName: "userName",
			showColumns: true,
			minimumCountColumns: 2,
			columns: [{
				checkbox: true
			}, {
				field: 'userName',
				title: '姓名'
			}, {
				field: 'currentUnit',
				title: '所在单位'
			}, {
				field: 'jobTitle',
				title: '职称'
			}, {
				field: 'major',
				title: '专业'
			},{
				field: 'infoDetail',
				title: '详细信息'
			}, {
				field: 'edit',
				title: '编辑'
			}, {
				field: 'delete',
				title: '删除'
			}]
		});
	}

	$scope.setTable();
});