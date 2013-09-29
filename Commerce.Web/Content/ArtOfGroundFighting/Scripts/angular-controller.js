'use strict';

var ngAjax = namespace("PushLibrary.NgAjax");
var urlLocator = namespace("CommerceWeb.UrlLocator");
var app = angular.module('push-market');

app.controller('ListController', function($scope, $http) {
    ngAjax.Get($http, 'products', function(data) {
        $scope.products = data;
    });
});

app.controller('DetailController', function($scope, $routeParams, $http) {
    ngAjax.Get($http, 'products/' + $routeParams.productid, function(data) {
        $scope.product = data;
    });
});

app.controller('ContentController', function ($scope, $routeParams) {
    $scope.templateUrl = 'Content/ArtOfGroundFighting/ng-templates/' + $routeParams.contentid + '.html';
});
