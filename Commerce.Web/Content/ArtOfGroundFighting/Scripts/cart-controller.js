'use strict';

var ngAjax = namespace("PushLibrary.NgAjax");
var urlLocator = namespace("CommerceWeb.UrlLocator");
var app = angular.module('push-market');

app.controller('CartController', function ($scope, $http) {
    ngAjax.Get($http, 'cart', function (data) {
        $scope.cart = data.Cart;
        
        // TODO: if Cart Quantity adjusted, show that message
    });

    $scope.ToMoney = function (input) {
        return input.toString().toMoney();
    };

    $scope.DeleteItem = function (skuCode) {
       var url = 'cart?skuCode=' + skuCode;
        ngAjax.Delete($http, url, function (data) {
            $scope.cart = data.Cart;

            // TODO: if Cart Quantity adjusted, show that message
        });
    };
    
});
