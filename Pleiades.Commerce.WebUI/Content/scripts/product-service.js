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

    self.Images = function (productId, callback) {
        self.AjaxGet("/Admin/Product/Images/" + productId, callback);
    };

    self.AddColorToColorList = function (color, callback) {
        self.AjaxPost("/Admin/Color/Insert", color, callback);
    };

    self.Insert = function (product, callback) {
        self.AjaxPost("/Admin/Product/Insert", product, callback);
	};

	self.AddProductColor = function (productId, colorId, callback) {
	    self.AjaxPost("/Admin/Product/AddProductColor/" + productId + "?colorId=" + colorId, {}, callback);
	};

	self.UpdateColorOrder = function (productId, sorted, callback) {
	    self.AjaxPost("/Admin/Product/UpdateColorOrder/" + productId + "?sorted=" + sorted, callback);
	};

	self.AddProductImage = function (productId, image, callback) {
	    self.AjaxPost("/Admin/Product/AddProductImage/" + productId, image, callback);
	};

	self.UpdateImageOrder = function (productId, sorted, callback) {
	    self.AjaxPost("/Admin/Product/UpdateImageOrder/" + productId + "?sorted=" + sorted, callback);
	};

	self.CreateBitmap = function (createRequest, callback) {
	    self.AjaxPost("/Admin/Product/CreateBitmap", createRequest, callback);
	};

    self.Update = function (product, callback) {
        self.AjaxPost("/Admin/Product/Update", product, callback);
	};

    self.Delete = function (product, callback) {
        self.AjaxPost("/Admin/Product/Delete", product, callback);
	};

    self.DeleteProductColor = function (productId, productColorId, callback) {
        self.AjaxPost("/Admin/Product/DeleteProductColor/" + productId + "?productColorId=" + productColorId, {}, callback);
    };

    self.DeleteProductImage = function (productId, productImageId, callback) {
        self.AjaxPost("/Admin/Product/DeleteProductImage/" + productId + "?productImageId=" + productImageId, {}, callback);
    };

	return self;
}