// TODO: how to add this to Array's prototype...?
Array.prototype.firstOrNull = function(lambda) {
	for (var arrayIndex = 0; arrayIndex < this.length; arrayIndex += 1) {
		var element = this[arrayIndex];
		if (lambda(element) === true) {
			return element;
		}
	}
	return null;
}

// What does the "int" keyword do...?		
Array.prototype.indexByLambda = function(lambda) {
	for (var arrayIndex = 0; arrayIndex < this.length; arrayIndex += 1) {
		if (lambda(this[arrayIndex]) === true) {
			return arrayIndex;
		}
	}
	return -1;
};

Array.prototype.removeByLambda = function(lambda) {
	var index = this.indexByLambda(lambda);
	this.remove(index, index + 1);
};

Array.prototype.remove = function(from, to) {
	var rest = this.slice((to || from) + 1 || this.length);
	this.length = from < 0 ? this.length + from : from;
	return this.push.apply(this, rest);
};

function DeepClone(input) {
	// Handle the 3 simple types, and null or undefined
	if (null == input || "object" != typeof input) return input;
	
	// Handle Date
	if (input instanceof Date) {
		var copy = new Date();
		copy.setTime(input.getTime());
		return copy;
	}
	
	// Handle Array
	if (input instanceof Array) {
		var copy = [];
		for (var i = 0, len = input.length; i < len; ++i) {
			copy[i] = DeepClone(input[i]);
		}
		return copy;
	}

	// Handle Object
	if (input instanceof Object) {
		var copy = {};
		for (var attr in input) {
			if (input.hasOwnProperty(attr)) copy[attr] = DeepClone(input[attr]);
		}
		return copy;
	}
	
	throw new Error("Unable to copy input! Its type isn't supported.");
}

function GliderWidget(glidingContainer, parentDiv, childDiv) {
	var self = this;
	self.LastParentScroll = 0;
	
	self.GlideToChild = function() {
		// save the old scroll position and set the new
		self.LastParentScroll = $(window).scrollTop();
		window.scrollTo(0, 0);
		
		// REMOVE - should happen INSIDE the prepareTheEditor callback
		// $("#sleeve2").find("h4").html("You selected: " + adjustedIndex);
		
		// prepare the editor
		$(childDiv).css({ display: "block", left: "940px", top: "0px" });
		
		// open the curtains
		$(parentDiv + ',' + childDiv).animate(
			{ left: "-=940" }, 
			{ 
				complete: function() {
					$(parentDiv).css({ display: "none" });
					$(glidingContainer).css({ height: $(childDiv).height() }); 
				},
				duration: 200
			});
		
		return false;
	};
	
	self.GlideToParent = function () {	
		// Should happen AFTER the prepareTheEditor callback						
		//$("#sleeve2 a[id='editorBack']").bind("click", function(event) {
		
		$(parentDiv).css({ display: "block" });
		$(glidingContainer).css({ height: $(parentDiv).height() });
		
		$(parentDiv + "," + childDiv).animate(
			{ left: "+=940" }, 
			{ duration: 200, complete: function() { window.scrollTo(0, self.LastParentScroll); } }
		);
	};
}

// TODO: I strongly desire functions which allow me to iterate through an object graph
// TODO: keep, but deprecated for now
var ApplyVisitor = function(where, operation) {
	$.each(self.DataStore, function(index, element) {
		if (where(element)) {
			operation(element);
		}
		
		$.each(self.ChildCategories, function(index2, element2) {
			if (where(element2)) {
				operation(element2);
			}
		});
	});
}		
