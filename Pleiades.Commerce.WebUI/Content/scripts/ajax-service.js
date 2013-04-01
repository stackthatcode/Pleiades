
function AjaxService(errorCallback, showLoadingCallback, hideLoadingCallback) {
	var self = this;
	
    // Configurable Settings
    self.Timeout = 5000;
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
                    url: self.BaseUrl + url,
                    data: data,
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