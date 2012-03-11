<%@ Page Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="height:40px; width:500px; float:left; z-index:50;">
        <%: Html.StandardActionButton(StandardButton.Back, "Index", routeValues: new { controller = "Home" }) %>
        <h1>Manage Categories</h1>
    </div>

    <div style="padding-top:10px; display:block; clear:both;">
        <!-- TODO: look at JRender library -->
        <%: Html.Javascript("~/Scripts/Domain/AjaxGrid.js") %>
        <%: Html.Stylesheet("~/Content/Stylesheets/PleiadesAjax.css") %>

        <div id="grid">
        </div>

        <script type="text/javascript">
            function GridRowToJson(tableRow) 
            {
                var id = $(tableRow).find('*').filter("input[id='Id']")[0].value;
                var name = $(tableRow).find('*').filter("input[id='Name']")[0].GetFieldValue();
                var description = $(tableRow).find('*').filter("input[id='Description']")[0].GetFieldValue();

                var activeCtrl = $(tableRow).find('*').filter("select[id='Active']")[0];
                var active = activeCtrl.options[activeCtrl.selectedIndex].value;

                var data = { Id: id, Name: name, Description: description, Active: active };                
                return data;
            }

            function EditRowPostRenderImpl(tableRow)
            {
                var nameField = $(tableRow).find('*').filter("input[id='Name']")[0];
                var descriptionField = $(tableRow).find('*').filter("input[id='Description']")[0];

                BasicTextFieldManager(nameField);
                BasicTextFieldManager(descriptionField);
            }

            function PostGridRenderImpl(gridDisplay)
            {
                $(gridDisplay).find("table tr:eq(0) td:eq(0) a").click(function () { grid.ChangeSortIndex(1); return false; });
                $(gridDisplay).find("table tr:eq(0) td:eq(1) a").click(function () { grid.ChangeSortIndex(2); return false; });
                $(gridDisplay).find("table tr:eq(0) td:eq(2) a").click(function () { grid.ChangeSortIndex(3); return false; });
            }

            function Validate(tableRow)
            {
                var data = GridRowToJson(tableRow);
                if (data.Name == "" || data.Description == "") 
                {
                    $(tableRow).effect("highlight", { color: "#ffffff" }, 1000);
                    return false;
                }
                else
                {
                    return true;
                }
            }

            var grid = AjaxGrid("grid", "1000px", "400px");

            grid.RetrievePageByNumberUrlBuilder =
                function (page, sortIndex) { return '<%: Url.Action("Rows") %>/Page' + page + '/Sort/' + sortIndex; };
            grid.RetrievePageByIdUrlBuilder =
                function (id, sortIndex) { return '<%: Url.Action("PageById") %>/' + id + '/Sort/' + sortIndex; };

            grid.AddRowUrlBuilder = function () { return '<%: Url.Action("AddRow") %>'; };
            grid.EditRowUrlBuilder = function (editid) { return '<%: Url.Action("EditRow") %>/' + editid; };
            grid.SaveRowUrlBuilder = function () { return '<%: Url.Action("SaveRow") %>'; };
            grid.DeleteRowUrlBuilder = function (deleteid) { return '<%: Url.Action("DeleteRow") %>/' + deleteid; };

            grid.GridRowToJsonBuilder = function (tableRow) { return GridRowToJson(tableRow); };
            grid.EditRowPostRender = function (tableRow) { EditRowPostRenderImpl(tableRow); };
            grid.PostGridRender = function (displayGrid) { return PostGridRenderImpl(displayGrid); };
            grid.ValidateRowMethod = function (tableRow) { return Validate(tableRow); }
            grid.RowIdFinderMethod = function (tableRow) { return $(tableRow).find('*').filter("input[id='Id']")[0].value; };

            $(grid)[0].Initialize();
            $(grid)[0].RetrievePageByNumber(1);
        </script>

        <div style="clear:both; height:20px;"></div>
    </div>
</asp:Content>

