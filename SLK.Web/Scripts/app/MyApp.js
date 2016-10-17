(function () {
    'use strict';

    var slkApp = angular.module('SLKApp', ['ngRoute']);

    slkApp.config(function ($routeProvider) {
        $routeProvider.when('/Products',
            {
                templateUrl: '/Scripts/app/templates/productsList.html',              
            });
    });

})();