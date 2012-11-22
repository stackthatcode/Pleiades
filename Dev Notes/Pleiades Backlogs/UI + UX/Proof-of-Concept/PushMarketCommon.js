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

Array.prototype.arrayFirstIndexOf = function(predicate, predicateOwner) {
    for (var i = 0, j = this.length; i < j; i++) {
        if (predicate.call(predicateOwner, this[i])) {
            return i;
        }
    }
    return -1;
}

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
	
	self.GlideToChild = function(completeFunc) {
		// save the old scroll position and set the new
		self.LastParentScroll = $(window).scrollTop();
		window.scrollTo(0, 0);
		
		// prepare the editor
		$(childDiv).css({ 
			display: "block", left: "940px", top: "0px" 
		});
		
		// open the curtains
		$(parentDiv).animate({ 	
				left: "-=940" 
			}, 
			{ 	
				complete: function() {
					$(parentDiv).css({ display: "none" });
				},
				duration: 250
			});
		
		$(childDiv).animate({ 	
				left: "-=940"
			}, 
			{ 	
				complete: function() {
					if (completeFunc) { 
						completeFunc();
					}
				},
				duration: 250
			});
		
		
		return false;
	};
	
	self.GlideToParent = function (completeFunc) {	
		$(parentDiv).css({ display: "block" });
		
		$(childDiv).animate({ 	
				left: "+=940" 
			}, 
			{ 	
				complete: function() {
					$(childDiv).css({ display: "none" });
				},
				duration: 250, 
			}
		);
		$(parentDiv).animate({ 	
				left: "+=940" 
			}, 
			{ 	
				complete: function() {
					if (completeFunc) { 
						completeFunc();
					}
				},
				duration: 250,
			}
		);
		
	};
}
