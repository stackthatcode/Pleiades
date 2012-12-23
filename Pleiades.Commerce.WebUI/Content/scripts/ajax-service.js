
function AjaxService(errorCallback, showLoadingCallback, hideLoadingCallback) {
	var self = this;
    self.Timeout = 15000;
	self.ShowLoadingCallback = showLoadingCallback;
	self.HideLoadingCallback = hideLoadingCallback;
	self.ErrorCallback = function() {
        self.HideLoadingCallback();
        errorCallback();
    }

    self.AjaxGet = function(url, successFunc) {
        flow.exec(
            function() {
                self.ShowLoadingCallback();
                $.ajax({
                    type: 'GET',
                    url: url,
                    timeout: self.Timeout,  
                    error: self.ErrorCallback,
                    success: this
                });
            },
            function(data, textStatus, jqXHR) {
                self.HideLoadingCallback();
                successFunc(data, textStatus, jqXHR);
            }
        );
    }

    self.AjaxPost = function(url, data, successFunc) {
        flow.exec(
            function() {
                self.ShowLoadingCallback();
                $.ajax({
                    type: 'POST',
                    url: url,
                    data: data,
                    timeout: self.Timeout,
                    error: self.ErrorCallback,
                    success: this
                });
            },
            function(data, textStatus, jqXHR) {
                self.HideLoadingCallback();
                successFunc(data, textStatus, jqXHR);
            }
        );
    }    	
}