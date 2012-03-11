// TODO: Again: how to test if JQuery has been properly loaded, etc...?

// *** Ajax Get method - provides uniform wrapper for JQuery AJAX functionality
// successFunc = anonymous callback method
// errorFunc = ...
function AjaxGet(url, successFunc, errorFunc) {
    $.ajax({
        //type: 'GET',
        url: url,
        timeout: 15000,  
        error: function (jqXHR, textStatus, errorThrown) { errorFunc(jqXHR, textStatus, errorThrown) },
        success: successFunc    
    });
}

// *** Ajax Post method - provides uniform wrapper for JQuery AJAX functionality
// successFunc = anonymous callback method
// errorFunc = ...
function AjaxPost(url, data, successFunc, errorFunc) {
    $.ajax({
        type: 'POST',
        url: url,
        data: data,
        timeout: 15000,  
        error: function (jqXHR, textStatus, errorThrown) { errorFunc(jqXHR, textStatus, errorThrown) },
        success: successFunc   
    });
}

// Always available Global context reference
var Global = (function () { return this; }).call();

// Returns anonymous function object which is invokable
function CreateDelegate(object, method) {
    return function () {
        // Arguments will be passed automatically to the delegate at the time that it's invoked
        method.apply(object, arguments);
    }
}

