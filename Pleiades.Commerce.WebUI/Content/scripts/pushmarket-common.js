var CopyPropertiesFromKo = function(from, target) { 
    target = target || {};
    for(var propertyName in from) {
        target[propertyName] = (typeof from[propertyName] == "function") ? from[propertyName]() : from[propertyName];
    }
    return target;
}

String.prototype.koTrunc = function(n, useWordBoundary){
    var toLong = 
        this.length > n, s_ = toLong ? this.substr(0,n-1) : this;
        s_ = useWordBoundary && toLong ? s_.substr(0,s_.lastIndexOf(' ')) : s_;
    return toLong ? s_ + '...' : s_;
}

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
	this.remove(index, index);
};

Array.prototype.remove = function(from, to) {
	var rest = this.slice((to || from) + 1 || this.length);
	this.length = from < 0 ? this.length + from : from;
	return this.push.apply(this, rest);
};

String.prototype.trim=function(){return this.replace(/^\s+|\s+$/g, '');};

String.prototype.ltrim=function(){return this.replace(/^\s+/,'');};

String.prototype.rtrim=function(){return this.replace(/\s+$/,'');};

String.prototype.fulltrim=function(){return this.replace(/(?:(?:^|\n)\s+|\s+(?:$|\n))/g,'').replace(/\s+/g,' ');};

var ToMoney = function(input) {
    return "$" + input.toFixed(2);
}

function CommonUI(containerDiv) {
	var self = this;

	self.Error = function() {
		$('#modal-error').modal();
		return false;
	}
			
	self.ShowLoading = function() {
		$("#spinner-layer").show();
	}
	
	self.HideLoading = function() {
		$("#spinner-layer").hide();
	}

    // Scroll Tracking - TODO: how to make this cross-cutting...?
    
    self.ScrollToIdTracker = function(id) {
		if (!id) {
            return;
        }
		self.ScrollToTracker('tracker' + id);
	}
    
    self.ScrollToTracker = function(trackerId) {
		if (!trackerId) {
            return;
        }
		var offset = $("#" + trackerId).parent().offset().top - $(containerDiv).offset().top;
		$("body").scrollTop(offset);
	}

	self.HighlightByTrackerId = function(id) {
		var set = $('#tracker' + id).closest("td");
		set.animate({ backgroundColor: "#AAA" }, { duration: 1000 });
		set.animate({ 
                backgroundColor: "#FFF" 
            }, 
			{ 
				duration: 1000, 
				complete: function() { set.css({ backgroundColor: "" }); 
            } 
		});
	}
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
		$(parentDiv).css({ display: "block", left: "-940px", top: "0px"  });
		
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