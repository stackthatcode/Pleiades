PushLibrary = namespace("PushLibrary");

PushLibrary.GliderWidget = function(glidingContainer, parentDiv, childDiv) {
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