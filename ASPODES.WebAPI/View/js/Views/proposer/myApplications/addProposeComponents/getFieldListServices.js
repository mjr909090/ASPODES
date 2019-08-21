angular.module("getFieldListServices", [])
.service('$getFieldList', function ($http, $q) {
    var that = this;

    return {
        get: function () {
            var getFieldUrl = "/api/field";
            var deferred = $q.defer();
            $http.get(getFieldUrl).then(function (response) {
                deferred.resolve(response);
            })
            return deferred.promise;
        }
    }

})