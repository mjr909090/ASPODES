addProposeApp.component('assistdivisionlist', {
    templateUrl: '/View/Views/proposer/myApplications/addProposeComponents/MemberManage/assistDivisionList.html',
    controller: assistDivisionCtrl,
    bindings: {
        ApplicationId: '<',
    }
});


function assistDivisionCtrl($scope, $http, $rootScope) {





    //清除modal所选人员
    $scope.cleanPerson = function () {
        $scope.selectedInstPerson = null;
        $scope.assistPersonTask = null;
        $scope.$broadcast('angucomplete-alt:clearInput');
    }


    //Url
    var getPersonListUrl = "/api/member/";
    var getInstituteUrl = "/api/inst/others";
    var getInstWholePersonUrl = "/api/person/combo/inst/";
    var postAddPersonUrl = "/api/member";
    var delPersonUrl = "/api/member/";
    var getOutInstListUrl = "/api/inst/partners";
    var checkCodeExistUrl = "/api/inst/validatecode/";
    var addNewOutInstUrl = "/api/inst/partners";
    var getOneOutInstPersonUrl = "/api/person/combo/inst/";
    var checkIdCardExistUrl = "/api/person/validateidcard/"
    var addNewOutPersonUrl = "/api/person/partners"



    //获取所有单位
    $scope.getAllInstitute = function () {
        $http.get(getInstituteUrl)
            .success(function (response) {
                $scope.Institutes = response.response;
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
            });
    }


    //获取单位所有人员
    $scope.getInInstitutePerson = function (instituteId) {
        $http.get(getInstWholePersonUrl + instituteId)
        .success(function (response) {
            $scope.inInstitutePerson = response.response;
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
        });
    };



    //获取协作单位参与人员列表
    $scope.getAssistPersonList = function () {
        $http.get(getPersonListUrl + "assistinst/" + ApplicationId)
        .success(function (response) {
            $scope.assistPersons = response.response;
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

        });
    }



    //发送添加的协作单位内部人员
    $scope.addAssistPerson = function (assistPersonId) {
        $http.post(postAddPersonUrl, {
            "ApplicationId": ApplicationId,
            "PersonId": assistPersonId,
            "Task": $scope.assistPersonTask
        })
        .success(function (response) {
            if (response.status == 0) {
                notif({
                    msg: "<b>恭喜:</b> 成功添加协作单位人员条目",
                    type: "success"
                });
                //获取主持单位参与人员列表
                $scope.getAssistPersonList();

            }
        })
        .error(function (response) {
            if (response.status == 4) {
                notif({
                    type: "error",
                    msg: "错误：" + response.errorMsg,
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            }
        });
        $scope.cleanPerson();
    }


    //显示添加外单位人员模态框
    $scope.showOutAddPersonModal = function () {

        //还原初始化数据
        $scope.addOutPersonStatus = 'selectOutInst';
        $scope.checkedCodeStatus = 'beforeCheck';
        $scope.checkedIdCardStatus = 'beforeCheck';
        $scope.outInstSelected = null;
        $scope.addOutPersonData = null;
        $scope.addNewInstData = {
            Type: 0
        };

        $scope.getOutInstList();
        $("#outAddPersonModal").modal('show');
    }

    $("#outAddPersonModal").on('hidden.bs.modal', function (e) {
        $scope.addOutPersonStatus = 'selectOutInst';
        $scope.checkedCodeStatus = 'beforeCheck';
    })

    //获取外单位列表
    $scope.getOutInstList = function () {
        $http.get(getOutInstListUrl)
        .success(function (response) {
            $scope.outInstList = response.response;
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

    //检查外单位是否存在
    $scope.checkCodeExist = function (Code) {
        if (Code != null) {
            $http.post(checkCodeExistUrl + Code)
            .success(function (response) {
                if (response.response == '该组织机构代码正确') {
                    $scope.checkedCodeStatus = 'correct';
                }
                else if (response.response == '该组织机构代码已存在，若找不到该单位请确定该单位状态是否锁定') {
                    $scope.checkedCodeStatus = 'wrong';
                }
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

    }

    //添加外单位
    $scope.addNewOutInst = function () {
        if ($scope.checkedCodeStatus == 'correct') {
            $http.post(addNewOutInstUrl, $scope.addNewInstData)
            .success(function (response) {
                notif({
                    msg: "<b>恭喜:</b> 成功添加院外单位",
                    type: "success"
                });
                $scope.addOutPersonStatus = 'selectOutInst';
                $scope.checkedCodeStatus = 'beforeCheck';
                $scope.getOutInstList();
                $scope.addNewInstData = {
                    Type: 0
                };
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
    }

    //验证是否能够跳转到选择外单位人员模组
    $scope.goToAddOutPersonContainer = function () {
        if ($scope.outInstSelected == null || typeof ($scope.outInstSelected) == 'undefine') {
            $scope.noOutInstSelected = true;
        }
        else {
            $scope.noOutInstSelected = false;
            $scope.getOneOutInstPerson($scope.outInstSelected);
            $scope.addOutPersonStatus = 'selectNewOutPerson';
        }
    }

    //获取某一单位的人员列表
    $scope.getOneOutInstPerson = function (instId) {
        $http.get(getOneOutInstPersonUrl + instId)
        .success(function (response) {
            $scope.outPersonList = response.response;
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

    //检测身份证号是否存在
    $scope.checkIdCardExist = function (IdCard) {
        if (IdCard != null) {
            $http.post(checkIdCardExistUrl + IdCard)
            .success(function (response) {
                console.log(response, 1, IdCard);
                if (response.response == '身份证验证通过') {
                    $scope.checkedIdCardStatus = 'correct';
                }
                else if (response.response == '该身份证已存在，若找不到该人员，请检查该人员状态' || response.response == '身份证格式错误') {
                    $scope.checkedIdCardStatus = 'wrong';
                }
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
    }

    //添加外单位新人员
    $scope.addNewOutPerson = function () {
        if ($scope.checkedIdCardStatus == 'correct') {
            $scope.addNewOutPersonData.InstituteId = $scope.outInstSelected;
            $http.post(addNewOutPersonUrl, $scope.addNewOutPersonData)
            .success(function (response) {
                notif({
                    msg: "<b>恭喜:</b> 成功添加院外人员",
                    type: "success"
                });
                $scope.addNewOutPersonData = {};
                $scope.addOutPersonStatus = 'selectNewOutPerson';
                $scope.checkedIdCardStatus = 'beforeCheck';
                $scope.getOneOutInstPerson($scope.outInstSelected);
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
    }



    //添加选中人员到申请书中
    $scope.addOutPerson = function () {
        $scope.addOutPersonData.ApplicationId = ApplicationId;
        $http.post(postAddPersonUrl, $scope.addOutPersonData)
        .success(function (response) {
            if (response.status == 0) {
                notif({
                    msg: "<b>恭喜:</b> 成功添加外单位人员",
                    type: "success"
                });
                //获取协作单位参与人员列表
                $scope.getAssistPersonList();
                $("#outAddPersonModal").modal('hide');
            }
        })
        .error(function (response) {
            if (response.status == 4) {
                notif({
                    type: "error",
                    msg: "错误：" + response.errorMsg,
                    position: "center",
                    width: 500,
                    height: 60,
                    autohide: false
                });
            }
        });
    }



    //删除单位成员
    $scope.delPerson = function () {
        if ($scope.postPersonId == null) {
            notif({
                type: "error",
                msg: "错误：请至少选择一个人员",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        }
        else {
            $http.delete(delPersonUrl + ApplicationId + "/" + $scope.postPersonId)
            .success(function (response) {
                if (response.status == 0) {
                    notif({
                        msg: "<b>恭喜:</b> 删除人员条目成功",
                        type: "success"
                    });
                }
                $scope.getAssistPersonList();
                $scope.postPersonId = null;
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
            });
        }
    }



    //初始化参数和函数

    $scope.$on('initMember', function () {
        $scope.getAllInstitute();
        $scope.getAssistPersonList();
    })

    $scope.postPersonId = null;
    $scope.addOutPersonStatus = 'selectOutInst';
    $scope.checkedCodeStatus = 'beforeCheck';
    $scope.checkedIdCardStatus = 'beforeCheck';
    $scope.addNewInstData = {
        Type: 0
    };










}