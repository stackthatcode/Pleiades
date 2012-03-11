
/*** 1st-generation Pleiades generalized AJAX Grid
Requires JQuery dependency -- btw: how to insure that it was properly loaded?  */

function AjaxGrid(divId, width, height)
{
    // Create reference to this object
    var thisObject = this;

    // Page Number state
    thisObject.PageNumber = 1;

    // Current Sort Index
    thisObject.SortIndex = 1;

    // Assign delegates to property refs
    var blankFunction = function () { alert("Not yet implemented"); }
    var doNothing = function () { /* do nothing */ };
    
    thisObject.RetrievePageByNumberUrlBuilder = blankFunction;
    thisObject.RetrievePageByIdUrlBuilder = blankFunction;
    thisObject.AddRowUrlBuilder = doNothing;
    thisObject.EditRowUrlBuilder = doNothing;
    thisObject.SaveRowUrlBuilder = doNothing;
    thisObject.DeleteRowUrlBuilder = doNothing;
    thisObject.GridRowToJsonBuilder = doNothing;
    thisObject.EditRowPostRender = doNothing;
    thisObject.PostGridRender = doNothing;
    thisObject.ValidateRowMethod = function () { return true; };
    thisObject.RowIdFinderMethod = function () { return null; };

    // TODO: BADLY need to know why this doesn't work:
    // thisObject.RetrievePageByNumberUrlBuilder = CreateDelegate(thisObject, retrievePageByNumberUrlBuilder);

    thisObject.GridLoadingClass = 'gridLoading';
    thisObject.GridErrorClass = 'gridError';

    this.Initialize = function () 
    {
        // Add grid rendering Divs to the DOM
        thisObject.RootDiv = $("#" + divId).css("width", width).css("height", height);

        thisObject.GridDisplay = $("<div></div>")
                .css("width", width).css("height", height).css("overflow", "visible");

        thisObject.GridLoading = $("<div></div>")
                .css("width", width).css("height", height)
                .html("<h1 style='padding-top:125px;'>Loading, please wait</h1>")
                .addClass(thisObject.GridLoadingClass);

        thisObject.GridError = $("<div></div>")
                .css("width", width).css("height", height)
                .html("<h1 style='padding-top:125px;'>System Error - Please Refresh</h1>")
                .addClass(thisObject.GridErrorClass);

        // Hide everything and append it
        thisObject.GridDisplay.hide().appendTo(RootDiv);
        thisObject.GridLoading.hide().appendTo(RootDiv);
        thisObject.GridError.hide().appendTo(RootDiv);
    }

    this.RetrievePageByNumber = function (page, highlightId) 
    {
        thisObject.GridDisplay.hide();
        thisObject.GridLoading.show();

        this.PageNumber = page;
        var url = thisObject.RetrievePageByNumberUrlBuilder(this.PageNumber, this.SortIndex);
        AjaxGet(url, CreateDelegate(this, function (html) { RenderPage(html, highlightId); }), thisObject.ErrorHandler);
    }

    this.RetrievePageById = function (id) 
    {
        var url = thisObject.RetrievePageByIdUrlBuilder(id, this.SortIndex);
        AjaxPost(url, 
            null, CreateDelegate(this, function (result) { this.RetrievePageByNumber(result.PageNumber, id); }, thisObject.ErrorHandler));
    }

    this.ChangeSortIndex = function (newIndex)
    {
        this.SortIndex = newIndex;
        thisObject.RetrievePageByNumber(this.PageNumber);
    }

    this.RenderPage = function (html, highlightId) 
    {
        thisObject.GridLoading.hide();
        thisObject.GridDisplay.html(html);
        thisObject.GridDisplay.show();

        // Decorate the Edit Links with click methods
        // NOTICE: in the context of a callback event, "this" operater points to the element itself
        thisObject.GridDisplay.find('*').filter(".editlink")
            .click(function () { thisObject.RetrieveEditRow(this); return false; });

        // Decorate the Paging with links
        thisObject.GridDisplay.find('*').filter(".pagelink")
            .click(function () { thisObject.RetrievePageByNumber(this.innerHTML.replace("Page ", "")); return false; });

        // Decorate the Delete Links with click methods
        thisObject.GridDisplay.find('*').filter(".deletelink")
            .click(function () { thisObject.DeleteRow(this); return false; });

        // Decorate the Add Links with click methods
        thisObject.GridDisplay.find('*').filter(".addnewbutton")
            .click(function () { thisObject.RetrieveAddRow(); return false; });

        if (highlightId != null) {
            $(GridDisplay).find("table tr:gt(0)").each(function () {
                thisObject.HighlightIfRowContainsId(this, highlightId);
            });
        }

        // Any additional considerations...
        thisObject.PostGridRender(this.GridDisplay);
    }

    // Automatically highlights the edited row
    this.HighlightIfRowContainsId = function (tableRow, id) 
    {
        var tableRowId = thisObject.RowIdFinderMethod(tableRow);
        if (tableRowId == id) {
            $(tableRow).effect("highlight", { color: "#999999" }, 3000);
        }
    }

    this.RetrieveEditRow = function (linkElement) 
    {
        thisObject.ClearEditRow();
        thisObject.GridLoading.show();
        thisObject.GridDisplay.hide();

        // The Edit link is required to have a sibling input field with an argument for the Edit Id in the URL
        var editId = $(linkElement).siblings('input')[0].value;
        var parentRow = $(linkElement).parent().parent();
        var url = thisObject.EditRowUrlBuilder(editId);

        AjaxPost(url, null, function (html) { RenderEditRow(html, parentRow); }, thisObject.ErrorHandler);
        return false;
    }

    this.RenderEditRow = function (html, parentRow) {
        thisObject.ClearEditRow();

        parentRow.hide();
        parentRow.after(html);
        thisObject.GridLoading.hide();
        thisObject.GridDisplay.show();

        thisObject.GridDisplay.find('*')
            .filter(".edittablerow a.cancelbutton")
            .click(function () { thisObject.ClearEditRow(); return false; });

        thisObject.GridDisplay.find('*')
            .filter(".edittablerow a.savebutton")
            .click(function () { thisObject.SaveEditRow(); return false; });

        var tableRow = ($(thisObject.GridDisplay).find(".edittablerow")[0]);
        thisObject.EditRowPostRender(tableRow);
    }

    this.RetrieveAddRow = function () 
    {
        thisObject.ClearEditRow();
        var url = thisObject.AddRowUrlBuilder();
        AjaxPost(url, null, function (jqXHR) { RenderAddRow(jqXHR); }, thisObject.ErrorHandler);
        return false;
    }

    this.RenderAddRow = function (jqXHR) 
    {
        thisObject.ClearEditRow();

        // Insert the Add Row in the Grid under the headings...
        var headerRow = ($(thisObject.GridDisplay).find("*").filter("table tr")[0]);
        $(headerRow).after(jqXHR);

        thisObject.GridDisplay.find('*')
            .filter(".edittablerow a.cancelbutton")
            .click(function () { thisObject.ClearEditRow(); return false; });

        thisObject.GridDisplay.find('*')
            .filter(".edittablerow a.savebutton")
            .click(function () { thisObject.SaveEditRow(); return false; });

        var tableRow = ($(thisObject.GridDisplay).find(".edittablerow")[0]);
        thisObject.EditRowPostRender(tableRow);
    }

    this.ClearEditRow = function () 
    {
        thisObject.GridDisplay.find('*').filter(".edittablerow").remove();
        thisObject.GridDisplay.find('*').filter(".displaytablerow").show();
    }

    this.SaveEditRow = function () {
        var tableRow = thisObject.GridDisplay.find('*').filter(".edittablerow")[0];
        if (!thisObject.ValidateRowMethod(tableRow)) {
            return false;
        }

        var data = thisObject.GridRowToJsonBuilder(tableRow);
        var url = SaveRowUrlBuilder();

        AjaxPost(url, 
            data, function (jqXHR) { thisObject.RetrievePageById(jqXHR.Id); }, thisObject.ErrorHandler);
    }

    this.DeleteRow = function (linkElement) 
    {
        var deleteId = $(linkElement).siblings('input')[0].value;
        var parentRow = $(linkElement).parent().parent();
        var url = this.DeleteRowUrlBuilder(deleteId);

        AjaxPost(url, 
            { id: deleteId }, function (jqXHR) { thisObject.RetrievePageByNumber(thisObject.PageNumber) }, thisObject.ErrorHandler);
    }

    this.ErrorHandler = function (jqXHR, textStatus, errorThrown) 
    {
        // PENDING: do something helpful using the parameters
        thisObject.GridDisplay.hide();
        thisObject.GridLoading.hide();
        thisObject.GridError.show();
    }

    return this;
}

function BasicTextFieldManager(field)
{
    var emptyFieldText = '(Please Enter)';
    var thisObject = this;

    this.onBlur = function (field) {
        if (field.value.trim() == "") {
            field.value = emptyFieldText;
        }
    };

    this.onFocus = function (field) {
        if (field.value == emptyFieldText) {
            field.value = '';
        }
    };

    $(onBlur(field));

    $(field)
        .focus(function () { thisObject.onFocus(this); })
        .blur(function () { thisObject.onBlur(this); });

    field.GetFieldValue = function () { return field.value.replace(emptyFieldText, "").trim(); }
}
