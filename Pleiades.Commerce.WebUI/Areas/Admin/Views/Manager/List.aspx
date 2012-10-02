<%@ Page Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="ViewPage<List<UserViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.QfRouteButton(QfButtonId.Back, "Back", OutboundNavigation.AdminHome()) %>
    <%: Html.QfClearDiv() %>
    <%: Html.ActionLink("Add New User", "Create") %>
    <%: Html.QfClearDiv() %>
    
    <h1>Manage Existing Admins</h1>
    <div style="padding-top:30px;">
        
        <!-- Make Partial View...? -->
        <table cellpadding="0" cellspacing="0" class="datagrid">
        <thead>
            <tr>
                <td style="width:75px;">Type</td>
                <td style="width:200px;">Email</td>
                <td style="width:175px;">Name</td>
                <td style="width:175px;">Date Created</td>
                <td align="center" style="width:100px;">Approved</td>
                <td align="center" style="width:100px;">Online</td>
                <td align="center" style="width:100px;">Locked Out</td>
                <td style="width:75px; text-align:right;">Action</td>
            </tr>
        </thead>
        
        <tbody>
        <% foreach (var user in this.Model)
           { %>
           <tr>
                <td><%: user.UserRole %></td>
                <td><%: user.Email.TruncateAfter(20) %></td>
                <td><%: user.FullName.TruncateAfter(20) %></td>
                <td><%: user.CreationDate %></td>
                <td align="center"><%: user.IsApproved %></td>
                <td align="center"><%: user.IsOnline %></td>
                <td align="center"><%: user.IsLockedOut %></td>
                <td align="right"><%: Html.RouteLink("View", OutboundNavigation.AdminManagerDetails(user.AggregateUserId)) %></td>
           </tr>
        <% } %>
        </tbody>
        </table>
    </div>
</asp:Content>