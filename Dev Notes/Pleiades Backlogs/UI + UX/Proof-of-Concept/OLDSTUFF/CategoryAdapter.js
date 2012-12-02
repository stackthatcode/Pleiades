
function CategoryDataAdapter(errorCallback, showLoadingCallback, hideLoadingCallback) {
	var self = this;
	self.ErrorCallback = errorCallback;
	self.ShowLoadingCallback = showLoadingCallback;
	self.HideLoadingCallback = hideLoadingCallback;
	
	self.DataStore = [
		{ ParentId: null, Id: "1001", Name: "Good Gear Section", SEO: "Men's Section" },		
			{ ParentId: "1001", Id: "3", Name: "Shoes", SEO: "sample-seo-text" },
				{ ParentId: "3", Id: "4", Name: "Golf Shoes", SEO: "sample-seo-text-4" },
				{ ParentId: "3", Id: "5", Name: "Playah Shoes", SEO: "sample-seo-text-5" },
				{ ParentId: "3", Id: "6", Name: "Mountain Shoes", SEO: "sample-seo-text-6" },
				{ ParentId: "3", Id: "7", Name: "Ass-Kicking Shoes", SEO: "sample-seo-text-7" },
			{ ParentId: "1001", Id: "8", Name: "Jiu-jitsu Gear", SEO: "sample-seo-text-8" },
				{ ParentId: "8", Id: "9", Name: "Choke-proof Gis", SEO: "sample-seo-text-9" },
				{ ParentId: "8", Id: "10", Name: "Belts", SEO: "sample-seo-text-10" },
				{ ParentId: "8", Id: "11", Name: "Mouth Guards", SEO: "sample-seo-text-11" } ,
			{ ParentId: "1001", Id: "12", Name: "Helmets", SEO: "sample-seo-text-12" },
				{ ParentId: "12", Id: "13", Name: "Open Face", SEO: "sample-seo-text-13" },
				{ ParentId: "12", Id: "14", Name: "Vintage", SEO: "sample-seo-text-14" },
				{ ParentId: "12", Id: "15", Name: "Viking", SEO: "sample-seo-text-15" },
				{ ParentId: "12", Id: "16", Name: "Samurai", SEO: "sample-seo-text-16" },
		
		{ ParentId: null, Id: "1002", Name: "MMA Section", SEO: "Men's Section" },
			{ ParentId: "1002", Id: "17", Name: "Boxing", SEO: "sample-seo-text-17" },
				{ ParentId: "17", Id: "18", Name: "Punching Bags", SEO: "sample-seo-text-18" },
				{ ParentId: "17", Id: "19", Name: "Gloves", SEO: "sample-seo-text-19" },
				{ ParentId: "17", Id: "20", Name: "Mouth Guards", SEO: "sample-seo-text-20" },
				{ ParentId: "17", Id: "21", Name: "Shorts", SEO: "sample-seo-text-21" },
			{ ParentId: "1002", Id: "22", Name: "Paddy Cake", SEO: "sample-seo-text-22" },
				{ ParentId: "22", Id: "23", Name: "Tea", SEO: "sample-seo-text-22" },
				{ ParentId: "22", Id: "24", Name: "Spice", SEO: "sample-seo-text-23" },
				{ ParentId: "22", Id: "25", Name: "Everything Nice", SEO: "sample-seo-text-23" },
				{ ParentId: "22", Id: "26", Name: "Crumpets", SEO: "sample-seo-text-24" },
				{ ParentId: "22", Id: "27", Name: "Crumpets X", SEO: "sample-seo-text-24" },
				{ ParentId: "22", Id: "28", Name: "Crumpets Y", SEO: "sample-seo-text-24" },
				{ ParentId: "22", Id: "29", Name: "Crumpets Z", SEO: "sample-seo-text-24" },
		{ ParentId: null, Id: "1000", Name: "Empty Section", SEO: "Empty Section" },
	];
	
	// These belong in the Common library (???)
	self.FindById = function(id) { 
		var output = self.FindAll(function(element) { return element.Id === id });
		if (output.length == 0)
			return null;
		else
			return output[0];
	}
	
	self.FindAll = function(lambda) {
		var output = [];
		$.each(self.DataStore, function(index, element) { if (lambda(element) === true) { output.push(element); } });
		return output;
	};
	
	self.FindByParentId = function(parentId) {
		var output = self.FindAll(function(element) { return element.ParentId === parentId });
		return output;
	}
	
	
	
	//
	// NOTE: this underscores the need to address asynchrony for ALL AJAX functions
	//
	self.SetErrorState = function() {
		var value = Math.floor((Math.random() * 10) + 1);
		if (value > 8 && self.ErrorCallback) {
			//self.ErrorCallback();
		}
		
		var wait = Math.floor((Math.random() * 10) + 1) * 1000;		
		//var wait = 0;
		if (self.ShowLoadingCallback && self.HideLoadingCallback) {
			//self.ShowLoadingCallback();
			//pausecomp(1000);
			//self.HideLoadingCallback();
		}
	}
	
	
		
	self.ExecuteAjaxStub = function(action, callback) {
		flow.exec(function() {
			var delay = 1000;
			self.ShowLoadingCallback();	
			window.setTimeout(this, delay);		
		}, function() {
			self.HideLoadingCallback();
			result = action();
			callback(result);
		});
	}
	
	self.ExecuteAjaxGet = function(url, payload, callback) {
		// TODO: stub out the JQuery Ajax stuff
	}

	self.ExecuteAjaxGet = function(url, payload, callback) {
		// TODO: stub out the JQuery Ajax stuff		
	}
	
	
	
	// *** Public Interface *** //
	self.RetrieveAllSections = function (callback) {
		self.ExecuteAjaxStub(
			function() { self.RetrieveByParentId(null) },
			callback,
		);
	}
	
	self.RetrieveAllCategoriesBySection = function (sectionId, callback) {
		self.ExecuteAjaxStub(
			function(callback) {
				var section = self.RetrieveById(sectionId);
				var output = [];
				$.each(section.Categories, function(index, element) { 
					output.push(element);
				});
				
				ajaxCallback(output);
			},
			callback
		);
	}
	
	self.RetrieveById = function (id, callback) {
		self.ExecuteAjaxStub(
			function(ajaxCallback) {
				var category = DeepClone(self.FindById(id));
				category.Categories = [];
				
				var children = self.FindByParentId(id);
				$.each(children, function(index, element) { 
					category.Categories.push(self.RetrieveById(element.Id)); 
				});				
				ajaxCallback(category);
			}, 
			callback
		);
	}
	
	self.RetrieveByParentId = function (parentId, callback) {
		self.ExecuteAjaxStub(
			function(ajaxCallback) {
				var categories = self.FindByParentId(parentId);
				var output = [];
				$.each(categories, function(index, element) {			
					output.push(self.RetrieveById(element.Id)); 
				});				
				ajaxCallback(output);
			},
			callback
		);
	}
	
	
	self.Delete = function(id, callback) {
		self.ExecuteAjaxStub(
			function(ajaxCallback) {
				self.DataStore.removeByLambda(function(element) { return element.Id === id; });
				ajaxCallback();
			},
			callback
		);
	}
	
	self.SwapParentChild = function(parent, child, callback) {
		self.ExecuteAjaxStub(
			function(ajaxCallback) {
				child.ParentId = parent.ParentId;		
				self.Save(child);
				
				parent.ParentId = child.Id;
				self.Save(parent);
				
				var allOtherChildren = self.RetrieveByParentId(parent.Id);
				$.each(allOtherChildren, function (index, elem) {
					elem.ParentId = child.Id;
					self.Save(elem);
				});
				
				callback();
			},
			callback
		);
	}
	
	self.Save = function(category, callback) {		
		self.ExecuteAjaxStub(
			function(ajaxCallback) {
				if (category.Id == null) {
					// TODO: check for wrong ParentId 123
					var newId = (100 + Math.floor(Math.random() * 11)).toString();
					
					self.DataStore.push({
						Id: newId,
						ParentId: category.ParentId,
						Name: category.Name,
						SEO: category.SEO,
					});
					
					self.SetErrorState(); // GET RID OFF!!!
					ajaxCallback(newId);
				}
				else {
					if (category.Id === category.ParentId) {
						throw "Oh noes! Infinite loop!";
					}
					
					var persistCategory = self.FindById(category.Id);
					persistCategory.ParentId = category.ParentId;
					persistCategory.Name = category.Name;
					persistCategory.SEO = category.SEO;
					
					self.SetErrorState(); // GET RID OFF!!!
					ajaxCallback(persistCategory.Id);
				}
			},
			callback
		);
	}	
}
