
function AjaxService(errorCallback, showLoadingCallback, hideLoadingCallback) {
	var self = this;
	
    // Configurable Settings
    self.Timeout = 60000;
    self.BaseUrl = "/Pleiades"; // TODO: move this into dynamic code

	self.ShowLoadingCallback = showLoadingCallback;
	self.HideLoadingCallback = hideLoadingCallback;
	self.ErrorCallback = function (jqXHR, textStatus, errorThrown) {
	    if (jqXHR.status != 0 || textStatus == "timeout") {
	        self.HideLoadingCallback();
	        errorCallback();
	    }
	}

    self.AjaxGet = function (url, successFunc) {
        flow.exec(
            function () {
                self.ShowLoadingCallback();
                $.ajax({
                    type: 'GET',
                    url: self.BaseUrl + url,
                    timeout: self.Timeout,
                    error: self.ErrorCallback,
                    success: this
                });
            },
            function (data, textStatus, jqXHR) {
                self.HideLoadingCallback();
                if (successFunc) {
                    successFunc(data, textStatus, jqXHR);
                }
            }
        );
    }

    self.AjaxPost = function (url, data, successFunc) {
        flow.exec(
            function () {
                self.ShowLoadingCallback();
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    url: self.BaseUrl + url,
                    data: JSON.stringify(data),
                    timeout: self.Timeout,
                    error: self.ErrorCallback,
                    success: this
                });
            },
            function (data, textStatus, jqXHR) {
                self.HideLoadingCallback();
                if (successFunc) {
                    successFunc(data, textStatus, jqXHR);
                }
            }
        );
    }    	
}