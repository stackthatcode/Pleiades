'use strict';


app.controller('ContactController', function ($scope, $http) {
    $scope.email = "";
    $scope.message = "";
    
    $("#contact-info").validate({
        rules: {
            email: { required: true, email: true },
            message: { required: true },
        },
        highlight: function (element) {
            $(element).closest('.form-group').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('has-error');
        },
        errorPlacement: function (error, element) {
            return true;
        },
        errorContainer: $("#validation-error"),
    });
    
    $scope.send = function () {
        if ($("#contact-info").valid()) {
            var url = 'contact?email=' + $scope.email + '&body=' + $scope.message;
            ngAjax.Post($http, url, null, function (data) {
                $("#validation-info").show();
                $("#email").attr("disabled", "disabled");
                $("#message").attr("disabled", "disabled");
                $("#send").attr("disabled", "disabled");
            });            
        }
    };
});
