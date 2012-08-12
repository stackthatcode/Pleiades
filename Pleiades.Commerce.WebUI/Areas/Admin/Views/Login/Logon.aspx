<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Pleiades.Commerce.WebUI.Areas.Admin.Models.LogOnViewModel>" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title>Pleiades Commerce - Administrative</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta name="description" content="UI Elements: Large Drop Down Menu" />
    <meta name="keywords" content="jquery, drop down, menu, navigation, large, mega " />

    <style type="text/css">
        h1
        {
            font-size:40px;
        }
        .field-validation-valid
        {
            display: none;
        }
        .validation-summary-valid
        {
            display: none;
        }
    </style>
</head>

<body "text-align:center;">

	<div style="height:300px; width:1024px; margin-left:auto; margin-right:auto; text-align:left;">
        <div style="width:800px;"> 
            <h1 style="float:left; padding-right:10px;">Login to </h1>
            <h1 style="float:left;">Pleiades Commerce</h1>
            <div style="clear:both;"></div>
            
            <% Html.EnableClientValidation(); %>
            <% using (Html.BeginForm("Logon", "Login", FormMethod.Post, new { autocomplete = "off" }))
            { %>
                <div style="float:left;">
                <h4>Username</h4>
                <%: Html.TextBoxFor(x => x.UserName, new { @class = "login-editor" })%>
                </div>

                <div style="float:left;">
                <h4>Password</h4>
                <%: Html.PasswordFor(x => x.Password, new { @class = "login-editor" })%>
                <input type="submit" value="Log On" class="loginbutton" />
                </div>

                <div style="clear:both; height:20px;"></div>
                <%: Html.ValidationMessage("", "WARNING - Invalid Authentication Credentials") %>
            <% } %>
        </div>

        <p>Please enter valid credentials to access the administrative area.</p>
        <p><a href="#">Click here</a> if you forgot your password.</p>
        <p>To return to the Storefront, <%: Html.RouteLink("click here", OutboundNavigation.PublicHome) %>.</p>

    </div>
</body>
</html>
