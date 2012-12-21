
function CategoryService(errorCallback, showLoadingCallback, hideLoadingCallback) {
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
	
	
	// *** Public Interface
	self.RetrieveAllSections = function (callback) {
        self.AjaxGet("/Pleiades/Admin/Category/Sections", callback);
	}
	
	self.RetrieveAllCategoriesBySection = function (sectionId, callback) {
        self.AjaxGet("/Pleiades/Admin/Category/CategoriesBySection/" + sectionId, callback);
	}
	
	self.RetrieveById = function (id, callback) {
        self.AjaxGet("/Pleiades/Admin/Category/Category/" + id, callback);
	}


	self.Save = function (category, callback) {
	    if (category.Id == null) {
	        self.AjaxPost("/Pleiades/Admin/Category/Insert", category, callback);
	    } else {
	        self.AjaxPost("/Pleiades/Admin/Category/Update", category, callback);
	    }
	}

	self.Delete = function (id, callback) {
	    self.AjaxPost("/Pleiades/Admin/Category/Delete/" + id, {}, callback);
	}

	self.SwapParentChild = function (parent, child, callback) {
	    self.AjaxPost("/Pleiades/Admin/Category/SwapParentChild/" + parent.Id + "/" + child.Id, {}, callback);
	}
}