(function () {
    'use strict';

    var slkApp = angular.module("SLKApp");

    slkApp.factory('productProvider', function ($http) {
        return {
            getProduct: function (successcb) {
                $http({ method: 'GET', url: 'Product/Edit/1' })
                    .success(function (data, status, headers, config) {
                        successcb(data);
                    })
                    .error(function (data, status, headers, config) {
                        $log.warn(data, status, headers, config);
                    })
            }
        }
    });

})();