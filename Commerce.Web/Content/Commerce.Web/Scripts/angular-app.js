'use strict';

angular.module('push-market', []).
  config(['$routeProvider', function($routeProvider) {
        $routeProvider
          .when('/products', {
              templateUrl: 'Content/Commerce.Web/ng-templates/ProductList.html', 
              controller: 'ListController'
          })
          .when('/products/:productid', {
              templateUrl: 'Content/Commerce.Web/ng-templates/ProductDetail.html', 
              controller: 'DetailController'
          })
          .otherwise({ redirectTo: '/products' });
    }
  ]);
