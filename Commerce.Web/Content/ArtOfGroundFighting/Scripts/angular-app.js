'use strict';

var templateUrlBase = 'Content/ArtOfGroundFighting/ng-templates/';

angular.module('push-market', []).
    config(['$routeProvider', function($routeProvider) {
        $routeProvider
            .when('/products', {
                templateUrl: templateUrlBase + 'product-list.html', 
                controller: 'ListController'
            })
            .when('/products/:productid', {
                templateUrl: templateUrlBase + 'product-detail.html', 
                controller: 'DetailController'
            })
            .when('/content/:contentid', {
                templateUrl: templateUrlBase + 'content-parent.html', 
                controller: 'ContentController'
            })
            .when('/cart', {
                templateUrl: templateUrlBase + 'cart.html',
                controller: 'CartController'
            })
            .when('/checkout', {
                templateUrl: templateUrlBase + 'checkout.html',
                controller: 'CheckoutController'
            })
            .otherwise({ redirectTo: '/products' });
        }
    ]
);
