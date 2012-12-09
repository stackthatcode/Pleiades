<%@ Page Title="Pleiades" Language="C#" 
        MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<ChangePasswordModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.QfActionButton(QfButtonId.Back, "Details", route: new { id = this.RouteData.Values["id"].ToString() })%>

    <h1>Change Password For > <%: Model.Email %></h1>

    <div style="padding-top:30px;">
        <% using (Html.BeginForm("ChangePassword", "Manager", FormMethod.Post, new { enctype = "multipart/form-data", autocomplete = "off" })) 
            { %>
            
            <div class="dottedline"></div>
            <%: Html.HiddenFor(x => x.Email) %>

            <table class="recordgrid" cellpadding="0" cellspacing="0">
            <tr>
                <td class="label-right" style="width:170px;">Old Password:</td>
                <td><%: Html.QfPasswordFor(x => x.OldPassword, 25)%></td>
            </tr>
            <tr>
                <td class="label-right" style="width:170px;">New Password:</td>
                <td><%: Html.QfPasswordFor(x => x.NewPassword, 25)%></td>
            </tr>
            <tr>
                <td class="label-right">Verify Password:</td>
                <td><%: Html.QfPasswordFor(x => x.PasswordVerify, 25)%></td>
            </tr>
            <tr>
                <td></td>
                <td align="left">
                    <div style="width:600px;">
                    <%: Html.QfActionButton(QfButtonId.Cancel, "Details", style: "float:left;", route: new { id = this.RouteData.Values["id"] })%>
                    <%: Html.QfSubmitButton(QfButtonId.Save, "save", style: "float:left;") %>
                    </div>
                </td>
            </tr>
            </table>
        <% } %>
    </div>
</asp:Content>
