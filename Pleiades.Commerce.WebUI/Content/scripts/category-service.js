
function CategoryService(errorCallback, showLoadingCallback, hideLoadingCallback) {
    var self = new AjaxService(errorCallback, showLoadingCallback, hideLoadingCallback);

    self.RetrieveAllSections = function (callback) {
        self.AjaxGet("/Admin/Category/Sections", callback);
    };

	self.RetrieveAllCategoriesBySection = function (sectionId, callback) {
	    self.AjaxGet("/Admin/Category/CategoriesBySection/" + sectionId, callback);
	};

	self.RetrieveById = function (id, callback) {
	    self.AjaxGet("/Admin/Category/Category/" + id, callback);
	};

	self.Save = function (category, callback) {
	    if (category.Id == null) {
	        self.AjaxPost("/Admin/Category/Insert", category, callback);
	    } else {
	        self.AjaxPost("/Admin/Category/Update", category, callback);
	    }
	};

	self.Delete = function (id, callback) {
	    self.AjaxPost("/Admin/Category/Delete/" + id, {}, callback);
	};

	self.SwapParentChild = function (parent, child, callback) {
	    self.AjaxPost("/Admin/Category/SwapParentChild/" + parent.Id + "/" + child.Id, {}, callback);
	};

	return self;
}