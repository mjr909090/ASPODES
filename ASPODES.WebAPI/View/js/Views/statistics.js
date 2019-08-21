var statisticsApp = angular.module('ngStatisticsApp', []);
statisticsApp.controller('ngStatisticsCtrl', function ($scope, $http, $cookieStore) {


    // URL
    // 单位
    //单位管理员统计申请书
    //var instGetAppUrl = "/api/InstStatistic/GetAppStatistic";
    ////单位管理员统计在研项目
    //var instGetProjectUrl = "/api/InstStatistic/GetProjectStatistic";
    ////单位管理员按大类统计经费
    //var instGetFundByCateUrl = "/api/InstStatistic/GetFundStatisticByCate";
    ////单位管理员按大类和年份统计经费
    //var instGetFundByCateAndYearUrl = "/api/InstStatistic/GetFundStatisticByCateAndYear";
    ////单位管理员统计申请书资助率
    //var instGetFundRatioByCateAndYearUrl = "/api/InstStatistic/GetFundRatioStatisticByYearAndCate";
    ////单位管理员统计经费排名前十的项目
    //var instGetTopTenFundProjectUrl = "/api/InstStatistic/GetTop10FundProject";
    ////单位申请书总数和资助数
    //var instGetFundAndAcceptAppByCate = "/api/InstStatistic/GetFundAndAcceptAppByCate/2018";

    //// 院
    ////院管理员统计申请书
    //var deptGetAppUrl = "/api/DeptStatistic/GetAppStatistic";
    ////院管理员统计在研项目
    //var deptGetProjectUrl = "/api/DeptStatistic/GetProjectStatistic";
    ////院管理员统计某年总申请书和资助申请书
    //var deptGetFundByCateUrl = "/api/DeptStatistic/GetFundStatisticByCate";
    ////院管理员按大类和年份统计经费
    //var deptGetFundByCateAndYearUrl = "/api/DeptStatistic/GetFundStatisticByCateAndYear";
    ////院管理员统计申请书资助率
    //var deptGetFundRatioByCateAndYearUrl = "/api/DeptStatistic/GetFundRatioStatisticByYearAndCate";
    ////院管理员统计经费排名前十的项目
    //var deptGetTopTenFundProjectUrl = "/api/DeptStatistic/GetTop10FundProject";
    ////院管理员统计在研项目按年份分组
    //var deptGetProjectByYearUrl = "/api/DeptStatistic/GetProjectStatisticByYear/2016";
    ////院管理员统计在研项目通过单位分组
    //var deptGetProjectByInstUrl = "/api/DeptStatistic/GetProjectStatisticByInst";
    ////院管理员统计各个单位的经费
    //var deptGetFundByInstUrl = "/api/DeptStatistic/GetFundStatisticByInst";
    ////院管理员统计申请书总数和资助数
    //var deptGetFundAndAcceptAppByCateUrl = "/api/DeptStatistic/GetFundAndAcceptAppByCate/2018";

    //数据获取json版
    // 单位
    var instGetAppUrl = "/View/json/InstGetAppStatistic(c1).json";
    var instGetProjectUrl = "/View/json/InstGetProjectStatistic(c2).json";
    var instGetFundByCateUrl = "/View/json/InstGetFundStatisticByCate(c3).json";
    var instGetFundByCateAndYearUrl = "/View/json/InstGetFundStatisticByCateAndYear(c4).json";
    var instGetFundRatioByCateAndYearUrl = "/View/json/InstGetFundRatioStatisticByYearAndCate(c5).json";
    var instGetTopTenFundProjectUrl = "/View/json/InstGetTop10FundProject(c6).json";
    var instGetFundAndAcceptAppByCateUrl = "/View/json/InstGetFundAndAcceptAppByCate(c16).json";
    // 院
    var deptGetAppUrl = "/View/json/DeptGetAppStatistic(c7).json";
    var deptGetProjectUrl = "/View/json/DeptGetProjectStatistic(c8).json";
    var deptGetFundByCateUrl = "/View/json/DeptGetFundStatisticByCate(c9).json";
    var deptGetFundByCateAndYearUrl = "/View/json/DeptGetFundStatisticByCateAndYear(c10).json";
    var deptGetFundRatioByCateAndYearUrl = "/View/json/DeptGetFundRatioStatisticByYearAndCate(c11).json";
    var deptGetTopTenFundProjectUrl = "/View/json/DeptGetTop10FundProject(c12).json";
    var deptGetProjectByYearUrl = "/View/json/DeptGetProjectStatisticByYear(c13).json";
    var deptGetProjectByInstUrl = "/View/json/DeptGetProjectStatisticByInst(14).json";
    var deptGetFundByInstUrl = "/View/json/DeptGetFundStatisticByInst(c15).json";
    var deptGetFundAndAcceptAppByCateUrl = "/View/json/DeptGetFundAndAcceptAppByCate(c17).json";

    // 定义
    // 单位
    $scope.instTopTenFundProjects = [];
    // 院
    $scope.deptTopTenFundProjects = [];

    $scope.extendBlurIsHide = true;


    //转置json
    $scope.changeJson = function (jsonData) {
        var _this = this;

        _this.jsonKeys = _.keys(jsonData[0]);
        _this.changedJson = {};

        angular.forEach(_this.jsonKeys, function (keyItem) {
            _this.str = "_this.changedJson." + keyItem + "= []";
            eval(_this.str);
        })

        //console.log(_this.changedJson);

       angular.forEach(jsonData, function (jsonDataItem, index, array) {
           angular.forEach(jsonDataItem, function (value, key) {
               _this._str = "_this.changedJson." + key + ".push(" + value + ")";
               eval(_this._str);
           })
       })

       //console.log(_this.changedJson);

       return _this.changedJson;
    }


    //判断某一元素是否在数组中
    Array.prototype.contains = function (obj) {
        var i = this.length;
        while (i--) {
            if (this[i] === obj) {
                return true;
            }
        }
        return false;
    }

    //用户角色信息状态（针对院管理员和单位管理员）
    $scope.selectRoles = function () {
        if ($cookieStore.get('role') != null) {
            $scope.roles = $cookieStore.get('role');
            $scope.role.divisionAdmin = $scope.roles.contains('单位管理员');
            $scope.role.superAdmin = $scope.roles.contains('院管理员');
            $scope.roleSelects = angular.copy($scope.role);
        }

        //初始化ng-switch
        if ($scope.roleSelects.divisionAdmin == true) {
            $scope.current = 1;

            //初始化获取单位数据
            $scope.instGetAppOption();
            $scope.instGetProjectOption();
            $scope.instGetFundByCateOption();
            $scope.instGetFundByCateAndYearOption();
            $scope.instGetFundRatioByCateAndYearOption();
            $scope.instGetTopTenFundProject();
            $scope.instGetFundAndAcceptAppByCateOption();


        }
        else if ($scope.roleSelects.divisionAdmin == false && $scope.roleSelects.superAdmin == true) {
            $scope.current = 2;


            // 初始化院数据
            $scope.deptGetAppOption();
            $scope.deptGetProjectOption();
            $scope.deptGetFundByCateOption();
            $scope.deptGetFundByCateAndYearOption();
            $scope.deptGetFundRatioByCateAndYearOption();
            $scope.deptGetTopTenFundProject();
            $scope.deptGetProjectByYearOption();
            $scope.deptGetProjectByInstOption();
            $scope.deptGetFundByInstOption();
            $scope.deptGetFundAndAcceptAppByCateOption();

        }

    }




    //配置图形和表格显示
    $scope.chartOrTable = {
        /*单位*/
        instGetAppChart: 'chart',
        instGetProjectChart: 'chart',
        instGetFundByCateChart: 'chart',
        instGetFundByCateAndYearChart: 'chart',
        instGetFundRatioByCateAndYearChart: 'chart',
        instGetFundAndAcceptAppByCateChart: 'chart',
        /*院*/
        deptGetAppChart: 'chart',
        deptGetProjectChart: 'chart',
        deptGetFundByCateChart: 'chart',
        deptGetProjectByYearChart: 'chart',
        deptGetFundRatioByCateAndYearChart: 'chart',
        deptGetFundByCateAndYearChart: 'chart',
        deptGetProjectByInstChart: 'chart',
        deptGetFundByInstChart: 'chart',
        deptGetFundAndAcceptAppByCateChart: 'chart'
    }



    // 图表绑定
    // 单位
    var instGetAppChart = echarts.init(document.getElementById('instGetAppChart'));
    var instGetProjectChart = echarts.init(document.getElementById('instGetProjectChart'));
    var instGetFundByCateChart = echarts.init(document.getElementById('instGetFundByCateChart'));
    var instGetFundByCateAndYearChart = echarts.init(document.getElementById('instGetFundByCateAndYearChart'));
    var instGetFundRatioByCateAndYearChart = echarts.init(document.getElementById('instGetFundRatioByCateAndYearChart'));
    var instGetFundAndAcceptAppByCateChart = echarts.init(document.getElementById('instGetFundAndAcceptAppByCateChart'));
    // 院
    var deptGetAppChart = echarts.init(document.getElementById('deptGetAppChart'));
    var deptGetProjectChart = echarts.init(document.getElementById('deptGetProjectChart'));
    var deptGetFundByCateChart = echarts.init(document.getElementById('deptGetFundByCateChart'));
    var deptGetFundByCateAndYearChart = echarts.init(document.getElementById('deptGetFundByCateAndYearChart'));
    var deptGetFundRatioByCateAndYearChart = echarts.init(document.getElementById('deptGetFundRatioByCateAndYearChart'));
    var deptGetProjectByYearChart = echarts.init(document.getElementById('deptGetProjectByYearChart'));
    var deptGetProjectByInstChart = echarts.init(document.getElementById('deptGetProjectByInstChart'));
    var deptGetFundByInstChart = echarts.init(document.getElementById('deptGetFundByInstChart'));
    var deptGetFundAndAcceptAppByCateChart = echarts.init(document.getElementById('deptGetFundAndAcceptAppByCateChart'));


    // 图表样式
    // 八大类柱状图样式
    var statisticsByCateBarOption = {
        title: {
            text: {},
            subtext: {},
            textStyle: {
                fontWeight: 'bold',
                fontSize: 18,
            },
            left: 'center'
        },
        legend: {
            itemWidth: 10,
            itemHeight: 10,
            orient: 'vertical',
            y: 'center',
            x: '80%',
            backgroundColor: 'rgba(255, 255, 255, 0.3)',
            padding: 10,
            align: 'left',
        },
        calculable: true,
        grid: {
            bottom: '20%',
            top: '15%',
            right: '25%',
            left: '5%'
        },
        xAxis: [
            {
                type: 'category',
                axisLabel: {
                    interval: 0,
                    rotate: 45,//倾斜度 -90 至 90 默认为0
                    margin: 2,
                    textStyle: {
                        fontWeight: "bolder",
                        color: "#000000",
                        fontSize: '10'
                    }
                },
            }
        ],
        yAxis: [
            {
                type: 'value',
            }
        ],
        series: [
            {
                type: 'bar',
            },
            {
                type: 'bar',
            },
            {
                type: 'bar',
                barGap: '0%',
            }
        ],
        color: ['#37A2DA', '#32C5E9', '#67E0E3'],
        emphasis: {
            itemStyle: {
                color: '#9FE6B8'
            },
        }
    };
    // 八大类总数资助数比较柱状图样式
    var statisticsFundRatioByCateBarOption = {
        title: {
            text: {},
            subtext: {},
            textStyle: {
                fontWeight: 'bold',
                fontSize: 18,
            },
            left: 'center'
        },
        legend: {
            itemWidth: 10,
            itemHeight: 10,
            orient: 'vertical',
            y: 'center',
            x: '80%',
            backgroundColor: 'rgba(255, 255, 255, 0.3)',
            padding: 10,
            align: 'left',
        },
        calculable: true,
        grid: {
            bottom: '20%',
            top: '15%',
            right: '25%',
            left: '5%'
        },
        xAxis: [
            {
                type: 'category',
                axisLabel: {
                    interval: 0,
                    rotate: 45,//倾斜度 -90 至 90 默认为0
                    margin: 2,
                    textStyle: {
                        fontWeight: "bolder",
                        color: "#000000",
                        fontSize: '10'
                    }
                },
            }
        ],
        yAxis: [
            {
                type: 'value',
            }
        ],
        series: [
            {
                type: 'bar',
            },
            {
                type: 'bar',
                barGap: '0%',
            }
        ],
        color: ['#37A2DA', '#32C5E9', '#67E0E3'],
        emphasis: {
            itemStyle: {
                color: '#9FE6B8'
            },
        }
    };
    // 八大类饼图
    var statisticsByCatePieOption = {
        title: {
            text: {},
            subtext: {},
            textStyle: {
                fontWeight: 'bold',
                fontSize: 18,
            },
            left: 'center'
        },
        legend: {
            itemWidth: 10,
            itemHeight: 10,
            orient: 'vertical',
            y: 'center',
            x: '70%',
            backgroundColor: 'rgba(255, 255, 255, 0.3)',
            padding: 10,
            align: 'left',
        },
        grid: {
            right: '15%'
        },
        calculable: true,
        series: [
            {
                center: ['35%', '50%'],
                type: 'pie',
                radius: [100, 130],
                label: {
                    normal: {
                        show: true
                    }
                },
                color: ['#37A2DA', '#32C5E9', '#67E0E3', '#9FE6B8', '#FFDB5C', '#ff9f7f', '#fb7293', '#E062AE'],
                itemStyle: {
                    normal: {
                        shadowBlur: 50,
                        shadowColor: 'rgba(0, 0, 0, 0.2)'
                    }
                },
                emphasis: {
                    label: {
                        show: true
                    },
                    shadowBlur: 200,
                    shadowColor: 'rgba(0, 0, 0, 0.5)'
                }
            }
        ]
    };
    // 八大类折线图
    var statisticsByCateLineOption = {
        title: {
            text: {},
            subtext: {},
            textStyle: {
                fontWeight: 'bold',
                fontSize: 18,
            },
            left: 'center'
        },
        legend: {
            itemWidth: 10,
            itemHeight: 7,
            orient: 'vertical',
            x: '72%',
            right: '10%',
            y: 'center',
            backgroundColor: 'rgba(255, 255, 255, 0.3)',
            textStyle: {
                fontSize: 10,
            },
        },
        tooltip: {
            trigger: 'axis',
            showContent: false
        },
        xAxis: { type: 'category' },
        yAxis: { gridIndex: 0 },
        grid: {
            left: '5%',
            right: '30%',
        },
        series: [
            { type: 'line', smooth: true },
            { type: 'line', smooth: true },
            { type: 'line', smooth: true },
            { type: 'line', smooth: true },
            { type: 'line', smooth: true },
            { type: 'line', smooth: true },
            { type: 'line', smooth: true },
            { type: 'line', smooth: true },
        ]
    };
    // 按单位分类饼图
    var statisticsByInstPieOption = {
        title: {
            text: {},
            subtext: {},
            textStyle: {
                fontWeight: 'bold',
                fontSize: 18,
            },
            left: 'center'
        },
        legend: {
            itemWidth: 10,
            itemHeight: 10,
            width: '70%',
            height: '70%',
            orient: 'vertical',
            y: 'center',
            x: '60%',
            backgroundColor: 'rgba(255, 255, 255, 0.3)',
            padding: 10,
            align: 'left',
        },
        grid: {
            right: '15%'
        },
        calculable: true,
        series: [
            {
                center: ['30%', '50%'],
                type: 'pie',
                radius: [25, 130],
                roseType: 'radius',
                label: {
                    normal: {
                        show: false
                    }
                },
                color: ['#37A2DA', '#32C5E9', '#67E0E3', '#9FE6B8', '#FFDB5C', '#ff9f7f', '#fb7293', '#E062AE', '#E690D1', '#e7bcf3', '#9d96f5', '#8378EA', '#96BFFF'],
                calculable: true,
                itemStyle: {
                    normal: {
                        shadowBlur: 50,
                        shadowColor: 'rgba(0, 0, 0, 0.2)'
                    }
                },
                emphasis: {
                    label: {
                        show: true
                    },
                    shadowBlur: 200,
                    shadowColor: 'rgba(0, 0, 0, 0.5)'
                }
            }
        ]
    };


    // 图表样式加载
    // 单位
    instGetAppChart.setOption(statisticsByCateBarOption);
    instGetProjectChart.setOption(statisticsByCatePieOption);
    instGetFundByCateChart.setOption(statisticsByCatePieOption);
    instGetFundByCateAndYearChart.setOption(statisticsByCateLineOption);
    instGetFundRatioByCateAndYearChart.setOption(statisticsByCateLineOption);
    instGetFundAndAcceptAppByCateChart.setOption(statisticsFundRatioByCateBarOption);

    // 院
    deptGetAppChart.setOption(statisticsByCateBarOption);
    deptGetProjectChart.setOption(statisticsByCatePieOption);
    deptGetFundByCateChart.setOption(statisticsByCatePieOption);
    deptGetFundByCateAndYearChart.setOption(statisticsByCateLineOption);
    deptGetFundRatioByCateAndYearChart.setOption(statisticsByCateLineOption);
    deptGetProjectByYearChart.setOption(statisticsByCateLineOption);
    deptGetProjectByInstChart.setOption(statisticsByInstPieOption);
    deptGetFundByInstChart.setOption(statisticsByInstPieOption);
    deptGetFundAndAcceptAppByCateChart.setOption(statisticsFundRatioByCateBarOption);

    window.addEventListener("resize", function () {
        //单位
        instGetAppChart.resize();
        instGetProjectChart.resize();
        instGetFundByCateChart.resize();
        instGetFundByCateAndYearChart.resize();
        instGetFundRatioByCateAndYearChart.resize();
        instGetFundAndAcceptAppByCateChart.resize();
        //院
        deptGetAppChart.resize();
        deptGetProjectChart.resize();
        deptGetFundByCateChart.resize();
        deptGetFundByCateAndYearChart.resize();
        deptGetFundRatioByCateAndYearChart.resize();
        deptGetProjectByYearChart.resize();
        deptGetProjectByInstChart.resize();
        deptGetFundByInstChart.resize();
        deptGetFundAndAcceptAppByCateChart.resize();
    });

    $scope.changeApplicationType = function (obj) {
        var data = JSON.parse(JSON.stringify(obj).replace(/Checking/g, "待审核"));
        data = JSON.parse(JSON.stringify(data).replace(/Reject/g, "驳回"));
        data = JSON.parse(JSON.stringify(data).replace(/Accept/g, "通过"));
        return data;
    };

    $scope.changeApplicationFundRatioType = function (obj) {
        var data = JSON.parse(JSON.stringify(obj).replace(/FundNumber/g, "资助数"));
        data = JSON.parse(JSON.stringify(data).replace(/AcceptNumber/g, "总数"));
        return data;
    };

    $scope.changeCategroyType = function (obj) {
        var data = JSON.parse(JSON.stringify(obj).replace(/AllianceFocusWork/g, "联盟重点工作"));
        data = JSON.parse(JSON.stringify(data).replace(/EmergencyResearchWork/g, "应急性科研工作"));
        data = JSON.parse(JSON.stringify(data).replace(/BasicScienceAndTechnologyWork/g, "科技基础工作"));
        data = JSON.parse(JSON.stringify(data).replace(/BasicResearchGuidancePlan/g, "基础研究引导计划"));
        data = JSON.parse(JSON.stringify(data).replace(/MajorResultsDevelopmentProgram/g, "重大成果培养计划"));
        data = JSON.parse(JSON.stringify(data).replace(/MajorPlatformPromotionPlan/g, "重大平台推进计划"));
        data = JSON.parse(JSON.stringify(data).replace(/MajorProjectReservePlan/g, "重大项目储备计划"));
        data = JSON.parse(JSON.stringify(data).replace(/AgriculturalThinkTankConstructionPlan/g, "农业智库建设计划"));
        return data;
    };

    // chart1 八大类柱状图
    $scope.instGetAppOption = function () {
        $http({
            method: 'GET',
            url: instGetAppUrl
        })
            .then(function successCallback(response) {
                if (response.data.response != '') {
                    // 对应图表新增样式
                    instGetAppChart.hideLoading();
                    $scope.instGetAppData = response.data.response;
                    $scope.instGetAppChartData = $scope.changeApplicationType(response.data.response);
                    $scope.instGetAppOption = {
                        title: {
                            text: '2018年度提交申请书状态数量',
                            subtext: '单位管理员'
                        },
                        dataset: {
                            sourceHeader: false,
                            source: $scope.instGetAppChartData,
                        },
                    };
                    instGetAppChart.setOption($scope.instGetAppOption);
                }
                else {
                    // 对应图表新增样式
                    instGetAppChart.hideLoading();
                    $scope.instGetAppData = [];
                    $scope.instGetAppOption = {
                        title: {
                            text: '2018年度提交申请书状态数量',
                            subtext: '单位管理员'
                        },
                        legend: {
                            backgroundColor: 'rgb(0,0,0,0)',
                        }
                    };
                    instGetAppChart.setOption($scope.instGetAppOption);
                }
            }, function errorCallback(response) {
            });
    };

    // chart2 八大类饼图
    $scope.instGetProjectOption = function () {
        $http({
            method: 'GET',
            url: instGetProjectUrl
        })
            .then(function successCallback(response) {
                if (response.data.response != '') {
                    // 对应图表新增样式
                    instGetProjectChart.hideLoading();
                    $scope.instGetProjectData = response.data.response;
                    $scope.instGetProjectOption = {
                        title: {
                            text: '2018年度在研项目分布',
                            subtext: '单位管理员'
                        },
                        dataset: {
                            sourceHeader: false,
                            source: response.data.response,
                        },
                    };
                    instGetProjectChart.setOption($scope.instGetProjectOption);
                }
                else {
                    instGetProjectChart.hideLoading();
                    $scope.instGetProjectData = [];
                    $scope.instGetProjectOption = {
                        title: {
                            text: '2018年度在研项目分布',
                            subtext: '单位管理员'
                        },
                        legend: {
                            backgroundColor: 'rgb(0,0,0,0)',
                        }
                    };
                    instGetProjectChart.setOption($scope.instGetProjectOption);
                }
            }, function errorCallback(response) {

            });
    };

    // chart3 八大类饼图
    $scope.instGetFundByCateOption = function () {
        $http({
            method: 'GET',
            url: instGetFundByCateUrl
        })
            .then(function successCallback(response) {
                if (response.data.response != '') {
                    instGetFundByCateChart.hideLoading();
                    // 对应图表新增样式
                    $scope.instGetFundByCateData = response.data.response;
                    $scope.instGetFundByCateOption = {
                        title: {
                            text: '2018年度各类项目经费分配',
                            subtext: '单位管理员'
                        },
                        dataset: {
                            sourceHeader: false,
                            source: response.data.response,
                        },
                    };
                    instGetFundByCateChart.setOption($scope.instGetFundByCateOption);
                }
                else {
                    instGetFundByCateChart.hideLoading();
                    // 对应图表新增样式
                    $scope.instGetFundByCateData = [];
                    $scope.instGetFundByCateOption = {
                        title: {
                            text: '2018年度各类项目经费分配',
                            subtext: '单位管理员'
                        },
                        legend: {
                            backgroundColor: 'rgb(0,0,0,0)',
                        }
                    };
                    instGetFundByCateChart.setOption($scope.instGetFundByCateOption);
                }
            }, function errorCallback(response) {
            });
    };

    // chart4 八大类折线图
    $scope.instGetFundByCateAndYearOption = function () {
        $http({
            method: 'GET',
            url: instGetFundByCateAndYearUrl
        })
            .then(function successCallback(response) {
                if (response.data.response != '') {

                    //test function changedJson
                    $scope.instGetFundByCateAndYearData = $scope.changeJson(response.data.response);
                    //console.log($scope.instGetFundByCateAndYearData);

                    instGetFundByCateAndYearChart.hideLoading();
                    // 对应图表新增样式
                    $scope.instGetFundByCateAndYearData = $scope.changeJson(response.data.response);
                    $scope.instGetFundByCateAndYearOption = {
                        title: {
                            text: '历年各类项目经费分配趋势',
                            subtext: '单位管理员'
                        },
                        dataset: {
                            sourceHeader: false,
                            source: $scope.changeCategroyType(response.data.response),
                        },
                    };
                    instGetFundByCateAndYearChart.setOption($scope.instGetFundByCateAndYearOption);
                }
                else {
                    instGetFundByCateAndYearChart.hideLoading();
                    // 对应图表新增样式
                    $scope.instGetFundByCateAndYearData = [];
                    $scope.instGetFundByCateAndYearOption = {
                        title: {
                            text: '历年各类项目经费分配趋势',
                            subtext: '单位管理员'
                        },
                        legend: {
                            backgroundColor: 'rgb(0,0,0,0)',
                        }
                    };
                    instGetFundByCateAndYearChart.setOption($scope.instGetFundByCateAndYearOption);
                }
            }, function errorCallback(response) {

            });
    };

    // chart5 八大类折线图
    $scope.instGetFundRatioByCateAndYearOption = function () {
        $http({
            method: 'GET',
            url: instGetFundRatioByCateAndYearUrl
        })
            .then(function successCallback(response) {
                if (response.data.response != '') {
                    // 对应图表新增样式
                    instGetFundRatioByCateAndYearChart.hideLoading();
                    $scope.instGetFundRatioByCateAndYearData = $scope.changeJson(response.data.response);
                    $scope.instGetFundRatioByCateAndYearOption = {
                        title: {
                            text: '历年申请书资助率趋势',
                            subtext: '单位管理员'
                        },
                        grid: {
                            left: "7%"
                        },
                        yAxis: [{
                            axisLabel: {
                                show: true,
                                interval: 'auto',
                                formatter: {}
                            }
                        }],
                        dataset: {
                            sourceHeader: false,
                            source: $scope.changeCategroyType(response.data.response),
                        },
                    };
                    instGetFundRatioByCateAndYearChart.setOption($scope.instGetFundRatioByCateAndYearOption);
                }
                else {
                    // 对应图表新增样式
                    instGetFundRatioByCateAndYearChart.hideLoading();
                    $scope.instGetFundRatioByCateAndYearData = [];
                    $scope.instGetFundRatioByCateAndYearOption = {
                        title: {
                            text: '历年各类项目经费分配趋势',
                            subtext: '单位管理员'
                        },
                        grid: {
                            left: "7%"
                        },
                        yAxis: [{
                            axisLabel: {
                                show: true,
                                interval: 'auto',
                                formatter: {}
                            }
                        }],
                        legend: {
                            backgroundColor: 'rgb(0,0,0,0)',
                        }
                    };
                    instGetFundRatioByCateAndYearChart.setOption($scope.instGetFundRatioByCateAndYearOption);
                }
            }, function errorCallback(response) {

            });
    };

    // chart6 表格
    $scope.instGetTopTenFundProject = function () {
        $http({
            method: 'GET',
            url: instGetTopTenFundProjectUrl
        })
            .then(function successCallback(response) {
                $scope.instTopTenFundProjects = response.data.response;
            }, function errorCallback(response) {

            });
    };

    // chart16
    $scope.instGetFundAndAcceptAppByCateOption = function () {
        $http({
            method: 'GET',
            url: instGetFundAndAcceptAppByCateUrl
        })
            .then(function successCallback(response) {
                if (response.data.response != '') {
                    // 对应图表新增样式
                    instGetFundAndAcceptAppByCateChart.hideLoading();
                    $scope.instGetFundAndAcceptAppByCateData = $scope.changeApplicationFundRatioType(response.data.response);
                    $scope.instGetFundAndAcceptAppByCateChartData = response.data.response;
                    $scope.instGetFundAndAcceptAppByCateOption = {
                        title: {
                            text: '2018年度提交申请书总数和资助数',
                            subtext: '单位管理员'
                        },
                        dataset: {
                            sourceHeader: false,
                            source: $scope.instGetFundAndAcceptAppByCateData,
                        },
                    };
                    instGetFundAndAcceptAppByCateChart.setOption($scope.instGetFundAndAcceptAppByCateOption);
                }
                else {
                    // 对应图表新增样式
                    instGetFundAndAcceptAppByCateChart.hideLoading();
                    $scope.instGetFundAndAcceptAppByCateData = [];
                    $scope.instGetFundAndAcceptAppByCateOption = {
                        title: {
                            text: '2018年度提交申请书总数和资助数',
                            subtext: '单位管理员'
                        },
                        legend: {
                            backgroundColor: 'rgb(0,0,0,0)',
                        }
                    };
                    instGetFundAndAcceptAppByCateChart.setOption($scope.instGetFundAndAcceptAppByCateOption);
                }
            }, function errorCallback(response) {
            });
    };


    // chart7 八大类柱状图
    $scope.deptGetAppOption = function () {
        $http({
            method: 'GET',
            url: deptGetAppUrl
        })
            .then(function successCallback(response) {
                if (response.data.response != '') {
                    // 对应图表新增样式
                    deptGetAppChart.hideLoading();
                    $scope.deptGetAppData = response.data.response;
                    $scope.deptGetAppOption = {
                        title: {
                            text: '2018年度提交申请书状态数量',
                            subtext: '院管理员'
                        },
                        dataset: {
                            sourceHeader: false,
                            source: $scope.changeApplicationType(response.data.response),
                        },
                    };
                    deptGetAppChart.setOption($scope.deptGetAppOption);
                }
                else {
                    // 对应图表新增样式
                    deptGetAppChart.hideLoading();
                    $scope.deptGetAppData = [];
                    $scope.deptGetAppOption = {
                        title: {
                            text: '2018年度提交申请书状态数量',
                            subtext: '院管理员'
                        },
                        legend: {
                            backgroundColor: 'rgb(0,0,0,0)',
                        }
                    };
                    deptGetAppChart.setOption($scope.deptGetAppOption);
                }
            }, function errorCallback(response) {

            });
    };

    // chart8 八大类饼图
    $scope.deptGetProjectOption = function () {
        $http({
            method: 'GET',
            url: deptGetProjectUrl
        })
            .then(function successCallback(response) {
                if (response.data.response != '') {
                    // 对应图表新增样式
                    deptGetProjectChart.hideLoading();
                    $scope.deptGetProjectData = response.data.response;
                    $scope.deptGetProjectOption = {
                        title: {
                            text: '2018年度在研项目分布',
                            subtext: '院管理员'
                        },
                        dataset: {
                            sourceHeader: false,
                            source: response.data.response,
                        },
                    };
                    deptGetProjectChart.setOption($scope.deptGetProjectOption);
                }
                else {
                    // 对应图表新增样式
                    deptGetProjectChart.hideLoading();
                    $scope.deptGetProjectData = [];
                    $scope.deptGetProjectOption = {
                        title: {
                            text: '2018年度在研项目分布',
                            subtext: '院管理员'
                        },
                        legend: {
                            backgroundColor: 'rgb(0,0,0,0)',
                        }
                    };
                    deptGetProjectChart.setOption($scope.deptGetProjectOption);
                }
            }, function errorCallback(response) {

            });
    };

    // chart9 八大类饼图
    $scope.deptGetFundByCateOption = function () {
        $http({
            method: 'GET',
            url: deptGetFundByCateUrl
        })
            .then(function successCallback(response) {
                if (response.data.response != '') {
                    // 对应图表新增样式
                    deptGetFundByCateChart.hideLoading();
                    $scope.deptGetFundByCateData = response.data.response;
                    $scope.deptGetFundByCateOption = {
                        title: {
                            text: '2018年度各类项目经费分配',
                            subtext: '院管理员'
                        },
                        dataset: {
                            sourceHeader: false,
                            source: response.data.response,
                        },
                    };
                    deptGetFundByCateChart.setOption($scope.deptGetFundByCateOption);
                }
                else {
                    // 对应图表新增样式
                    deptGetFundByCateChart.hideLoading();
                    $scope.deptGetFundByCateData = [];
                    $scope.deptGetFundByCateOption = {
                        title: {
                            text: '2018年度各类项目经费分配',
                            subtext: '院管理员'
                        },
                        legend: {
                            backgroundColor: 'rgb(0,0,0,0)',
                        }
                    };
                    deptGetFundByCateChart.setOption($scope.deptGetFundByCateOption);
                }
            }, function errorCallback(response) {

            });
    };

    // chart10 八大类折线图
    $scope.deptGetFundByCateAndYearOption = function () {
        $http({
            method: 'GET',
            url: deptGetFundByCateAndYearUrl
        })
            .then(function successCallback(response) {
                if (response.data.response != '') {
                    // 对应图表新增样式
                    deptGetFundByCateAndYearChart.hideLoading();
                    $scope.deptGetFundByCateAndYearData = $scope.changeJson(response.data.response);
                    $scope.deptGetFundByCateAndYearOption = {
                        title: {
                            text: '历年各类项目经费分配趋势',
                            subtext: '院管理员'
                        },
                        dataset: {
                            sourceHeader: false,
                            source: $scope.changeCategroyType(response.data.response),
                        },
                    };
                    deptGetFundByCateAndYearChart.setOption($scope.deptGetFundByCateAndYearOption);
                }
                else {
                    // 对应图表新增样式
                    deptGetFundByCateAndYearChart.hideLoading();
                    $scope.deptGetFundByCateAndYearData = [];
                    $scope.deptGetFundByCateAndYearOption = {
                        title: {
                            text: '历年各类项目经费分配趋势',
                            subtext: '院管理员'
                        },
                        legend: {
                            backgroundColor: 'rgb(0,0,0,0)',
                        }
                    };
                    deptGetFundByCateAndYearChart.setOption($scope.deptGetFundByCateAndYearOption);
                }
            }, function errorCallback(response) {

            });
    };

    // chart11 八大类折线图
    $scope.deptGetFundRatioByCateAndYearOption = function () {
        $http({
            method: 'GET',
            url: deptGetFundRatioByCateAndYearUrl
        })
            .then(function successCallback(response) {
                if (response.data.response != '') {
                    // 对应图表新增样式
                    deptGetFundRatioByCateAndYearChart.hideLoading();
                    $scope.deptGetFundRatioByCateAndYearData = $scope.changeJson(response.data.response);
                    $scope.deptGetFundRatioByCateAndYearOption = {
                        title: {
                            text: '历年申请书资助率趋势',
                            subtext: '院管理员'
                        },
                        grid: {
                            left: "7%"
                        },
                        yAxis: [{
                            axisLabel: {
                                show: true,
                                interval: 'auto',
                                formatter: {}
                            }
                        }],
                        dataset: {
                            sourceHeader: false,
                            source: $scope.changeCategroyType(response.data.response),
                        },
                    };
                    deptGetFundRatioByCateAndYearChart.setOption($scope.deptGetFundRatioByCateAndYearOption);
                }
                else {
                    // 对应图表新增样式
                    deptGetFundRatioByCateAndYearChart.hideLoading();
                    $scope.deptGetFundRatioByCateAndYearData = [];
                    $scope.deptGetFundRatioByCateAndYearOption = {
                        title: {
                            text: '历年申请书资助率趋势',
                            subtext: '院管理员'
                        },
                        legend: {
                            backgroundColor: 'rgb(0,0,0,0)',
                        }
                    };
                    deptGetFundRatioByCateAndYearChart.setOption($scope.deptGetFundRatioByCateAndYearOption);
                }
            }, function errorCallback(response) {

            });
    };

    // chart12 表格
    $scope.deptGetTopTenFundProject = function () {
        $http({
            method: 'GET',
            url: deptGetTopTenFundProjectUrl
        })
            .then(function successCallback(response) {
                $scope.deptTopTenFundProjects = response.data.response;
            }, function errorCallback(response) {

            });
    };

    // chart13 八大类折线图
    $scope.deptGetProjectByYearOption = function () {
        $http({
            method: 'GET',
            url: deptGetProjectByYearUrl
        })
            .then(function successCallback(response) {
                if (response.data.response != '') {
                    // 对应图表新增样式
                    deptGetProjectByYearChart.hideLoading();
                    $scope.deptGetProjectByYearData = $scope.changeJson(response.data.response);
                    $scope.deptGetProjectByYearOption = {
                        title: {
                            text: '历年各类项目立项趋势',
                            subtext: '院管理员'
                        },
                        dataset: {
                            sourceHeader: false,
                            source: $scope.changeCategroyType(response.data.response),
                        },
                    };
                    deptGetProjectByYearChart.setOption($scope.deptGetProjectByYearOption);
                }
                else {
                    // 对应图表新增样式
                    deptGetProjectByYearChart.hideLoading();
                    $scope.deptGetProjectByYearData = [];
                    $scope.deptGetProjectByYearOption = {
                        title: {
                            text: '历年各类项目立项趋势',
                            subtext: '院管理员'
                        },
                        legend: {
                            backgroundColor: 'rgb(0,0,0,0)',
                        }
                    };
                    deptGetProjectByYearChart.setOption($scope.deptGetProjectByYearOption);
                }
            }, function errorCallback(response) {

            });
    };

    // chart14 按单位分类饼图
    $scope.deptGetProjectByInstOption = function () {
        $http({
            method: 'GET',
            url: deptGetProjectByInstUrl
        })
            .then(function successCallback(response) {
                if (response.data.response != '') {
                    // 对应图表新增样式
                    deptGetProjectByInstChart.hideLoading();
                    $scope.deptGetProjectByInstData = response.data.response;
                    $scope.deptGetProjectByInstOption = {
                        title: {
                            text: '2018年度各单位在研项目数量分布',
                            subtext: '院管理员'
                        },
                        dataset: {
                            sourceHeader: false,
                            source: response.data.response,
                        },
                    };
                    deptGetProjectByInstChart.setOption($scope.deptGetProjectByInstOption);
                }
                else {
                    // 对应图表新增样式
                    deptGetProjectByInstChart.hideLoading();
                    $scope.deptGetProjectByInstData = [];
                    $scope.deptGetProjectByInstOption = {
                        title: {
                            text: '2018年度各单位在研项目数量分布',
                            subtext: '院管理员'
                        },
                        legend: {
                            backgroundColor: 'rgb(0,0,0,0)',
                        }
                    };
                    deptGetProjectByInstChart.setOption($scope.deptGetProjectByInstOption);
                }
            }, function errorCallback(response) {

            });
    };

    // chart15 按单位分类饼图
    $scope.deptGetFundByInstOption = function () {
        $http({
            method: 'GET',
            url: deptGetFundByInstUrl
        })
            .then(function successCallback(response) {
                if (response.data.response != '') {
                    // 对应图表新增样式
                    deptGetFundByInstChart.hideLoading();
                    $scope.deptGetFundByInstData = response.data.response;
                    $scope.deptGetFundByInstOption = {
                        title: {
                            text: '2018年度各单位经费分配(W)',
                            subtext: '院管理员'
                        },
                        dataset: {
                            sourceHeader: false,
                            source: response.data.response,
                        },
                    };
                    deptGetFundByInstChart.setOption($scope.deptGetFundByInstOption);
                }
                else {
                    // 对应图表新增样式
                    deptGetFundByInstChart.hideLoading();
                    $scope.deptGetFundByInstData = [];
                    $scope.deptGetFundByInstOption = {
                        title: {
                            text: '2018年度各单位经费分配(W)',
                            subtext: '院管理员'
                        },
                        legend: {
                            backgroundColor: 'rgb(0,0,0,0)',
                        }
                    };
                    deptGetFundByInstChart.setOption($scope.deptGetFundByInstOption);
                }
            }, function errorCallback(response) {

            });
    };

    // chart17 
    $scope.deptGetFundAndAcceptAppByCateOption = function () {
        $http({
            method: 'GET',
            url: deptGetFundAndAcceptAppByCateUrl
        })
            .then(function successCallback(response) {
                if (response.data.response != '') {
                    // 对应图表新增样式
                    instGetFundAndAcceptAppByCateChart.hideLoading();
                    $scope.deptGetFundAndAcceptAppByCateData = $scope.changeApplicationFundRatioType(response.data.response);
                    $scope.deptGetFundAndAcceptAppByCateChartData = response.data.response;
                    $scope.deptGetFundAndAcceptAppByCateOption = {
                        title: {
                            text: '2018年度提交申请书总数和资助数',
                            subtext: '单位管理员'
                        },
                        dataset: {
                            sourceHeader: false,
                            source: $scope.deptGetFundAndAcceptAppByCateData,
                        },
                    };
                    deptGetFundAndAcceptAppByCateChart.setOption($scope.deptGetFundAndAcceptAppByCateOption);
                }
                else {
                    // 对应图表新增样式
                    instGetFundAndAcceptAppByCateChart.hideLoading();
                    $scope.deptGetFundAndAcceptAppByCateData = [];
                    $scope.deptGetFundAndAcceptAppByCateOption = {
                        title: {
                            text: '2018年度提交申请书总数和资助数',
                            subtext: '单位管理员'
                        },
                        legend: {
                            backgroundColor: 'rgb(0,0,0,0)',
                        }
                    };
                    deptGetFundAndAcceptAppByCateChart.setOption($scope.deptGetFundAndAcceptAppByCateOption);
                }
            }, function errorCallback(response) {
            });
    };

    $scope.extendChart = function (chartID,chartType,chartOptionID) {
        $scope.extendBlurIsHide = false;
        $scope.chartID = chartID;
        $("#extendChartsContainer").append("<div id=\'" + chartID + "\' style=\'width: 70vw;height: 70vh; z-index: 9999; \'></div>");
        var chartElement = echarts.init(document.getElementById(chartID));
        switch (chartType) {
            case 1: chartElement.setOption(statisticsByCateBarOption);
                break;
            case 2: chartElement.setOption(statisticsByCatePieOption);
                break;
            case 3: chartElement.setOption(statisticsByCateLineOption);
                break;
            case 4: chartElement.setOption(statisticsByInstPieOption);
                break;
            case 5: chartElement.setOption(statisticsFundRatioByCateBarOption);
                break;
        };
        switch (chartOptionID) {
            // 单位
            case 'instGetAppOption': chartElement.setOption($scope.instGetAppOption);
                break;
            case 'instGetProjectOption': chartElement.setOption($scope.instGetProjectOption);
                break;
            case 'instGetFundByCateOption': chartElement.setOption($scope.instGetFundByCateOption);
                break;
            case 'instGetFundByCateAndYearOption': chartElement.setOption($scope.instGetFundByCateAndYearOption);
                break;
            case 'instGetFundRatioByCateAndYearOption': chartElement.setOption($scope.instGetFundRatioByCateAndYearOption);
                break;
            case 'instGetFundAndAcceptAppByCateOption': chartElement.setOption($scope.instGetFundAndAcceptAppByCateOption);
                break;
            // 院
            case 'deptGetAppOption': chartElement.setOption($scope.deptGetAppOption);
                break;
            case 'deptGetProjectOption': chartElement.setOption($scope.deptGetProjectOption);
                break;
            case 'deptGetFundByCateOption': chartElement.setOption($scope.deptGetFundByCateOption);
                break;
            case 'deptGetProjectByYearOption': chartElement.setOption($scope.deptGetProjectByYearOption);
                break;
            case 'deptGetFundRatioByCateAndYearOption': chartElement.setOption($scope.deptGetFundRatioByCateAndYearOption);
                break;
            case 'deptGetFundByCateAndYearOption': chartElement.setOption($scope.deptGetFundByCateAndYearOption);
                break;
            case 'deptGetProjectByInstOption': chartElement.setOption($scope.deptGetProjectByInstOption);
                break;
            case 'deptGetFundByInstOption': chartElement.setOption($scope.deptGetFundByInstOption);
                break;
            case 'deptGetFundAndAcceptAppByCateOption': chartElement.setOption($scope.deptGetFundAndAcceptAppByCateOption);
                break;
        };
    }

    $scope.hideChartBlur = function () {
        $scope.extendBlurIsHide = true;
        angular.element(document.getElementById("extendChartsContainer")).empty();
    }


    

    //ng-switch跳转
    $scope.jumpTo = function (page) {
        $scope.current = page;
        //获取数据
        if (page == 1) {

            // 单位
            $scope.instGetAppOption();
            $scope.instGetProjectOption();
            $scope.instGetFundByCateOption();
            $scope.instGetFundByCateAndYearOption();
            $scope.instGetFundRatioByCateAndYearOption();
            $scope.instGetTopTenFundProject();
            $scope.instGetFundAndAcceptAppByCateOption();
        }
        else if (page == 2) {

            // 院
            $scope.deptGetAppOption();
            $scope.deptGetProjectOption();
            $scope.deptGetFundByCateOption();
            $scope.deptGetFundByCateAndYearOption();
            $scope.deptGetFundRatioByCateAndYearOption();
            $scope.deptGetTopTenFundProject();
            $scope.deptGetProjectByYearOption();
            $scope.deptGetProjectByInstOption();
            $scope.deptGetFundByInstOption();
            $scope.deptGetFundAndAcceptAppByCateOption();
        }
    }


    //
    $scope.selectRoles();




    //window.onload = function () {
    //    $('.chartContainer').niceScroll({
    //        cursorcolor: "aquamarine",//#CC0071 光标颜色
    //        cursoropacitymax: 1, //改变不透明度非常光标处于活动状态（scrollabar“可见”状态），范围从1到0
    //        touchbehavior: false, //使光标拖动滚动像在台式电脑触摸设备
    //        cursorwidth: "10px", //像素光标的宽度
    //        cursorborder: "0", // 游标边框css定义
    //        cursorborderradius: "10px",//以像素为光标边界半径
    //        autohidemode: false //是否隐藏滚动条
    //    })
    //}

})