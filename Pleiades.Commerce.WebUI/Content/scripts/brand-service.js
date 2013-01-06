function SectionService(errorCallback, showLoadingCallback, hideLoadingCallback) {
    var self = new AjaxService(errorCallback, showLoadingCallback, hideLoadingCallback);

    self.RetrieveAllSections = function (callback) {
        self.AjaxGet("/Admin/Section/Sections", callback);
    };

    self.Insert = function (section, callback) {
	    self.AjaxPost("/Admin/Section/Insert", section, callback);
	};

	self.Update = function (section, callback) {
	    self.AjaxPost("/Admin/Section/Update", section, callback);
	}

	self.Delete = function (section, callback) {
	    self.AjaxPost("/Admin/Section/Delete", section, callback);
	};

	return self;
}