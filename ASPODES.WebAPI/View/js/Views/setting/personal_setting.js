var personalSettingApp = angular.module('ngPersonalSettingApp', []);

personalSettingApp.controller('ngPersonalSettingCtrl', function ($scope, $http) {



    $('.datepicker').datepicker({
        format: 'yyyy-mm-dd',
        language: 'zh-CN'
    });


    //$scope初始化
    $scope.newStudyHistory = {
        "EducationId": 123
    }
    $scope.newWorkingHistory = {
        "WorkingId": 123
    }
    $scope.newAchievement = {
        "AchievementId": 123
    }
    $scope.newField = {
        "PersonId": null,
        "ExpertFieldId": null
    }
    $scope.updataField = {
        "PersonId": null,
        "ExpertFieldId": null
    }






    //BasicInfo
    var getBasicInfoUrl = "/api/profile";
    var putBasicInfoUrl = "/api/profile";

    $scope.getBasicInfo = function () {
        $http.get(getBasicInfoUrl)
        .success(function (response) {
            if (response.response != null) {
                $scope.BasicInfo = response.response;
                $scope.studyHistorys = response.response.EducationList;
                $scope.workingHistorys = response.response.WorkingList;
                $scope.Achievements = response.response.AchievementList;
                $scope.postInfo = {
                    "Email": response.response.Email,
                    "Phone": response.response.Phone,
                    "Address": response.response.Address
                }
                $scope.newField.PersonId = $scope.BasicInfo.PersonId;
                $scope.updataField.PersonId = $scope.BasicInfo.PersonId;
                $scope.getUserField();
            }
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "没有获取到个人信息，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        });
    }

    $scope.putBasicInfo = function () {
        $http.put(putBasicInfoUrl, $scope.postInfo)
        .success(function (response) {
            notif({
                msg: "<b>恭喜:</b> 成功保存个人基本信息",
                type: "success"
            });
            $scope.getBasicInfo();
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "未能成功保存个人基本信息，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        });
    }

    //StudyHistory
    var addNewStudyHistoryUrl = "/api/profile/Education";
    var editStudyHistoryUrl = "/api/profile/Education";
    var deleteStudyHistoryUrl = "/api/profile/Education/";


    $scope.addNewStudyHistory = function () {
        $http.post(addNewStudyHistoryUrl, $scope.newStudyHistory)
        .success(function (response) {
            notif({
                msg: "<b>恭喜:</b> 添加新教育经历条目成功",
                type: "success"
            });
            $scope.getBasicInfo();
        })
        .error(function () {
            notif({
                type: "error",
                msg: "未能成功添加教育经历条目，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        });
    }

    $scope.initUpdataModel = function (studyHistory) {
        $scope.updataStudyHistory = angular.copy(studyHistory);    
    }

    $scope.editStudyHistory = function () {
        $http.put(editStudyHistoryUrl, $scope.updataStudyHistory)
        .success(function () {
            notif({
                msg: "<b>恭喜:</b> 修改教育经历条目成功",
                type: "success"
            });
            $scope.getBasicInfo();
        })
        .error(function () {
            notif({
                type: "error",
                msg: "未能成功修改教育经历条目，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }

    $scope.deleteStudyHistory = function () {
        $http.delete(deleteStudyHistoryUrl + $scope.updataStudyHistory.EducationId)
        .success(function () {
            notif({
                msg: "<b>恭喜:</b> 删除教育经历条目成功",
                type: "success"
            });
            $scope.getBasicInfo();
        })
        .error(function () {
            notif({
                type: "error",
                msg: "未能成功删除教育经历条目，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }


    //ProjectHistory
    var addNewWorkingHistoryUrl = "/api/profile/Working";
    var editWorkingHistoryUrl = "/api/profile/Working";
    var deleteWorkingHistoryUrl = "/api/profile/Working";


    $scope.addNewWorkingHistory = function () {
        $http.post(addNewWorkingHistoryUrl, $scope.newWorkingHistory)
        .success(function (response) {
            notif({
                msg: "<b>恭喜:</b> 添加新工作经历条目成功",
                type: "success"
            });
            $scope.getBasicInfo();
        })
        .error(function () {
            notif({
                type: "error",
                msg: "未能成功添加工作经历条目，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        });
    }

    $scope.initWorkingUpdataModel = function (workingHistory) {
        $scope.updataWorkingHistory = angular.copy(workingHistory);

    }

    $scope.editWorkingHistory = function () {
        $http.put(editWorkingHistoryUrl, $scope.updataWorkingHistory)
        .success(function () {
            notif({
                msg: "<b>恭喜:</b> 修改工作经历条目成功",
                type: "success"
            });
            $scope.getBasicInfo();
        })
        .error(function () {
            notif({
                type: "error",
                msg: "未能成功修改工作经历条目，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }

    $scope.deleteWorkingHistory = function () {
        $http.delete(deleteWorkingHistoryUrl + "/" + $scope.updataWorkingHistory.WorkingId)
        .success(function () {
            notif({
                msg: "<b>恭喜:</b> 删除工作经历条目成功",
                type: "success"
            });
            $scope.getBasicInfo();
        })
        .error(function () {
            notif({
                type: "error",
                msg: "未能成功删除工作经历条目，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }


    //Achievement
    var addAchievementUrl = "/api/Profile/Achievement";
    var editAchievementUrl = "/api/Profile/Achievement";
    var deleteAchievementUrl = "/api/Profile/Achievement";


    $scope.addAchievementHistory = function () {
        $http.post(addAchievementUrl, $scope.newAchievement)
        .success(function (response) {
            notif({
                msg: "<b>恭喜:</b> 添加新研究成果成功",
                type: "success"
            });
            $scope.getBasicInfo();
        })
        .error(function (response) {
            notif({
                type: "error",
                msg: "未能成功添加研究成果条目，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        });
    }

    $scope.initAchievementUpdataModel = function (Achievement) {
        $scope.updataAchievement = angular.copy(Achievement);
    }

    $scope.editAchievementHistory = function () {
        $http.put(editAchievementUrl, $scope.updataAchievement)
        .success(function () {
            notif({
                msg: "<b>恭喜:</b> 修改研究成果条目成功",
                type: "success"
            });
            $scope.getBasicInfo();
        })
        .error(function () {
            notif({
                type: "error",
                msg: "未能成功修改研究成果条目，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }

    $scope.deleteAchievementHistory = function () {
        $http.delete(deleteAchievementUrl + "/" + $scope.updataAchievement.AchievementId)
        .success(function () {
            notif({
                msg: "<b>恭喜:</b> 删除研究成果条目成功",
                type: "success"
            });
            $scope.getBasicInfo();
        })
        .error(function () {
            notif({
                type: "error",
                msg: "未能成功删除研究成果条目，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }

    //研究领域

    //Url
    var getFieldUrl = "/api/field";
    var getSubfieldUrl = "/api/subfield/";
    var getUserFieldUrl = "/api/expertfield/";
    var postFieldUrl = "/api/expertfield/user"

    //获取所有研究领域
    $scope.getField = function () {
        $http.get(getFieldUrl)
	    .success(function (response) {
	        $scope.fields = response.response;
	    })
	    .error(function () {
	        notif({
	            type: "error",
	            msg: "没有获取到可选的研究领域，请检查您的网络是否畅通！",
	            position: "center",
	            width: 500,
	            height: 60,
	            autohide: false
	        });
	    });
    }
    //获取子领域
    $scope.getSubField = function (field) {
        $http.get(getSubfieldUrl + field)
       .success(function (response) {
           $scope.subFields = response.response;
       })
       .error(function () {
           notif({
               type: "error",
               msg: "没有获取到该研究领域下的子领域，请检查您的网络是否畅通！",
               position: "center",
               width: 500,
               height: 60,
               autohide: false
           });
       });
    }

    //获取用户研究领域
    $scope.getUserField = function () {
        $http.get(getUserFieldUrl + $scope.BasicInfo.PersonId)
        .success(function (response) {
            if (response.status == 0) {
                $scope.Fields = response.response;
            }
        })
        .error(function () {
            notif({
                type: "error",
                msg: "没有获取到用户的研究领域，请检查您的网络是否畅通！",
                position: "center",
                width: 500,
                height: 60,
                autohide: false
            });
        })
    }

    //新建研究领域
    $scope.addNewField = function () {
        $http.post(postFieldUrl, [$scope.newField])
        .success(function (response) {
            if (response.status == 0) {
                notif({
                    msg: "<b>恭喜:</b> 添加研究领域成功",
                    type: "success"
                });
                $scope.getUserField();
            }
        })
        .error(function () {

        })
    }

    //updataField数据绑定
    $scope.initFieldUpdataModel = function(index){
        $scope.updataField.FieldId = $scope.Fields[index].FieldId;
        $scope.updataField.SubFieldId = $scope.Fields[index].SubFieldId;
        $scope.updataField.KeyWordsCN = $scope.Fields[index].KeyWordsCN;
        $scope.updataField.KeyWordsEN = $scope.Fields[index].KeyWordsEN;
        $scope.updataField.ExpertFieldId = $scope.Fields[index].ExpertFieldId;
    }

    //编辑研究领域
    $scope.editField = function () {
        $http.post(postFieldUrl, [$scope.updataField])
        .success(function (response) {
            if (response.status == 0) {
                notif({
                    msg: "<b>恭喜:</b> 修改研究领域成功",
                    type: "success"
                });
                $scope.getUserField();
            }
        })
        .error(function () {

        })
    }







    //初始化
    $scope.getBasicInfo();
    $scope.getField();





    //ModalCtrl
    $scope.showAddNewStudyHistoryModal = function () {
        $scope.newStudyHistory = {
            "EducationId": 123
        }
        $("#addNewStudyHistoryModal").modal("show");
    }
    $scope.showEditStudyHistoryModal = function () {
        $("#editStudyHistoryModal").modal("show");
    }
    $scope.showDeleteStudyHistoryModal = function () {
        $("#deleteStudyHistoryModal").modal("show");
    }
    $scope.showAddNewProjectHistoryModal = function () {
        $scope.newWorkingHistory = {
            "WorkingId": 123
        }
        $("#inputProjectHistory").modal("show");
    }
    $scope.showEditWorkingHistoryModal = function () {
        $("#editWorkingHistoryModal").modal("show");
    }
    $scope.showDeleteWorkingHistoryModal = function () {
        $("#deleteWorkingHistoryModal").modal("show");
    }
    $scope.showAddNewAchievementModal = function () {
        $scope.newAchievement = {
            "AchievementId": 123
        }
        $("#inputAchievement").modal("show");
    }
    $scope.showEditAchievementModal = function () {
        $("#editAchievementModal").modal("show");
    }
    $scope.showDeleteAchievementModal = function () {
        $("#deleteAchievementModal").modal("show");
    }
    $scope.showAddNewFieldModal = function () {
        $scope.newField = {
            "PersonId": null,
            "ExpertFieldId": null
        }
        $("#inputField").modal("show");
    }
    $scope.showEditFieldModal = function () {
        $("#editFieldModal").modal("show");
    }

});