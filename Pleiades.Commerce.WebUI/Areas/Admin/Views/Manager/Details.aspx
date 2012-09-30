<%@ Page Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<UserViewModel>" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.QfRouteButton(QfButtonId.Back, "Back", OutboundNavigation.AdminManagerList())%>
    <h1>View Admin User &gt; <%: Model.Email  %></h1>

    <div style="padding-top:30px;">
        <div class="dottedline"></div>
        <%: Html.Partial("AdminUserDetails", this.Model) %>
        
        <div style="font-size:1px;"></div>
        
        <table class="recordgrid" cellpadding="0" cellspacing="0">
        <% if (this.Model.IsLockedOut)
           { %>
        <tr>
            <td class="label-right">Unlock This User</td>
            <td><%: Html.QfActionButton(QfButtonId.Unlock, "Unlock", route: new { id = Model.AggregateUserId }) %></td>
        </tr>
        <% } %>
        <tr>
            <td class="label-right" style="width:160px;">Edit this User</td>
            <td><%: Html.QfActionButton(QfButtonId.Edit, "Edit", route: new { id = Model.AggregateUserId }) %></td>
        </tr>
        <tr>
            <td class="label-right">Reset Password</td>
            <td><%: Html.QfActionButton(QfButtonId.Reset, "Reset", route: new { id = Model.AggregateUserId })%></td>
        </tr>
        <tr>
            <td class="label-right">Change Password</td>
            <td><%: Html.QfActionButton(QfButtonId.Change, "Change", route: new { id = Model.AggregateUserId })%></td>
        </tr>
        <% if (this.Model.UserRole != UserRole.Supreme) { %>
        <tr>
            <td class="label-right">Delete this User</td>
            <td><%: Html.QfActionButton(QfButtonId.Delete, "Delete", route: new { id = Model.AggregateUserId })%></td>
        </tr>
        <% } %>
        </table>
    </div>
</asp:Content>
