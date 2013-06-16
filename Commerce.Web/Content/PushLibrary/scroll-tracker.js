PushLibrary = namespace("PushLibrary");

PushLibrary.ScrollTracker = function (containerDiv) {
    var self = this;

    self.TrackToIdIfNotNull = function (id) {
        if (!id) {
            return;
        }
        self.ScrollToTracker('tracker' + id);
    }

    self.ScrollToTracker = function (trackerId) {
        if (!trackerId) {
            return;
        }
        var offset = $("#" + trackerId).parent().offset().top - $(containerDiv).offset().top;
        $("body").scrollTop(offset);
    }

    self.HighlightByTrackerId = function (id) {
        var set = $('#tracker' + id).closest("td");
        set.animate({ backgroundColor: "#AAA" }, { duration: 1000 });
        set.animate({
            backgroundColor: "#FFF"
        },
		{
			duration: 1000,
			complete: function () {
			    set.css({ backgroundColor: "" });
			}
		});
    }
}
