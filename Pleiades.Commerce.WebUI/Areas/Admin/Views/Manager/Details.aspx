<%@ Page Language="C#" 
    MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<UserViewModel>" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.StandardActionButton(StandardButton.Back, "List") %>
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
            <td><%: Html.StandardActionButton(
                    StandardButton.Unlock, "Unlock", routeValues: new { id = Model.DomainUserId })%></td>
        </tr>
        <% } %>
        <tr>
            <td class="label-right" style="width:160px;">Edit this User</td>
            <td><%: Html.StandardActionButton(
                    StandardButton.Edit, "Edit", routeValues: new { id = Model.DomainUserId } )%></td>
        </tr>
        <tr>
            <td class="label-right">Reset Password</td>
            <td><%: Html.StandardActionButton(
                    StandardButton.Reset, "Reset", routeValues: new { id = Model.DomainUserId })%></td>
        </tr>
        <tr>
            <td class="label-right">Change Password</td>
            <td><%: Html.StandardActionButton(
                    StandardButton.Change, "Change", routeValues: new { id = Model.DomainUserId })%></td>
        </tr>
        <% if (!this.Model.UserName.IsLeadAdmin())
        { %>
        <tr>
            <td class="label-right">Delete this User</td>
            <td><%: Html.StandardActionButton(
                    StandardButton.Delete, "Delete", routeValues: new { id = Model.DomainUserId } )%></td>
        </tr>
        <% } %>
        </table>
    </div>
</asp:Content>
