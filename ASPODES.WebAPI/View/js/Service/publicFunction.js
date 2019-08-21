var publicFunctionApp = angular.module('publicFunction', []);
publicFunctionApp.service('$downloadFile', function ($http) {

    var self = this;

    self.downloadFile = function(downloadPDFUrl, applicationId, downloadName){
        var pdfUrl = downloadPDFUrl + applicationId;
        $http.get(pdfUrl, { responseType: 'arraybuffer' }).success(function (data) {
            var blob = new Blob([data], { type: "application/pdf" });
            var objectUrl = URL.createObjectURL(blob);
            var aForPDF = $("<a download='" + downloadName + ".pdf'><span class='aForPDF' ></span></a>").attr("href", objectUrl);
            $("body").append(aForPDF);
            $(".aForPDF").click();
            aForPDF.remove();
        });
    }

    self.downloadFileType = function (downloadPDFUrl, fileType, downloadName) {
        var pdfUrl = downloadPDFUrl;
        $http.get(pdfUrl, { responseType: 'arraybuffer' }).success(function (data) {
            var blob = new Blob([data], { type: "application/pdf" });
            var objectUrl = URL.createObjectURL(blob);
            var aForPDF = $("<a download='" + downloadName + fileType + "'><span class='aForPDF' ></span></a>").attr("href", objectUrl);
            $("body").append(aForPDF);
            $(".aForPDF").click();
            aForPDF.remove();
        });
    }

    self.downloadFileWithoutFileType = function (downloadPDFUrl, downloadName) {
        var pdfUrl = downloadPDFUrl;
        $http.get(pdfUrl, { responseType: 'arraybuffer' }).success(function (data) {
            var blob = new Blob([data], { type: "application/pdf" });
            var objectUrl = URL.createObjectURL(blob);
            var aForPDF = $("<a download='" + downloadName + "'><span class='aForPDF' ></span></a>").attr("href", objectUrl);
            $("body").append(aForPDF);
            $(".aForPDF").click();
            aForPDF.remove();
        });
    }

    self.exportApplication = function(exportApplicationUrl, exportApplicationFields, exportApplicationName) {
        var exportUrl = exportApplicationUrl;
        $http({
            method: "POST",
            url: exportApplicationUrl,
            data: exportApplicationFields,
            responseType: 'arraybuffer'
        })
            .success(function (data) {
            var blob = new Blob([data], { type: "application/vnd.ms-excel" });  
            var objectUrl = URL.createObjectURL(blob);  
            var aForExcel = $("<a download=' " + exportApplicationName + " '><span class='forExcel'></span></a>").attr("href",objectUrl);  
            $("body").append(aForExcel);  
            $(".forExcel").click();  
            aForExcel.remove();  
            })
            .error(function (response) {

            });
    }

})