'use strict';

var ngAjax = namespace("PushLibrary.NgAjax");
var urlLocator = namespace("CommerceWeb.UrlLocator");
var app = angular.module('push-market');

app.controller('CheckoutController', function ($scope, $http) {
    $scope.ExpirationMonths = ["01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"];
    $scope.ExpirationYears = AQ.range(2013, 2030).toArray();

    $scope.ToMoney = function (input) {
        return input ? input.toString().toMoney() : "";
    };

    $scope.ItemsText = function (quantity) {
        return (quantity == 1 ? "item" : "items");
    };

    $scope.SalesTaxText = function (totalObject) {
        return totalObject && totalObject.StateTax ? "(" + totalObject.StateTax.Abbreviation + " " + totalObject.StateTax.TaxRate + "%)" : "";
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

    $scope.RetrieveCart = function () {
        flow.exec(
            function () {
                ngAjax.Get($http, 'cart', this);
            },
            function (data) {
                DecorateAdjustedCartModelWithQuantities(data);
                $scope.cart = data.Cart;
                $scope.cart.BillingInfo = {};

                ngAjax.Get($http, 'list?listId=StatesAndTaxes', this);
            },
            function (data) {
                $scope.StatesAndTaxes = data;
                console.log(data);
            }
        );
    };

    $scope.ChangeQuantity = function (item) {
        var url = 'cart?skuCode=' + item.Sku.SkuCode + "&quantity=" + item.Quantity;
        ngAjax.Put($http, url, null, function (data) {
            DecorateAdjustedCartModelWithQuantities(data);
            $scope.cart = data.Cart;
        });
    };

    $scope.Checkout = function () {
        console.log($scope.cart);
    };

    $scope.RetrieveCart();
});
