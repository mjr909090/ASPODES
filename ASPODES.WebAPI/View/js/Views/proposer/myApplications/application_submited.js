var applicationSavedApp = angular.module('ngApplicationSubmitApp', []);

applicationSavedApp.controller('ngApplicationSubmitCtrl', function ($scope, $http) {

    $scope.noResult = false;
    $scope.proposeInfos = {};
    $scope.listAlready = false;
    

    //URL
    var getProposeInfoUrl = "/api/applciation/submited";
    var getReasonUrl = "/api/applicationlog/";

    //获取申请书基本信息
    $scope.getProposeInfo = function () {
        $scope.listAlready = false;
        $http.get(getProposeInfoUrl)
        .success(function (response) {
            if (response.response != null) {
                $scope.proposeInfos = response.response;
                $scope.listAlready = true;
            }
            else {
                $scope.listAlready = true;
            }
        })
        .error(function () {
            notif({
                type: "error",
                msg: "错误：" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
            $scope.listAlready = true;
        })
    }

    //进度条长度
    $scope.checkProgressWidth = function (status) {
        var progressFullWidth = $(".progress").width();
        var progressWidth = 32 / progressFullWidth * 100;
        $scope.eachEmptyWidth = (progressFullWidth - 64 * 5) / 4;
        if (status >= 5 && status <= 7) {
            var progressWidth = (1 * (64 + $scope.eachEmptyWidth) + 32) / progressFullWidth * 100;
        }
        else if (status >= 8 && status <= 9) {
            var progressWidth = (2 * (64 + $scope.eachEmptyWidth) + 32) / progressFullWidth * 100;
        }
        else if (status >= 10 && status <= 15) {
            var progressWidth = (3 * (64 + $scope.eachEmptyWidth) + 32) / progressFullWidth * 100;
        }
        else if (status >= 16) {
            var progressWidth = (4 * (64 + $scope.eachEmptyWidth) + 32) / progressFullWidth * 100;
        }
        else {
            var progressWidth = (status * (64 + $scope.eachEmptyWidth) + 32) / progressFullWidth * 100;
        }
        return progressWidth;
    }
	
	
	$(".pop").popover({ trigger: "manual" , html: true, animation:false})
	    .on("mouseenter", function () {
	        var _this = this;
	        $(this).popover("show");
	        $(".popover").on("mouseleave", function () {
	            $(_this).popover('hide');
	        });
	    }).on("mouseleave", function () {
	        var _this = this;
	        setTimeout(function () {
	            if (!$(".popover:hover").length) {
	                $(_this).popover("hide");
	            }
	        }, 300);
	    });
	
	
	$scope.replyApplication = function(){
		$("#backApplication").modal("show");
	}

    //显示不受理理由
	$scope.showNotPassReason = function (applicationId) {
	    $http.get(getReasonUrl + applicationId + '/9')
        .success(function (response) {
            $scope.noPassReason = response.response[0].Comment;
            $('#noPassReasonModal').modal('show');
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "错误：" + response.errorMsg,
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
	}

    //显示PDF页面
	$scope.showPDFpage = function (PDFUrl) {
	    var PDFPageUrl = "/View/plugins/pdfjs/web/viewer.html?file=" + PDFUrl;
	    window.open(PDFPageUrl);
	}

    //初始化
	$scope.getProposeInfo();
	

})