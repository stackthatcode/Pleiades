function ColorService(errorCallback, showLoadingCallback, hideLoadingCallback) {
    var self = new AjaxService(errorCallback, showLoadingCallback, hideLoadingCallback);

    self.RetrieveAll = function (callback) {
        self.AjaxGet("/Admin/Color/Colors", callback);
    };

    self.Retrieve = function (id, callback) {
        self.AjaxGet("/Admin/Color/Color/" + id, callback);
    };

    self.Insert = function (color, callback) {
        self.AjaxPost("/Admin/Color/Insert", color, callback);
	};

    self.Update = function (color, callback) {
        self.AjaxPost("/Admin/Color/Update", color, callback);
	}

	self.CreateBitmap = function (createRequest, callback) {
	    self.AjaxPost("/Admin/Color/CreateBitmap", createRequest, callback);        
	}

    self.Delete = function (color, callback) {
        self.AjaxPost("/Admin/Color/Delete", color, callback);
	};

	return self;
}