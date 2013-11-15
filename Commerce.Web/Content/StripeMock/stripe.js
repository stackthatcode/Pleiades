var stripe = namespace("Stripe");

stripe.card = {
    createToken: function(form, responseHandler) {
        var response = {
            id: "tok_u5dg20Gra",    // String of token identifier,
            card: {},               // Dictionary of the card used to create the token
            created: 1383448534,    // Integer of date token was created
            currency: "usd",        // String currency that the token was created in
            livemode: true,         // Boolean of whether this token was created with a live or test API key
            object: "token",        // String identifier of the type of object, always "token"
            used: false,            // Boolean of whether this token has been used,  
        };
        
        //response.error = { message: "Unable to Generate your Card.  Somethings wrong!" };
        // TODO: card 4111* is invalid
        // TODO: card 4242* is valid        
        responseHandler("OK", response);
    },
};

stripe.setPublishableKey = function(key) {
    console.log("Public Key: " + key);
};
