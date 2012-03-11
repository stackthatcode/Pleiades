<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Pleiades.Commerce.WebUI.Models.ProductListModel>" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Pleiades Commerce - Administrative</title>
</head>
<body>
    <h1>Products Home Page - Under Construction</h1>
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
