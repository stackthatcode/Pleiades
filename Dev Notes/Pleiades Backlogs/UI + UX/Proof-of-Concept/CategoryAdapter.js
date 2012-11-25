
// I WANT SYMMETRY!  I WANT TO MAKE ASSEMBLING STUFF EASY FOR THE SERVER.  <=== FUCK YOU!!!  BUILDING JSON IS TRIVIAL FOR C#
// UPDATE: everything was upside down.  This should store everything flat for ease-of-manipulation, then translate into JSON accordingly.
// THEREFORE: refactor this and eliminate the tree structure bullshit in our mock data layer.


// Another Update: 
// 1.) problems if we use SectionId -- why does this bother me?
//		A.) RetrieveByParentId has SectionId
//		B.) Not beautiful abstract
//		C.) How often will we repeat ourselves...?  In the database!  In queries!
//		D.) Moving a Category to a new section is too much work


// 2.) problems if we use ParentId... 
//		A.) Query complexity... may have to run twice
//		SELECT * FROM Category t1 WHERE t1.ParentId = "1001" AND t2.ParentId = t1.Id

//  http://mikehillyer.com/articles/managing-hierarchical-data-in-mysql/

function CategoryDataAdapter() {
	var self = this;
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
	
	
	// *** Public Interface *** //	
	self.RetrieveById = function (id) {
		var category = DeepClone(self.FindById(id));
		category.Categories = [];
		
		var children = self.FindByParentId(id);
		$.each(children, function(index, element) { 
			category.Categories.push(self.RetrieveById(element.Id)); 
		});
		
		return category;
	}
	
	self.RetrieveByParentId = function (parentId) {
		var categories = self.FindByParentId(parentId);
		var output = [];
		$.each(categories, function(index, element) {			
			output.push(self.RetrieveById(element.Id)); 
		});
		return output;
	}
	
	self.RetrieveAllCategoriesBySection = function (sectionId) {
		var section = self.RetrieveById(sectionId);
		var output = [];
		$.each(section.Categories, function(index, element) { 
			output.push(element);
		});
		return output;
	}
	
	self.RetrieveAllSections = function () {
		return self.RetrieveByParentId(null);		
	}
	
	self.Delete = function(id) {
		self.DataStore.removeByLambda(function(element) { return element.Id === id; });
	}
	
	self.SwapParentChild = function(parent, child) {
		child.ParentId = parent.ParentId;		
		self.Save(child);
		
		parent.ParentId = child.Id;
		self.Save(parent);
		
		var allOtherChildren = self.RetrieveByParentId(parent.Id);
		$.each(allOtherChildren, function (index, elem) {
			elem.ParentId = child.Id;
			self.Save(elem);
		});
	}
	
	self.Save = function(category) {
		if (category.Id == null) {
			// TODO: check for wrong ParentId 123
			var newId = (100 + Math.floor(Math.random() * 11)).toString();
			
			self.DataStore.push({
				Id: newId,
				ParentId: category.ParentId,
				Name: category.Name,
				SEO: category.SEO,
			});
			
			return newId;
		}
		else {
			if (category.Id === category.ParentId) {
				throw "Oh noes! Infinite loop!";
			}
			
			var persistCategory = self.FindById(category.Id);
			persistCategory.ParentId = category.ParentId;
			persistCategory.Name = category.Name;
			persistCategory.SEO = category.SEO;
			
			return persistCategory.Id;
		}
	}
	
	
	// *** DEPRECATED *** //
	self.RetreiveProductsByCategoryId = function(id) {
		return [
			{ Id: "1", Name: "Black Beauty Gloves", Sku: "BBG-12346" },			
			{ Id: "2", Name: "Red Sparring Gloves", Sku: "BBG-44801" },
			{ Id: "3", Name: "White Gloves", Sku: "BBG-69892" },
			{ Id: "4", Name: "Suede Leather Boxing Gloves", Sku: "BBG-77778" },
		];
	}
}
