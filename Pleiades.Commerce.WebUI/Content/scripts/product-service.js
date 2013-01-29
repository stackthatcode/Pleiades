function ProductService(errorCallback, showLoadingCallback, hideLoadingCallback) {
    var self = new AjaxService(errorCallback, showLoadingCallback, hideLoadingCallback);
    
    self.Categories = function (callback) {
        self.AjaxGet("/Admin/Product/Categories", callback);
    };

    self.Brands = function (callback) {
        self.AjaxGet("/Admin/Product/Brands", callback);
    };

    self.SizeGroups = function (callback) {
        self.AjaxGet("/Admin/Product/SizeGroups", callback);
    };

    self.Search = function (brandId, categoryId, searchText, callback) {
        self.AjaxGet("/Admin/Product/Search" +
            "?brandId=" + brandId + "&categoryId=" + categoryId + "&searchText=" + searchText, callback);
    };

    self.Info = function (productId, callback) {
        self.AjaxGet("/Admin/Product/Info/" + productId, callback);
    };

    self.Colors = function (productId, callback) {
        self.AjaxGet("/Admin/Product/Colors/" + productId, callback);
    };

    self.ColorsList = function (callback) {
        self.AjaxGet("/Admin/Color/Colors", callback);
    };

    self.Insert = function (product, callback) {
        self.AjaxPost("/Admin/Product/Insert", product, callback);
	};

    self.Update = function (product, callback) {
        self.AjaxPost("/Admin/Product/Update", product, callback);
	};


    self.Delete = function (product, callback) {
        self.AjaxPost("/Admin/Product/Delete", product, callback);
	};

	return self;
}