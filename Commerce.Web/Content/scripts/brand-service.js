function BrandService(errorCallback, showLoadingCallback, hideLoadingCallback) {
    var self = new AjaxService(errorCallback, showLoadingCallback, hideLoadingCallback);

    self.RetrieveAll = function (callback) {
        self.AjaxGet("/Admin/Brand/Brands", callback);
    };

    self.Retrieve = function (id, callback) {
        self.AjaxGet("/Admin/Brand/Brand/" + id, callback);
    };

    self.Insert = function (brand, callback) {
        self.AjaxPost("/Admin/Brand/Insert", brand, callback);
	};

    self.Update = function (brand, callback) {
        self.AjaxPost("/Admin/Brand/Update", brand, callback);
	}

    self.Delete = function (brand, callback) {
        self.AjaxPost("/Admin/Brand/Delete", brand, callback);
	};

	return self;
}