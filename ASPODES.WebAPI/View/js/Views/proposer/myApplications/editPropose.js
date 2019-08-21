var editProposeApp = angular.module('ngEditProposeApp',[]);
editProposeApp.controller('ngEditProposeCtrl',function($scope, $http){
	
	$scope.yusuanYear = 2017;
	
	$scope.setTable = function(){
					$("#inTable").bootstrapTable({
						url: '../../../data/inPrefessor.json',
						method: 'get',
						toolbar: "#inToolbar",
						striped: true,
						cache: false,
						pagination: true,
						sortable: true,
						sortOrder: "asc",
						sidePagination: "client",
						pageNumber: 1,
						pageSize: 5,
						pageList: [5, 10, 25, 50],
						showRefresh: true,
						clickToSelect: true,
						uniqueId: 'ID',
						showToggle: true,
						detailView: false,
						showExport: true,
						exportDataType: "selected",
						search: true,
						columns: [
						{
							title: '',
							field: 'select',
							radio : true,
							width: 20,
							align: 'center',
							valign: 'middle'
						},
						{
							title: '姓名',
							field: 'Name',
							sortable: false
						},
						{
							title: '单位',
							field: 'Danwei',
							sortable: false
						},
						{
							title: '身份证号',
							field: 'Idcard',
							sortable: false
						},
						{
							title: '性别',
							field: 'Sex',
							sortable: false
						},
						{
							title: '专业',
							field: 'Zhuanye',
							sortable: false
						},
						{
							title: '职称/职务',
							field: 'Zhicheng',
							sortable: false
						},
						{
							title: '责任分工',
							field: 'Fengong',
							sortable: false
						},
						{
							title: '电话',
							field: 'Phone',
							sortable: false
						}
						]
					});
					
					$("#outTable").bootstrapTable({
						url: '../../../data/outPrefessor.json',
						method: 'get',
						toolbar: "#outToolbar",
						striped: true,
						cache: false,
						pagination: true,
						sortable: true,
						sortOrder: "asc",
						sidePagination: "client",
						pageNumber: 1,
						pageSize: 5,
						pageList: [5, 10, 25, 50],
						showRefresh: true,
						clickToSelect: true,
						uniqueId: 'ID',
						showToggle: true,
						detailView: false,
						showExport: true,
						exportDataType: "selected",
						search: true,
						columns: [
						{
							title: '',
							field: 'select',
							radio : true,
							width: 20,
							align: 'center',
							valign: 'middle'
						},
						{
							title: '姓名',
							field: 'Name',
							sortable: false
						},
						{
							title: '单位',
							field: 'Danwei',
							sortable: false
						},
						{
							title: '身份证号',
							field: 'Idcard',
							sortable: false
						},
						{
							title: '性别',
							field: 'Sex',
							sortable: false
						},
						{
							title: '专业',
							field: 'Zhuanye',
							sortable: false
						},
						{
							title: '职称/职务',
							field: 'Zhicheng',
							sortable: false
						},
						{
							title: '责任分工',
							field: 'Fengong',
							sortable: false
						},
						{
							title: '电话',
							field: 'Phone',
							sortable: false
						}
						]
					});
					
					
					
					$("#yusuanFirstyearTable").bootstrapTable({
						url: '../../../data/yusuanFirst.json',
						method: 'get',
						toolbar: "#yusuanFirstyearToolbar",
						striped: true,
						cache: false,
						pagination: true,
						sortable: true,
						sortOrder: "asc",
						sidePagination: "client",
						pageNumber: 1,
						pageSize: 5,
						pageList: [5, 10, 25, 50],
						showRefresh: true,
						clickToSelect: true,
						uniqueId: 'ID',
						showToggle: true,
						detailView: false,
						showExport: true,
						exportDataType: "selected",
						search: true,
						editable: true,
						columns: [
						{
							title: '',
							field: 'select',
							radio : true,
							width: 20,
							align: 'center',
							valign: 'middle'
						},
						{
							title: '科目',
							field: 'Kemu',
							sortable: false,
							editable: {
								type: 'text',
								validate: function (v) {
			                        if (!v) return '科目名不能为空';
			                    }
							}
						},
						{
							title: '经费预算（万元）',
							field: 'Yusuan',
							sortable: false,
							editable: {
								type: 'number',
								step: 0.01
							}
						},
						{
							title: '简述预算测试依据与说明',
							field: 'Jianshu',
							sortable: false,
							editable: {
								type: 'text'
							}
						}
						],
					});
					
					
					
					$("#yusuanSecondyearTable").bootstrapTable({
						url: '../../../data/yusuanSecond.json',
						method: 'get',
						toolbar: "#yusuanSecondyearToolbar",
						striped: true,
						cache: false,
						pagination: true,
						sortable: true,
						sortOrder: "asc",
						sidePagination: "client",
						pageNumber: 1,
						pageSize: 5,
						pageList: [5, 10, 25, 50],
						showRefresh: true,
						clickToSelect: true,
						uniqueId: 'ID',
						showToggle: true,
						detailView: false,
						showExport: true,
						exportDataType: "selected",
						search: true,
						editable: true,
						columns: [
						{
							title: '',
							field: 'select',
							radio : true,
							width: 20,
							align: 'center',
							valign: 'middle'
						},
						{
							title: '科目',
							field: 'Kemu',
							sortable: false,
							editable: {
								type: 'text',
								validate: function (v) {
			                        if (!v) return '科目名不能为空';
			                    }
							}
						},
						{
							title: '经费预算（万元）',
							field: 'Yusuan',
							sortable: false,
							editable: {
								type: 'number',
								step: 0.01
							}
						},
						{
							title: '简述预算测试依据与说明',
							field: 'Jianshu',
							sortable: false,
							editable: {
								type: 'text'
							}
						}
						],
					});
					
					
					
					$("#yusuanThirdyearTable").bootstrapTable({
						url: '../../../data/yusuanThird.json',
						method: 'get',
						toolbar: "#yusuanThirdyearToolbar",
						striped: true,
						cache: false,
						pagination: true,
						sortable: true,
						sortOrder: "asc",
						sidePagination: "client",
						pageNumber: 1,
						pageSize: 5,
						pageList: [5, 10, 25, 50],
						showRefresh: true,
						clickToSelect: true,
						uniqueId: 'ID',
						showToggle: true,
						detailView: false,
						showExport: true,
						exportDataType: "selected",
						search: true,
						editable: true,
						columns: [
						{
							title: '',
							field: 'select',
							radio : true,
							width: 20,
							align: 'center',
							valign: 'middle'
						},
						{
							title: '科目',
							field: 'Kemu',
							sortable: false,
							editable: {
								type: 'text',
								validate: function (v) {
			                        if (!v) return '科目名不能为空';
			                    }
							}
						},
						{
							title: '经费预算（万元）',
							field: 'Yusuan',
							sortable: false,
							editable: {
								type: 'number',
								step: 0.01
							}
						},
						{
							title: '简述预算测试依据与说明',
							field: 'Jianshu',
							sortable: false,
							editable: {
								type: 'text'
							}
						}
						],
					});
					
					
					$("#gedanweiTable").bootstrapTable({
						url: '../../../data/gedanwei.json',
						method: 'get',
						toolbar: "#gedanweiToolbar",
						striped: true,
						cache: false,
						pagination: true,
						sortable: true,
						sortOrder: "asc",
						sidePagination: "client",
						pageNumber: 1,
						pageSize: 5,
						pageList: [5, 10, 25, 50],
						showRefresh: true,
						clickToSelect: true,
						uniqueId: 'ID',
						showToggle: true,
						detailView: false,
						showExport: true,
						exportDataType: "selected",
						search: true,
						editable: true,
						columns: [
						{
							title: '',
							field: 'select',
							radio : true,
							width: 20,
							align: 'center',
							valign: 'middle'
						},
						{
							title: '单位名称',
							field: 'Name',
							sortable: false,
							editable:{
								type: 'text',
								validate: function (v) {
			                        if (!v) return '单位名称名不能为空';
			                    }
							}
						},
						{
							title: '是否牵头单位',
							field: 'isQiantou',
							sortable: false,
						},
						{
							title: '预算总经费（万元）',
							field: 'Zongyusuan',
							sortable: false,
							editable: {
								type: 'number',
								step: 0.01
							}
						},
						{
							title: '2017年预算（万元）',
							field: 'First',
							sortable: false,
							editable: {
								type: 'number',
								step: 0.01
							}
						},
						{
							title: '2018年预算（万元）',
							field: 'Second',
							sortable: false,
							editable: {
								type: 'number',
								step: 0.01
							}
						},
						{
							title: '2019年预算（万元）',
							field: 'Third',
							sortable: false,
							editable: {
								type: 'number',
								step: 0.01
							}
						}
						],
					});
					
					
	}
	
	
	
				
	$scope.setTable();
	
	$("#yusuanFirstyear_btn_add").click(function(){
		$("#yusuanFirstyearTable").bootstrapTable('insertRow',{
			index: $("#yusuanFirstyearTable").bootstrapTable('getOptions').totalRows,
			row: {
				"Kemu": "科目",
				"Yusuan": 0.00,
				"Jianshu": "无"
			}
		});
	});
	
	
	$("#yusuanSecondyear_btn_add").click(function(){
		$("#yusuanSecondyearTable").bootstrapTable('insertRow',{
			index: $("#yusuanSecondyearTable").bootstrapTable('getOptions').totalRows,
			row: {
				"Kemu": "科目",
				"Yusuan": 0.00,
				"Jianshu": "无"
			}
		});
	});
	
	
	$("#yusuanThirdyear_btn_add").click(function(){
		$("#yusuanThirdyearTable").bootstrapTable('insertRow',{
			index: $("#yusuanThirdyearTable").bootstrapTable('getOptions').totalRows,
			row: {
				"Kemu": "科目",
				"Yusuan": 0.00,
				"Jianshu": "无"
			}
		});
	});
	
	
	$("#gedanwei_btn_add").click(function(){
		$("#gedanweiTable").bootstrapTable('insertRow',{
			index: $("#gedanweiTable").bootstrapTable('getOptions').totalRows,
			row: {
				"Name": "协作单位",
		        "isQiantou": "否",
		        "Zongyusuan": 0,
		        "First": 0,
		        "Second": 0,
		        "Third": 0
			}
		});
	});
	
	
	$("#centerSubmit").click(function(){
		alert("提交成功！即将为您关闭此窗口了！")
		window.close();
	});
	
});