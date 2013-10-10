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
    $scope.SelectedSizes = null;
    $scope.SelectedSizeId = null;
    $scope.AddToCartValidationMessage = null;
    $scope.SelectedQuantities = null;
    $scope.SelectedQuantityValue = null;

    var parentScope = $scope;

    $scope.HasColors = function () {
        return $scope.Product && $scope.Product.Colors;
    };

    $scope.HasMultipleColors = function () {
        return $scope.HasColors() && ($scope.Product.Colors.length > 1);
    };

    $scope.SelectColor = function (colorid) {
        $scope.SelectedColorId = colorid;
        $scope.RefreshImages();
        $scope.RefreshSizes();
    };

    $scope.SelectColorDefault = function () {
        $scope.SelectedColorId = $scope.HasMultipleColors() && $scope.Product.Colors[0].Id;
    };

    $scope.IsSelectedColorId = function (colorid) {
        return colorid == $scope.SelectedColorId;
    };

    $scope.FirstColorId = function () {
        return $scope.HasColors() ? $scope.Product.Colors[0].Id : null;
    };

    $scope.SelectedColorName = function () {
        if ($scope.Product && $scope.SelectedColorId) {
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

            if ($scope.SelectedImages.length == 0) {
                $scope.SelectedImages =
                    AQ($scope.Product.Images)
                        .where(function (x) { return x.ColorId == $scope.FirstColorId(); })
                        .toArray();
            }
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

    $scope.HasSizes = function () {
        return $scope.Product && $scope.Product.Sizes && $scope.Product.Sizes.length >= 1;
    };

    $scope.AvailableInventory = function () {
        if (!$scope.Product || !$scope.Product.Inventory) {
            return [];
        }
        if ($scope.HasMultipleColors()) {
            return AQ($scope.Product.Inventory)
                    .where(function (x) { return x.ColorId == $scope.SelectedColorId && x.Quantity > 0; })
                    .toArray();
        } else {
            return AQ($scope.Product.Inventory).where(function (x) { return x.Quantity > 0; }).toArray();
        }
    };

    $scope.RefreshSizes = function () {
        $scope.SelectedSizes = [];
        $scope.AddToCartValidationMessage = null;

        if ($scope.HasSizes()) {
            AQ($scope.AvailableInventory()).each(function (inventoryItem) {
                var size = AQ($scope.Product.Sizes)
                    .firstOrDefault(function (x) { return x.Id == inventoryItem.SizeId; });

                $scope.SelectedSizes.push(size);
            });
        } else {
            // Is this necessary...?
            $scope.SelectedSizes = $scope.Product.Sizes;
        }
    };

    $scope.HasInventory = function () {
        return $scope.AvailableInventory().length > 0;
    };

    $scope.GetSelectedSku = function () {
        if (!$scope.HasInventory()) {
            return null;
        }
        if ($scope.HasSizes() && $scope.SelectedSizeId) {
            return AQ($scope.AvailableInventory()).firstOrDefault(function (x) { return x.SizeId == $scope.SelectedSizeId; });
        } else {
            return $scope.AvailableInventory()[0];
        }
    };

    $scope.SizeClick = function () {
        if ($scope.SelectedSizeId) {
            $scope.AddToCartValidationMessage = null;
            $scope.RefreshQuantities();
        }
    };

    $scope.RefreshQuantities = function () {
        $scope.SelectedQuantities = [];
        console.log($scope.GetSelectedSku());

        if ($scope.GetSelectedSku()) {
            // Should have the Cart and find item by Sku - subtract its Quantity
            for (var i = 1; i <= $scope.GetSelectedSku().Quantity; i++) {
                $scope.SelectedQuantities.push(i);
            }
            $scope.SelectedQuantityValue = $scope.SelectedQuantities[0];
        }
        console.log($scope.SelectedQuantities.length);
    };

    $scope.QuantitiesVisible = function () {        
        return $scope.SelectedQuantities.length > 0;
    };

    $scope.AddToCart = function () {
        // There are one-to-many images

        if ($scope.HasSizes() && !$scope.SelectedSizeId) {
            $scope.AddToCartValidationMessage = "Please choose your Size";
            return;
        }
        console.log($scope.GetSelectedSku());
        console.log("Quantity = " + $scope.GetSelectedSku());
    };

    ngAjax.Get($http, 'products/' + $routeParams.productid, function (product) {
        $scope.Product = product;
        $scope.SelectColorDefault();
        $scope.RefreshSizes();
        $scope.RefreshQuantities();
        $scope.RefreshImages();
    });
});

app.controller('ContentController', function ($scope, $routeParams) {
    $scope.templateUrl = 'Content/ArtOfGroundFighting/ng-templates/' + $routeParams.contentid + '.html';
});
