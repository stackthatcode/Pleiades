<%@ Page Title="Pleiades" Language="C#"
    MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<ResetPasswordModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Reset Password For > <%: Model.Email %></h1>
    <div style="padding-top:30px;">
        <div class="dottedline"></div>
            
        <%: Html.HiddenFor(x => x.Email) %>
        <table class="recordgrid" cellpadding="0" cellspacing="0">
        <tr>
            <td class="label-right" style="width:170px;">New Password:</td>
            <td><%: Model.NewPassword %></td>
        </tr>
        <tr>
            <td></td>
            <td align="left">
                <div style="width:600px;">
                <%: Html.QfActionButton(QfButtonId.Done, "Details", style: "float:left;", route: new { id = this.RouteData.Values["id"] })%>
                </div>
            </td>
        </tr>
        </table>
    </div>
</asp:Content>
