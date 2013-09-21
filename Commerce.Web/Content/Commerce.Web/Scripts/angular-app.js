'use strict';

/* App Module */

angular.module('phonecat', []).
  config(['$routeProvider', function($routeProvider) {
  $routeProvider.
      when('/products', { templateUrl: '/Content/C/phone-list.html', controller: ListController }).
      when('/products/:productid', { templateUrl: 'partials/phone-detail.html', controller: DetailController }).
      otherwise({redirectTo: '/phones'});
}]);
