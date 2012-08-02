<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<ProductListModel>" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Pleiades Commerce - Administrative</title>
</head>
<body>
    <h1>Welcome Back to Pleiades!- Under Construction</h1>

    <%: Html.RouteLink("Go To Admin Area", new { area = "Admin", controller = "Home", action = "Index" }) %>
    <br />

    <h4>Sample Data Entry Form</h4>
    <div>
        <% using(Html.BeginForm("List", "Products", FormMethod.Post)) 
           { %>
            <%: Html.FormLineEditorFor(x => x.Name) %>
            <%: Html.FormLineEditorFor(x => x.AccountBalance) %>
            <%: Html.FormLineDropDownListFor(x => x.SelectedValue, this.Model.EzList, "Select ...") %>
            <input id="savebutton" value="Click Here To Save" type="submit" />
        <% } %>
    </div>
</body>
</html>
