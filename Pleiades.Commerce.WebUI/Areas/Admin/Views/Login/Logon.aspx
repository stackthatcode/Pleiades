<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Pleiades.Commerce.WebUI.Areas.Admin.Models.LogOnViewModel>" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title>Pleiades Commerce - Administrative</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta name="description" content="UI Elements: Large Drop Down Menu" />
    <meta name="keywords" content="jquery, drop down, menu, navigation, large, mega " />

    <%: Html.Stylesheet("~/Content/Stylesheets/AdminMenu.css") %>
    <%: Html.Stylesheet("~/Content/Stylesheets/Pleiades.css") %>
    <%: Html.Stylesheet("~/Content/Stylesheets/Buttons.css") %>
    <%: Html.Javascript("~/Scripts/Generic/jquery-1.5.1.js")%>
    <%: Html.Javascript("~/Scripts/Domain/AdminMenu.js")%>

    <style type="text/css">
        body
        {
            color: #FFF;
        }
        
        a
        {
            color: #BBF;
        }
        
        h1
        {
            font-size:45px;
        }
    </style>
</head>

<body style="background-repeat:no-repeat; background-image:url('/Content/Images/body-background.gif'); background-color:#0e0a08; margin:0px; text-align:center;">

	<div style="height:300px; width:1024px; margin-left:auto; margin-right:auto; padding-top:80px; text-align:left;">
        <div style="width:800px;"> 
            <h1 style="float:left; color:#FFF; padding-right:10px;">Login to </h1>
            <h1 style="float:left; color:#BBF;">Pleiades Commerce</h1>
            <div style="clear:both;"></div>
            <div style="clear:both; height:30px;"></div>
            <p>Please enter valid credentials to access the administrative area.</p>
            <p><a href="#">Click here</a> if you forgot your password.</p>
            <p>To return to the Storefront, <%: Html.RouteLink("click here", new { area = "", controller = "Products" }) %>.</p>

            <div style="clear:both; height:25px;"></div>
            <% Html.EnableClientValidation(); %>
            <% using (Html.BeginForm("Logon", "Login", FormMethod.Post, new { autocomplete = "off" }))
            { %>
                <table>
                <tr>
                    <td style="width:350px;">
                        <h2>Username</h2>
                        <%: Html.TextBoxFor(x => x.UserName, new { @class = "login-editor" })%>
                    </td>
                    
                    <td style="width:350px;">
                        <h2>Password</h2>
                        <%: Html.PasswordFor(x => x.Password, new { @class = "login-editor" })%>
                    </td>

                    <td valign="bottom">
                        <input type="submit" value="Log On" class="loginbutton" />
                    </td>
                </tr>
                </table>

                <div style="clear:both; height:20px;"></div>
                <%: Html.ValidationMessage("", "WARNING - Invalid Authentication Credentials") %>
            <% } %>
        </div>
    </div>
</body>
</html>
