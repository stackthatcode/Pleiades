function ProductService(errorCallback, showLoadingCallback, hideLoadingCallback) {
    var self = new AjaxService(errorCallback, showLoadingCallback, hideLoadingCallback);
        self.Categories = function (callback) {
        self.AjaxGet("/Admin/Product/Categories", callback);
    };

    self.Brands = function (callback) {
        self.AjaxGet("/Admin/Product/Brands", callback);
    };

    self.Search = function (brandId, categoryId, searchText, callback) {
        self.AjaxGet("/Admin/Product/Search" +
            "?brandId=" + brandId + "&categoryId=" + categoryId + "&searchText=" + searchText, callback);
    };

    self.Retrieve = function (productId, callback) {
        self.AjaxGet("/Admin/Product/Retrieve/" + productId, callback);
    };

    self.Insert = function (color, callback) {
        self.AjaxPost("/Admin/Product/Insert", color, callback);
	};

	self.Update = function (color, callback) {
	    self.AjaxPost("/Admin/Product/Update", color, callback);
	};

	self.CreateBitmap = function (createRequest, callback) {
	    self.AjaxPost("/Admin/Product/CreateBitmap", createRequest, callback);
	};

    self.Delete = function (color, callback) {
        self.AjaxPost("/Admin/Product/Delete", color, callback);
	};

	return self;
}