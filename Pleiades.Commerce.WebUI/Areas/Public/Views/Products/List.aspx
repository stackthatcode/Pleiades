﻿<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<ProductListModel>" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Pleiades Commerce - Administrative</title>
</head>
<body>
    <h1>Welcome Back to Pleiades!- Under Construction</h1>

    <%: Html.RouteLink("Go To Admin Area", OutboundNavigation.AdminHome()) %>
    <%: Html.RouteLink("Go To Admin Login", OutboundNavigation.AdminLogin()) %>

    <br />

    <h4>Sample Data Entry Form</h4>
    <div>
        <% using(Html.BeginForm("List", "Products", FormMethod.Post)) 
           { %>
            <%: Html.QfTextEditorFor(x => x.Name) %>
            <%: Html.QfTextEditorFor(x => x.AccountBalance)%>

            <!-- Replace  these -->
            <%: Html.FormLineDropDownListFor(x => x.SelectedValue, this.Model.EzList, "Select ...") %>
            <input id="savebutton" value="Click Here To Save" type="submit" />
        <% } %>
    </div>
</body>
</html>
