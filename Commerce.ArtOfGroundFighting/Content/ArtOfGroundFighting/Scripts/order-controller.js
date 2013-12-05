'use strict';

var ngAjax = namespace("PushLibrary.NgAjax");
var urlLocator = namespace("CommerceWeb.UrlLocator");
var app = angular.module('push-market');


app.controller('OrderController', function ($scope, $http, $routeParams) {
    $scope.ToMoney = function (input) {
        return input ? input.toString().toMoney() : "";
    };

    $scope.ItemsText = function (quantity) {
        return (quantity == 1 ? "item" : "items");
    };

    $scope.Initialize = function () {
        $scope.externalId = $routeParams.externalId;
        $scope.emailAddress = $routeParams.emailAddress;
    };

    $scope.Initialize();
});
