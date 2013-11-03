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

    $scope.SalesTaxText = function () {
        return $scope.cart && $scope.cart.StateTax ? "(" + 
            $scope.cart.StateTax.Abbreviation + " " + 
            $scope.cart.StateTax.TaxRate + "%)" : "";
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

    $scope.Initialize = function(complete) {
        flow.exec(
            function() {
                ngAjax.Get($http, 'list?listId=StatesAndTaxes', this);
            },
            function(data) {
                $scope.StatesAndTaxes = data;
                ngAjax.Get($http, 'list?listId=ShippingMethods', this);
            },
            function(data) {
                $scope.ShippingMethods = data;
                $scope.BillingInfo = {};
                $scope.BillingInfo.ExpirationMonth = $scope.ExpirationMonths[0];
                $scope.BillingInfo.ExpirationYear = $scope.ExpirationYears[0];
                $scope.BillingInfo.State = $scope.StatesAndTaxes[0].Abbreviation;
                
                $scope.ShippingInfo = {};
                $scope.ShippingInfo.State = $scope.StatesAndTaxes[0].Abbreviation;
                $scope.ShippingInfo.ShippingMethodId = $scope.ShippingMethods[0].Id;
                
                if (complete) {
                    complete();
                }
            });
    };

    $scope.RetrieveCart = function (complete) {
        flow.exec(
            function () {
                ngAjax.Get($http, 'cart', this);
            },
            function(data) {
                DecorateAdjustedCartModelWithQuantities(data);
                $scope.cart = data.Cart;
                
                if ($scope.cart.StateTax) {
                    $scope.ShippingInfo.State = $scope.cart.StateTax.Abbreviation;
                }
                if ($scope.cart.ShippingMethod) {
                    $scope.ShippingInfo.ShippingMethodId = $scope.cart.ShippingMethod.Id;
                }                
                if (complete) {
                    complete();
                }
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

    $scope.ChangeShippingMethod = function() {
        var url = 'cart?shippingMethodId=' + $scope.ShippingInfo.ShippingMethodId;
        ngAjax.Put($http, url, null, function (data) {
            DecorateAdjustedCartModelWithQuantities(data);
            $scope.cart = data.Cart;
        });
    };

    $scope.ChangeStateTax = function() {
        var url = 'cart?stateTaxAbbr=' + $scope.ShippingInfo.State;
        ngAjax.Put($http, url, null, function (data) {
            DecorateAdjustedCartModelWithQuantities(data);
            $scope.cart = data.Cart;
        });
    };

    $scope.Checkout = function() {
        var validator = new CheckOutValidationInitializer();
        validator.ValidateBillingAddress();
        validator.ValidateCreditCard();
        validator.ValidateShippingInfo();

        var failed = false;
        if (!$("#shippingInfoForm").valid()) {
            $("#shipping-info-error").show();
            failed = true;
        }
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

        $("#exp-month").val($scope.BillingInfo.ExpirationMonth);
        $("#exp-year").val($scope.BillingInfo.ExpirationYear);
        Stripe.card.createToken($("#paymentForm"), $scope.ProcessStripeResponse);
    };

    $scope.ProcessStripeResponse = function(status, response) {
        var orderRequest = {
            shippingInfo: $scope.ShippingInfo,
            token: response.id,            
        };
        // For debugging purposes
        //response.error = { message: "oh shit!!!" };

        $('#payment-processing-feedback').hide();
        if (response.error) {
            $('#payment-processing-message').text(response.error.message);            
            $('#payment-processing-feedback').show();            
            return;
        }
        
        ngAjax.Post($http, "order", orderRequest,
            function(data) {
                console.log(data);
                window.location = "#/order?externalId=" + data.Order.ExternalId + "&emailAddress=" + data.Order.EmailAddress;
            }
        );
    };
    
    $scope.Initialize($scope.RetrieveCart);
    
    $scope.AutomationLoadTestData = function() {
        $scope.ShippingInfo.Name = "Jessie JAmes";
        $scope.ShippingInfo.EmailAddress = "Jessie@JAmes.com";
        $scope.ShippingInfo.Phone = "847-333-1234";
        $scope.ShippingInfo.Address1 = "123 Test Street";
        $scope.ShippingInfo.City = "Heliopoulis";
        $scope.ShippingInfo.State = "MN";
        $scope.ShippingInfo.ZipCode = "44432";
        $scope.ShippingInfo.ShippingMethodId = 3;
        
        $scope.BillingInfo.Name = "Jessie JAmes";
        $scope.BillingInfo.EmailAddress = "Jessie@JAmes.com";
        $scope.BillingInfo.Phone = "847-333-1234";
        $scope.BillingInfo.Address1 = "123 Test Street";
        $scope.BillingInfo.City = "Heliopoulis";
        $scope.BillingInfo.State = "MN";
        $scope.BillingInfo.ZipCode = "44432";
        $scope.BillingInfo.CardNumber = "4111111111111111";
        $scope.BillingInfo.CVV = "445";
        $scope.BillingInfo.ExpirationMonth = "09";
        $scope.BillingInfo.ExpirationYear = 2015;
    };
});

var CheckOutValidationInitializer = function() {
    this.ValidateBillingAddress = function() {
        $("#billingAddressForm").validate({
            rules: {
                name: { required: true },
                address1: { required: true },
                city: { required: true },
                zipCode: { required: true },
                state: { required: true },
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

    this.ValidateShippingInfo = function() {
        $("#shippingInfoForm").validate({
            rules: {
                name: { required: true },
                address1: { required: true },
                city: { required: true },
                zipCode: { required: true },
                state: { required: true },
                phone: { required: true },
                emailAddress: { required: true },
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
            errorContainer: $("#shipping-info-error"),
        });
    };

    this.ValidateCreditCard = function() {
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
};

var EnableAutomation = function() {
    $("#automation-trigger").show();
};
