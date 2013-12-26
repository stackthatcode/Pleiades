PushLibrary = namespace("PushLibrary");

PushLibrary.AjaxSettings = {
    // Override these settings in the PushLibrary Shim
    BaseUrl: "/Commerce.Web",
    Timeout: 60000,
    ModalErrorTagSelector : "#modal-error",
    SpinnerLayerTagSelector : "#spinner-layer",
};

PushLibrary.Ajax = function() {
    var self = this;

    // Configurable Settings  
    self.Error = function() {
        $(ajaxSettings.ModalErrorTagSelector).modal();
        return false;
    };

    self.ErrorCallback = function(jqXHR, textStatus, errorThrown) {
        if (jqXHR.status != 0 || textStatus == "timeout") {
            self.HideLoading();
            self.Error();
        }
    };

    self.AjaxGet = function(url, successFunc) {
        flow.exec(
            function() {
                self.ShowLoading();
                $.ajax({
                    type: 'GET',
                    url: ajaxSettings.BaseUrl + url,
                    timeout: ajaxSettings.Timeout,
                    error: self.ErrorCallback,
                    success: this
                });
            },
            function(data, textStatus, jqXHR) {
                self.HideLoading();
                if (successFunc) {
                    successFunc(data, textStatus, jqXHR);
                }
            }
        );
    };

    self.AjaxPost = function(url, data, successFunc) {
        flow.exec(
            function() {
                self.ShowLoading();
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    url: ajaxSettings.BaseUrl + url,
                    data: JSON.stringify(data),
                    timeout: ajaxSettings.Timeout,
                    error: self.ErrorCallback,
                    success: this
                });
            },
            function(data, textStatus, jqXHR) {
                self.HideLoading();
                if (successFunc) {
                    successFunc(data, textStatus, jqXHR);
                }
            }
        );
    };

    self.ShowLoading = function() {
        $(ajaxSettings.SpinnerLayerTagSelector).show();
    };

    self.HideLoading = function() {
        $(ajaxSettings.SpinnerLayerTagSelector).hide();
    };
};
