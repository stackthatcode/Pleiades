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
