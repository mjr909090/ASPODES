addProposeApp.component('field', {
    templateUrl: '/View/Views/proposer/myApplications/addProposeComponents/BasicInfo/field.html',
    controller: fieldCtrl,
    bindings: {
        fieldResource: '=',
        fieldList:'='
    }
});

function fieldCtrl($http, $timeout, $scope) {



    var that = this;

    var getSubfieldUrl = "/api/subfield/";

    
    //获取子领域
    that.getSubField = function (field) {
        $http.get(getSubfieldUrl + field)
       .success(function (response) {
           that.fieldResource.SubFields = response.response;
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

    
}