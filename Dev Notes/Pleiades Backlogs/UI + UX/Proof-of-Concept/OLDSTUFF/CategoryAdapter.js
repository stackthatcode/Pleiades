
// I WANT SYMMETRY!  I WANT TO MAKE ASSEMBLING STUFF EASY FOR THE SERVER.  <=== FUCK YOU!!!  BUILDING JSON IS TRIVIAL FOR C#

// UPDATE: everything was upside down.  This should store everything flat for ease-of-manipulation, then translate into JSON accordingly.

// THEREFORE: refactor this and eliminate the tree structure bullshit in our mock data layer.

function CategoryDataAdapter() {
	var self = this;
	self.Categories = [
		{ 
			Id: "3", Name: "Shoes", SEO: "sample-seo-text", 
			Categories: [
				{ ParentId: "3", Id: "4", Name: "Golf Shoes", SEO: "sample-seo-text-4" },
				{ ParentId: "3", Id: "5", Name: "Playah Shoes", SEO: "sample-seo-text-5" },
				{ ParentId: "3", Id: "6", Name: "Mountain Shoes", SEO: "sample-seo-text-6" },
				{ ParentId: "3", Id: "7", Name: "Ass-Kicking Shoes", SEO: "sample-seo-text-7" },
			]
		},
		{
			Id: "8", Name: "Jiu-jitsu Gear", SEO: "sample-seo-text-8",
			Categories: [
				{ ParentId: "8", Id: "9", Name: "Choke-proof Gis", SEO: "sample-seo-text-9" },
				{ ParentId: "8", Id: "10", Name: "Belts", SEO: "sample-seo-text-10" },
				{ ParentId: "8", Id: "11", Name: "Mouth Guards", SEO: "sample-seo-text-11" } ,
			]
		},
		{
			Id: "12", Name: "Helmets", SEO: "sample-seo-text-12",
			Categories: [
				{ ParentId: "12", Id: "13", Name: "Open Face", SEO: "sample-seo-text-13" },
				{ ParentId: "12", Id: "14", Name: "Vintage", SEO: "sample-seo-text-14" },
				{ ParentId: "12", Id: "15", Name: "Viking", SEO: "sample-seo-text-15" },
				{ ParentId: "12", Id: "16", Name: "Samurai", SEO: "sample-seo-text-16" },
			],
		},
		{
			Id: "17", Name: "Boxing", SEO: "sample-seo-text-17",
			Categories: [
				{ ParentId: "17", Id: "18", Name: "Punching Bags", SEO: "sample-seo-text-18" },
				{ ParentId: "17", Id: "19", Name: "Gloves", SEO: "sample-seo-text-19" },
				{ ParentId: "17", Id: "20", Name: "Mouth Guards", SEO: "sample-seo-text-20" },
				{ ParentId: "17", Id: "21", Name: "Shorts", SEO: "sample-seo-text-21" },
			],
		},
		{
			Id: "22", Name: "Paddy Cake", SEO: "sample-seo-text-22",
			Categories: [
				{ ParentId: "22", Id: "21", Name: "Tea", SEO: "sample-seo-text-22" },
				{ ParentId: "22", Id: "22", Name: "Spice", SEO: "sample-seo-text-23" },
				{ ParentId: "22", Id: "23", Name: "Everything Nice", SEO: "sample-seo-text-23" },
				{ ParentId: "22", Id: "24", Name: "Crumpets", SEO: "sample-seo-text-24" },
			],
		},
	];
	
	self.Find = function(id) { 
		var output = self.FindAll(function(node) { return node.Id === id });
		return output[0];
	}
	
	self.FindParent = function(id) {
		var output = [];
		var lambda = function(node) { 
			if (node.Categories) {
				var index = node.Categories.indexOf(function(node2) { return node2.Id === id; } );
				return index != -1;  
			} 
			else {
				return false;
			}
		};
		self.FindWorker(self, lambda, output);
		return output[0];
	}
	
	self.FindAll = function(lambda) {
		var output = [];
		self.FindWorker(self, lambda, output);
		return output;
	};
	
	self.FindWorker = function(node, lambda, output) {
		if (lambda(node)) {
			output.push(node);
		}
		if (node.Categories) {
			$.each(node.Categories, function(index, element) { self.FindWorker(element, lambda, output); });
		}
	};
	
	self.RetrieveAllCategories = function () {
		var output = [];
		$.each(self.Categories, function(index, element) { output.push(DeepClone(element)); });
		return output;
	}
	
	self.RetrieveSingleCategory = function (id) {
		return DeepClone(self.Find(id));
	}
	
	self.Delete = function(id) {
		var parent = self.FindParent(id);
		if (parent) {
			parent.Categories.removeByLambda( function(node) { return node.Id === id; });
		}
	}
	
	// To support out madness, it's necessary to make this more sophisticated.  Ok?  Ok.
	self.SaveSingleCategory = function (category) {
		if (category.Id == null) {
			var newId = (100 + Math.floor(Math.random() * 11)).toString();
			category.Id = newId;
			if (category.ParentId) {
				var parent = self.Find(category.ParentId);
				parent.Categories.unshift(category);
			}
			else {
				self.Categories.unshift(category);
			}
			return category.Id;
		}
		else {
			var existingCategory = self.Find(category.Id);
			existingCategory.Name = category.Name;
			existingCategory.SEO = category.SEO;
			
			if (existingCategory.ParentId !== category.ParentId) {
				self.Delete(category.Id);
				existingCategory.ParentId = category.ParentId;
				var parent = self.Find(category.ParentId);
				parent.Categories.push(existingCategory);
			}
			
			return existingCategory.Id;
		}
	}
}
