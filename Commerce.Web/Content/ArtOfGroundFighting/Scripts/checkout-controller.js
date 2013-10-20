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
                ngAjax.Get($http, 'list?listId=StatesAndTaxes', this);
            },
            function (data) {
                $scope.StatesAndTaxes = data;
                ngAjax.Get($http, 'list?listId=ShippingMethods', this);
            },
            function (data) {
                console.log(data);
                $scope.ShippingMethods = data;
                ngAjax.Get($http, 'cart', this);
            },
            function(data) {
                DecorateAdjustedCartModelWithQuantities(data);
                $scope.cart = data.Cart;

                $scope.BillingInfo = {};
                $scope.BillingInfo.State = $scope.StatesAndTaxes[0].Abbreviation;
                $scope.BillingInfo.ExpirationMonth = $scope.ExpirationMonths[0];
                $scope.BillingInfo.ExpirationYear = $scope.ExpirationYears[0];
                
                $scope.ShippingInfo = {};
                $scope.ShippingInfo.State = $scope.StatesAndTaxes[0].Abbreviation;
                $scope.ShippingInfo.ShippingMethodId = $scope.ShippingMethods[0].Id;

                // TODO: if cart contains these, then populate
                // StateTax.Abbrevation
                // ShippingMethod.Id
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

    $scope.ValidateBillingAddress = function() {
        $("#billingAddressForm").validate({
            rules: {
                name: {
                    required: true,
                },
                address1: {
                    required: true,
                },
                city: {
                    required: true,
                },
                zipCode: {
                    required: true,
                },
                state: {
                    required: true,
                },
                phone: {
                    required: true,
                },
            },
            highlight: function(element) {
                $(element).closest('.form-group').addClass('has-error');
            },
            unhighlight: function(element) {
                $(element).closest('.form-group').removeClass('has-error');
            },
            errorPlacement: function(error, element) {
                return true;
            },
            errorContainer: $("#billing-address-errors"),
        });
    };

    $scope.ValidateCreditCard = function() {
        $("#paymentForm").validate({
            rules: {
                cardNumber: {
                    required: true,
                    creditcard: true,
                },
                CVV: {
                    required: true,
                    number: true,
                    minlength: 3,
                },
                expirationMonth: {
                    required: true,
                },
                expirationYear: {
                    required: true,
                },
            },
            highlight: function(element) {
                $(element).closest('.form-group').addClass('has-error');
            },
            unhighlight: function(element) {
                $(element).closest('.form-group').removeClass('has-error');
            },
            errorPlacement: function(error, element) {
                return true;
            },
            errorContainer: $("#payment-info-errors"),
        });
    };
        
    $scope.Checkout = function() {
        console.log($scope.cart);
        
        $scope.ValidateBillingAddress();
        $scope.ValidateCreditCard();
        
        var failed = false;
        if (!$("#billingAddressForm").valid()) {
            $("#billing-address-errors").show();
            failed = true;
        }
        if (!$("#paymentForm").valid()) {
            $("#payment-info-errors").show();
            failed = true;
        }
        if (failed) {
            return;
        }
    };

    $scope.RetrieveCart();
});
