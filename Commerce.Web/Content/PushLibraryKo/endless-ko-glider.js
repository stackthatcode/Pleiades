var pushLibrary = namespace("PushLibrary");

pushLibrary.EndlessGlider = function(container, divId1, divId2) {
    var self = this;

    self.WorkspaceTemplate = ko.observable(null);
    self.EditorTemplate1 = ko.observable(null);
    self.EditorTemplate2 = ko.observable(null);
    self.ActiveEditor = null;

    self.Init = function(workspaceTemplate, editorTemplate) {
        self.ActiveEditor = 1;
        self.WorkspaceTemplate(workspaceTemplate);
        self.EditorTemplate1(editorTemplate);
        $(divId2).hide();
    };

    self.GlideToRight = function(workspaceTemplate, editorTemplate, userCallback) {
        self.Glide(workspaceTemplate, editorTemplate, "RIGHT", userCallback);
    };

    self.GlideToLeft = function(workspaceTemplate, editorTemplate, userCallback) {
        self.Glide(workspaceTemplate, editorTemplate, "LEFT", userCallback);
    };

    self.Glide = function(workspaceTemplate, editorTemplate, direction, userCallback) {
        var prevDiv, nextDiv, nextEditorTemplate, prevEditorTemplate;
        if (self.ActiveEditor == 1) {
            prevDiv = divId1;
            nextDiv = divId2;
            nextEditorTemplate = self.EditorTemplate2;
            prevEditorTemplate = self.EditorTemplate1;
        } else {
            prevDiv = divId2;
            nextDiv = divId1;
            nextEditorTemplate = self.EditorTemplate1;
            prevEditorTemplate = self.EditorTemplate2;
        }

        var glideLeftFunction = function(callback) {
            $(nextDiv).css({ display: "block", left: "-940px", top: "0px" });

            $(prevDiv).animate({
                    left: "+=940"
                },
                {
                    complete: function() { $(prevDiv).css({ display: "none" }); },
                    duration: 250,
                }
            );
            $(nextDiv).animate({
                    left: "+=940"
                },
                {
                    complete: callback,
                    duration: 250,
                }
            );
        };

        var glideRightFunction = function(callback) {
            $(nextDiv).css({ display: "block", left: "+940px", top: "0px" });

            $(prevDiv).animate({
                    left: "-=940"
                },
                {
                    complete: function() { $(prevDiv).css({ display: "none" }); },
                    duration: 250,
                }
            );
            $(nextDiv).animate({
                    left: "-=940"
                },
                {
                    complete: callback,
                    duration: 250,
                }
            );
        };

        flow.exec(
            function() {
                nextEditorTemplate(editorTemplate);
                if (direction == "LEFT") {
                    glideLeftFunction(this);
                } else {
                    glideRightFunction(this);
                }
            },
            function() {
                self.ActiveEditor = self.ActiveEditor == 1 ? 2 : 1;
                self.WorkspaceTemplate(workspaceTemplate);
                prevEditorTemplate(null);
                if (userCallback) {
                    userCallback();
                }
            }
        );
    };
};
