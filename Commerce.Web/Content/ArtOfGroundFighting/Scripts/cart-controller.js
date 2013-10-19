'use strict';

var ngAjax = namespace("PushLibrary.NgAjax");
var urlLocator = namespace("CommerceWeb.UrlLocator");
var app = angular.module('push-market');

app.controller('CartController', function ($scope, $http) {

    $scope.ToMoney = function (input) {
        return input ? input.toString().toMoney() : "";
    };

    $scope.NonEmptyCart = function () {
        return $scope.cart && $scope.cart.CartItems && $scope.cart.CartItems.length > 0;
    };

    $scope.DeleteItem = function (skuCode) {
        var url = 'cart?skuCode=' + skuCode;
        ngAjax.Delete($http, url, function (data) {
            $scope.LoadDataIntoModel(data);
        });
    };

    $scope.LoadDataIntoModel = function (data) {
        for (var i = 0; i < data.Cart.CartItems.length; i++) {
            var item = data.Cart.CartItems[i];
            item.QuantityList = [];
            for (var x = 0; x < item.Sku.Available; x++) {
                item.QuantityList.push(x + 1);
            }
        }

        console.log(data);
        $scope.cart = data.Cart;
        if (data.InventoryAdjusted) {
            ngAjax.ModalGeneric("Changes were made to your Cart", "Due to changes in inventory, items were removed from your cart.");
        }
    };

    $scope.RetrieveCart = function () {
        ngAjax.Get($http, 'cart', function (data) {
            $scope.LoadDataIntoModel(data);
        });
    };

    $scope.ChangeQuantity = function (item) {
        var url = 'cart?skuCode=' + item.Sku.SkuCode + "&quantity=" + item.Quantity;
        ngAjax.Put($http, url, null, function (data) {
            $scope.LoadDataIntoModel(data);
        });
    };

    $scope.RetrieveCart();
});
