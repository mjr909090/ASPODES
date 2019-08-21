var constParamsModule = angular.module('constParamsModule', []);
constParamsModule.service('annualTaskFileType', function () {
    var self = this;

    self.getAnnualTaskFileType = function (fileType) {
        
        var constFileType = [
            {
                fileType: 'yearReport',
                value: 0
            },
            {
                fileType: 'mainBody',
                value: 1
            },
            {
                fileType: 'other',
                value: 2
            },
            {
                fileType: 'PDF',
                value: 3
            }
        ];

        for (var i = 0; i < constFileType.length; i++) {
            if (fileType == constFileType[i].fileType) {
                return constFileType[i].value;
            }
        }



    }

})