
function SizeService(errorCallback, showLoadingCallback, hideLoadingCallback) {
    var self = new AjaxService(errorCallback, showLoadingCallback, hideLoadingCallback);
    	
	self.Sizes = function(callback) {
	    self.AjaxGet("/Admin/Size/SizeGroups", callback);
	}

	self.SizeGroup = function(id, callback) {
	    self.AjaxGet("/Admin/Size/SizeGroup/" + id, callback);
	}
	
	self.InsertGroup = function(group, callback) {
	    self.AjaxPost("/Admin/Size/InsertGroup", group, callback);
	}

	self.InsertSize = function(size, callback) {
	    self.AjaxPost("/Admin/Size/InsertSize", size, callback);
	}

	self.UpdateGroup = function (group, callback) {
	    self.AjaxPost("/Admin/Size/UpdateGroup", group, callback);
	}

	self.UpdateSize = function (size, callback) {
	    self.AjaxPost("/Admin/Size/UpdateSize", size, callback);
	}

	self.DeleteGroup = function (group, callback) {
	    self.AjaxPost("/Admin/Size/DeleteGroup", group, callback);
	}

	self.DeleteSize = function (size, callback) {
	    self.AjaxPost("/Admin/Size/DeleteSize", size, callback);
	}

	return self;
}