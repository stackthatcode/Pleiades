function ProductService(errorCallback, showLoadingCallback, hideLoadingCallback) {
    var self = new AjaxService(errorCallback, showLoadingCallback, hideLoadingCallback);

    // Search
    self.Search = function (brandId, categoryId, searchText, callback) {
        self.AjaxGet("/Admin/Product/Search" +
            "?brandId=" + brandId + "&categoryId=" + categoryId + "&searchText=" + searchText, callback);
    };

    self.Categories = function (callback) {
        self.AjaxGet("/Admin/Product/Categories", callback);
    };

    self.Brands = function (callback) {
        self.AjaxGet("/Admin/Product/Brands", callback);
    };

    self.SizeGroups = function (callback) {
        self.AjaxGet("/Admin/Product/SizeGroups", callback);
    };


    // Info
    self.Info = function (productId, callback) {
        self.AjaxGet("/Admin/Product/Info/" + productId, callback);
    };

    self.Update = function (product, callback) {
        self.AjaxPost("/Admin/Product/Update", product, callback);
    };

    self.Delete = function (id, callback) {
        self.AjaxPost("/Admin/Product/Delete/" + id, {}, callback);
    };

    self.Save = function (product, callback) {
        self.AjaxPost("/Admin/Product/Save", product, callback);
    };


    // Sizes
    self.SizeGroups = function (callback) {
        self.AjaxGet("/Admin/Size/SizeGroups", callback);
    }

    self.AddProductSize = function (productId, sizeId, callback) {
        self.AjaxPost("/Admin/Product/AddProductSize/" + productId + "?sizeId=" + sizeId, {}, callback);
    };

    self.Sizes = function (productId, callback) {
        self.AjaxGet("/Admin/Product/Sizes/" + productId, callback);
    };

    self.DeleteProductSize = function (productId, sizeId, callback) {
        self.AjaxPost("/Admin/Product/DeleteProductSize/" + productId + "?sizeId=" + sizeId, {}, callback);
    };

    self.UpdateSizeOrder = function (productId, sortedIds, callback) {
        self.AjaxPost("/Admin/Product/UpdateSizeOrder/" + productId + "?sorted=" + sortedIds, {}, callback);
    };



    // Colors
    self.Colors = function (productId, callback) {
        self.AjaxGet("/Admin/Product/Colors/" + productId, callback);
    };

    self.ColorsList = function (callback) {
        self.AjaxGet("/Admin/Color/Colors", callback);
    };

    self.AddColorToColorList = function (color, callback) {
        self.AjaxPost("/Admin/Color/Insert", color, callback);
    };

	self.AddProductColor = function (productId, colorId, callback) {
	    self.AjaxPost("/Admin/Product/AddProductColor/" + productId + "?colorId=" + colorId, {}, callback);
	};

	self.UpdateColorOrder = function (productId, sorted, callback) {
	    self.AjaxPost("/Admin/Product/UpdateColorOrder/" + productId + "?sorted=" + sorted, {}, callback);
	};

	self.DeleteProductColor = function (productId, productColorId, callback) {
	    self.AjaxPost("/Admin/Product/DeleteProductColor/" + productId + "?productColorId=" + productColorId, {}, callback);
	};

	self.ChangeImageColor = function (productId, productImageId, newColorId, callback) {
	    self.AjaxPost(
                "/Admin/Product/ChangeImageColor/" + productId +
                    "?productImageId=" + productImageId + "&newColorId=" + newColorId, {}, callback);
	};

	self.AssignImagesToColor = function (productId, callback) {
	    self.AjaxPost("/Admin/Product/AssignImagesToColor/" + productId, {}, callback);
	};

	self.UnassignImagesFromColor = function (productId, callback) {
	    self.AjaxPost("/Admin/Product/UnassignImagesFromColor/" + productId, {}, callback);
	};


    // Images
	self.Images = function (productId, callback) {
	    self.AjaxGet("/Admin/Product/Images/" + productId, callback);
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

    self.DeleteProductImage = function (productId, productImageId, callback) {
        self.AjaxPost("/Admin/Product/DeleteProductImage/" + productId + "?productImageId=" + productImageId, {}, callback);
    };

	return self;
}