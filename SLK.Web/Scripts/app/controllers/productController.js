(function () {
    'use strict';

    var slkApp = angular.module("SLKApp");

    slkApp.controller("productController",
        function productController($scope, productProvider) {
            productProvider.getProduct(function (product) {
                $scope.product = product;
            });
        });
})();