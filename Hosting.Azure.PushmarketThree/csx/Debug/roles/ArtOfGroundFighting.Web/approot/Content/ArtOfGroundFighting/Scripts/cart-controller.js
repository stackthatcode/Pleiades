'use strict';

var ngAjax = namespace("PushLibrary.NgAjax");
var urlLocator = namespace("CommerceWeb.UrlLocator");
var app = angular.module('push-market');
var imageLocator = namespace("PushLibrary.ImageLocator");
    
app.controller('CartController', function ($scope, $http) {
    $scope.ImageUrlGenator = imageLocator.GenerateUrl;
    
    $scope.ToMoney = function (input) {
        return input ? input.toString().toMoney() : "";
    };

    $scope.NonEmptyCart = function () {
        return $scope.cart && $scope.cart.CartItems && $scope.cart.CartItems.length > 0;
    };

    $scope.DeleteItem = function (skuCode) {
        var url = 'cart?skuCode=' + skuCode;
        ngAjax.Delete($http, url, function (data) {
            DecorateAdjustedCartModelWithQuantities(data);
            $scope.cart = data.Cart;
        });
    };

    $scope.LoadDataIntoModel = function (data) {
        DecorateAdjustedCartModelWithQuantities(data);
        $scope.cart = data.Cart;
    };

    $scope.RetrieveCart = function () {
        ngAjax.Get($http, 'cart', function (data) {
            DecorateAdjustedCartModelWithQuantities(data);
            $scope.cart = data.Cart;
        });
    };

    $scope.ChangeQuantity = function (item) {
        var url = 'cart?skuCode=' + item.Sku.SkuCode + "&quantity=" + item.Quantity;
        ngAjax.Put($http, url, null, function (data) {
            DecorateAdjustedCartModelWithQuantities(data);
            $scope.cart = data.Cart;
        });
    };

    $scope.RetrieveCart();
});
