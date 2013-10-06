'use strict';

var ngAjax = namespace("PushLibrary.NgAjax");
var urlLocator = namespace("CommerceWeb.UrlLocator");
var app = angular.module('push-market');

app.controller('ListController', function($scope, $http) {
    ngAjax.Get($http, 'products', function(data) {
        $scope.products = data;
    });
});

app.controller('DetailController', function ($scope, $routeParams, $http) {
    $scope.SelectedColorId = null;
    $scope.SelectedImages = null;
    $scope.SelectedImageId = null;

    var parentScope = $scope;

    $scope.HasMultipleColors = function () {
        return $scope.Product && $scope.Product.Colors && ($scope.Product.Colors.length > 1);
    };

    $scope.HasSizes = function () {
        return $scope.Product && $scope.Product.Sizes && $scope.Product.Sizes.length >= 1;
    };

    $scope.SelectColor = function (colorid) {
        $scope.SelectedColorId = colorid;
        $scope.RefreshImages();
    };

    $scope.SelectColorDefault = function () {
        $scope.SelectedColorId = $scope.HasMultipleColors() && $scope.Product.Colors[0].Id;
    };

    $scope.IsSelectedColorId = function (colorid) {
        return colorid == $scope.SelectedColorId;
    };

    $scope.SelectedColorName = function () {
        if ($scope.Product) {
            var color = AQ($scope.Product.Colors).first(function (x) { return x.Id == $scope.SelectedColorId; });
            return color ? color.Name : "";
        }
    };

    $scope.RefreshImages = function () {
        if ($scope.HasMultipleColors()) {
            $scope.SelectedImages =
                AQ($scope.Product.Images)
                    .where(function (x) { return x.ColorId == $scope.SelectedColorId; })
                    .toArray();
        } else {
            $scope.SelectedImages = $scope.Product.Images;
        }
        $scope.SelectedImageId = $scope.SelectedImages[0].ImageBundleExternalId;
    };

    $scope.SelectImage = function (id) {
        parentScope.SelectedImageId = id;
    };

    $scope.ImageUrl = function (ibexternalId, size) {
        return ibexternalId && ("image/" + ibexternalId + "?size=" + size);
    };

    $scope.GetSelectedSku = function () {
        // TODO: HasColors
        // TODO: HasSizes
        // TODO: HasColors & HasSizes
        // TODO: !HasColors & !HasSizes
    };

    // There are one-to-many images
    $scope.AddToCart = function () {
        console.log("got it!");

        // Ok, how to change the model via the ng way
        $scope.SelectedImageId = $scope.Product.Images[2].ImageBundleExternalId;
    };

    ngAjax.Get($http, 'products/' + $routeParams.productid, function (product) {
        $scope.Product = product;

        // Initialize State
        $scope.SelectColorDefault();
        $scope.RefreshImages();
    });
});

app.controller('ContentController', function ($scope, $routeParams) {
    $scope.templateUrl = 'Content/ArtOfGroundFighting/ng-templates/' + $routeParams.contentid + '.html';
});

