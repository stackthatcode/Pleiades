<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<ProductListModel>" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Pleiades Commerce - Administrative</title>
    <%= Html.QfRenderHtmlHead()%>
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
           <table>
           <tr>
            <td><%: Html.QfLabelFor(x => x.Name) %></td>
            <td><%: Html.QfTextEditorFor(x => x.Name) %></td>
           </tr>

           <tr>
            <td><%: Html.QfLabelFor(x => x.AccountBalance)%></td>
            <td><%: Html.QfTextEditorFor(x => x.AccountBalance)%></td>
           </tr>
           
           <tr>
            <td><%: Html.QfLabelFor(x => x.Password)%></td>
            <td><%: Html.QfPasswordFor(x => x.Password)%></td>
           </tr>

           <tr>
            <td><%: Html.QfLabelFor(x => x.SelectedValueForDropDownList)%></td>
            <td><%: Html.QfDropDownListFor(x => x.SelectedValueForDropDownList, Model.SelectListGenerator)%></td>
           </tr>
           
           <tr>
            <td><%: Html.QfLabelFor(x => x.SelectedValueForRadio)%></td>
            <td><%: Html.QfRadioButtonFor(x => x.SelectedValueForRadio, "1", "Option 1") %>
                <%: Html.QfRadioButtonFor(x => x.SelectedValueForRadio, "2", "Option 2") %>
                <%: Html.QfRadioButtonFor(x => x.SelectedValueForRadio, "3", "Option 3") %></td>
           </tr>

           <tr>
            <td colspan="2"><%: Html.ValidationSummary() %></td>
           </tr>
           </table>

            <%: Html.QfSubmitButton(QfButtonId.Save, "Save") %>
        <% } %>
    </div>
</body>
</html>
