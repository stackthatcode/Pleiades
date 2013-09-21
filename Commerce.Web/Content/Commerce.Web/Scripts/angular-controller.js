'use strict';

var ngAjax = namespace("PushLibrary.NgAjax");
var urlLocator = namespace("CommerceWeb.UrlLocator");

/* Controllers */
function ListController($scope, $http) {
    $scope.testVariable = 'hello there!';
    $scope.urlLocator = urlLocator;

    ngAjax.Get($http, 'products', function (data) {
        $scope.products = data;
    });
}

function DetailController($scope, $routeParams, $http) {
    $scope.UrlLocator = urlLocator;

    ngAjax.Get($http, 'products/' + $routeParams.productid, function (data) {
        $scope.product = data;
    });
}
