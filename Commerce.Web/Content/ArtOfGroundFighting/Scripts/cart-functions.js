'use strict';

var CartResponseCodeData = [
    {
        Id: "1",
        ResponseFunction: null
    },
    {
        Id: "2",
        ResponseFunction: null
    },
    {
        Id: "3",
        ResponseFunction: function () {
            ngAjax.ModalGeneric("No Items Added to Cart", "Due to changes in inventory, items were removed from your cart.");
        }
    },
    {
        Id: "4",
        ResponseFunction: null,
        //function () {
        //    ngAjax.ModalGeneric("Items Added to Cart", "Due to changes in inventory, reduced quantities were added to your cart.");
        //}
    },
    {
        Id: "5",
        ResponseFunction: function () {
            ngAjax.ModalGeneric("No Items Added to Cart", "You already have the maximum amount of that Item in your cart.");
        }
    },
    {
        Id: "6",
        ResponseFunction: function () {
            ngAjax.ModalGeneric("No Items Added to Cart", "That item is no longer available.");
        }
    },
    {
        Id: "7",
        ResponseFunction: function () {
            ngAjax.ModalGeneric("No Changes Made", "That item is no longer in your cart.");
        }
    },    
];

var CartResponseFunction = function(code) {
    var response = AQ(CartResponseCodeData).firstOrDefault(function (x) {
        return x.Id == code;
    });
    if (response) {
        return response.ResponseFunction;
    }
};
